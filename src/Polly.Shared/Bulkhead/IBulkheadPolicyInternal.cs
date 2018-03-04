using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.Bulkhead
{
    internal interface IBulkheadPolicyInternal
    {
         SemaphoreSlim MaxParallelizationSemaphore { get; }
         SemaphoreSlim MaxQueuedActionsSemaphore { get; }

         Action<Context> OnBulkheadRejected { get; }
         Func<Context, Task> OnBulkheadRejectedAsync { get; }
    }
}
