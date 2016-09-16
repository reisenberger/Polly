
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.Bulkhead
{
    /// <summary>
    /// Asynchronous Bulkhead Engine
    /// </summary>
    internal static partial class BulkheadEngine
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action"></param>
        /// <param name="maxParallelizationSemaphore"></param>
        /// <param name="maxQueuedActionsSemaphore"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="continueOnCapturedContext"></param>
        /// <returns></returns>
        internal static async Task<TResult> ImplementationAsync<TResult>(
            Func<CancellationToken, Task<TResult>> action, 
            SemaphoreSlim maxParallelizationSemaphore,
            SemaphoreSlim maxQueuedActionsSemaphore,
            CancellationToken cancellationToken, 
            bool continueOnCapturedContext)
        {
#if NET40
            if (!maxQueuedActionsSemaphore.Wait(TimeSpan.Zero, cancellationToken)) { throw new SemaphoreRejectedException(); }
#else
            if (!await maxQueuedActionsSemaphore.WaitAsync(TimeSpan.Zero, cancellationToken).ConfigureAwait(continueOnCapturedContext)) { throw new SemaphoreRejectedException(); }
#endif
            try
            {
#if NET40
                maxParallelizationSemaphore.Wait(cancellationToken);
#else
                await maxParallelizationSemaphore.WaitAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext);
#endif
                try 
                {
                    return await action(cancellationToken).ConfigureAwait(continueOnCapturedContext);
                }
                finally
                {
                    maxParallelizationSemaphore.Release();
                }
            }
            finally
            {
                maxQueuedActionsSemaphore.Release();
            }
        }
    }
}
