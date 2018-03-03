using System;
using System.Threading;
using System.Threading.Tasks;
using Polly.Utilities;

namespace Polly.Execution
{
    /// <inheritdoc/>
    internal struct AsyncPollyExecutableAction : IAsyncPollyExecutable<object>
    {
        private readonly Func<Context, CancellationToken, Task> _action;

        /// <summary>
        /// Creates a <see cref="AsyncPollyExecutableAction"/> struct for the passed asynchronous action, which may be executed through a policy at a later point in time.
        /// </summary>
        /// <param name="action">The action.</param>
        public AsyncPollyExecutableAction(Func<Context, CancellationToken, Task> action)
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

    internal struct AsyncPollyExecutableAction<T1> : IAsyncPollyExecutable<object>
    {
        private readonly Func<Context, CancellationToken, T1, Task> _action;
        private readonly T1 _arg1;

        /// <summary>
        /// Creates a <see cref="AsyncPollyExecutableAction"/> instance combining an action and an input parameter of type <typeparamref name="T1"/> to be used when executing it. 
        /// This action may be executed asynchronously through a policy at a later point in time.
        /// <remarks>Combining the action and its input parameter into a short-lived struct, as shown here, allows users to pass data to actions executed through Polly policies without using closures, which would result in extra heap allocations.</remarks>
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="arg1">The parameter to pass, when executing the action.</param>
        public AsyncPollyExecutableAction(Func<Context, CancellationToken, T1, Task> action, T1 arg1)
        {
            _action = action;
            _arg1 = arg1;
        }

        public async Task<object> ExecuteAsync(Context context, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            await _action(context, cancellationToken, _arg1).ConfigureAwait(continueOnCapturedContext);
            return null;
        }
    }
}
