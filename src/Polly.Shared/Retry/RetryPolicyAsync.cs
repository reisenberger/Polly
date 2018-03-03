using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.Retry
{
    public partial class RetryPolicy : IRetryPolicy
    {
        internal RetryPolicy(PolicyBuilder builder, Func<IAsyncPolicy, IAsyncPolicyImplementation<object>> factory)
            : base(builder, factory)
        {
        }
    }

    public partial class RetryPolicy<TResult> : IRetryPolicy<TResult>
    {
        internal RetryPolicy(PolicyBuilder<TResult> builder, Func<IAsyncPolicy<TResult>, IAsyncPolicyImplementation<TResult>> factory)
            : base(builder, factory)
        {
        }
    }
}

