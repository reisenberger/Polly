using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.Caching
{
    public partial class CachePolicy 
    {
        private readonly IAsyncCacheProvider _asyncCacheProvider;

        internal CachePolicy(
            IAsyncCacheProvider asyncCacheProvider, 
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
            _asyncCacheProvider = asyncCacheProvider;
            _ttlStrategy = ttlStrategy;
            _cacheKeyStrategy = cacheKeyStrategy;

            _onCacheGet = onCacheGet;
            _onCachePut = onCachePut;
            _onCacheMiss = onCacheMiss;
            _onCacheGetError = onCacheGetError;
            _onCachePutError = onCachePutError;
        }

        /// <summary>
        /// Overrides execution of void-returning async calls, for cache policies, to be a simple pass-through.
        /// </summary>
        internal override Task ExecuteInternalAsync<TExecutable>(TExecutable action, Context context, CancellationToken cancellationToken,
            bool continueOnCapturedContext)
        {
            return action.ExecuteAsync(context, cancellationToken, continueOnCapturedContext);
        }

        /// <summary>
        /// Overrides method-generic async executions through non-generic cache policies, to execute through a fully-typed implementation of the cache policy,
        /// to ensure that caching functionality on method-generic executions is invoked.
        /// </summary>
        internal override Task<TMethodGeneric> ExecuteInternalAsync<TExecutable, TMethodGeneric>(TExecutable func, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            return new CacheAsyncImplementation<TMethodGeneric>(
                    _asyncCacheProvider.AsyncFor<TMethodGeneric>(),
                    _ttlStrategy.For<TMethodGeneric>(),
                    _cacheKeyStrategy,
                    _onCacheGet,
                    _onCacheMiss,
                    _onCachePut,
                    _onCacheGetError,
                    _onCachePutError
                )
                .ExecuteAsync(func, context, cancellationToken, continueOnCapturedContext);
        }
    }

    public partial class CachePolicy<TResult>
    {
        internal CachePolicy(Func<IAsyncPolicy<TResult>, IAsyncPolicyImplementation<TResult>> factory)
            : base(PolicyBuilder<TResult>.Empty, factory)
        { }
    }

}
