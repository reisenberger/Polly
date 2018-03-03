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
        #region TO REMOVE

        private readonly Action<Action<Context, CancellationToken>, Context, CancellationToken> _exceptionPolicy;

        /// <summary>
        /// Constructs a new instance of a derived <see cref="Policy"/> type with the passed <paramref name="exceptionPolicy"/> and <paramref name="exceptionPredicates"/> 
        /// </summary>
        /// <param name="exceptionPolicy">The execution policy that will be applied to delegates executed synchronously through the policy.</param>
        /// <param name="exceptionPredicates">Predicates indicating which exceptions the policy should handle. </param>
        protected Policy(
            Action<Action<Context, CancellationToken>, Context, CancellationToken> exceptionPolicy,
            IEnumerable<ExceptionPredicate> exceptionPredicates)
        {
            _exceptionPolicy = exceptionPolicy ?? throw new ArgumentNullException(nameof(exceptionPolicy));
            ExceptionPredicates = exceptionPredicates ?? PredicateHelper.EmptyExceptionPredicates;
        }

        # endregion

        private readonly ISyncPolicyImplementation<EmptyStruct> _voidSyncImplementation;
        private readonly ISyncPolicyImplementationFactory _implementationFactory;

        /// <summary>
        /// Constructs a new instance of a derived <see cref="Policy"/> type, using the passed <paramref name="policyBuilder"/>, and the passed <paramref name="implementationFactory"/> to generate implementations for executions for different return types.
        /// </summary>
        /// <param name="policyBuilder">The policy builder holding configuration information for the policy.</param>
        /// <param name="implementationFactory">A factory for providing synchronous implementations for the given non-generic policy</param>
        protected Policy(PolicyBuilder policyBuilder, ISyncPolicyImplementationFactory implementationFactory)
        {
            if (policyBuilder == null) throw new ArgumentNullException(nameof(policyBuilder));
            ExceptionPredicates = policyBuilder.ExceptionPredicates ?? PredicateHelper.EmptyExceptionPredicates;

            _implementationFactory = implementationFactory ?? throw new ArgumentNullException(nameof(implementationFactory));
            _voidSyncImplementation = implementationFactory.GetImplementation<EmptyStruct>(this) ?? throw new ArgumentOutOfRangeException(nameof(implementationFactory), $"{nameof(implementationFactory)} returned a null implementation.");
        }

        internal virtual void ExecuteInternal<TExecutable>(TExecutable action, Context context, CancellationToken cancellationToken) where TExecutable : ISyncPollyExecutable<EmptyStruct>
        {
            if (_voidSyncImplementation == null) throw NotConfiguredForSyncExecution();

            _voidSyncImplementation.Execute(action, context, cancellationToken);
        }

        internal virtual TMethodGeneric ExecuteInternal<TExecutable, TMethodGeneric>(TExecutable func, Context context, CancellationToken cancellationToken) where TExecutable : ISyncPollyExecutable<TMethodGeneric>
        {
            if (_implementationFactory == null) throw NotConfiguredForSyncExecution();

            return _implementationFactory.GetImplementation<TMethodGeneric>(this).Execute(func, context, cancellationToken);
        }
        
        #region TOREMOVE - or leave adapted

        /// <summary>
        /// Executes the specified action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        [DebuggerStepThrough]
        protected internal
            // virtual // THESE ARE NO LONGER THE ONES WE WANT TO LET PEOPLE OVERRIDE - because there will be several, not just action.  Want them to override PollyAction ones.
            void ExecuteInternal(Action<Context, CancellationToken> action, Context context, CancellationToken cancellationToken)
        {
            ExecuteInternal(new SyncPollyExecutableAction(action), context, cancellationToken);

            //if (_exceptionPolicy == null) throw new InvalidOperationException("Please use the synchronous-defined policies when calling the synchronous Execute (and similar) methods.");

            //_exceptionPolicy(action, context, cancellationToken);
        }

        /// <summary>
        /// Executes the specified action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The value returned by the action</returns>
        [DebuggerStepThrough]
        protected internal
            // virtual // THESE ARE NO LONGER THE ONES WE WANT TO LET PEOPLE OVERRIDE - because there will be several, not just action.  Want them to override PollyAction ones.
            TResult ExecuteInternal<TResult>(Func<Context, CancellationToken, TResult> action, Context context, CancellationToken cancellationToken)
        {
            return ExecuteInternal<SyncPollyExecutableFunc<TResult>, TResult>(new SyncPollyExecutableFunc<TResult>(action), context, cancellationToken);

            //if (_exceptionPolicy == null) throw new InvalidOperationException("Please use the synchronous-defined policies when calling the synchronous Execute (and similar) methods.");

            //var result = default(TResult);
            //_exceptionPolicy((ctx, ct) => { result = action(ctx, ct); }, context, cancellationToken);
            //return result;
        }

        #endregion

    }
}