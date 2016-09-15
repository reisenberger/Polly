using Polly.Bulkhead;
using Polly.Utilities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polly
{
    /// <summary>
    /// Fluent API for defining a <see cref="BulkheadPolicy"/>. 
    /// </summary>    
    public partial class Policy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static BulkheadPolicy BulkheadAsync()
        {
            return BulkheadAsync(Environment.ProcessorCount);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxParallelization"></param>
        /// <param name="maxQueuedActions"></param>
        /// <returns></returns>
        public static BulkheadPolicy BulkheadAsync(int maxParallelization, int maxQueuedActions = int.MaxValue)
        {
            if (maxParallelization <= 0) throw new ArgumentOutOfRangeException(nameof(maxParallelization));
            if (maxQueuedActions < 0) throw new ArgumentOutOfRangeException(nameof(maxQueuedActions));
            if (maxQueuedActions != 0 && maxQueuedActions < maxParallelization) throw new ArgumentOutOfRangeException(nameof(maxQueuedActions));
            var maxParallelizationSemaphore = new SemaphoreSlim(maxParallelization);
            if ((long)maxParallelization + (long)maxQueuedActions > (long)int.MaxValue)
            {
                maxQueuedActions = int.MaxValue;
            }
            else maxQueuedActions += maxParallelization;
            var maxQueuedActionsSemaphore = new SemaphoreSlim(maxQueuedActions, maxQueuedActions);
            return new BulkheadPolicy((action, context, cancellationToken, continueOnCapturedContext) =>
                BulkheadEngine.ImplementationAsync(
                  async ct => { await action(ct).ConfigureAwait(continueOnCapturedContext); return EmptyStruct.Instance; },
                  maxParallelizationSemaphore,
                  maxQueuedActionsSemaphore,
                  cancellationToken,
                  continueOnCapturedContext));
        }
    }
}