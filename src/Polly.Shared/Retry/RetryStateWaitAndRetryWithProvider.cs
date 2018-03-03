using System;

namespace Polly.Retry
{
    internal class RetryStateWaitAndRetryWithProvider<TResult> : IRetryPolicyState<TResult>
    {
        private readonly int _numberOfRetriesPermitted;
        private readonly Func<int, DelegateResult<TResult>, Context, TimeSpan> _sleepDurationProvider;

        public RetryStateWaitAndRetryWithProvider(int numberOfRetriesPermitted, Func<int, DelegateResult<TResult>, Context, TimeSpan> sleepDurationProvider)
        {
            _numberOfRetriesPermitted = numberOfRetriesPermitted;
            _sleepDurationProvider = sleepDurationProvider;
        }

        public bool CanRetry(int failureCount) => failureCount <= _numberOfRetriesPermitted;

        public TimeSpan GetWaitDuration(DelegateResult<TResult> delegateResult, int failureCount, Context context) => _sleepDurationProvider(failureCount, delegateResult, context);
        
    }
}
