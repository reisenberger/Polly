using System;
using System.Threading;
using Polly.Execution;

namespace Polly.Caching
{
    internal class CacheSyncImplementation<TResult> : ISyncPolicyImplementation<TResult>
    {
        private readonly ISyncCacheProvider<TResult> _cacheProvider;
        private readonly ITtlStrategy<TResult> _ttlStrategy;
        private readonly Func<Context, string> _cacheKeyStrategy;
        private readonly Action<Context, string> _onCacheGet;
        private readonly Action<Context, string> _onCacheMiss;
        private readonly Action<Context, string> _onCachePut;
        private readonly Action<Context, string, Exception> _onCacheGetError;
        private readonly Action<Context, string, Exception> _onCachePutError;

        internal CacheSyncImplementation(
            ISyncCacheProvider<TResult> cacheProvider,
            ITtlStrategy<TResult> ttlStrategy,
            Func<Context, string> cacheKeyStrategy,
            Action<Context, string> onCacheGet,
            Action<Context, string> onCacheMiss,
            Action<Context, string> onCachePut,
            Action<Context, string, Exception> onCacheGetError,
            Action<Context, string, Exception> onCachePutError)
        {
            _cacheProvider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider));
            _ttlStrategy = ttlStrategy ?? throw new ArgumentNullException(nameof(ttlStrategy));
            _cacheKeyStrategy = cacheKeyStrategy ?? throw new ArgumentNullException(nameof(cacheKeyStrategy));
            _onCacheGet = onCacheGet ?? throw new ArgumentNullException(nameof(onCacheGet));
            _onCacheMiss = onCacheMiss ?? throw new ArgumentNullException(nameof(onCacheMiss));
            _onCachePut = onCachePut ?? throw new ArgumentNullException(nameof(onCachePut));
            _onCacheGetError = onCacheGetError ?? throw new ArgumentNullException(nameof(onCacheGetError));
            _onCachePutError = onCachePutError ?? throw new ArgumentNullException(nameof(onCachePutError));
        }

        public TResult Execute<TExecutable>(in TExecutable action, Context context, in CancellationToken cancellationToken) where TExecutable : ISyncPollyExecutable<TResult>
        {
            cancellationToken.ThrowIfCancellationRequested();

            string cacheKey = _cacheKeyStrategy(context);
            if (cacheKey == null)
            {
                return action.Execute(context, cancellationToken);
            }

            TResult valueFromCache;
            try
            {
                valueFromCache = _cacheProvider.Get(cacheKey);
            }
            catch (Exception ex)
            {
                valueFromCache = default(TResult);
                _onCacheGetError(context, cacheKey, ex);
            }
            if (valueFromCache != null && !valueFromCache.Equals(default(TResult)))
            {
                _onCacheGet(context, cacheKey);
                return valueFromCache;
            }
            else
            {
                _onCacheMiss(context, cacheKey);
            }

            TResult result = action.Execute(context, cancellationToken);

            Ttl ttl = _ttlStrategy.GetTtl(context, result);
            if (ttl.Timespan > TimeSpan.Zero && result != null && !result.Equals(default(TResult)))
            {
                try
                {
                    _cacheProvider.Put(cacheKey, result, ttl);
                    _onCachePut(context, cacheKey);
                }
                catch (Exception ex)
                {
                    _onCachePutError(context, cacheKey, ex);
                }
            }

            return result;
        }
    }
}
