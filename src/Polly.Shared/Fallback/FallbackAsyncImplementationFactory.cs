using System;
using System.Threading;
using System.Threading.Tasks;
using Polly.Utilities;

namespace Polly.Fallback
{
    internal class FallbackAsyncImplementationFactory : IAsyncPolicyImplementationFactory
    {
        private Func<Exception, Context, Task> _onFallback;
        private Func<Exception, Context, CancellationToken, Task> _fallbackAction;

        public FallbackAsyncImplementationFactory(Func<Exception, Context, Task> onFallback, Func<Exception, Context, CancellationToken, Task> fallbackAction)
        {
            _onFallback = onFallback ?? throw new ArgumentNullException(nameof(onFallback));
            _fallbackAction = fallbackAction ?? throw new ArgumentNullException(nameof(fallbackAction));
        }

        public IAsyncPolicyImplementation<TResult> GetImplementation<TResult>(IAsyncPolicy policy)
        {
            if (typeof(TResult) != typeof(EmptyStruct))
            {
                throw FallbackPolicy.GenericExecuteMethodsAreAnInvalidOperationOnNonGenericFallbackPolicy<TResult>();
            }

            FallbackPolicy fallback = policy as FallbackPolicy ?? throw new ArgumentException($"policy supplied to {this.GetType().Name} is not a {typeof(FallbackPolicy).Name}", nameof(policy));

            return new FallbackAsyncImplementation<TResult>(fallback,
                fallback.ExceptionPredicates,
                PredicateHelper<TResult>.EmptyResultPredicates,
                (outcome, ctx) => _onFallback(outcome.Exception, ctx),
                async (outcome, ctx, ct, capture) => { await _fallbackAction(outcome.Exception, ctx, ct).ConfigureAwait(capture); return default(TResult); }
                );
        }
    }

    internal class FallbackAsyncImplementationFactory<TResult> : IAsyncPolicyImplementationFactory<TResult>
    {
        private Func<DelegateResult<TResult>, Context, Task> _onFallback;
        private Func<DelegateResult<TResult>, Context, CancellationToken, Task<TResult>> _fallbackAction;

        public FallbackAsyncImplementationFactory(Func<DelegateResult<TResult>, Context, Task> onFallback, Func<DelegateResult<TResult>, Context, CancellationToken, Task<TResult>> fallbackAction)
        {
            _onFallback = onFallback ?? throw new ArgumentNullException(nameof(onFallback));
            _fallbackAction = fallbackAction ?? throw new ArgumentNullException(nameof(fallbackAction));
        }

        public IAsyncPolicyImplementation<TResult> GetImplementation(IAsyncPolicy<TResult> policy)
        {
            FallbackPolicy<TResult> fallback = policy as FallbackPolicy<TResult> ?? throw new ArgumentException($"policy supplied to {this.GetType().Name} is not a {typeof(FallbackPolicy<TResult>).Name}", nameof(policy));

            return new FallbackAsyncImplementation<TResult>(
                fallback,
                fallback.ExceptionPredicates,
                fallback.ResultPredicates,
                _onFallback,
                (outcome, ctx, ct, capture) => _fallbackAction(outcome, ctx, ct)
                );
        }
    }
}
