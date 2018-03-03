using System;
using System.Collections.Generic;

namespace Polly.Retry
{
    internal class RetryStateWaitAndRetry<TResult> : IRetryPolicyState<TResult>
    {
        private readonly IEnumerator<TimeSpan> _sleepDurationsEnumerator;

        public RetryStateWaitAndRetry(IEnumerable<TimeSpan> sleepDurations)
        {
            _sleepDurationsEnumerator = sleepDurations.GetEnumerator();
        }

        public bool CanRetry(int failureCount) => _sleepDurationsEnumerator.MoveNext();

        public TimeSpan GetWaitDuration(DelegateResult<TResult> delegateResult, int failureCount, Context context) => _sleepDurationsEnumerator.Current;

    }
}