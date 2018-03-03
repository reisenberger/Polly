using System;
using System.Threading;
using System.Threading.Tasks;
using Polly.Execution;

namespace Polly.Caching
{
    internal class CacheAsyncImplementation<TResult> : IAsyncPolicyImplementation<TResult>
    {
        private readonly IAsyncCacheProvider<TResult> _cacheProvider;
        private readonly ITtlStrategy<TResult> _ttlStrategy;
        private readonly Func<Context, string> _cacheKeyStrategy;
        private readonly Action<Context, string> _onCacheGet;
        private readonly Action<Context, string> _onCacheMiss;
        private readonly Action<Context, string> _onCachePut;
        private readonly Action<Context, string, Exception> _onCacheGetError;
        private readonly Action<Context, string, Exception> _onCachePutError;

        internal CacheAsyncImplementation(
            IAsyncCacheProvider<TResult> cacheProvider,
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

        public async Task<TResult> ExecuteAsync<TExecutableAsync>(TExecutableAsync action, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext) where TExecutableAsync : IAsyncPollyExecutable<TResult>
        {
            cancellationToken.ThrowIfCancellationRequested();

            string cacheKey = _cacheKeyStrategy(context);
            if (cacheKey == null)
            {
                return await action.ExecuteAsync(context, cancellationToken, continueOnCapturedContext).ConfigureAwait(continueOnCapturedContext);
            }

            TResult valueFromCache;
            try
            {
                valueFromCache = await _cacheProvider.GetAsync(cacheKey, cancellationToken, continueOnCapturedContext).ConfigureAwait(continueOnCapturedContext);
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

            TResult result = await action.ExecuteAsync(context, cancellationToken, continueOnCapturedContext).ConfigureAwait(continueOnCapturedContext);

            Ttl ttl = _ttlStrategy.GetTtl(context, result);
            if (ttl.Timespan > TimeSpan.Zero && result != null && !result.Equals(default(TResult)))
            {
                try
                {
                    await _cacheProvider.PutAsync(cacheKey, result, ttl, cancellationToken, continueOnCapturedContext).ConfigureAwait(continueOnCapturedContext);
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
