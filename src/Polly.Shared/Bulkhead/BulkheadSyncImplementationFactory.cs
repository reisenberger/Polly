using System;

#if NET40
using SemaphoreSlim = Nito.AsyncEx.AsyncSemaphore;
#else
using SemaphoreSlim = System.Threading.SemaphoreSlim;
#endif

namespace Polly.Bulkhead
{
    internal class BulkheadSyncImplementationFactory : ISyncPolicyImplementationFactory
    {
        public ISyncPolicyImplementation<TResult> GetImplementation<TResult>(ISyncPolicy policy)
        {
            BulkheadPolicy bulkhead = policy as BulkheadPolicy ?? throw new ArgumentException($"policy supplied to {this.GetType().Name} is not a {typeof(BulkheadPolicy).Name}", nameof(policy));

            return new BulkheadSyncImplementation<TResult>(bulkhead);
        }
    }

    internal class BulkheadSyncImplementationFactory<TResult> : ISyncPolicyImplementationFactory<TResult>
    {
        public ISyncPolicyImplementation<TResult> GetImplementation(ISyncPolicy<TResult> policy)
        {
            BulkheadPolicy<TResult> bulkhead = policy as BulkheadPolicy<TResult> ?? throw new ArgumentException($"policy supplied to {this.GetType().Name} is not a {typeof(BulkheadPolicy<TResult>).Name}", nameof(policy));

            return new BulkheadSyncImplementation<TResult>(bulkhead);
        }
    }
}
