using System;
using System.Threading;
using System.Threading.Tasks;
using Polly.Execution;

#if NET40
using SemaphoreSlim = Nito.AsyncEx.AsyncSemaphore;
using Polly.Utilities;
#else
using SemaphoreSlim = System.Threading.SemaphoreSlim;
#endif

namespace Polly.Bulkhead
{
    internal class BulkheadAsyncImplementation<TResult> : IAsyncPolicyImplementation<TResult>
    {
        private IBulkheadPolicyInternal _bulkhead;

        internal BulkheadAsyncImplementation(IBulkheadPolicyInternal policy)
        {
            _bulkhead = policy ?? throw new ArgumentNullException(nameof(policy));
        }

        public async Task<TResult> ExecuteAsync<TExecutableAsync>(TExecutableAsync action, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext) where TExecutableAsync : IAsyncPollyExecutable<TResult>
        {
            if (!await _bulkhead.MaxQueuedActionsSemaphore.WaitAsync(TimeSpan.Zero, cancellationToken).ConfigureAwait(continueOnCapturedContext))
            {
                await _bulkhead.OnBulkheadRejectedAsync(context).ConfigureAwait(continueOnCapturedContext);
                throw new BulkheadRejectedException();
            }

            try
            {
                await _bulkhead.MaxParallelizationSemaphore.WaitAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext);
                try
                {
                    return await action.ExecuteAsync(context, cancellationToken, continueOnCapturedContext).ConfigureAwait(continueOnCapturedContext);
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
