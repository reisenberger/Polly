using System;
using System.Threading;

namespace Polly.Execution
{
    /// <inheritdoc/>
    internal struct SyncPollyExecutableActionWithMissingParams : ISyncPollyExecutable<object>
    {
        private readonly Action _action;

        /// <summary>
        /// Creates a <see cref="SyncPollyExecutableActionWithMissingParams"/> struct for the passed action, which may be executed through a policy at a later point in time.
        /// </summary>
        /// <param name="action">The action.</param>
        public SyncPollyExecutableActionWithMissingParams(Action action)
        {
            _action = action;
        }

        /// <inheritdoc/>
        public object Execute(Context context, CancellationToken cancellationToken)
        {
            _action();
            return null;
        }
    }
    /// <inheritdoc/>
    internal struct SyncPollyExecutableActionWithMissingContextParam : ISyncPollyExecutable<object>
    {
        private readonly Action<CancellationToken> _action;

        /// <summary>
        /// Creates a <see cref="SyncPollyExecutableActionWithMissingContextParam"/> struct for the passed action, which may be executed through a policy at a later point in time.
        /// </summary>
        /// <param name="action">The action.</param>
        public SyncPollyExecutableActionWithMissingContextParam(Action<CancellationToken> action)
        {
            _action = action;
        }

        /// <inheritdoc/>
        public object Execute(Context context, CancellationToken cancellationToken)
        {
            _action(cancellationToken);
            return null;
        }
    }

    /// <inheritdoc/>
    internal struct SyncPollyExecutableActionWithMissingCancellationTokenParam : ISyncPollyExecutable<object>
    {
        private readonly Action<Context> _action;

        /// <summary>
        /// Creates a <see cref="SyncPollyExecutableActionWithMissingCancellationTokenParam"/> struct for the passed action, which may be executed through a policy at a later point in time.
        /// </summary>
        /// <param name="action">The action.</param>
        public SyncPollyExecutableActionWithMissingCancellationTokenParam(Action<Context> action)
        {
            _action = action;
        }

        /// <inheritdoc/>
        public object Execute(Context context, CancellationToken cancellationToken)
        {
            _action(context);
            return null;
        }
    }
    
    /// <inheritdoc/>
    internal struct SyncPollyExecutableAction : ISyncPollyExecutable<object>
    {
        private readonly Action<Context, CancellationToken> _action;

        /// <summary>
        /// Creates a <see cref="SyncPollyExecutableAction"/> struct for the passed action, which may be executed through a policy at a later point in time.
        /// </summary>
        /// <param name="action">The action.</param>
        public SyncPollyExecutableAction(Action<Context, CancellationToken> action)
        {
            _action = action;
        }

        /// <inheritdoc/>
        public object Execute(Context context, CancellationToken cancellationToken)
        {
            _action(context, cancellationToken);
            return null;
        }
    }

    /// <inheritdoc/>
    internal struct SyncPollyExecutableAction<T1> : ISyncPollyExecutable<object>
    {
        private readonly Action<Context, CancellationToken, T1> _action;
        private readonly T1 _arg1;

        /// <summary>
        /// Creates a <see cref="SyncPollyExecutableAction"/> instance combining an action and an input parameter of type <typeparamref name="T1"/> to be used when executing it. 
        /// This action may be executed through a policy at a later point in time.
        /// <remarks>Combining the action and its input parameter into a short-lived struct, as shown here, allows users to pass data to actions executed through Polly policies without using closures, which would result in extra heap allocations.</remarks>
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="arg1">The parameter to pass, when executing the action.</param>
        public SyncPollyExecutableAction(Action<Context, CancellationToken, T1> action, T1 arg1)
        {
            _action = action;
            _arg1 = arg1;
        }

        public object Execute(Context context, CancellationToken cancellationToken)
        {
            _action(context, cancellationToken, _arg1);
            return null;
        }
    }

    /// <inheritdoc/>
    internal struct SyncPollyExecutableAction<T1, T2> : ISyncPollyExecutable<object>
    {
        private readonly Action<Context, CancellationToken, T1, T2> _action;
        private readonly T1 _arg1;
        private readonly T2 _arg2;

        /// <summary>
        /// Creates a <see cref="SyncPollyExecutableAction{T1, T2}"/> instance combining an action and input parameters of type <typeparamref name="T1"/> and <typeparamref name="T2"/> to be used when executing it. 
        /// This action may be executed through a policy at a later point in time.
        /// <remarks>Combining the action and its input parameter into a short-lived struct, as shown here, allows users to pass data to actions executed through Polly policies without using closures, which would result in extra heap allocations.</remarks>
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="arg1">The parameter to pass, when executing the action.</param>
        /// <param name="arg2">The parameter to pass, when executing the action.</param>
        public SyncPollyExecutableAction(Action<Context, CancellationToken, T1, T2> action, T1 arg1, T2 arg2)
        {
            _action = action;
            _arg1 = arg1;
            _arg2 = arg2;
        }

        public object Execute(Context context, CancellationToken cancellationToken)
        {
            _action(context, cancellationToken, _arg1, _arg2);
            return null;
        }
    }
}
