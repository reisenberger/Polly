using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.Timeout
{
    public partial class TimeoutPolicy : ITimeoutPolicy
    {
        private Func<Context, TimeSpan, Task, Task> _onTimeoutAsync;
        Func<Context, TimeSpan, Task, Task> ITimeoutPolicyInternal.OnTimeoutAsync => _onTimeoutAsync;

        internal TimeoutPolicy(
            Func<Context, TimeSpan> timeoutProvider,
            TimeoutStrategy timeoutStrategy,
            Func<Context, TimeSpan, Task, Task> onTimeoutAsync,
            Func<IAsyncPolicy, IAsyncPolicyImplementation<object>> factory
            ) : base(PolicyBuilder.Empty, factory)

        {
            _timeoutProvider = timeoutProvider;
            _timeoutStrategy = timeoutStrategy;
            _onTimeoutAsync = onTimeoutAsync;
        }

    }

    public partial class TimeoutPolicy<TResult> : ITimeoutPolicy<TResult>
    {
        private Func<Context, TimeSpan, Task, Task> _onTimeoutAsync;
        Func<Context, TimeSpan, Task, Task> ITimeoutPolicyInternal.OnTimeoutAsync => _onTimeoutAsync;

        internal TimeoutPolicy(
            Func<Context, TimeSpan> timeoutProvider,
            TimeoutStrategy timeoutStrategy,
            Func<Context, TimeSpan, Task, Task> onTimeoutAsync,
            Func<IAsyncPolicy<TResult>, IAsyncPolicyImplementation<TResult>> factory
            ) : base(PolicyBuilder<TResult>.Empty, factory)
        {
            _timeoutProvider = timeoutProvider;
            _timeoutStrategy = timeoutStrategy;
            _onTimeoutAsync = onTimeoutAsync;
        }
    }
}