using System;
using System.Threading;
using Polly.Utilities;

namespace Polly.Execution
{
    /// <inheritdoc/>

    public struct SyncPollyExecutableFunc<TResult> : ISyncPollyExecutable<TResult>
    {
        private readonly Func<Context, CancellationToken, TResult> _func;

        /// <summary>
        /// Creates a <see cref="SyncPollyExecutableFunc{TResult}"/> struct for the passed func, which may be executed through a policy at a later point in time.
        /// </summary>
        /// <param name="func">The function.</param>
        public SyncPollyExecutableFunc(Func<Context, CancellationToken, TResult> func)
        {
            _func = func;
        }

        /// <inheritdoc/>
        public TResult Execute(Context context, CancellationToken cancellationToken) => _func(context, cancellationToken);
    }

    /// <inheritdoc/>
    public struct SyncPollyExecutableFunc<T1, TResult> : ISyncPollyExecutable<TResult>
    {
        private readonly Func<Context, CancellationToken, T1, TResult> _func;
        private readonly T1 _arg1;

        /// <summary>
        /// Creates a <see cref="SyncPollyExecutableFunc{T1, TResult}"/> instance combining a func and an input parameter of type <typeparamref name="T1"/> to be used when executing it. 
        /// This func may be executed through a policy at a later point in time.
        /// <remarks>Combining the action and its input parameter into a short-lived struct, as shown here, allows users to pass data to actions executed through Polly policies without using closures, which would result in extra heap allocations.</remarks>
        /// </summary>
        /// <param name="func">The function.</param>
        /// <param name="arg1">The parameter to pass, when executing the function.</param>
        public SyncPollyExecutableFunc(Func<Context, CancellationToken, T1, TResult> func, T1 arg1)
        {
            _func = func;
            _arg1 = arg1;
        }

        /// <inheritdoc/>
        public TResult Execute(Context context, CancellationToken cancellationToken) => _func(context, cancellationToken, _arg1);
    }

}
