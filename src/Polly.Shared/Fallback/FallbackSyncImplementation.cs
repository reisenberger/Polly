using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Polly.Execution;
using Polly.Utilities;

namespace Polly.Fallback
{
    internal class FallbackSyncImplementation<TResult> : ISyncPolicyImplementation<TResult>
    {
        private IsPolicy _policy;
        private IEnumerable<ExceptionPredicate> _shouldHandleExceptionPredicates;
        private IEnumerable<ResultPredicate<TResult>> _shouldHandleResultPredicates;
        private Action<DelegateResult<TResult>, Context> _onFallback;
        private Func<DelegateResult<TResult>, Context, CancellationToken, TResult> _fallbackAction;

        internal FallbackSyncImplementation(
            IsPolicy policy, 
            IEnumerable<ExceptionPredicate> shouldHandleExceptionPredicates, 
            IEnumerable<ResultPredicate<TResult>> shouldHandleResultPredicates,
            Action<DelegateResult<TResult>, Context> onFallback,
            Func<DelegateResult<TResult>, Context, CancellationToken, TResult> fallbackAction)
        {
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));

            _shouldHandleExceptionPredicates = shouldHandleExceptionPredicates ?? PredicateHelper.EmptyExceptionPredicates;
            _shouldHandleResultPredicates = shouldHandleResultPredicates ?? PredicateHelper<TResult>.EmptyResultPredicates;
            _onFallback = onFallback ?? throw new ArgumentNullException(nameof(onFallback));
            _fallbackAction = fallbackAction ?? throw new ArgumentNullException(nameof(fallbackAction));
        }

        public TResult Execute<TExecutable>(in TExecutable action, Context context, in CancellationToken cancellationToken) where TExecutable : ISyncPollyExecutable<TResult>
        {
            DelegateResult<TResult> delegateOutcome;

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                TResult result = action.Execute(context, cancellationToken);

                if (!_shouldHandleResultPredicates.Any(predicate => predicate(result)))
                {
                    return result;
                }

                delegateOutcome = new DelegateResult<TResult>(result);
            }
            catch (Exception ex)
            {
                Exception handledException = _shouldHandleExceptionPredicates
                    .Select(predicate => predicate(ex))
                    .FirstOrDefault(e => e != null);
                if (handledException == null)
                {
                    throw;
                }

                delegateOutcome = new DelegateResult<TResult>(handledException);
            }

            _onFallback(delegateOutcome, context);

            return _fallbackAction(delegateOutcome, context, cancellationToken);
        }
    }
}
