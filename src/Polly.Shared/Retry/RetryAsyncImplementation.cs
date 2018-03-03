using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Polly.Execution;
using Polly.Utilities;

#if NET40
using ExceptionDispatchInfo = Polly.Utilities.ExceptionDispatchInfo;
#endif

namespace Polly.Retry
{
    internal class RetryAsyncImplementation<TResult> : IAsyncPolicyImplementation<TResult>
    {
        private IsPolicy _policy;
        private IEnumerable<ExceptionPredicate> _shouldRetryExceptionPredicates;
        private IEnumerable<ResultPredicate<TResult>> _shouldRetryResultPredicates;
        private Func<DelegateResult<TResult>, TimeSpan, int, Context, Task> _onRetryAsync;
        private Func<IRetryPolicyState<TResult>> _policyStateFactory;

        internal RetryAsyncImplementation(
            IsPolicy policy,
            IEnumerable<ExceptionPredicate> shouldRetryExceptionPredicates,
            IEnumerable<ResultPredicate<TResult>> shouldRetryResultPredicates,
            Func<DelegateResult<TResult>, TimeSpan, int, Context, Task> onRetryAsync,
            Func<IRetryPolicyState<TResult>> policyStateFactory
            )
        {
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
            _shouldRetryExceptionPredicates = shouldRetryExceptionPredicates ?? PredicateHelper.EmptyExceptionPredicates;
            _shouldRetryResultPredicates = shouldRetryResultPredicates ?? PredicateHelper<TResult>.EmptyResultPredicates;
            _onRetryAsync = onRetryAsync ?? throw new ArgumentNullException(nameof(onRetryAsync));
            _policyStateFactory = policyStateFactory ?? throw new ArgumentNullException(nameof(policyStateFactory));
        }

        public async Task<TResult> ExecuteAsync<TExecutable>(TExecutable action, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext) where TExecutable : IAsyncPollyExecutable<TResult>
        {
            int failureCount = 0;
            IRetryPolicyState<TResult> policyState = null; // To optimise the hotpath, we avoid allocating policyState unless the first try fails.

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                DelegateResult<TResult> delegateOutcome;

                try
                {
                    TResult result = await action.ExecuteAsync(context, cancellationToken, continueOnCapturedContext).ConfigureAwait(continueOnCapturedContext);

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
                await _onRetryAsync(delegateOutcome, waitDuration, failureCount, context).ConfigureAwait(continueOnCapturedContext);
                if (waitDuration > TimeSpan.Zero) await SystemClock.SleepAsync(waitDuration, cancellationToken).ConfigureAwait(continueOnCapturedContext);
            }
        }
    }
}
