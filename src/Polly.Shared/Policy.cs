using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Polly.Execution;
using Polly.Utilities;

namespace Polly
{
    /// <summary>
    /// Transient exception handling policies that can
    /// be applied to delegates
    /// </summary>
    public abstract partial class Policy : PolicyBase
    {
        internal readonly ISyncPolicyImplementation<Object> _nonGenericSyncImplementation;

        /// <summary>
        /// Constructs a new instance of a derived <see cref="Policy"/> type, using the passed <paramref name="policyBuilder"/>, and the passed <paramref name="implementationFactory"/> to generate implementations for executions for different return types.
        /// </summary>
        /// <param name="policyBuilder">The policy builder holding configuration information for the policy.</param>
        /// <param name="implementationFactory">A factory for providing synchronous implementations for the given non-generic policy</param>
        protected Policy(PolicyBuilder policyBuilder, Func<ISyncPolicy, ISyncPolicyImplementation<object>> implementationFactory)
        {
            if (policyBuilder == null) throw new ArgumentNullException(nameof(policyBuilder));
            ExceptionPredicates = policyBuilder.ExceptionPredicates ?? PredicateHelper.EmptyExceptionPredicates;

            if (implementationFactory == null) throw new ArgumentNullException(nameof(implementationFactory));
            _nonGenericSyncImplementation = implementationFactory(this) ?? throw new ArgumentOutOfRangeException(nameof(implementationFactory), $"{nameof(implementationFactory)} returned a null implementation.");
        }

        /// <summary>
        /// Executes the original delegate supplied for execution (now <paramref name="executable"/> in the form of a ISyncPollyExecutable) through the implementation configured on this policy.
        /// <remarks>Override this method if you need to prevent or modify standard execution through the implementation configured on the policy.</remarks>
        /// </summary>
        /// <typeparam name="TExecutable">The type of the executable</typeparam>
        /// <param name="executable">The original delegate supplied for execution, now as a ISyncPollyExecutable</param>
        /// <param name="context">The execution context.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> governing cancellation of the execution.</param>
        [DebuggerStepThrough]
        protected virtual void ExecuteThroughImplementationInternal<TExecutable>(in TExecutable executable, Context context, in CancellationToken cancellationToken) where TExecutable : ISyncPollyExecutable<object>
        {
            // Public overloads should always call via ExecuteAsyncExecutableThroughPolicy(), to ensure that context is set on the execution.  Context is not set on the execution in this method, because custom policy types may override this method (and omit to set context).

            if (_nonGenericSyncImplementation == null) throw NotConfiguredForSyncExecution();

            _nonGenericSyncImplementation.Execute(executable, context, cancellationToken);
        }

        /// <summary>
        /// Executes the original delegate supplied for execution (now <paramref name="executable"/> in the form of a <see cref="ISyncPollyExecutable{TMethodGeneric}"/>), through the implementation configured on this policy.
        /// <remarks>Override this method if you need to prevent or modify standard execution through the implementation configured on the policy.</remarks>
        /// </summary>
        /// <typeparam name="TExecutable">The type of the executable</typeparam>
        /// <typeparam name="TMethodGeneric">The return type of the executable</typeparam>
        /// <param name="executable">The original delegate supplied for execution, now as a <see cref="ISyncPollyExecutable{TResult}"/></param>
        /// <param name="context">The execution context.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> governing cancellation of the execution.</param>
        /// <returns>A <typeparamref name="TMethodGeneric"/> return value.</returns>

        [DebuggerStepThrough]
        protected virtual TMethodGeneric ExecuteThroughImplementationInternal<TExecutable, TMethodGeneric>(in TExecutable executable, Context context, in CancellationToken cancellationToken) where TExecutable : ISyncPollyExecutable<TMethodGeneric>
        {
            // Public overloads should always call via ExecuteAsyncExecutableThroughPolicy(), to ensure that context is set on the execution.  Context is not set on the execution in this method, because custom policy types may override this method (and in doing so might omit to set context).

            if (_nonGenericSyncImplementation == null) throw NotConfiguredForSyncExecution();

            return (TMethodGeneric)_nonGenericSyncImplementation.Execute(new SyncPollyExecutableFunc<TExecutable, object>((ctx, ct, exec) => exec.Execute(ctx, ct), executable), context, cancellationToken);
        }

        [DebuggerStepThrough]
        internal virtual void ExecuteSyncExecutableThroughPolicy<TExecutable>(in TExecutable action, Context context, in CancellationToken cancellationToken) where TExecutable : ISyncPollyExecutable<object>
        {
            SetPolicyExecutionContext(context);

            ExecuteThroughImplementationInternal<TExecutable>(action, context, cancellationToken);
        }

        [DebuggerStepThrough]
        internal virtual TMethodGeneric ExecuteSyncExecutableThroughPolicy<TExecutable, TMethodGeneric>(in TExecutable func, Context context, in CancellationToken cancellationToken) where TExecutable : ISyncPollyExecutable<TMethodGeneric>
        {
            SetPolicyExecutionContext(context);

            return ExecuteThroughImplementationInternal<TExecutable, TMethodGeneric>(func, context, cancellationToken);
        }

    }
}