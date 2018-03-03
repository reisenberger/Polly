using System;
using System.Threading;
using Polly.Utilities;

namespace Polly.Execution
{
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
    
}
