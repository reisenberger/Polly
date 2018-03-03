using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Polly.Execution;

namespace Polly
{
    public abstract partial class Policy : ISyncPolicy
    {
        #region Execute overloads

        /// <summary>
        /// Executes the specified action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        [DebuggerStepThrough]
        public void Execute(Action action)
        {
            ExecuteSyncExecutableThroughPolicy(new SyncPollyExecutableActionNoParams(action), new Context(), DefaultCancellationToken);
        }

        /// <summary>
        /// Executes the specified action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        [DebuggerStepThrough]
        public void Execute(Action action, IDictionary<string, object> contextData)
        {
            ExecuteSyncExecutableThroughPolicy(new SyncPollyExecutableActionNoParams(action), new Context(contextData), DefaultCancellationToken);
        }

        /// <summary>
        /// Executes the specified action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        [DebuggerStepThrough]
        public void Execute(Action action, Context context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            ExecuteSyncExecutableThroughPolicy(new SyncPollyExecutableActionNoParams(action), context, DefaultCancellationToken);
        }

        /// <summary>
        /// Executes the specified action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        [DebuggerStepThrough]
        public void Execute(Action<Context> action, IDictionary<string, object> contextData)
        {
            ExecuteSyncExecutableThroughPolicy(new SyncPollyExecutableActionOnContext(action), new Context(contextData), DefaultCancellationToken);
        }

        /// <summary>
        /// Executes the specified action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        [DebuggerStepThrough]
        public void Execute(Action<Context> action, Context context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            ExecuteSyncExecutableThroughPolicy(new SyncPollyExecutableActionOnContext(action), context, DefaultCancellationToken);
        }

        /// <summary>
        /// Executes the specified action within the policy
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cancellationToken"></param>
        [DebuggerStepThrough]
        public void Execute(Action<CancellationToken> action, CancellationToken cancellationToken)
        {
            ExecuteSyncExecutableThroughPolicy(new SyncPollyExecutableActionOnCancellationToken(action), new Context(), cancellationToken);
        }

        /// <summary>
        /// Executes the specified action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        [DebuggerStepThrough]
        public void Execute(Action<CancellationToken> action, IDictionary<string, object> contextData, CancellationToken cancellationToken)
        {
            ExecuteSyncExecutableThroughPolicy(new SyncPollyExecutableActionOnCancellationToken(action), new Context(contextData), cancellationToken);
        }

        /// <summary>
        /// Executes the specified action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        [DebuggerStepThrough]
        public void Execute(Action<CancellationToken> action, Context context, CancellationToken cancellationToken)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            ExecuteSyncExecutableThroughPolicy(new SyncPollyExecutableActionOnCancellationToken(action), context, cancellationToken);
        }

        /// <summary>
        /// Executes the specified action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        [DebuggerStepThrough]
        public void Execute(Action<Context, CancellationToken> action, IDictionary<string, object> contextData, CancellationToken cancellationToken)
        {
            ExecuteSyncExecutableThroughPolicy(new SyncPollyExecutableAction(action), new Context(contextData), cancellationToken);
        }

        /// <summary>
        /// Executes the specified action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        [DebuggerStepThrough]
        public void Execute(Action<Context, CancellationToken> action, Context context, CancellationToken cancellationToken)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            ExecuteSyncExecutableThroughPolicy(new SyncPollyExecutableAction(action), context, cancellationToken);
        }

        #region Overloads method-generic in TResult

        /// <summary>
        /// Executes the specified action within the policy and returns the Result.
        /// </summary>
        /// <typeparam name="TResult">The type of the Result.</typeparam>
        /// <param name="func">The action to perform.</param>
        /// <returns>The value returned by the action</returns>
        [DebuggerStepThrough]
        public TResult Execute<TResult>(Func<TResult> func)
        {
            return ExecuteSyncExecutableThroughPolicy<SyncPollyExecutableFuncNoParams<TResult>, TResult>(new SyncPollyExecutableFuncNoParams<TResult>(func), new Context(), DefaultCancellationToken);
        }

        /// <summary>
        /// Executes the specified action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        /// <returns>
        /// The value returned by the action
        /// </returns>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        [DebuggerStepThrough]
        public TResult Execute<TResult>(Func<TResult> func, IDictionary<string, object> contextData)
        {
            return ExecuteSyncExecutableThroughPolicy<SyncPollyExecutableFuncNoParams<TResult>, TResult>(new SyncPollyExecutableFuncNoParams<TResult>(func), new Context(contextData), DefaultCancellationToken);
        }

        /// <summary>
        /// Executes the specified action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        /// <returns>
        /// The value returned by the action
        /// </returns>
        /// <exception cref="System.ArgumentNullException">context</exception>
        [DebuggerStepThrough]
        public TResult Execute<TResult>(Func<TResult> func, Context context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return ExecuteSyncExecutableThroughPolicy<SyncPollyExecutableFuncNoParams<TResult>, TResult>(new SyncPollyExecutableFuncNoParams<TResult>(func), context, DefaultCancellationToken);
        }

        /// <summary>
        /// Executes the specified action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        /// <returns>
        /// The value returned by the action
        /// </returns>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        [DebuggerStepThrough]
        public TResult Execute<TResult>(Func<Context, TResult> func, IDictionary<string, object> contextData)
        {
            return ExecuteSyncExecutableThroughPolicy<SyncPollyExecutableFuncOnContext<TResult>, TResult>(new SyncPollyExecutableFuncOnContext<TResult>(func), new Context(contextData), DefaultCancellationToken);
        }

        /// <summary>
        /// Executes the specified action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        /// <returns>
        /// The value returned by the action
        /// </returns>
        /// <exception cref="System.ArgumentNullException">context</exception>
        [DebuggerStepThrough]
        public TResult Execute<TResult>(Func<Context, TResult> func, Context context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return ExecuteSyncExecutableThroughPolicy<SyncPollyExecutableFuncOnContext<TResult>, TResult>(new SyncPollyExecutableFuncOnContext<TResult>(func), context, DefaultCancellationToken);
        }

        /// <summary>
        /// Executes the specified action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The action to perform.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The value returned by the action</returns>
        [DebuggerStepThrough]
        public TResult Execute<TResult>(Func<CancellationToken, TResult> func, CancellationToken cancellationToken)
        {
            return ExecuteSyncExecutableThroughPolicy<SyncPollyExecutableFuncOnCancellationToken<TResult>, TResult>(new SyncPollyExecutableFuncOnCancellationToken<TResult>(func), new Context(), cancellationToken);
        }

        /// <summary>
        /// Executes the specified action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The value returned by the action</returns>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        [DebuggerStepThrough]
        public TResult Execute<TResult>(Func<CancellationToken, TResult> func, IDictionary<string, object> contextData, CancellationToken cancellationToken)
        {
            return ExecuteSyncExecutableThroughPolicy<SyncPollyExecutableFuncOnCancellationToken<TResult>, TResult>(new SyncPollyExecutableFuncOnCancellationToken<TResult>(func), new Context(contextData), cancellationToken);
        }

        /// <summary>
        /// Executes the specified action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The value returned by the action</returns>
        /// <exception cref="System.ArgumentNullException">context</exception>
        [DebuggerStepThrough]
        public TResult Execute<TResult>(Func<CancellationToken, TResult> func, Context context, CancellationToken cancellationToken)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return ExecuteSyncExecutableThroughPolicy<SyncPollyExecutableFuncOnCancellationToken<TResult>, TResult>(new SyncPollyExecutableFuncOnCancellationToken<TResult>(func), context, cancellationToken);
        }

        /// <summary>
        /// Executes the specified action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The value returned by the action</returns>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        [DebuggerStepThrough]
        public TResult Execute<TResult>(Func<Context, CancellationToken, TResult> func, IDictionary<string, object> contextData, CancellationToken cancellationToken)
        {
            return ExecuteSyncExecutableThroughPolicy<SyncPollyExecutableFunc<TResult>, TResult>(new SyncPollyExecutableFunc<TResult>(func), new Context(contextData), cancellationToken);
        }

        /// <summary>
        /// Executes the specified function within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function to execute.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="System.ArgumentNullException">context</exception>
        /// <returns>The value returned by the action</returns>
        [DebuggerStepThrough]
        public TResult Execute<TResult>(Func<Context, CancellationToken, TResult> func, Context context, CancellationToken cancellationToken)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return ExecuteSyncExecutableThroughPolicy<SyncPollyExecutableFunc<TResult>, TResult>(new SyncPollyExecutableFunc<TResult>(func), context, cancellationToken);
        }

        #endregion

        #endregion

        #region ExecuteAndCapture overloads

        /// <summary>
        /// Executes the specified action within the policy and returns the captured result
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public PolicyResult ExecuteAndCapture(Action action)
        {
            return ExecuteAndCapture(action, new Context());
        }

        /// <summary>
        /// Executes the specified action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public PolicyResult ExecuteAndCapture(Action action, IDictionary<string, object> contextData)
        {
            return ExecuteAndCapture(action, new Context(contextData));
        }

        /// <summary>
        /// Executes the specified action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public PolicyResult ExecuteAndCapture(Action action, Context context)
        {
            if (_nonGenericSyncImplementation == null) throw NotConfiguredForSyncExecution();
            if (context == null) throw new ArgumentNullException(nameof(context));

            try
            {
                Execute(action, context);
                return PolicyResult.Successful(context);
            }
            catch (Exception exception)
            {
                return PolicyResult.Failure(exception, GetExceptionType(ExceptionPredicates, exception), context);
            }
        }

        /// <summary>
        /// Executes the specified action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public PolicyResult ExecuteAndCapture(Action<Context> action, IDictionary<string, object> contextData)
        {
            return ExecuteAndCapture(action, new Context(contextData));
        }

        /// <summary>
        /// Executes the specified action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public PolicyResult ExecuteAndCapture(Action<Context> action, Context context)
        {
            if (_nonGenericSyncImplementation == null) throw NotConfiguredForSyncExecution();
            if (context == null) throw new ArgumentNullException(nameof(context));

            try
            {
                Execute(action, context);
                return PolicyResult.Successful(context);
            }
            catch (Exception exception)
            {
                return PolicyResult.Failure(exception, GetExceptionType(ExceptionPredicates, exception), context);
            }
        }

        /// <summary>
        /// Executes the specified action within the policy and returns the captured result
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public PolicyResult ExecuteAndCapture(Action<CancellationToken> action, CancellationToken cancellationToken)
        {
            return ExecuteAndCapture(action, new Context(), cancellationToken);
        }

        /// <summary>
        /// Executes the specified action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The captured result</returns>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        [DebuggerStepThrough]
        public PolicyResult ExecuteAndCapture(Action<CancellationToken> action, IDictionary<string, object> contextData, CancellationToken cancellationToken)
        {
            return ExecuteAndCapture(action, new Context(contextData), cancellationToken);
        }

        /// <summary>
        /// Executes the specified action within the policy and returns the captured result
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public PolicyResult ExecuteAndCapture(Action<CancellationToken> action, Context context, CancellationToken cancellationToken)
        {
            if (_nonGenericSyncImplementation == null) throw NotConfiguredForSyncExecution();
            if (context == null) throw new ArgumentNullException(nameof(context));

            try
            {
                Execute(action, context, cancellationToken);
                return PolicyResult.Successful(context);
            }
            catch (Exception exception)
            {
                return PolicyResult.Failure(exception, GetExceptionType(ExceptionPredicates, exception), context);
            }
        }

        /// <summary>
        /// Executes the specified action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The captured result</returns>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        [DebuggerStepThrough]
        public PolicyResult ExecuteAndCapture(Action<Context, CancellationToken> action, IDictionary<string, object> contextData, CancellationToken cancellationToken)
        {
            return ExecuteAndCapture(action, new Context(contextData), cancellationToken);
        }

        /// <summary>
        /// Executes the specified action within the policy and returns the captured result
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public PolicyResult ExecuteAndCapture(Action<Context, CancellationToken> action, Context context, CancellationToken cancellationToken)
        {
            if (_nonGenericSyncImplementation == null) throw NotConfiguredForSyncExecution();
            if (context == null) throw new ArgumentNullException(nameof(context));

            try
            {
                Execute(action, context, cancellationToken);
                return PolicyResult.Successful(context);
            }
            catch (Exception exception)
            {
                return PolicyResult.Failure(exception, GetExceptionType(ExceptionPredicates, exception), context);
            }
        }

        #region Overloads method-generic in TResult

        /// <summary>
        /// Executes the specified action within the policy and returns the captured result
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public PolicyResult<TResult> ExecuteAndCapture<TResult>(Func<TResult> action)
        {
            return ExecuteAndCapture(action, new Context());
        }

        /// <summary>
        /// Executes the specified action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public PolicyResult<TResult> ExecuteAndCapture<TResult>(Func<TResult> action, IDictionary<string, object> contextData)
        {
            return ExecuteAndCapture(action, new Context(contextData));
        }


        /// <summary>
        /// Executes the specified action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public PolicyResult<TResult> ExecuteAndCapture<TResult>(Func<TResult> action, Context context)
        {
            if (_nonGenericSyncImplementation == null) throw NotConfiguredForSyncExecution();
            if (context == null) throw new ArgumentNullException(nameof(context));

            try
            {
                return PolicyResult<TResult>.Successful(Execute(action, context), context);
            }
            catch (Exception exception)
            {
                return PolicyResult<TResult>.Failure(exception, GetExceptionType(ExceptionPredicates, exception), context);
            }
        }

        /// <summary>
        /// Executes the specified action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public PolicyResult<TResult> ExecuteAndCapture<TResult>(Func<Context, TResult> action, IDictionary<string, object> contextData)
        {
            return ExecuteAndCapture(action, new Context(contextData));
        }


        /// <summary>
        /// Executes the specified action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public PolicyResult<TResult> ExecuteAndCapture<TResult>(Func<Context, TResult> action, Context context)
        {
            if (_nonGenericSyncImplementation == null) throw NotConfiguredForSyncExecution();
            if (context == null) throw new ArgumentNullException(nameof(context));

            try
            {
                return PolicyResult<TResult>.Successful(Execute(action, context), context);
            }
            catch (Exception exception)
            {
                return PolicyResult<TResult>.Failure(exception, GetExceptionType(ExceptionPredicates, exception), context);
            }
        }

        /// <summary>
        /// Executes the specified action within the policy and returns the captured result
        /// </summary>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The captured result</returns>
        public PolicyResult<TResult> ExecuteAndCapture<TResult>(Func<CancellationToken, TResult> action, CancellationToken cancellationToken)
        {
            return ExecuteAndCapture(action, new Context(), cancellationToken);
        }


        /// <summary>
        /// Executes the specified action within the policy and returns the captured result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The captured result</returns>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        [DebuggerStepThrough]
        public PolicyResult<TResult> ExecuteAndCapture<TResult>(Func<CancellationToken, TResult> action, IDictionary<string, object> contextData, CancellationToken cancellationToken)
        {
            return ExecuteAndCapture(action, new Context(contextData), cancellationToken);
        }

        /// <summary>
        /// Executes the specified action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public PolicyResult<TResult> ExecuteAndCapture<TResult>(Func<CancellationToken, TResult> action, Context context, CancellationToken cancellationToken)
        {
            if (_nonGenericSyncImplementation == null) throw NotConfiguredForSyncExecution();
            if (context == null) throw new ArgumentNullException(nameof(context));

            try
            {
                return PolicyResult<TResult>.Successful(Execute(action, context, cancellationToken), context);
            }
            catch (Exception exception)
            {
                return PolicyResult<TResult>.Failure(exception, GetExceptionType(ExceptionPredicates, exception), context);
            }
        }

        /// <summary>
        /// Executes the specified action within the policy and returns the captured result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The captured result</returns>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        [DebuggerStepThrough]
        public PolicyResult<TResult> ExecuteAndCapture<TResult>(Func<Context, CancellationToken, TResult> action, IDictionary<string, object> contextData, CancellationToken cancellationToken)
        {
            return ExecuteAndCapture(action, new Context(contextData), cancellationToken);
        }

        /// <summary>
        /// Executes the specified action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public PolicyResult<TResult> ExecuteAndCapture<TResult>(Func<Context, CancellationToken, TResult> action, Context context, CancellationToken cancellationToken)
        {
            if (_nonGenericSyncImplementation == null) throw NotConfiguredForSyncExecution();
            if (context == null) throw new ArgumentNullException(nameof(context));

            try
            {
                return PolicyResult<TResult>.Successful(Execute(action, context, cancellationToken), context);
            }
            catch (Exception exception)
            {
                return PolicyResult<TResult>.Failure(exception, GetExceptionType(ExceptionPredicates, exception), context);
            }
        }

        #endregion

        internal static ExceptionType GetExceptionType(IEnumerable<ExceptionPredicate> exceptionPredicates, Exception exception)
        {
            bool isExceptionTypeHandledByThisPolicy = exceptionPredicates.Any(predicate => predicate(exception) != null);

            return isExceptionTypeHandledByThisPolicy
                ? ExceptionType.HandledByThisPolicy
                : ExceptionType.Unhandled;
        }

        #endregion

    }
}
