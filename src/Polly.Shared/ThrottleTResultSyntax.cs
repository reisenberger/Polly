using Polly.Throttle;
using Polly.Utilities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polly
{
    /// <summary>
    /// Fluent API for defining a Throttle <see cref="Policy"/>. 
    /// </summary>
    public partial class Policy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ThrottlePolicy<TResult> Throttle<TResult>()
        {
            return Throttle<TResult>(Environment.ProcessorCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="maxParallelization"></param>
        /// <param name="maxQueuedActions"></param>
        /// <returns></returns>
        public static ThrottlePolicy<TResult> Throttle<TResult>(int maxParallelization, int maxQueuedActions = int.MaxValue)
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
            return new ThrottlePolicy<TResult>(
               (action, context, cancellationToken) => ThrottleEngine.Implementation(
                   action,
                   maxParallelizationSemaphore,
                   maxQueuedActionsSemaphore,
                   cancellationToken
               )
           );
        }
        
    }
}
