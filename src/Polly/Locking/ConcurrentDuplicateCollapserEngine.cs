using System;
using System.Collections.Concurrent;
using System.Threading;
using Polly.Caching;

namespace Polly.Locking
{
    internal static class CollapseConcurrentDuplicatesEngine
    {
        internal static TResult Implementation<TResult>(
            Func<Context, CancellationToken, TResult> action,
            Context context,
            CancellationToken cancellationToken,
            ICacheKeyStrategy keyStrategy, // Suggested improvement: generalise (rename) ICacheKeyStrategy to IKeyStrategy. Use IKeyStrategy here (to take word 'cache' out). Then define ICacheKeyStrategy : IKeyStrategy { }, for backward compatibility for cache policy.
            ConcurrentDictionary<string, Lazy<object>> collapser, 
            ILockProvider lockProvider)
        {
            cancellationToken.ThrowIfCancellationRequested();

            string key = keyStrategy.GetCacheKey(context);

            // Fast-path if no key specified on Context (similar to CachePolicy).
            if (key == null)
            {
                return action(context, cancellationToken);
            }

            Lazy<object> lazy;

            using (lockProvider.AcquireLock(key, context, cancellationToken))
            {
                lazy = collapser.GetOrAdd(key, new Lazy<object>(() => action(context, cancellationToken), LazyThreadSafetyMode.ExecutionAndPublication)); // Note: per documentation, LazyThreadSafetyMode.ExecutionAndPublication guarantees single execution, but means the executed code must not lock, as this risks deadlocks.  We should document.
            }

            TResult result = (TResult)lazy.Value;

            // As soon as the lazy has returned a result to one thread, the concurrent request set is over, so we evict the lazy from the ConcurrentDictionary.
            // We need to evict within a lock, to be sure we are not, due to potential race with new threads populating, evicting a different lazy created by a different thread.
            // To reduce lock contention, first check outside the lock whether we still need to remove it (we will double-check inside the lock).
            if (collapser[key] == lazy)
            {
                using (lockProvider.AcquireLock(key, context, cancellationToken))
                {
                    // Double-check that there has not been a race which updated the dictionary with a new value.
                    if (collapser[key] == lazy)
                    {
                        collapser.TryRemove(key, out _);
                    }
                }
            }

            return result;
        }
    }
}
