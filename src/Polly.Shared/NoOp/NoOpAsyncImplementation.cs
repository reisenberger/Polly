using System.Threading;
using System.Threading.Tasks;
using Polly.Execution;

namespace Polly.NoOp
{
    internal class NoOpAsyncImplementation<TResult> : IAsyncPolicyImplementation<TResult>
    {
        public Task<TResult> ExecuteAsync<TExecutableAsync>(TExecutableAsync action, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext) where TExecutableAsync : IAsyncPollyExecutable<TResult>
        {
            return action.ExecuteAsync(context, cancellationToken, continueOnCapturedContext);
        }
    }
}
