using System;
using System.Collections.Generic;
using System.Threading;

namespace Polly.Retry
{
    /// <summary>
    /// A retry policy that can be applied to delegates.
    /// </summary>
    public partial class RetryPolicy : Policy, IRetryPolicy
    {
        internal RetryPolicy(PolicyBuilder builder, Func<ISyncPolicy, ISyncPolicyImplementation<object>> factory) 
            : base(builder, factory)
        {
        }
    }

    /// <summary>
    /// A retry policy that can be applied to delegates returning a value of type <typeparamref name="TResult"/>.
    /// </summary>
    public partial class RetryPolicy<TResult> : Policy<TResult>, IRetryPolicy<TResult>
    {
        internal RetryPolicy(PolicyBuilder<TResult> builder, Func<ISyncPolicy<TResult>, ISyncPolicyImplementation<TResult>> factory)
            : base(builder, factory)
        {
        }
    }
}