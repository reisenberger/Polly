using System;
using System.Threading;
using Polly.Execution;

#if NET40
using SemaphoreSlim = Nito.AsyncEx.AsyncSemaphore;
using Polly.Utilities;
#else
using SemaphoreSlim = System.Threading.SemaphoreSlim;
#endif

namespace Polly.Bulkhead
{
    internal class BulkheadSyncImplementation<TResult> : ISyncPolicyImplementation<TResult>
    {
        private IBulkheadPolicyInternal _bulkhead;

        internal BulkheadSyncImplementation(IBulkheadPolicyInternal policy)
        {
            _bulkhead = policy ?? throw new ArgumentNullException(nameof(policy));
        }

        public TResult Execute<TExecutable>(in TExecutable action, Context context, in CancellationToken cancellationToken) where TExecutable : ISyncPollyExecutable<TResult>
        {
            if (!_bulkhead.MaxQueuedActionsSemaphore.Wait(TimeSpan.Zero, cancellationToken))
            {
                _bulkhead.OnBulkheadRejected(context);
                throw new BulkheadRejectedException();
            }

            try
            {
                _bulkhead.MaxParallelizationSemaphore.Wait(cancellationToken);
                try
                {
                    return action.Execute(context, cancellationToken);
                }
                finally
                {
                    _bulkhead.MaxParallelizationSemaphore.Release();
                }
            }
            finally
            {
                _bulkhead.MaxQueuedActionsSemaphore.Release();
            }
        }
    }
}
