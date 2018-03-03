using System;
using System.Threading;

namespace Polly.Caching
{
    /// <summary>
    /// A cache policy that can be applied to the results of delegate executions.
    /// </summary>
    public partial class CachePolicy : Policy, ICachePolicy
    {
        private readonly ISyncCacheProvider _syncCacheProvider;
        private readonly ITtlStrategy _ttlStrategy;
        private readonly Func<Context, string> _cacheKeyStrategy;

        private readonly Action<Context, string> _onCacheGet;
        private readonly Action<Context, string> _onCacheMiss;
        private readonly Action<Context, string> _onCachePut;
        private readonly Action<Context, string, Exception> _onCacheGetError;
        private readonly Action<Context, string, Exception> _onCachePutError;

        internal CachePolicy(
            ISyncCacheProvider syncCacheProvider, 
            ITtlStrategy ttlStrategy,
            Func<Context, string> cacheKeyStrategy,
            Action<Context, string> onCacheGet,
            Action<Context, string> onCacheMiss,
            Action<Context, string> onCachePut,
            Action<Context, string, Exception> onCacheGetError,
            Action<Context, string, Exception> onCachePutError,
            Func<ISyncPolicy, ISyncPolicyImplementation<Object>> factory)
            : base(PolicyBuilder.Empty, factory)
        {
            _syncCacheProvider = syncCacheProvider;
            _ttlStrategy = ttlStrategy;
            _cacheKeyStrategy = cacheKeyStrategy;

            _onCacheGet = onCacheGet;
            _onCachePut = onCachePut;
            _onCacheMiss = onCacheMiss;
            _onCacheGetError = onCacheGetError;
            _onCachePutError = onCachePutError;
        }

        /// <summary>
        /// Overrides execution of void-returning calls, for cache policies, to be a simple pass-through.
        /// </summary>
        protected override void ExecuteThroughImplementationInternal<TExecutable>(TExecutable action, Context context, CancellationToken cancellationToken)
        {
            // Void-returning calls have no cached result to check and no result to cache; fast-path execute the call without intervention.
            action.Execute(context, cancellationToken);
        }

        /// <summary>
        /// Overrides method-generic executions through non-generic cache policies, to execute through a fully-typed implementation of the cache policy,
        /// to ensure that caching functionality on method-generic executions is invoked.
        /// </summary>
        protected override TMethodGeneric ExecuteThroughImplementationInternal<TExecutable, TMethodGeneric>(TExecutable func, Context context, CancellationToken cancellationToken)
        {
            return new CacheSyncImplementation<TMethodGeneric>(
                _syncCacheProvider.For<TMethodGeneric>(),
                _ttlStrategy.For<TMethodGeneric>(),
                _cacheKeyStrategy,
                _onCacheGet,
                _onCacheMiss,
                _onCachePut,
                _onCacheGetError,
                _onCachePutError
                )
                .Execute(func, context, cancellationToken);
        }

    }

    /// <summary>
    /// A cache policy that can be applied to the results of delegate executions.
    /// </summary>
    public partial class CachePolicy<TResult> : Policy<TResult>, ICachePolicy<TResult>
    {
        internal CachePolicy(Func<ISyncPolicy<TResult>, ISyncPolicyImplementation<TResult>> factory)
            : base(PolicyBuilder<TResult>.Empty, factory)
        { }

    }
}
