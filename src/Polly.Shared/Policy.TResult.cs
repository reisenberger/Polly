using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Polly.Execution;
using Polly.Utilities;

namespace Polly
{
    /// <summary>
    /// Transient fault handling policies that can be applied to delegates returning results of type <typeparamref name="TResult"/>
    /// </summary>
    public abstract partial class Policy<TResult> : PolicyBase
    {
        internal readonly ISyncPolicyImplementation<TResult> _genericImplementation;

        internal IEnumerable<ResultPredicate<TResult>> ResultPredicates { get; }

        /// <summary>
        /// Constructs a new instance of a derived <see cref="Policy"/> type, using the passed <paramref name="policyBuilder"/>, and the passed <paramref name="implementationFactory"/> to generate implementations for executions for different return types.
        /// </summary>
        /// <param name="policyBuilder">The policy builder holding configuration information for the policy.</param>
        /// <param name="implementationFactory">A factory for providing synchronous implementations for the given non-generic policy</param>
        protected Policy(PolicyBuilder<TResult> policyBuilder, Func<ISyncPolicy<TResult>, ISyncPolicyImplementation<TResult>> implementationFactory)
        {
            if (policyBuilder == null) throw new ArgumentNullException(nameof(policyBuilder));
            ExceptionPredicates = policyBuilder.ExceptionPredicates ?? PredicateHelper.EmptyExceptionPredicates;
            ResultPredicates = policyBuilder.ResultPredicates ?? PredicateHelper<TResult>.EmptyResultPredicates;

            if (implementationFactory == null) throw new ArgumentNullException(nameof(implementationFactory));
            _genericImplementation = implementationFactory(this) ?? throw new ArgumentOutOfRangeException(nameof(implementationFactory), $"{nameof(implementationFactory)} returned a null implementation.");
        }

        /// <summary>
        /// Executes the original delegate supplied for execution (now <paramref name="executable"/> in the form of a <see cref="ISyncPollyExecutable{TResult}"/>), through the implementation configured on this policy.
        /// <remarks>Override this method if you need to prevent or modify standard execution through the implementation configured on the policy.</remarks>
        /// </summary>
        /// <typeparam name="TExecutable">The type of the executable</typeparam>
        /// <param name="executable">The original delegate supplied for execution, now as a <see cref="ISyncPollyExecutable{TResult}"/></param>
        /// <param name="context">The execution context.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> governing cancellation of the execution.</param>
        /// <returns>A <typeparamref name="TResult"/> return value.</returns>
        [DebuggerStepThrough]
        protected virtual TResult ExecuteThroughImplementationInternal<TExecutable>(TExecutable executable, Context context, in CancellationToken cancellationToken) where TExecutable : ISyncPollyExecutable<TResult>
        {
            // Public overloads should always call via ExecuteSyncExecutableThroughPolicy(), to ensure that context is set on the execution.  Context is not set on the execution in this method, because custom policy types may override this method (and in doing so might omit to set context).

            if (_genericImplementation == null) throw NotConfiguredForSyncExecution();

            return _genericImplementation.Execute(executable, context, cancellationToken);
        }

        [DebuggerStepThrough]
        internal virtual TResult ExecuteSyncExecutableThroughPolicy<TExecutable>(in TExecutable action, Context context, in CancellationToken cancellationToken) where TExecutable : ISyncPollyExecutable<TResult>
        {
            SetPolicyExecutionContext(context);

            return ExecuteThroughImplementationInternal(action, context, cancellationToken);
        }
        
    }

}
