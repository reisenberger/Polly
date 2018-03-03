using System;

#if NET40
using SemaphoreSlim = Nito.AsyncEx.AsyncSemaphore;
#else
using SemaphoreSlim = System.Threading.SemaphoreSlim;
#endif

namespace Polly.Bulkhead
{
    internal class BulkheadAsyncImplementationFactory : IAsyncPolicyImplementationFactory
    {
        public IAsyncPolicyImplementation<TResult> GetImplementation<TResult>(IAsyncPolicy policy)
        {
            BulkheadPolicy bulkhead = policy as BulkheadPolicy ?? throw new ArgumentException($"policy supplied to {this.GetType().Name} is not a {typeof(BulkheadPolicy).Name}", nameof(policy));

            return new BulkheadAsyncImplementation<TResult>(bulkhead);
        }
    }

    internal class BulkheadAsyncImplementationFactory<TResult> : IAsyncPolicyImplementationFactory<TResult>
    {
        public IAsyncPolicyImplementation<TResult> GetImplementation(IAsyncPolicy<TResult> policy)
        {
            BulkheadPolicy<TResult> bulkhead = policy as BulkheadPolicy<TResult> ?? throw new ArgumentException($"policy supplied to {this.GetType().Name} is not a {typeof(BulkheadPolicy<TResult>).Name}", nameof(policy));

            return new BulkheadAsyncImplementation<TResult>(bulkhead);
        }
    }
}
