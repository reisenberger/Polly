using System;

namespace Polly.Retry
{
    internal class RetryStateWaitAndRetryForever<TResult> : IRetryPolicyState<TResult>
    {
        private readonly Func<int, DelegateResult<TResult>, Context, TimeSpan> _sleepDurationProvider;

        public RetryStateWaitAndRetryForever(Func<int, DelegateResult<TResult>, Context, TimeSpan> sleepDurationProvider)
        {
            _sleepDurationProvider = sleepDurationProvider;
        }

        public bool CanRetry(int failureCount) => true;

        public TimeSpan GetWaitDuration(DelegateResult<TResult> delegateResult, int failureCount, Context context) => _sleepDurationProvider(failureCount, delegateResult, context);
    }
}
