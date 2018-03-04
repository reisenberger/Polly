using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using Polly.Execution;
using Polly.Utilities;

namespace Polly.Retry
{
    internal class RetrySyncImplementation<TResult> : ISyncPolicyImplementation<TResult>
    {
        private IsPolicy _policy;
        private IEnumerable<ExceptionPredicate> _shouldRetryExceptionPredicates;
        private IEnumerable<ResultPredicate<TResult>> _shouldRetryResultPredicates;
        private Action<DelegateResult<TResult>, TimeSpan, int, Context> _onRetry;
        private Func<IRetryPolicyState<TResult>> _policyStateFactory;

        internal RetrySyncImplementation(
            IsPolicy policy,
            IEnumerable<ExceptionPredicate> shouldRetryExceptionPredicates,
            IEnumerable<ResultPredicate<TResult>> shouldRetryResultPredicates,
            Action<DelegateResult<TResult>, TimeSpan, int, Context> onRetry,
            Func<IRetryPolicyState<TResult>> policyStateFactory
            )
        {
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
            _shouldRetryExceptionPredicates = shouldRetryExceptionPredicates ?? PredicateHelper.EmptyExceptionPredicates;
            _shouldRetryResultPredicates = shouldRetryResultPredicates ?? PredicateHelper<TResult>.EmptyResultPredicates;
            _onRetry = onRetry ?? throw new ArgumentNullException(nameof(onRetry));
            _policyStateFactory = policyStateFactory ?? throw new ArgumentNullException(nameof(policyStateFactory));
        }

        public TResult Execute<TExecutable>(in TExecutable action, Context context, in CancellationToken cancellationToken) where TExecutable : ISyncPollyExecutable<TResult>
        {
            int failureCount = 0;
            IRetryPolicyState<TResult> policyState = null; // To optimise the hotpath, we avoid allocating policyState unless the first try fails.

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                DelegateResult<TResult> delegateOutcome;

                try
                {
                    TResult result = action.Execute(context, cancellationToken);

                    if (!_shouldRetryResultPredicates.Any(predicate => predicate(result)))
                    {
                        return result;
                    }

                    if (failureCount < int.MaxValue) { failureCount ++; }

                    policyState = policyState ?? _policyStateFactory();
                    if (!policyState.CanRetry(failureCount))
                    {
                        return result;
                    }

                    delegateOutcome = new DelegateResult<TResult>(result);
                }
                catch (Exception ex)
                {
                    Exception handledException = _shouldRetryExceptionPredicates
                        .Select(predicate => predicate(ex))
                        .FirstOrDefault(e => e != null);
                    if (handledException == null)
                    {
                        throw;
                    }

                    if (failureCount < int.MaxValue) { failureCount ++; }

                    policyState = policyState ?? _policyStateFactory();
                    if (!policyState.CanRetry(failureCount))
                    {
                        if (handledException != ex)
                        {
                            ExceptionDispatchInfo.Capture(handledException).Throw();
                        }
                        throw;
                    }

                    delegateOutcome = new DelegateResult<TResult>(handledException);
                }

                TimeSpan waitDuration = policyState.GetWaitDuration(delegateOutcome, failureCount, context);
                _onRetry(delegateOutcome, waitDuration, failureCount, context);
                if (waitDuration > TimeSpan.Zero) SystemClock.Sleep(waitDuration, cancellationToken);
            }
        }
    }
}
