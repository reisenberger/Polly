using System;
using System.Threading;
using System.Threading.Tasks;
using Polly.Execution;

namespace Polly.Caching
{
    /// <summary>
    /// Indicates that we do not expect a non-generic cache implementation ever to be invoked.
    /// <remarks>Void-returning calls should be overridden by <see cref="CachePolicy"/> as a pass-through operation (just execute the passed delegate).</remarks>
    /// <remarks>Method-generic executions on a non-generic policy should be executed by manufacturing a relevant generic cache implementation.</remarks>
    /// </summary>
    internal class NoNonGenericCacheImplementation<TResult> : ISyncPolicyImplementation<TResult>, IAsyncPolicyImplementation<TResult>
    {
        public TResult Execute<TExecutable>(TExecutable action, Context context, CancellationToken cancellationToken) where TExecutable : ISyncPollyExecutable<TResult>
        {
            throw InvalidOperation();
        }

        public Task<TResult> ExecuteAsync<TExecutableAsync>(TExecutableAsync action, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext) where TExecutableAsync : IAsyncPollyExecutable<TResult>
        {
            throw InvalidOperation();
        }

        private InvalidOperationException InvalidOperation() => new InvalidOperationException($"Internal Polly ${typeof(CachePolicy).Name} error: executions on a non-generic cache policy should never use a standard non-generic implementation.");
    }
}