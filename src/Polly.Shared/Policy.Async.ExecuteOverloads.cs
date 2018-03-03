using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Polly.Execution;

namespace Polly
{
    public abstract partial class Policy : IAsyncPolicy
    {
        #region ExecuteAsync overloads

        /// <summary>
        ///     Executes the specified asynchronous action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        [DebuggerStepThrough]
        public Task ExecuteAsync(Func<Task> action)
        {
            return ExecuteAsyncExecutableThroughPolicy(new AsyncPollyExecutableActionMissingParams(action), new Context(), DefaultCancellationToken, DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        [DebuggerStepThrough]
        public Task ExecuteAsync(Func<Task> action, IDictionary<string, object> contextData)
        {
            return ExecuteAsyncExecutableThroughPolicy(new AsyncPollyExecutableActionMissingParams(action), new Context(contextData), DefaultCancellationToken, DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        [DebuggerStepThrough]
        public Task ExecuteAsync(Func<Task> action, Context context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return ExecuteAsyncExecutableThroughPolicy(new AsyncPollyExecutableActionMissingParams(action), context, DefaultCancellationToken, DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        [DebuggerStepThrough]
        public Task ExecuteAsync(Func<Context, Task> action, IDictionary<string, object> contextData)
        {
            return ExecuteAsyncExecutableThroughPolicy(new AsyncPollyExecutableActionMissingCancellationCaptureParams(action), new Context(contextData), DefaultCancellationToken, DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        [DebuggerStepThrough]
        public Task ExecuteAsync(Func<Context, Task> action, Context context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return ExecuteAsyncExecutableThroughPolicy(new AsyncPollyExecutableActionMissingCancellationCaptureParams(action), context, DefaultCancellationToken, DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        [DebuggerStepThrough]
        public Task ExecuteAsync(Func<Task> action, bool continueOnCapturedContext)
        {
            return ExecuteAsyncExecutableThroughPolicy(new AsyncPollyExecutableActionMissingParams(action), new Context(), DefaultCancellationToken, continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        [DebuggerStepThrough]
        public Task ExecuteAsync(Func<Task> action, IDictionary<string, object> contextData, bool continueOnCapturedContext)
        {
            return ExecuteAsyncExecutableThroughPolicy(new AsyncPollyExecutableActionMissingParams(action), new Context(contextData), DefaultCancellationToken, continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        [DebuggerStepThrough]
        public Task ExecuteAsync(Func<Task> action, Context context, bool continueOnCapturedContext)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return ExecuteAsyncExecutableThroughPolicy(new AsyncPollyExecutableActionMissingParams(action), context, DefaultCancellationToken, continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        [DebuggerStepThrough]
        public Task ExecuteAsync(Func<Context, Task> action, IDictionary<string, object> contextData, bool continueOnCapturedContext)
        {
            return ExecuteAsyncExecutableThroughPolicy(new AsyncPollyExecutableActionMissingCancellationCaptureParams(action), new Context(contextData), DefaultCancellationToken, continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        [DebuggerStepThrough]
        public Task ExecuteAsync(Func<Context, Task> action, Context context, bool continueOnCapturedContext)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return ExecuteAsyncExecutableThroughPolicy(new AsyncPollyExecutableActionMissingCancellationCaptureParams(action), context, DefaultCancellationToken, continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        [DebuggerStepThrough]
        public Task ExecuteAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken)
        {
            return ExecuteAsyncExecutableThroughPolicy(new AsyncPollyExecutableActionMissingContextCaptureParams(action), new Context(), cancellationToken, DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        [DebuggerStepThrough]
        public Task ExecuteAsync(Func<CancellationToken, Task> action, IDictionary<string, object> contextData, CancellationToken cancellationToken)
        {
            return ExecuteAsyncExecutableThroughPolicy(new AsyncPollyExecutableActionMissingContextCaptureParams(action), new Context(contextData), cancellationToken, DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        [DebuggerStepThrough]
        public Task ExecuteAsync(Func<CancellationToken, Task> action, Context context, CancellationToken cancellationToken)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return ExecuteAsyncExecutableThroughPolicy(new AsyncPollyExecutableActionMissingContextCaptureParams(action), context, cancellationToken, DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        [DebuggerStepThrough]
        public Task ExecuteAsync(Func<Context, CancellationToken, Task> action, IDictionary<string, object> contextData, CancellationToken cancellationToken)
        {
            return ExecuteAsyncExecutableThroughPolicy(new AsyncPollyExecutableActionMissingCaptureParam(action), new Context(contextData), cancellationToken, DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        [DebuggerStepThrough]
        public Task ExecuteAsync(Func<Context, CancellationToken, Task> action, Context context, CancellationToken cancellationToken)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return ExecuteAsyncExecutableThroughPolicy(new AsyncPollyExecutableActionMissingCaptureParam(action), context, cancellationToken, DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <exception cref="System.InvalidOperationException">This policy is not configured for asynchronous executions.</exception>
        [DebuggerStepThrough]
        public Task ExecuteAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            return ExecuteAsyncExecutableThroughPolicy(new AsyncPollyExecutableActionMissingContextCaptureParams(action), new Context(), cancellationToken, continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        [DebuggerStepThrough]
        public Task ExecuteAsync(Func<CancellationToken, Task> action, IDictionary<string, object> contextData, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            return ExecuteAsyncExecutableThroughPolicy(new AsyncPollyExecutableActionMissingContextCaptureParams(action), new Context(contextData), cancellationToken, continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <exception cref="System.InvalidOperationException">This policy is not configured for asynchronous executions.</exception>
        [DebuggerStepThrough]
        public Task ExecuteAsync(Func<CancellationToken, Task> action, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return ExecuteAsyncExecutableThroughPolicy(new AsyncPollyExecutableActionMissingContextCaptureParams(action), context, cancellationToken, continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        [DebuggerStepThrough]
        public Task ExecuteAsync(Func<Context, CancellationToken, Task> action, IDictionary<string, object> contextData, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            return ExecuteAsyncExecutableThroughPolicy(new AsyncPollyExecutableActionMissingCaptureParam(action), new Context(contextData), cancellationToken, continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <exception cref="System.InvalidOperationException">This policy is not configured for asynchronous executions.</exception>
        [DebuggerStepThrough]
        public Task ExecuteAsync(Func<Context, CancellationToken, Task> action, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return ExecuteAsyncExecutableThroughPolicy(new AsyncPollyExecutableActionMissingCaptureParam(action), context, cancellationToken, continueOnCapturedContext);
        }

        #region Overloads method-generic in TResult

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The action to perform.</param>
        /// <returns>The value returned by the action</returns>
        [DebuggerStepThrough]
        public Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> func)
        {
            return ExecuteAsyncExecutableThroughPolicy<AsyncPollyExecutableFuncMissingParams<TResult>, TResult>(
                new AsyncPollyExecutableFuncMissingParams<TResult>(func),
                new Context(),
                DefaultCancellationToken,
                DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="func">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <returns>The value returned by the action</returns>
        [DebuggerStepThrough]
        public Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> func, IDictionary<string, object> contextData)
        {
            return ExecuteAsyncExecutableThroughPolicy<AsyncPollyExecutableFuncMissingParams<TResult>, TResult>(
                new AsyncPollyExecutableFuncMissingParams<TResult>(func),
                new Context(contextData),
                DefaultCancellationToken,
                DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <returns>The value returned by the action</returns>
        [DebuggerStepThrough]
        public Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> func, Context context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return ExecuteAsyncExecutableThroughPolicy<AsyncPollyExecutableFuncMissingParams<TResult>, TResult>(
                new AsyncPollyExecutableFuncMissingParams<TResult>(func),
                context,
                DefaultCancellationToken,
                DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="func">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <returns>The value returned by the action</returns>
        [DebuggerStepThrough]
        public Task<TResult> ExecuteAsync<TResult>(Func<Context, Task<TResult>> func, IDictionary<string, object> contextData)
        {
            return ExecuteAsyncExecutableThroughPolicy<AsyncPollyExecutableFuncMissingCancellationCapture<TResult>, TResult>(
                new AsyncPollyExecutableFuncMissingCancellationCapture<TResult>(func),
                new Context(contextData),
                DefaultCancellationToken,
                DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <returns>The value returned by the action</returns>
        [DebuggerStepThrough]
        public Task<TResult> ExecuteAsync<TResult>(Func<Context, Task<TResult>> func, Context context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return ExecuteAsyncExecutableThroughPolicy<AsyncPollyExecutableFuncMissingCancellationCapture<TResult>, TResult>(
                new AsyncPollyExecutableFuncMissingCancellationCapture<TResult>(func),
                context,
                DefaultCancellationToken,
                DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The action to perform.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <returns>The value returned by the action</returns>
        [DebuggerStepThrough]
        public Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> func, bool continueOnCapturedContext)
        {
            return ExecuteAsyncExecutableThroughPolicy<AsyncPollyExecutableFuncMissingParams<TResult>, TResult>(
                new AsyncPollyExecutableFuncMissingParams<TResult>(func),
                new Context(),
                DefaultCancellationToken,
                continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="func">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <returns>The value returned by the action</returns>
        [DebuggerStepThrough]
        public Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> func, IDictionary<string, object> contextData, bool continueOnCapturedContext)
        {
            return ExecuteAsyncExecutableThroughPolicy<AsyncPollyExecutableFuncMissingParams<TResult>, TResult>(
                new AsyncPollyExecutableFuncMissingParams<TResult>(func),
                new Context(contextData),
                DefaultCancellationToken,
                continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <returns>The value returned by the action</returns>
        [DebuggerStepThrough]
        public Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> func, Context context, bool continueOnCapturedContext)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return ExecuteAsyncExecutableThroughPolicy<AsyncPollyExecutableFuncMissingParams<TResult>, TResult>(
                new AsyncPollyExecutableFuncMissingParams<TResult>(func),
                context,
                DefaultCancellationToken,
                continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="func">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <returns>The value returned by the action</returns>
        [DebuggerStepThrough]
        public Task<TResult> ExecuteAsync<TResult>(Func<Context, Task<TResult>> func, IDictionary<string, object> contextData, bool continueOnCapturedContext)
        {
            return ExecuteAsyncExecutableThroughPolicy<AsyncPollyExecutableFuncMissingCancellationCapture<TResult>, TResult>(
                new AsyncPollyExecutableFuncMissingCancellationCapture<TResult>(func),
                new Context(contextData),
                DefaultCancellationToken,
                continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <returns>The value returned by the action</returns>
        [DebuggerStepThrough]
        public Task<TResult> ExecuteAsync<TResult>(Func<Context, Task<TResult>> func, Context context, bool continueOnCapturedContext)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return ExecuteAsyncExecutableThroughPolicy<AsyncPollyExecutableFuncMissingCancellationCapture<TResult>, TResult>(
                new AsyncPollyExecutableFuncMissingCancellationCapture<TResult>(func),
                context,
                DefaultCancellationToken,
                continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The action to perform.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy is in use, also cancels any further retries.</param>
        /// <returns>The value returned by the action</returns>
        [DebuggerStepThrough]
        public Task<TResult> ExecuteAsync<TResult>(Func<CancellationToken, Task<TResult>> func, CancellationToken cancellationToken)
        {
            return ExecuteAsyncExecutableThroughPolicy<AsyncPollyExecutableFuncMissingContextCapture<TResult>, TResult>(
                new AsyncPollyExecutableFuncMissingContextCapture<TResult>(func),
                new Context(),
                cancellationToken,
                DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="func">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <returns>The value returned by the action</returns>
        [DebuggerStepThrough]
        public Task<TResult> ExecuteAsync<TResult>(Func<CancellationToken, Task<TResult>> func, IDictionary<string, object> contextData, CancellationToken cancellationToken)
        {
            return ExecuteAsyncExecutableThroughPolicy<AsyncPollyExecutableFuncMissingContextCapture<TResult>, TResult>(
                new AsyncPollyExecutableFuncMissingContextCapture<TResult>(func),
                new Context(contextData),
                cancellationToken,
                DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy is in use, also cancels any further retries.</param>
        /// <returns>The value returned by the action</returns>
        [DebuggerStepThrough]
        public Task<TResult> ExecuteAsync<TResult>(Func<CancellationToken, Task<TResult>> func, Context context, CancellationToken cancellationToken)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return ExecuteAsyncExecutableThroughPolicy<AsyncPollyExecutableFuncMissingContextCapture<TResult>, TResult>(
                new AsyncPollyExecutableFuncMissingContextCapture<TResult>(func),
                context,
                cancellationToken,
                DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="func">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <returns>The value returned by the action</returns>
        [DebuggerStepThrough]
        public Task<TResult> ExecuteAsync<TResult>(Func<Context, CancellationToken, Task<TResult>> func, IDictionary<string, object> contextData, CancellationToken cancellationToken)
        {
            return ExecuteAsyncExecutableThroughPolicy<AsyncPollyExecutableFuncMissingCapture<TResult>, TResult>(
                new AsyncPollyExecutableFuncMissingCapture<TResult>(func),
                new Context(contextData),
                cancellationToken,
                DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy is in use, also cancels any further retries.</param>
        /// <returns>The value returned by the action</returns>
        [DebuggerStepThrough]
        public Task<TResult> ExecuteAsync<TResult>(Func<Context, CancellationToken, Task<TResult>> func, Context context, CancellationToken cancellationToken)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return ExecuteAsyncExecutableThroughPolicy<AsyncPollyExecutableFuncMissingCapture<TResult>, TResult>(
                new AsyncPollyExecutableFuncMissingCapture<TResult>(func),
                context,
                cancellationToken,
                DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The action to perform.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy is in use, also cancels any further retries.</param>
        /// <returns>The value returned by the action</returns>
        /// <exception cref="System.InvalidOperationException">This policy is not configured for asynchronous executions.</exception>
        [DebuggerStepThrough]
        public Task<TResult> ExecuteAsync<TResult>(Func<CancellationToken, Task<TResult>> func, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            return ExecuteAsyncExecutableThroughPolicy<AsyncPollyExecutableFuncMissingContextCapture<TResult>, TResult>(
                new AsyncPollyExecutableFuncMissingContextCapture<TResult>(func),
                new Context(),
                cancellationToken,
                continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="func">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <returns>The value returned by the action</returns>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        [DebuggerStepThrough]
        public Task<TResult> ExecuteAsync<TResult>(Func<CancellationToken, Task<TResult>> func, IDictionary<string, object> contextData, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            return ExecuteAsyncExecutableThroughPolicy<AsyncPollyExecutableFuncMissingContextCapture<TResult>, TResult>(
                new AsyncPollyExecutableFuncMissingContextCapture<TResult>(func),
                new Context(contextData),
                cancellationToken,
                continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy is in use, also cancels any further retries.</param>
        /// <returns>The value returned by the action</returns>
        /// <exception cref="System.InvalidOperationException">This policy is not configured for asynchronous executions.</exception>
        [DebuggerStepThrough]
        public Task<TResult> ExecuteAsync<TResult>(Func<CancellationToken, Task<TResult>> func, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return ExecuteAsyncExecutableThroughPolicy<AsyncPollyExecutableFuncMissingContextCapture<TResult>, TResult>(
                new AsyncPollyExecutableFuncMissingContextCapture<TResult>(func),
                context,
                cancellationToken,
                continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <param name="func">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <returns>The value returned by the action</returns>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        [DebuggerStepThrough]
        public Task<TResult> ExecuteAsync<TResult>(Func<Context, CancellationToken, Task<TResult>> func, IDictionary<string, object> contextData, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            return ExecuteAsyncExecutableThroughPolicy<AsyncPollyExecutableFuncMissingCapture<TResult>, TResult>(
                new AsyncPollyExecutableFuncMissingCapture<TResult>(func),
                new Context(contextData),
                cancellationToken,
                continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous function within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function to execute.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy is in use, also cancels any further retries.</param>
        /// <returns>The value returned by the action</returns>
        /// <exception cref="System.InvalidOperationException">This policy is not configured for asynchronous executions.</exception>
        [DebuggerStepThrough]
        public Task<TResult> ExecuteAsync<TResult>(Func<Context, CancellationToken, Task<TResult>> func, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return ExecuteAsyncExecutableThroughPolicy<AsyncPollyExecutableFuncMissingCapture<TResult>, TResult>(
                new AsyncPollyExecutableFuncMissingCapture<TResult>(func),
                context,
                cancellationToken,
                continueOnCapturedContext);
        }

        #endregion

        #endregion

        #region ExecuteAndCaptureAsync overloads

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult> ExecuteAndCaptureAsync(Func<Task> action)
        {
            return ExecuteAndCaptureAsync(action, new Context(), DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult> ExecuteAndCaptureAsync(Func<Task> action, IDictionary<string, object> contextData)
        {
            return ExecuteAndCaptureAsync(action, new Context(contextData), DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult> ExecuteAndCaptureAsync(Func<Task> action, Context context)
        {
            return ExecuteAndCaptureAsync(action, context, DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult> ExecuteAndCaptureAsync(Func<Context, Task> action, IDictionary<string, object> contextData)
        {
            return ExecuteAndCaptureAsync(action, new Context(contextData), DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult> ExecuteAndCaptureAsync(Func<Context, Task> action, Context context)
        {
            return ExecuteAndCaptureAsync(action, context, DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult> ExecuteAndCaptureAsync(Func<Task> action, bool continueOnCapturedContext)
        {
            return ExecuteAndCaptureAsync(action, new Context(), continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult> ExecuteAndCaptureAsync(Func<Task> action, IDictionary<string, object> contextData, bool continueOnCapturedContext)
        {
            return ExecuteAndCaptureAsync(action, new Context(contextData), continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public async Task<PolicyResult> ExecuteAndCaptureAsync(Func<Task> action, Context context, bool continueOnCapturedContext)
        {
            if (_nonGenericAsyncImplementation == null) throw NotConfiguredForAsyncExecution();
            if (context == null) throw new ArgumentNullException(nameof(context));

            try
            {
                await ExecuteAsync(action, context, continueOnCapturedContext).ConfigureAwait(continueOnCapturedContext);
                return PolicyResult.Successful(context);
            }
            catch (Exception exception)
            {
                return PolicyResult.Failure(exception, GetExceptionType(ExceptionPredicates, exception), context);
            }
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult> ExecuteAndCaptureAsync(Func<Context, Task> action, IDictionary<string, object> contextData, bool continueOnCapturedContext)
        {
            return ExecuteAndCaptureAsync(action, new Context(contextData), continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public async Task<PolicyResult> ExecuteAndCaptureAsync(Func<Context, Task> action, Context context, bool continueOnCapturedContext)
        {
            if (_nonGenericAsyncImplementation == null) throw NotConfiguredForAsyncExecution();
            if (context == null) throw new ArgumentNullException(nameof(context));

            try
            {
                await ExecuteAsync(action, context, continueOnCapturedContext).ConfigureAwait(continueOnCapturedContext);
                return PolicyResult.Successful(context);
            }
            catch (Exception exception)
            {
                return PolicyResult.Failure(exception, GetExceptionType(ExceptionPredicates, exception), context);
            }
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        [DebuggerStepThrough]
        public Task<PolicyResult> ExecuteAndCaptureAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken)
        {
            return ExecuteAndCaptureAsync(action, new Context(), cancellationToken, DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult> ExecuteAndCaptureAsync(Func<CancellationToken, Task> action, IDictionary<string, object> contextData, CancellationToken cancellationToken)
        {
            return ExecuteAndCaptureAsync(action, new Context(contextData), cancellationToken, DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        [DebuggerStepThrough]
        public Task<PolicyResult> ExecuteAndCaptureAsync(Func<CancellationToken, Task> action, Context context, CancellationToken cancellationToken)
        {
            return ExecuteAndCaptureAsync(action, context, cancellationToken, DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult> ExecuteAndCaptureAsync(Func<Context, CancellationToken, Task> action, IDictionary<string, object> contextData, CancellationToken cancellationToken)
        {
            return ExecuteAndCaptureAsync(action, new Context(contextData), cancellationToken, DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        [DebuggerStepThrough]
        public Task<PolicyResult> ExecuteAndCaptureAsync(Func<Context, CancellationToken, Task> action, Context context, CancellationToken cancellationToken)
        {
            return ExecuteAndCaptureAsync(action, context, cancellationToken, DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <exception cref="System.InvalidOperationException">This policy is not configured for asynchronous executions.</exception>
        [DebuggerStepThrough]
        public Task<PolicyResult> ExecuteAndCaptureAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            return ExecuteAndCaptureAsync(action, new Context(), cancellationToken, continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult> ExecuteAndCaptureAsync(Func<CancellationToken, Task> action, IDictionary<string, object> contextData, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            return ExecuteAndCaptureAsync(action, new Context(contextData), cancellationToken, continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <exception cref="System.InvalidOperationException">This policy is not configured for asynchronous executions.</exception>
        [DebuggerStepThrough]
        public async Task<PolicyResult> ExecuteAndCaptureAsync(Func<CancellationToken, Task> action, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            if (_nonGenericAsyncImplementation == null) throw NotConfiguredForAsyncExecution();
            if (context == null) throw new ArgumentNullException(nameof(context));

            try
            {
                await ExecuteAsync(action, context, cancellationToken, continueOnCapturedContext).ConfigureAwait(continueOnCapturedContext);
                return PolicyResult.Successful(context);
            }
            catch (Exception exception)
            {
                return PolicyResult.Failure(exception, GetExceptionType(ExceptionPredicates, exception), context);
            }
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult> ExecuteAndCaptureAsync(Func<Context, CancellationToken, Task> action, IDictionary<string, object> contextData, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            return ExecuteAndCaptureAsync(action, new Context(contextData), cancellationToken,
                continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the captured result.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <exception cref="System.InvalidOperationException">This policy is not configured for asynchronous executions.</exception>
        [DebuggerStepThrough]
        public async Task<PolicyResult> ExecuteAndCaptureAsync(Func<Context, CancellationToken, Task> action, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            if (_nonGenericAsyncImplementation == null) throw NotConfiguredForAsyncExecution();
            if (context == null) throw new ArgumentNullException(nameof(context));

            try
            {
                await ExecuteAsync(action, context, cancellationToken, continueOnCapturedContext).ConfigureAwait(continueOnCapturedContext);
                return PolicyResult.Successful(context);
            }
            catch (Exception exception)
            {
                return PolicyResult.Failure(exception, GetExceptionType(ExceptionPredicates, exception), context);
            }
        }

        #region Overloads method-generic in TResult

        /// <summary>
        /// Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult<TResult>> ExecuteAndCaptureAsync<TResult>(Func<Task<TResult>> action)
        {
            return ExecuteAndCaptureAsync(action, new Context(), DefaultContinueOnCapturedContext);
        }


        /// <summary>
        /// Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult<TResult>> ExecuteAndCaptureAsync<TResult>(Func<Task<TResult>> action, IDictionary<string, object> contextData)
        {
            return ExecuteAndCaptureAsync(action, new Context(contextData), DefaultContinueOnCapturedContext);
        }

        /// <summary>
        /// Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult<TResult>> ExecuteAndCaptureAsync<TResult>(Func<Task<TResult>> action, Context context)
        {
            return ExecuteAndCaptureAsync(action, context, DefaultContinueOnCapturedContext);
        }

        /// <summary>
        /// Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult<TResult>> ExecuteAndCaptureAsync<TResult>(Func<Context, Task<TResult>> action, IDictionary<string, object> contextData)
        {
            return ExecuteAndCaptureAsync(action, new Context(contextData), DefaultContinueOnCapturedContext);
        }

        /// <summary>
        /// Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult<TResult>> ExecuteAndCaptureAsync<TResult>(Func<Context, Task<TResult>> action, Context context)
        {
            return ExecuteAndCaptureAsync(action, context, DefaultContinueOnCapturedContext);
        }

        /// <summary>
        /// Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult<TResult>> ExecuteAndCaptureAsync<TResult>(Func<Task<TResult>> action, bool continueOnCapturedContext)
        {
            return ExecuteAndCaptureAsync(action, new Context(), continueOnCapturedContext);
        }

        /// <summary>
        /// Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult<TResult>> ExecuteAndCaptureAsync<TResult>(Func<Task<TResult>> action, IDictionary<string, object> contextData, bool continueOnCapturedContext)
        {
            return ExecuteAndCaptureAsync(action, new Context(contextData), continueOnCapturedContext);
        }

        /// <summary>
        /// Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public async Task<PolicyResult<TResult>> ExecuteAndCaptureAsync<TResult>(Func<Task<TResult>> action, Context context, bool continueOnCapturedContext)
        {
            if (_nonGenericAsyncImplementation == null) throw NotConfiguredForAsyncExecution();
            if (context == null) throw new ArgumentNullException(nameof(context));

            try
            {
                return PolicyResult<TResult>.Successful(
                    await ExecuteAsync(action, context, continueOnCapturedContext).ConfigureAwait(continueOnCapturedContext), context);
            }
            catch (Exception exception)
            {
                return PolicyResult<TResult>.Failure(exception, GetExceptionType(ExceptionPredicates, exception), context);
            }
        }

        /// <summary>
        /// Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult<TResult>> ExecuteAndCaptureAsync<TResult>(Func<Context, Task<TResult>> action, IDictionary<string, object> contextData, bool continueOnCapturedContext)
        {
            return ExecuteAndCaptureAsync(action, new Context(contextData), continueOnCapturedContext);
        }

        /// <summary>
        /// Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public async Task<PolicyResult<TResult>> ExecuteAndCaptureAsync<TResult>(Func<Context, Task<TResult>> action, Context context, bool continueOnCapturedContext)
        {
            if (_nonGenericAsyncImplementation == null) throw NotConfiguredForAsyncExecution();
            if (context == null) throw new ArgumentNullException(nameof(context));

            try
            {
                return PolicyResult<TResult>.Successful(
                    await ExecuteAsync(action, context, continueOnCapturedContext).ConfigureAwait(continueOnCapturedContext), context);
            }
            catch (Exception exception)
            {
                return PolicyResult<TResult>.Failure(exception, GetExceptionType(ExceptionPredicates, exception), context);
            }
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult<TResult>> ExecuteAndCaptureAsync<TResult>(Func<CancellationToken, Task<TResult>> action, CancellationToken cancellationToken)
        {
            return ExecuteAndCaptureAsync(action, new Context(), cancellationToken, DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult<TResult>> ExecuteAndCaptureAsync<TResult>(Func<CancellationToken, Task<TResult>> action, IDictionary<string, object> contextData, CancellationToken cancellationToken)
        {
            return ExecuteAndCaptureAsync(action, new Context(contextData), cancellationToken, DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult<TResult>> ExecuteAndCaptureAsync<TResult>(Func<CancellationToken, Task<TResult>> action, Context context, CancellationToken cancellationToken)
        {
            return ExecuteAndCaptureAsync(action, context, cancellationToken, DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult<TResult>> ExecuteAndCaptureAsync<TResult>(Func<Context, CancellationToken, Task<TResult>> action, IDictionary<string, object> contextData, CancellationToken cancellationToken)
        {
            return ExecuteAndCaptureAsync(action, new Context(contextData), cancellationToken, DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <returns>The captured result</returns>
        [DebuggerStepThrough]
        public Task<PolicyResult<TResult>> ExecuteAndCaptureAsync<TResult>(Func<Context, CancellationToken, Task<TResult>> action, Context context, CancellationToken cancellationToken)
        {
            return ExecuteAndCaptureAsync(action, context, cancellationToken, DefaultContinueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <returns>The captured result</returns>
        /// <exception cref="System.InvalidOperationException">This policy is not configured for asynchronous executions.</exception>
        [DebuggerStepThrough]
        public Task<PolicyResult<TResult>> ExecuteAndCaptureAsync<TResult>(Func<CancellationToken, Task<TResult>> action, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            return ExecuteAndCaptureAsync(action, new Context(), cancellationToken, continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <returns>The captured result</returns>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        [DebuggerStepThrough]
        public Task<PolicyResult<TResult>> ExecuteAndCaptureAsync<TResult>(Func<CancellationToken, Task<TResult>> action, IDictionary<string, object> contextData, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            return ExecuteAndCaptureAsync(action, new Context(contextData), cancellationToken, continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <returns>The captured result</returns>
        /// <exception cref="System.InvalidOperationException">This policy is not configured for asynchronous executions.</exception>
        [DebuggerStepThrough]
        public async Task<PolicyResult<TResult>> ExecuteAndCaptureAsync<TResult>(Func<CancellationToken, Task<TResult>> action, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            if (_nonGenericAsyncImplementation == null) throw NotConfiguredForAsyncExecution();
            if (context == null) throw new ArgumentNullException(nameof(context));

            try
            {
                return PolicyResult<TResult>.Successful(
                    await ExecuteAsync(action, context, cancellationToken, continueOnCapturedContext).ConfigureAwait(continueOnCapturedContext), context);
            }
            catch (Exception exception)
            {
                return PolicyResult<TResult>.Failure(exception, GetExceptionType(ExceptionPredicates, exception), context);
            }
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <param name="contextData">Arbitrary data that is passed to the exception policy.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <returns>The captured result</returns>
        /// <exception cref="System.ArgumentNullException">contextData</exception>
        [DebuggerStepThrough]
        public Task<PolicyResult<TResult>> ExecuteAndCaptureAsync<TResult>(Func<Context, CancellationToken, Task<TResult>> action, IDictionary<string, object> contextData, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            return ExecuteAndCaptureAsync(action, new Context(contextData), cancellationToken, continueOnCapturedContext);
        }

        /// <summary>
        ///     Executes the specified asynchronous action within the policy and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <param name="context">Context data that is passed to the exception policy.</param>
        /// <param name="cancellationToken">A cancellation token which can be used to cancel the action.  When a retry policy in use, also cancels any further retries.</param>
        /// <param name="continueOnCapturedContext">Whether to continue on a captured synchronization context.</param>
        /// <returns>The captured result</returns>
        /// <exception cref="System.InvalidOperationException">This policy is not configured for asynchronous executions.</exception>
        [DebuggerStepThrough]
        public async Task<PolicyResult<TResult>> ExecuteAndCaptureAsync<TResult>(Func<Context, CancellationToken, Task<TResult>> action, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            if (_nonGenericAsyncImplementation == null) throw NotConfiguredForAsyncExecution();
            if (context == null) throw new ArgumentNullException(nameof(context));

            try
            {
                return PolicyResult<TResult>.Successful(
                    await ExecuteAsync(action, context, cancellationToken, continueOnCapturedContext).ConfigureAwait(continueOnCapturedContext), context);
            }
            catch (Exception exception)
            {
                return PolicyResult<TResult>.Failure(exception, GetExceptionType(ExceptionPredicates, exception), context);
            }
        }

        #endregion

        #endregion

    }
}
