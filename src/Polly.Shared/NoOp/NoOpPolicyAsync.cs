using System;

namespace Polly.NoOp
{
    /// <summary>
    /// A no op policy that can be applied to delegates.
    /// </summary>
    public partial class NoOpPolicy : Policy, INoOpPolicy
    {
        internal NoOpPolicy(Func<IAsyncPolicy, IAsyncPolicyImplementation<object>> factory) : base(PolicyBuilder.Empty, factory)
        {
        }
    }

    public partial class NoOpPolicy<TResult> : INoOpPolicy<TResult>
    {
        internal NoOpPolicy(Func<IAsyncPolicy<TResult>, IAsyncPolicyImplementation<TResult>> factory) : base(PolicyBuilder<TResult>.Empty, factory)
        {
        }
    }
}
