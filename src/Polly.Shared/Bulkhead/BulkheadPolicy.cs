using System;
using System.Threading;

namespace Polly.Bulkhead
{
    /// <summary>
    /// A bulkhead-isolation policy which can be applied to delegates.
    /// </summary>
    public partial class BulkheadPolicy : Policy, IBulkheadPolicy, IBulkheadPolicyInternal
    {
        private readonly SemaphoreSlim _maxParallelizationSemaphore;
        private readonly SemaphoreSlim _maxQueuedActionsSemaphore;
        private readonly int _maxParallelization;
        private readonly int _maxQueueingActions;

        private Action<Context> _onBulkheadRejected;

        internal BulkheadPolicy(
            Action<Context> onBulkheadRejected, 
            int maxParallelization,
            int maxQueueingActions,
            SemaphoreSlim maxParallelizationSemaphore, 
            SemaphoreSlim maxQueuedActionsSemaphore,
            Func<ISyncPolicy, ISyncPolicyImplementation<Object>> factory
            ) : base(PolicyBuilder.Empty, factory)
        {
            _onBulkheadRejected = onBulkheadRejected;
            _maxParallelization = maxParallelization;
            _maxQueueingActions = maxQueueingActions;
            _maxParallelizationSemaphore = maxParallelizationSemaphore;
            _maxQueuedActionsSemaphore = maxQueuedActionsSemaphore;
        }

        /// <summary>
        /// Gets the number of slots currently available for executing actions through the bulkhead.
        /// </summary>
        public int BulkheadAvailableCount => _maxParallelizationSemaphore.CurrentCount;

        /// <summary>
        /// Gets the number of slots currently available for queuing actions for execution through the bulkhead.
        /// </summary>
        public int QueueAvailableCount => Math.Min(_maxQueuedActionsSemaphore.CurrentCount, _maxQueueingActions);

        SemaphoreSlim IBulkheadPolicyInternal.MaxParallelizationSemaphore => _maxParallelizationSemaphore;

        SemaphoreSlim IBulkheadPolicyInternal.MaxQueuedActionsSemaphore => _maxQueuedActionsSemaphore;

        Action<Context> IBulkheadPolicyInternal.OnBulkheadRejected => _onBulkheadRejected;

        /// <summary>
        /// Disposes of the <see cref="BulkheadPolicy"/>, allowing it to dispose its internal resources.  
        /// <remarks>Only call <see cref="Dispose()"/> on a <see cref="BulkheadPolicy"/> after all actions executed through the policy have completed.  If actions are still executing through the policy when <see cref="Dispose()"/> is called, an <see cref="ObjectDisposedException"/> may be thrown on the actions' threads when those actions complete.</remarks>
        /// </summary>
        public void Dispose()
        {
            _maxParallelizationSemaphore.Dispose();
            _maxQueuedActionsSemaphore.Dispose();
        }
    }

    /// <summary>
    /// A bulkhead-isolation policy which can be applied to delegates returning a value of type <typeparamref name="TResult"/>.
    /// </summary>
    public partial class BulkheadPolicy<TResult> : Policy<TResult>, IBulkheadPolicy<TResult>, IBulkheadPolicyInternal
    {
        private readonly SemaphoreSlim _maxParallelizationSemaphore;
        private readonly SemaphoreSlim _maxQueuedActionsSemaphore;
        private readonly int _maxParallelization;
        private readonly int _maxQueueingActions;

        private Action<Context> _onBulkheadRejected;

        internal BulkheadPolicy(
            Action<Context> onBulkheadRejected,
            int maxParallelization,
            int maxQueueingActions,
            SemaphoreSlim maxParallelizationSemaphore,
            SemaphoreSlim maxQueuedActionsSemaphore,
            Func<ISyncPolicy<TResult>, ISyncPolicyImplementation<TResult>> factory
            ) : base(PolicyBuilder<TResult>.Empty, factory)
        {
            _onBulkheadRejected = onBulkheadRejected;
            _maxParallelization = maxParallelization;
            _maxQueueingActions = maxQueueingActions;
            _maxParallelizationSemaphore = maxParallelizationSemaphore;
            _maxQueuedActionsSemaphore = maxQueuedActionsSemaphore;
        }


        /// <summary>
        /// Gets the number of slots currently available for executing actions through the bulkhead.
        /// </summary>
        public int BulkheadAvailableCount => _maxParallelizationSemaphore.CurrentCount;

        /// <summary>
        /// Gets the number of slots currently available for queuing actions for execution through the bulkhead.
        /// </summary>
        public int QueueAvailableCount => Math.Min(_maxQueuedActionsSemaphore.CurrentCount, _maxQueueingActions);

        SemaphoreSlim IBulkheadPolicyInternal.MaxParallelizationSemaphore => _maxParallelizationSemaphore;

        SemaphoreSlim IBulkheadPolicyInternal.MaxQueuedActionsSemaphore => _maxQueuedActionsSemaphore;

        Action<Context> IBulkheadPolicyInternal.OnBulkheadRejected => _onBulkheadRejected;

        /// <summary>
        /// Disposes of the <see cref="BulkheadPolicy"/>, allowing it to dispose its internal resources.  
        /// <remarks>Only call <see cref="Dispose()"/> on a <see cref="BulkheadPolicy"/> after all actions executed through the policy have completed.  If actions are still executing through the policy when <see cref="Dispose()"/> is called, an <see cref="ObjectDisposedException"/> may be thrown on the actions' threads when those actions complete.</remarks>
        /// </summary>
        public void Dispose()
        {
            _maxParallelizationSemaphore.Dispose();
            _maxQueuedActionsSemaphore.Dispose();
        }
    }
}
