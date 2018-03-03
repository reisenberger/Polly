using System;
using System.Threading.Tasks;

#if NET40
using SemaphoreSlim = Nito.AsyncEx.AsyncSemaphore;
#else
using SemaphoreSlim = System.Threading.SemaphoreSlim;
#endif


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
