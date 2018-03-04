using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Polly.Fallback
{
    /// <summary>
    /// A fallback policy that can be applied to delegates.
    /// </summary>
    public partial class FallbackPolicy : Policy, IFallbackPolicy
    {
        internal FallbackPolicy(PolicyBuilder builder, Func<ISyncPolicy, ISyncPolicyImplementation<object>> factory)
            : base(builder, factory)
        { }

        /// <summary>
        /// Overrides method-generic executions through non-generic fallback policy, to throw.  
        /// See the exception thrown by <see cref="M:GenGenericExecuteMethodsAreAnInvalidOperationOnNonGenericFallbackPolicy()"/> for further explanation.
        /// </summary>
        protected override TMethodGeneric ExecuteThroughImplementationInternal<TExecutable, TMethodGeneric>(in TExecutable func, Context context, in CancellationToken cancellationToken)
        {
            throw GenericExecuteMethodsAreAnInvalidOperationOnNonGenericFallbackPolicy<TMethodGeneric>();
        }

        internal static Exception GenericExecuteMethodsAreAnInvalidOperationOnNonGenericFallbackPolicy<TResult>()
        {
            return new InvalidOperationException($"You have executed the generic .Execute<{nameof(TResult)}> method on a non-generic {nameof(FallbackPolicy)}.  A non-generic {nameof(FallbackPolicy)} only defines a fallback action which returns void; it can never return a substitute {nameof(TResult)} value.  To use {nameof(FallbackPolicy)} to provide fallback {nameof(TResult)} values you must define a generic fallback policy {nameof(FallbackPolicy)}<{nameof(TResult)}>.  For example, define the policy as Policy<{nameof(TResult)}>.Handle<Whatever>.Fallback<{nameof(TResult)}>(/* some {nameof(TResult)} value or Func<..., {nameof(TResult)}> */);");
        }
    }

    /// <summary>
    /// A fallback policy that can be applied to delegates returning a value of type <typeparamref name="TResult"/>.
    /// </summary>
    public partial class FallbackPolicy<TResult> : Policy<TResult>, IFallbackPolicy<TResult>
    {
        internal FallbackPolicy(PolicyBuilder<TResult> builder, Func<ISyncPolicy<TResult>, ISyncPolicyImplementation<TResult>> factory)
           : base(builder, factory)
        { }
    }
}