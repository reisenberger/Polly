using System;

namespace Polly.Timeout
{
    internal class TimeoutAsyncImplementationFactory : IAsyncPolicyImplementationFactory
    {
        public IAsyncPolicyImplementation<TResult> GetImplementation<TResult>(IAsyncPolicy policy)
        {
            TimeoutPolicy timeout = policy as TimeoutPolicy ?? throw new ArgumentException($"policy supplied to {this.GetType().Name} is not a {typeof(TimeoutPolicy).Name}", nameof(policy));

            return new TimeoutAsyncImplementation<TResult>(timeout);
        }
    }

    internal class TimeoutAsyncImplementationFactory<TResult> : IAsyncPolicyImplementationFactory<TResult>
    {
        public IAsyncPolicyImplementation<TResult> GetImplementation(IAsyncPolicy<TResult> policy)
        {
            TimeoutPolicy<TResult> timeout = policy as TimeoutPolicy<TResult> ?? throw new ArgumentException($"policy supplied to {this.GetType().Name} is not a {typeof(TimeoutPolicy<TResult>).Name}", nameof(policy));

            return new TimeoutAsyncImplementation<TResult>(timeout);
        }
    }
}
