using System;
using System.Threading;
using Polly.Execution;

namespace Polly.Wrap
{
    /// <summary>
    /// A policy that allows two (and by recursion more) Polly policies to wrap executions of delegates.
    /// </summary>
    public partial class PolicyWrap : Policy, IPolicyWrap
    {
        private IsPolicy _outer;
        private IsPolicy _inner;

        /// <summary>
        /// Returns the outer <see cref="IsPolicy"/> in this <see cref="IPolicyWrap"/>
        /// </summary>
        public IsPolicy Outer => _outer;

        /// <summary>
        /// Returns the next inner <see cref="IsPolicy"/> in this <see cref="IPolicyWrap"/>
        /// </summary>
        public IsPolicy Inner => _inner;

        internal PolicyWrap(Policy outer, ISyncPolicy inner, Func<ISyncPolicy, ISyncPolicyImplementation<Object>> factory) 
            : base(new PolicyBuilder(outer.ExceptionPredicates), factory)
        {
            _outer = outer;
            _inner = inner;
        }

        /// <summary>
        /// Override method-generic invocations .Execute{TMethodGeneric} on a non-generic PolicyWrap configured with non-generic Outer and Inner.  
        /// <remarks>When a method-generic execution is being made on non-generic PolicyWrap, we have to be sure to pass it through the strongly-typed method-generic .Execute{TMethodGeneric} overloads on the non-generic Outer and Inner policies, in order to pick up any overrides of those methods on the wrapped policies.  For instance, non-generic CachePolicy overrides its method-generic .Execute{TMethodGeneric} to ensure a strongly-typed CacheProvider is used.</remarks>
        /// <remarks>If we did not override this method, the default sync implementation in {Object} of the inner policies, would be used, missing some nuances of TResult-typed execution.</remarks>
        /// </summary>
        /// <typeparam name="TExecutable">The type of the executable</typeparam>
        /// <typeparam name="TMethodGeneric">The return type of the executable</typeparam>
        /// <param name="executable">The original delegate supplied for execution, now as a <see cref="ISyncPollyExecutable{TMethodGeneric}"/></param>
        /// <param name="context">The execution context.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> governing cancellation of the execution.</param>
        /// <returns>A <typeparamref name="TMethodGeneric"/> return value.</returns>
        protected override TMethodGeneric ExecuteThroughImplementationInternal<TExecutable, TMethodGeneric>(in TExecutable executable, Context context, in CancellationToken cancellationToken)
        {
            return new PolicyWrapSyncImplementationNonGenericNonGeneric<TMethodGeneric>(this, Outer, Inner as ISyncPolicy)
                .Execute(executable, context, cancellationToken);
        }
        
    }

    /// <summary>
    /// A policy that allows two (and by recursion more) Polly policies to wrap executions of delegates.
    /// </summary>
    /// <typeparam name="TResult">The return type of delegates which may be executed through the policy.</typeparam>
    public partial class PolicyWrap<TResult> : Policy<TResult>, IPolicyWrap<TResult>
    {
        /// <summary>
        /// Returns the outer <see cref="IsPolicy"/> in this <see cref="IPolicyWrap{TResult}"/>
        /// </summary>
        public IsPolicy Outer { get; private set; }

        /// <summary>
        /// Returns the next inner <see cref="IsPolicy"/> in this <see cref="IPolicyWrap{TResult}"/>
        /// </summary>
        public IsPolicy Inner { get; private set; }

        internal PolicyWrap(Policy outer, IsPolicy inner, Func<ISyncPolicy<TResult>, ISyncPolicyImplementation<TResult>> factory)
            : base(new PolicyBuilder<TResult>(outer.ExceptionPredicates), factory)
        {
            Outer = outer;
            Inner = inner;
        }

        internal PolicyWrap(Policy<TResult> outer, IsPolicy inner, Func<ISyncPolicy<TResult>, ISyncPolicyImplementation<TResult>> factory)
            : base(new PolicyBuilder<TResult>(outer.ExceptionPredicates, outer.ResultPredicates), factory)
        {
            Outer = outer;
            Inner = inner;
        }
    }
}
