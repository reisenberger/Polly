
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.Throttle
{
    /// <summary>
    /// Asynchronous Throttle Engine
    /// </summary>
    internal static partial class ThrottleEngine
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
            cancellationToken.ThrowIfCancellationRequested();
#if NET40
            if (!maxQueuedActionsSemaphore.Wait(TimeSpan.Zero, cancellationToken)) throw new SemaphoreRejectedException();
#else
            if (!(await maxQueuedActionsSemaphore.WaitAsync(TimeSpan.Zero, cancellationToken).ConfigureAwait(continueOnCapturedContext))) throw new SemaphoreRejectedException();
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
                    DelegateResult<TResult> delegateOutcome = new DelegateResult<TResult>(await action(cancellationToken).ConfigureAwait(continueOnCapturedContext));
                    cancellationToken.ThrowIfCancellationRequested();
                    return delegateOutcome.Result;
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
