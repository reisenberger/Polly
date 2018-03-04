using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.Bulkhead
{
    public partial class BulkheadPolicy : IBulkheadPolicy
    {
        private Func<Context, Task> _onBulkheadRejectedAsync;

        internal BulkheadPolicy(
            Func<Context, Task> onBulkheadRejectedAsync,
            int maxParallelization,
            int maxQueueingActions,
            SemaphoreSlim maxParallelizationSemaphore,
            SemaphoreSlim maxQueuedActionsSemaphore,
            Func<IAsyncPolicy, IAsyncPolicyImplementation<object>> factory
            ) : base(PolicyBuilder.Empty, factory)
        {
            _onBulkheadRejectedAsync = onBulkheadRejectedAsync;
            _maxParallelization = maxParallelization;
            _maxQueueingActions = maxQueueingActions;
            _maxParallelizationSemaphore = maxParallelizationSemaphore;
            _maxQueuedActionsSemaphore = maxQueuedActionsSemaphore;
        }

        Func<Context, Task> IBulkheadPolicyInternal.OnBulkheadRejectedAsync => _onBulkheadRejectedAsync;
    }

    public partial class BulkheadPolicy<TResult> : IBulkheadPolicy<TResult>
    {
        private Func<Context, Task> _onBulkheadRejectedAsync;

        internal BulkheadPolicy(
            Func<Context, Task> onBulkheadRejectedAsync,
            int maxParallelization,
            int maxQueueingActions,
            SemaphoreSlim maxParallelizationSemaphore,
            SemaphoreSlim maxQueuedActionsSemaphore,
            Func<IAsyncPolicy<TResult>, IAsyncPolicyImplementation<TResult>> factory
            ) : base(PolicyBuilder<TResult>.Empty, factory)
        {
            _onBulkheadRejectedAsync = onBulkheadRejectedAsync;
            _maxParallelization = maxParallelization;
            _maxQueueingActions = maxQueueingActions;
            _maxParallelizationSemaphore = maxParallelizationSemaphore;
            _maxQueuedActionsSemaphore = maxQueuedActionsSemaphore;
        }

        Func<Context, Task> IBulkheadPolicyInternal.OnBulkheadRejectedAsync => _onBulkheadRejectedAsync;
    }
}