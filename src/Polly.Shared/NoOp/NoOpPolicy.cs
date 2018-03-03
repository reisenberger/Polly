using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Polly.Utilities;

namespace Polly.NoOp
{
    /// <summary>
    /// A no op policy that can be applied to delegates.
    /// </summary>
    public partial class NoOpPolicy : Policy, INoOpPolicy
    {
        internal NoOpPolicy(Func<ISyncPolicy, ISyncPolicyImplementation<object>> factory) : base(PolicyBuilder.Empty, factory)
        {
        }
    }

    /// <summary>
    /// A no op policy that can be applied to delegates returning a value of type <typeparamref name="TResult" />
    /// </summary>
    /// <typeparam name="TResult">The type of return values this policy will handle.</typeparam>
    public partial class NoOpPolicy<TResult> : Policy<TResult>, INoOpPolicy<TResult>
    {
        internal NoOpPolicy(Func<ISyncPolicy<TResult>, ISyncPolicyImplementation<TResult>> factory) : base(PolicyBuilder<TResult>.Empty, factory)
        {
        }
    }
}
