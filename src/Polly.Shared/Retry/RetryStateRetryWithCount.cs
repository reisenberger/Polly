using System;

namespace Polly.Retry
{
    internal class RetryStateRetryWithCount<TResult> : IRetryPolicyState<TResult>
    {
        private readonly int _numberOfRetriesPermitted;
        public RetryStateRetryWithCount(int numberOfRetriesPermitted)
        {
            _numberOfRetriesPermitted = numberOfRetriesPermitted;
        }

        public bool CanRetry(int failureCount) => failureCount <= _numberOfRetriesPermitted; 

        public TimeSpan GetWaitDuration(DelegateResult<TResult> delegateResult, int failureCount, Context context) => TimeSpan.Zero;
    }
}