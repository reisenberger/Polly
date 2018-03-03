using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Polly.Execution;
using Polly.Utilities;

namespace Polly
{
    public abstract partial class Policy<TResult> 
    {
        internal readonly IAsyncPolicyImplementation<TResult> _genericAsyncImplementation;

        /// <summary>
        /// Constructs a new instance of a derived <see cref="Policy"/> type, using the passed <paramref name="policyBuilder"/>, and the passed <paramref name="implementationFactory"/> to generate implementations for executions for different return types.
        /// </summary>
        /// <param name="policyBuilder">The policy builder holding configuration information for the policy.</param>
        /// <param name="implementationFactory">A factory for providing synchronous implementations for the given non-generic policy</param>
        protected Policy(PolicyBuilder<TResult> policyBuilder, Func<IAsyncPolicy<TResult>, IAsyncPolicyImplementation<TResult>> implementationFactory)
        {
            if (policyBuilder == null) throw new ArgumentNullException(nameof(policyBuilder));
            ExceptionPredicates = policyBuilder.ExceptionPredicates ?? PredicateHelper.EmptyExceptionPredicates;
            ResultPredicates = policyBuilder.ResultPredicates ?? PredicateHelper<TResult>.EmptyResultPredicates;

            if (implementationFactory == null) throw new ArgumentNullException(nameof(implementationFactory));
            _genericAsyncImplementation = implementationFactory(this) ?? throw new ArgumentOutOfRangeException(nameof(implementationFactory), $"{nameof(implementationFactory)} returned a null implementation.");
        }

        /// <summary>
        /// Executes the original delegate supplied for execution (now <paramref name="executable"/> in the form of a <see cref="IAsyncPollyExecutable{TResult}"/>), through the implementation configured on this policy.
        /// <remarks>Override this method if you need to prevent or modify standard execution through the implementation configured on the policy.</remarks>
        /// </summary>
        /// <typeparam name="TExecutable">The type of the executable</typeparam>
        /// <param name="executable">The original delegate supplied for execution, now as a <see cref="IAsyncPollyExecutable{TResult}"/></param>
        /// <param name="context">The execution context.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> governing cancellation of the execution.</param>
        /// <param name="continueOnCapturedContext">Whether async continuations should be on a captured synchronization context.</param>
        /// <returns>A <see cref="Task{TResult}"/> promise of a <typeparamref name="TResult"/> return value.</returns>
        [DebuggerStepThrough]
        protected virtual Task<TResult> ExecuteAsyncThroughImplementationInternal<TExecutable>(TExecutable executable, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext) where TExecutable : IAsyncPollyExecutable<TResult>
        {
            // Public overloads should always call via ExecuteAsyncExecutableThroughPolicy(), to ensure that context is set on the execution.  Context is not set on the execution in this method, because custom policy types may override this method (and omit to set context).

            if (_genericAsyncImplementation == null) throw NotConfiguredForAsyncExecution();

            return _genericAsyncImplementation.ExecuteAsync(executable, context, cancellationToken, continueOnCapturedContext);
        }

        [DebuggerStepThrough]
        internal virtual Task<TResult> ExecuteAsyncExecutableThroughPolicy<TExecutable>(TExecutable action, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext) where TExecutable : IAsyncPollyExecutable<TResult>
        {
            SetPolicyExecutionContext(context);

            return ExecuteAsyncThroughImplementationInternal(action, context, cancellationToken,
                continueOnCapturedContext);
        }
    }

}
