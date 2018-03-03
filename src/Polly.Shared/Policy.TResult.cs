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
        #region TO REMOVE

        private readonly Func<Func<Context, CancellationToken, TResult>, Context, CancellationToken, TResult> _executionPolicy;

        /// <summary>
        /// Constructs a new instance of a derived <see cref="Policy"/> type with the passed <paramref name="executionPolicy"/>, <paramref name="exceptionPredicates"/> and <paramref name="resultPredicates"/> 
        /// </summary>
        /// <param name="executionPolicy">The execution policy that will be applied to delegates executed synchronously through the policy.</param>
        /// <param name="exceptionPredicates">Predicates indicating which exceptions the policy should handle. </param>
        /// <param name="resultPredicates">Predicates indicating which results the policy should handle. </param>
        protected Policy(
            Func<Func<Context, CancellationToken, TResult>, Context, CancellationToken, TResult> executionPolicy,
            IEnumerable<ExceptionPredicate> exceptionPredicates,
            IEnumerable<ResultPredicate<TResult>> resultPredicates
            )
        {
            _executionPolicy = executionPolicy ?? throw new ArgumentNullException(nameof(executionPolicy));
            ExceptionPredicates = exceptionPredicates ?? PredicateHelper.EmptyExceptionPredicates;
            ResultPredicates = resultPredicates ?? PredicateHelper<TResult>.EmptyResultPredicates;
        }

        #endregion

        private readonly ISyncPolicyImplementation<TResult> _genericImplementation;
        internal IEnumerable<ResultPredicate<TResult>> ResultPredicates { get; }

        /// <summary>
        /// Constructs a new instance of a derived <see cref="Policy"/> type, using the passed <paramref name="policyBuilder"/>, and the passed <paramref name="implementationFactory"/> to generate implementations for executions for different return types.
        /// </summary>
        /// <param name="policyBuilder">The policy builder holding configuration information for the policy.</param>
        /// <param name="implementationFactory">A factory for providing synchronous implementations for the given non-generic policy</param>
        protected Policy(PolicyBuilder<TResult> policyBuilder, ISyncPolicyImplementationFactory<TResult> implementationFactory)
        {
            if (policyBuilder == null) throw new ArgumentNullException(nameof(policyBuilder));
            ExceptionPredicates = policyBuilder.ExceptionPredicates ?? PredicateHelper.EmptyExceptionPredicates;
            ResultPredicates = policyBuilder.ResultPredicates ?? PredicateHelper<TResult>.EmptyResultPredicates;

            if (implementationFactory == null) throw new ArgumentNullException(nameof(implementationFactory));
            _genericImplementation = implementationFactory.GetImplementation(this) ?? throw new ArgumentOutOfRangeException(nameof(implementationFactory), $"{nameof(implementationFactory)} returned a null implementation.");
        }

        internal virtual TResult ExecuteInternal<TExecutable>(TExecutable action, Context context, CancellationToken cancellationToken) where TExecutable : ISyncPollyExecutable<TResult>
        {
            if (_genericImplementation == null) throw NotConfiguredForSyncExecution();

            return _genericImplementation.Execute(action, context, cancellationToken);
        }

        #region TO REMOVE - or keep adaptation

        /// <summary>
        /// Executes the specified action within the policy and returns the result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The value returned by the action</returns>
        [DebuggerStepThrough]
        protected internal TResult ExecuteInternal(Func<Context, CancellationToken, TResult> action, Context context, CancellationToken cancellationToken)
        {
            return ExecuteInternal(new SyncPollyExecutableFunc<TResult>(action), context, cancellationToken);

            //if (_executionPolicy == null) throw new InvalidOperationException("Please use the synchronous-defined policies when calling the synchronous Execute (and similar) methods.");

            //return _executionPolicy(action, context, cancellationToken);
        }

        #endregion
    }

}
