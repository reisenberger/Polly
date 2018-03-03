using System;

namespace Polly.Retry
{
    internal interface IRetryPolicyState<TResult>
    {
        bool CanRetry(int failureCount);

        TimeSpan GetWaitDuration(DelegateResult<TResult> delegateResult, int failureCount, Context context);
    }
}