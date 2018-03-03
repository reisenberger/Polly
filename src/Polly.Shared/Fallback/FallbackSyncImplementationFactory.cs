using System;
using System.Threading;
using Polly.Utilities;

namespace Polly.Fallback
{
    internal class FallbackSyncImplementationFactory : ISyncPolicyImplementationFactory
    {
        private Action<Exception, Context> _onFallback;
        private Action<Exception, Context, CancellationToken> _fallbackAction;

        public FallbackSyncImplementationFactory(Action<Exception, Context> onFallback, Action<Exception, Context, CancellationToken> fallbackAction)
        {
            _onFallback = onFallback ?? throw new ArgumentNullException(nameof(onFallback));
            _fallbackAction = fallbackAction ?? throw new ArgumentNullException(nameof(fallbackAction));
        }

        public ISyncPolicyImplementation<TResult> GetImplementation<TResult>(ISyncPolicy policy)
        {
            if (typeof(TResult) != typeof(EmptyStruct))
            {
                throw FallbackPolicy.GenericExecuteMethodsAreAnInvalidOperationOnNonGenericFallbackPolicy<TResult>();
            }

            FallbackPolicy fallback = policy as FallbackPolicy ?? throw new ArgumentException($"policy supplied to {this.GetType().Name} is not a {typeof(FallbackPolicy).Name}", nameof(policy));

            return new FallbackSyncImplementation<TResult>(fallback, 
                fallback.ExceptionPredicates, 
                PredicateHelper<TResult>.EmptyResultPredicates,
                (outcome, ctx) => _onFallback(outcome.Exception, ctx),
                (outcome, ctx, ct) => { _fallbackAction(outcome.Exception, ctx, ct); return default(TResult); }
                );
        }
    }

    internal class FallbackSyncImplementationFactory<TResult> : ISyncPolicyImplementationFactory<TResult>
    {
        private Action<DelegateResult<TResult>, Context> _onFallback;
        private Func<DelegateResult<TResult>, Context, CancellationToken, TResult> _fallbackAction;

        public FallbackSyncImplementationFactory(Action<DelegateResult<TResult>, Context> onFallback, Func<DelegateResult<TResult>, Context, CancellationToken, TResult> fallbackAction)
        {
            _onFallback = onFallback ?? throw new ArgumentNullException(nameof(onFallback));
            _fallbackAction = fallbackAction ?? throw new ArgumentNullException(nameof(fallbackAction));
        }

        public ISyncPolicyImplementation<TResult> GetImplementation(ISyncPolicy<TResult> policy)
        {
            FallbackPolicy<TResult> fallback = policy as FallbackPolicy<TResult> ?? throw new ArgumentException($"policy supplied to {this.GetType().Name} is not a {typeof(FallbackPolicy<TResult>).Name}", nameof(policy));

            return new FallbackSyncImplementation<TResult>(
                fallback,
                fallback.ExceptionPredicates,
                fallback.ResultPredicates,
                _onFallback,
                _fallbackAction
                );
        }
    }
}
