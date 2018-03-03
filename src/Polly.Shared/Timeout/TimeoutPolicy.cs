using System;
using System.Threading;
using System.Threading.Tasks;
using Polly.Utilities;

namespace Polly.Timeout
{
    /// <summary>
    /// A timeout policy which can be applied to delegates.
    /// </summary>
    public partial class TimeoutPolicy : Policy, ITimeoutPolicy, ITimeoutPolicyInternal
    {
        private Func<Context, TimeSpan> _timeoutProvider;
        private TimeoutStrategy _timeoutStrategy;
        private Action<Context, TimeSpan, Task> _onTimeout;

        Func<Context, TimeSpan> ITimeoutPolicyInternal.TimeoutProvider => _timeoutProvider;
        TimeoutStrategy ITimeoutPolicyInternal.TimeoutStrategy => _timeoutStrategy;
        Action<Context, TimeSpan, Task> ITimeoutPolicyInternal.OnTimeout => _onTimeout;

        internal TimeoutPolicy(
            Func<Context, TimeSpan> timeoutProvider,
            TimeoutStrategy timeoutStrategy,
            Action<Context, TimeSpan, Task> onTimeout,
            Func<ISyncPolicy, ISyncPolicyImplementation<object>> factory
            ) : base(PolicyBuilder.Empty, factory)
        {
            _timeoutProvider = timeoutProvider;
            _timeoutStrategy = timeoutStrategy;
            _onTimeout = onTimeout;
        }
    }

    /// <summary>
    /// A timeout policy which can be applied to delegates returning a value of type <typeparamref name="TResult"/>.
    /// </summary>
    public partial class TimeoutPolicy<TResult> : Policy<TResult>, ITimeoutPolicy<TResult>, ITimeoutPolicyInternal
    {
        private Func<Context, TimeSpan> _timeoutProvider;
        private TimeoutStrategy _timeoutStrategy;
        private Action<Context, TimeSpan, Task> _onTimeout;

        Func<Context, TimeSpan> ITimeoutPolicyInternal.TimeoutProvider => _timeoutProvider;
        TimeoutStrategy ITimeoutPolicyInternal.TimeoutStrategy => _timeoutStrategy;
        Action<Context, TimeSpan, Task> ITimeoutPolicyInternal.OnTimeout => _onTimeout;

        internal TimeoutPolicy(
            Func<Context, TimeSpan> timeoutProvider,
            TimeoutStrategy timeoutStrategy,
            Action<Context, TimeSpan, Task> onTimeout,
            Func<ISyncPolicy<TResult>, ISyncPolicyImplementation<TResult>> factory
            ) : base(PolicyBuilder<TResult>.Empty, factory)
        {
            _timeoutProvider = timeoutProvider;
            _timeoutStrategy = timeoutStrategy;
            _onTimeout = onTimeout;
        }
    }
}