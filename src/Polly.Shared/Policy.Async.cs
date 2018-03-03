using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Polly.Execution;
using Polly.Utilities;

namespace Polly
{
    public abstract partial class Policy : IAsyncPolicy
    {
        internal readonly IAsyncPolicyImplementation<object> _nonGenericAsyncImplementation;

        /// <summary>
        /// Constructs a new instance of a derived <see cref="Policy"/> type, using the passed <paramref name="policyBuilder"/>, and the passed <paramref name="implementationFactory"/> to generate implementations for executions for different return types.
        /// </summary>
        /// <param name="policyBuilder">The policy builder holding configuration information for the policy.</param>
        /// <param name="implementationFactory">A factory for providing synchronous implementations for the given non-generic policy</param>
        protected Policy(PolicyBuilder policyBuilder, Func<IAsyncPolicy, IAsyncPolicyImplementation<object>> implementationFactory)
        {
            if (policyBuilder == null) throw new ArgumentNullException(nameof(policyBuilder));
            ExceptionPredicates = policyBuilder.ExceptionPredicates ?? PredicateHelper.EmptyExceptionPredicates;

            if (implementationFactory == null) throw new ArgumentNullException(nameof(implementationFactory));
            _nonGenericAsyncImplementation = implementationFactory(this) ?? throw new ArgumentOutOfRangeException(nameof(implementationFactory), $"{nameof(implementationFactory)} returned a null implementation.");
        }

        /// <summary>
        /// Executes the original delegate supplied for execution (now <paramref name="executable"/> in the form of a IAsyncPollyExecutable) through the implementation configured on this policy.
        /// <remarks>Override this method if you need to prevent or modify standard execution through the implementation configured on the policy.</remarks>
        /// </summary>
        /// <typeparam name="TExecutable">The type of the executable</typeparam>
        /// <param name="executable">The original delegate supplied for execution, now as a IAsyncPollyExecutable</param>
        /// <param name="context">The execution context.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> governing cancellation of the execution.</param>
        /// <param name="continueOnCapturedContext">Whether async continuations should be on a captured synchronization context.</param>
        /// <returns>A <see cref="Task"/> promise of completion.</returns>
        [DebuggerStepThrough]
        protected virtual Task ExecuteAsyncThroughImplementationInternal<TExecutable>(TExecutable executable, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext) where TExecutable : IAsyncPollyExecutable<object>
        {
            // Public overloads should always call via ExecuteAsyncExecutableThroughPolicy(), to ensure that context is set on the execution.  Context is not set on the execution in this method, because custom policy types may override this method (and omit to set context).

            if (_nonGenericAsyncImplementation == null) throw NotConfiguredForAsyncExecution();

            return _nonGenericAsyncImplementation.ExecuteAsync(executable, context, cancellationToken, continueOnCapturedContext);
        }

        /// <summary>
        /// Executes the original delegate supplied for execution (now <paramref name="executable"/> in the form of a <see cref="IAsyncPollyExecutable{TMethodGeneric}"/>), through the implementation configured on this policy.
        /// <remarks>Override this method if you need to prevent or modify standard execution through the implementation configured on the policy.</remarks>
        /// </summary>
        /// <typeparam name="TExecutable">The type of the executable</typeparam>
        /// <typeparam name="TMethodGeneric">The return type of the executable</typeparam>
        /// <param name="executable">The original delegate supplied for execution, now as a <see cref="IAsyncPollyExecutable{TResult}"/></param>
        /// <param name="context">The execution context.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> governing cancellation of the execution.</param>
        /// <param name="continueOnCapturedContext">Whether async continuations should be on a captured synchronization context.</param>
        /// <returns>A <see cref="Task{TMethodGeneric}"/> promise of a <typeparamref name="TMethodGeneric"/> return value.</returns>
        [DebuggerStepThrough]
        protected virtual async Task<TMethodGeneric> ExecuteAsyncThroughImplementationInternal<TExecutable, TMethodGeneric>(TExecutable executable, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext) where TExecutable : IAsyncPollyExecutable<TMethodGeneric>
        {
            // Public overloads should always call via ExecuteAsyncExecutableThroughPolicy(), to ensure that context is set on the execution.  Context is not set on the execution in this method, because custom policy types may override this method (and omit to set context).

            if (_nonGenericAsyncImplementation == null) throw NotConfiguredForAsyncExecution();

            return (TMethodGeneric) 
                await _nonGenericAsyncImplementation
                .ExecuteAsync(new AsyncPollyExecutableFunc<object>(async (ctx, ct, capture) => await executable.ExecuteAsync(ctx, ct, capture).ConfigureAwait(continueOnCapturedContext)), context, cancellationToken, continueOnCapturedContext)
                .ConfigureAwait(continueOnCapturedContext);
        }

        [DebuggerStepThrough]
        internal virtual Task ExecuteAsyncExecutableThroughPolicy<TExecutable>(TExecutable action, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext) where TExecutable : IAsyncPollyExecutable<object>
        {
            SetPolicyExecutionContext(context);

            return ExecuteAsyncThroughImplementationInternal<TExecutable>(action, context, cancellationToken, continueOnCapturedContext);
        }

        [DebuggerStepThrough]
        internal virtual Task<TMethodGeneric> ExecuteAsyncExecutableThroughPolicy<TExecutable, TMethodGeneric>(TExecutable func, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext) where TExecutable : IAsyncPollyExecutable<TMethodGeneric>
        {
            SetPolicyExecutionContext(context);

            return ExecuteAsyncThroughImplementationInternal<TExecutable, TMethodGeneric>(func, context, cancellationToken, continueOnCapturedContext);
        }

    }
}
