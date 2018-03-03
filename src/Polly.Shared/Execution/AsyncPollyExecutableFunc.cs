using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.Execution
{
    /// <inheritdoc/>
    public struct AsyncPollyExecutableFuncNoParams<TResult> : IAsyncPollyExecutable<TResult>
    {
        private readonly Func<Task<TResult>> _func;

        /// <summary>
        /// Creates a <see cref="AsyncPollyExecutableFuncNoParams{TResult}"/> struct for the passed func, which may be executed through a policy at a later point in time.
        /// </summary>
        /// <param name="func">The function.</param>
        public AsyncPollyExecutableFuncNoParams(Func<Task<TResult>> func)
        {
            _func = func;
        }

        /// <inheritdoc/>
        public Task<TResult> ExecuteAsync(Context context, CancellationToken cancellationToken, bool continueOnCapturedContext) => _func();
    }

    /// <inheritdoc/>
    public struct AsyncPollyExecutableFuncOnContext<TResult> : IAsyncPollyExecutable<TResult>
    {
        private readonly Func<Context, Task<TResult>> _func;

        /// <summary>
        /// Creates a <see cref="AsyncPollyExecutableFuncOnContext{TResult}"/> struct for the passed func, which may be executed through a policy at a later point in time.
        /// </summary>
        /// <param name="func">The function.</param>
        public AsyncPollyExecutableFuncOnContext(Func<Context, Task<TResult>> func)
        {
            _func = func;
        }

        /// <inheritdoc/>
        public Task<TResult> ExecuteAsync(Context context, CancellationToken cancellationToken, bool continueOnCapturedContext) => _func(context);
    }

    /// <inheritdoc/>
    public struct AsyncPollyExecutableFuncOnCancellationToken<TResult> : IAsyncPollyExecutable<TResult>
    {
        private readonly Func<CancellationToken, Task<TResult>> _func;

        /// <summary>
        /// Creates a <see cref="AsyncPollyExecutableFuncOnCancellationToken{TResult}"/> struct for the passed func, which may be executed through a policy at a later point in time.
        /// </summary>
        /// <param name="func">The function.</param>
        public AsyncPollyExecutableFuncOnCancellationToken(Func<CancellationToken, Task<TResult>> func)
        {
            _func = func;
        }

        /// <inheritdoc/>
        public Task<TResult> ExecuteAsync(Context context, CancellationToken cancellationToken, bool continueOnCapturedContext) => _func(cancellationToken);
    }

    /// <inheritdoc/>
    public struct AsyncPollyExecutableFuncOnContextCancellationToken<TResult> : IAsyncPollyExecutable<TResult>
    {
        private readonly Func<Context, CancellationToken, Task<TResult>> _func;

        /// <summary>
        /// Creates a <see cref="AsyncPollyExecutableFuncOnContextCancellationToken{TResult}"/> struct for the passed func, which may be executed through a policy at a later point in time.
        /// </summary>
        /// <param name="func">The function.</param>
        public AsyncPollyExecutableFuncOnContextCancellationToken(Func<Context, CancellationToken, Task<TResult>> func)
        {
            _func = func;
        }

        /// <inheritdoc/>
        public Task<TResult> ExecuteAsync(Context context, CancellationToken cancellationToken, bool continueOnCapturedContext) => _func(context, cancellationToken);
    }

    /// <inheritdoc/>
    public struct AsyncPollyExecutableFunc<TResult> : IAsyncPollyExecutable<TResult>
    {
        private readonly Func<Context, CancellationToken, bool, Task<TResult>> _func;

        /// <summary>
        /// Creates a <see cref="AsyncPollyExecutableFunc{TResult}"/> struct for the passed func, which may be executed through a policy at a later point in time.
        /// </summary>
        /// <param name="func">The function.</param>
        public AsyncPollyExecutableFunc(Func<Context, CancellationToken, bool, Task<TResult>> func)
        {
            _func = func;
        }

        /// <inheritdoc/>
        public Task<TResult> ExecuteAsync(Context context, CancellationToken cancellationToken, bool continueOnCapturedContext) => _func(context, cancellationToken, continueOnCapturedContext);
    }

    /// <inheritdoc/>
    public struct AsyncPollyExecutableFunc<T1, TResult> : IAsyncPollyExecutable<TResult>
    {
        private readonly Func<Context, CancellationToken, bool, T1, Task<TResult>> _func;
        private readonly T1 _arg1;

        /// <summary>
        /// Creates a <see cref="AsyncPollyExecutableFunc{T1, TResult}"/> instance combining a func and an input parameter of type <typeparamref name="T1"/> to be used when executing it. 
        /// This func may be executed through a policy at a later point in time.
        /// <remarks>Combining the action and its input parameter into a short-lived struct, as shown here, allows users to pass data to actions executed through Polly policies without using closures, which would result in extra heap allocations.</remarks>
        /// </summary>
        /// <param name="func">The function.</param>
        /// <param name="arg1">The parameter to pass, when executing the function.</param>
        public AsyncPollyExecutableFunc(Func<Context, CancellationToken, bool, T1, Task<TResult>> func, T1 arg1)
        {
            _func = func;
            _arg1 = arg1;
        }

        /// <inheritdoc/>
        public Task<TResult> ExecuteAsync(Context context, CancellationToken cancellationToken, bool continueOnCapturedContext) => _func(context, cancellationToken, continueOnCapturedContext, _arg1);
    }

    /// <inheritdoc/>
    public struct AsyncPollyExecutableFunc<T1, T2, TResult> : IAsyncPollyExecutable<TResult>
    {
        private readonly Func<Context, CancellationToken, bool, T1, T2, Task<TResult>> _func;
        private readonly T1 _arg1;
        private readonly T2 _arg2;

        /// <summary>
        /// Creates a <see cref="AsyncPollyExecutableFunc{T1, TResult}"/> instance combining a func and an input parameter of type <typeparamref name="T1"/> to be used when executing it. 
        /// This func may be executed through a policy at a later point in time.
        /// <remarks>Combining the action and its input parameter into a short-lived struct, as shown here, allows users to pass data to actions executed through Polly policies without using closures, which would result in extra heap allocations.</remarks>
        /// </summary>
        /// <param name="func">The function.</param>
        /// <param name="arg1">The parameter to pass, when executing the function.</param>
        /// <param name="arg2">The parameter to pass, when executing the action.</param>
        public AsyncPollyExecutableFunc(Func<Context, CancellationToken, bool, T1, T2, Task<TResult>> func, T1 arg1, T2 arg2)
        {
            _func = func;
            _arg1 = arg1;
            _arg2 = arg2;
        }

        /// <inheritdoc/>
        public Task<TResult> ExecuteAsync(Context context, CancellationToken cancellationToken, bool continueOnCapturedContext) => _func(context, cancellationToken, continueOnCapturedContext, _arg1, _arg2);
    }
}
