using System;
using System.Threading;
using System.Threading.Tasks;
using Polly.Execution;

namespace Polly.Wrap
{
    public partial class PolicyWrap : IPolicyWrap
    {
        internal PolicyWrap(Policy outer, IAsyncPolicy inner, Func<IAsyncPolicy, IAsyncPolicyImplementation<Object>> factory)
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
        /// <param name="executable">The original delegate supplied for execution, now as a <see cref="IAsyncPollyExecutable{TMethodGeneric}"/></param>
        /// <param name="context">The execution context.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> governing cancellation of the execution.</param>
        /// <param name="continueOnCapturedContext">Whether async executions should continue on a captured synchronisation context.</param>
        /// <returns>A <typeparamref name="TMethodGeneric"/> return value.</returns>
        protected override Task<TMethodGeneric> ExecuteAsyncThroughImplementationInternal<TExecutable, TMethodGeneric>(TExecutable executable, Context context,
            CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            return new PolicyWrapAsyncImplementationNonGenericNonGeneric<TMethodGeneric>(this, Outer, Inner as IAsyncPolicy)
                .ExecuteAsync(executable, context, cancellationToken, continueOnCapturedContext);
        }
        
    }

    public partial class PolicyWrap<TResult> : IPolicyWrap<TResult>
    {
        internal PolicyWrap(Policy outer, IsPolicy inner, Func<IAsyncPolicy<TResult>, IAsyncPolicyImplementation<TResult>> factory)
            : base(new PolicyBuilder<TResult>(outer.ExceptionPredicates), factory)
        {
            Outer = outer;
            Inner = inner;
        }

        internal PolicyWrap(Policy<TResult> outer, IsPolicy inner, Func<IAsyncPolicy<TResult>, IAsyncPolicyImplementation<TResult>> factory)
            : base(new PolicyBuilder<TResult>(outer.ExceptionPredicates, outer.ResultPredicates), factory)
        {
            Outer = outer;
            Inner = inner;
        }
    }
}
