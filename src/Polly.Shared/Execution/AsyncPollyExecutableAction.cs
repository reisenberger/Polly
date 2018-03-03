﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.Execution
{

    /// <inheritdoc/>
    internal struct AsyncPollyExecutableActionMissingParams : IAsyncPollyExecutable<object>
    {
        private readonly Func<Task> _action;

        /// <summary>
        /// Creates a <see cref="AsyncPollyExecutableActionMissingParams"/> struct for the passed asynchronous action, which may be executed through a policy at a later point in time.
        /// </summary>
        /// <param name="action">The action.</param>
        public AsyncPollyExecutableActionMissingParams(Func<Task> action)
        {
            _action = action;
        }

        /// <inheritdoc/>
        public async Task<object> ExecuteAsync(Context context, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            await _action().ConfigureAwait(continueOnCapturedContext);
            return null;
        }
    }

    /// <inheritdoc/>
    internal struct AsyncPollyExecutableActionMissingCancellationCaptureParams : IAsyncPollyExecutable<object>
    {
        private readonly Func<Context, Task> _action;

        /// <summary>
        /// Creates a <see cref="AsyncPollyExecutableActionMissingCancellationCaptureParams"/> struct for the passed asynchronous action, which may be executed through a policy at a later point in time.
        /// </summary>
        /// <param name="action">The action.</param>
        public AsyncPollyExecutableActionMissingCancellationCaptureParams(Func<Context, Task> action)
        {
            _action = action;
        }

        /// <inheritdoc/>
        public async Task<object> ExecuteAsync(Context context, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            await _action(context).ConfigureAwait(continueOnCapturedContext);
            return null;
        }
    }

    /// <inheritdoc/>
    internal struct AsyncPollyExecutableActionMissingContextCaptureParams : IAsyncPollyExecutable<object>
    {
        private readonly Func<CancellationToken, Task> _action;

        /// <summary>
        /// Creates a <see cref="AsyncPollyExecutableActionMissingContextCaptureParams"/> struct for the passed asynchronous action, which may be executed through a policy at a later point in time.
        /// </summary>
        /// <param name="action">The action.</param>
        public AsyncPollyExecutableActionMissingContextCaptureParams(Func<CancellationToken, Task> action)
        {
            _action = action;
        }

        /// <inheritdoc/>
        public async Task<object> ExecuteAsync(Context context, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            await _action(cancellationToken).ConfigureAwait(continueOnCapturedContext);
            return null;
        }
    }

    /// <inheritdoc/>
    internal struct AsyncPollyExecutableActionMissingCaptureParam : IAsyncPollyExecutable<object>
    {
        private readonly Func<Context, CancellationToken, Task> _action;

        /// <summary>
        /// Creates a <see cref="AsyncPollyExecutableActionMissingCaptureParam"/> struct for the passed asynchronous action, which may be executed through a policy at a later point in time.
        /// </summary>
        /// <param name="action">The action.</param>
        public AsyncPollyExecutableActionMissingCaptureParam(Func<Context, CancellationToken, Task> action)
        {
            _action = action;
        }

        /// <inheritdoc/>
        public async Task<object> ExecuteAsync(Context context, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            await _action(context, cancellationToken).ConfigureAwait(continueOnCapturedContext);
            return null;
        }
    }

    /// <inheritdoc/>
    internal struct AsyncPollyExecutableAction : IAsyncPollyExecutable<object>
    {
        private readonly Func<Context, CancellationToken, bool, Task> _action;

        /// <summary>
        /// Creates a <see cref="AsyncPollyExecutableAction"/> struct for the passed asynchronous action, which may be executed through a policy at a later point in time.
        /// </summary>
        /// <param name="action">The action.</param>
        public AsyncPollyExecutableAction(Func<Context, CancellationToken, bool, Task> action)
        {
            _action = action;
        }

        /// <inheritdoc/>
        public async Task<object> ExecuteAsync(Context context, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            await _action(context, cancellationToken, continueOnCapturedContext).ConfigureAwait(continueOnCapturedContext);
            return null;
        }
    }

    internal struct AsyncPollyExecutableAction<T1> : IAsyncPollyExecutable<object>
    {
        private readonly Func<Context, CancellationToken, bool, T1, Task> _action;
        private readonly T1 _arg1;

        /// <summary>
        /// Creates a <see cref="AsyncPollyExecutableAction"/> instance combining an action and an input parameter of type <typeparamref name="T1"/> to be used when executing it. 
        /// This action may be executed asynchronously through a policy at a later point in time.
        /// <remarks>Combining the action and its input parameter into a short-lived struct, as shown here, allows users to pass data to actions executed through Polly policies without using closures, which would result in extra heap allocations.</remarks>
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="arg1">The parameter to pass, when executing the action.</param>
        public AsyncPollyExecutableAction(Func<Context, CancellationToken, bool, T1, Task> action, T1 arg1)
        {
            _action = action;
            _arg1 = arg1;
        }

        public async Task<object> ExecuteAsync(Context context, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            await _action(context, cancellationToken, continueOnCapturedContext, _arg1).ConfigureAwait(continueOnCapturedContext);
            return null;
        }
    }

    internal struct AsyncPollyExecutableAction<T1, T2> : IAsyncPollyExecutable<object>
    {
        private readonly Func<Context, CancellationToken, bool, T1, T2, Task> _action;
        private readonly T1 _arg1;
        private readonly T2 _arg2;

        /// <summary>
        /// Creates a <see cref="AsyncPollyExecutableAction"/> instance combining an action and an input parameter of type <typeparamref name="T1"/> to be used when executing it. 
        /// This action may be executed asynchronously through a policy at a later point in time.
        /// <remarks>Combining the action and its input parameter into a short-lived struct, as shown here, allows users to pass data to actions executed through Polly policies without using closures, which would result in extra heap allocations.</remarks>
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="arg1">The parameter to pass, when executing the action.</param>
        /// <param name="arg2">The parameter to pass, when executing the action.</param>
        public AsyncPollyExecutableAction(Func<Context, CancellationToken, bool, T1, T2, Task> action, T1 arg1, T2 arg2)
        {
            _action = action;
            _arg1 = arg1;
            _arg2 = arg2;
        }

        public async Task<object> ExecuteAsync(Context context, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            await _action(context, cancellationToken, continueOnCapturedContext, _arg1, _arg2).ConfigureAwait(continueOnCapturedContext);
            return null;
        }
    }
}
