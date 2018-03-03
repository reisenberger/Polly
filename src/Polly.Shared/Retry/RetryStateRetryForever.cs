using System;

namespace Polly.Retry
{
    internal class RetryStateRetryForever<TResult> : IRetryPolicyState<TResult>
    {
        internal static RetryStateRetryForever<TResult> Instance = new RetryStateRetryForever<TResult>();

        public bool CanRetry(int failureCount) => true;

        public TimeSpan GetWaitDuration(DelegateResult<TResult> delegateResult, int failureCount, Context context) => TimeSpan.Zero;
    }
}