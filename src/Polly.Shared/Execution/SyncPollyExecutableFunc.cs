using System;
using System.Threading;
using Polly.Utilities;

namespace Polly.Execution
{
    /// <inheritdoc/>

    public struct SyncPollyExecutableFuncWithMissingParams<TResult> : ISyncPollyExecutable<TResult>
    {
        private readonly Func<TResult> _func;

        /// <summary>
        /// Creates a <see cref="SyncPollyExecutableFunc{TResult}"/> struct for the passed func, which may be executed through a policy at a later point in time.
        /// </summary>
        /// <param name="func">The function.</param>
        public SyncPollyExecutableFuncWithMissingParams(Func<TResult> func)
        {
            _func = func;
        }

        /// <inheritdoc/>
        public TResult Execute(Context context, CancellationToken cancellationToken) => _func();
    }

    /// <inheritdoc/>

    public struct SyncPollyExecutableFuncWithMissingContextParam<TResult> : ISyncPollyExecutable<TResult>
    {
        private readonly Func<CancellationToken, TResult> _func;

        /// <summary>
        /// Creates a <see cref="SyncPollyExecutableFunc{TResult}"/> struct for the passed func, which may be executed through a policy at a later point in time.
        /// </summary>
        /// <param name="func">The function.</param>
        public SyncPollyExecutableFuncWithMissingContextParam(Func<CancellationToken, TResult> func)
        {
            _func = func;
        }

        /// <inheritdoc/>
        public TResult Execute(Context context, CancellationToken cancellationToken) => _func(cancellationToken);
    }

    /// <inheritdoc/>

    public struct SyncPollyExecutableFuncWithMissingCancellationTokenParam<TResult> : ISyncPollyExecutable<TResult>
    {
        private readonly Func<Context, TResult> _func;

        /// <summary>
        /// Creates a <see cref="SyncPollyExecutableFunc{TResult}"/> struct for the passed func, which may be executed through a policy at a later point in time.
        /// </summary>
        /// <param name="func">The function.</param>
        public SyncPollyExecutableFuncWithMissingCancellationTokenParam(Func<Context, TResult> func)
        {
            _func = func;
        }

        /// <inheritdoc/>
        public TResult Execute(Context context, CancellationToken cancellationToken) => _func(context);
    }

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

    /// <inheritdoc/>
    public struct SyncPollyExecutableFunc<T1, T2, TResult> : ISyncPollyExecutable<TResult>
    {
        private readonly Func<Context, CancellationToken, T1, T2, TResult> _func;
        private readonly T1 _arg1;
        private readonly T2 _arg2;

        /// <summary>
        /// Creates a <see cref="SyncPollyExecutableFunc{T1, T2, TResult}"/> instance combining a func and input parameters of type <typeparamref name="T1"/> and <typeparamref name="T2"/> to be used when executing it. 
        /// This func may be executed through a policy at a later point in time.
        /// <remarks>Combining the action and its input parameter into a short-lived struct, as shown here, allows users to pass data to actions executed through Polly policies without using closures, which would result in extra heap allocations.</remarks>
        /// </summary>
        /// <param name="func">The function.</param>
        /// <param name="arg1">The parameter to pass, when executing the function.</param>
        /// <param name="arg2">The parameter to pass, when executing the function.</param>
        public SyncPollyExecutableFunc(Func<Context, CancellationToken, T1, T2, TResult> func, T1 arg1, T2 arg2)
        {
            _func = func;
            _arg1 = arg1;
            _arg2 = arg2;
        }

        /// <inheritdoc/>
        public TResult Execute(Context context, CancellationToken cancellationToken) => _func(context, cancellationToken, _arg1, _arg2);
    }
}
