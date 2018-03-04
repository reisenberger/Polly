using System.Threading;
using Polly.Execution;

namespace Polly.NoOp
{
    internal class NoOpSyncImplementation<TResult> : ISyncPolicyImplementation<TResult>
    {
        public TResult Execute<TExecutable>(in TExecutable action, Context context, in CancellationToken cancellationToken) where TExecutable : ISyncPollyExecutable<TResult>
        {
            return action.Execute(context, cancellationToken);
        }
    }
}
