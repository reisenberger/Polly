using System;

namespace Polly.Timeout
{
    internal class TimeoutSyncImplementationFactory : ISyncPolicyImplementationFactory
    {
        public ISyncPolicyImplementation<TResult> GetImplementation<TResult>(ISyncPolicy policy)
        {
            TimeoutPolicy timeout = policy as TimeoutPolicy ?? throw new ArgumentException($"policy supplied to {this.GetType().Name} is not a {typeof(TimeoutPolicy).Name}", nameof(policy));

            return new TimeoutSyncImplementation<TResult>(timeout);
        }
    }

    internal class TimeoutSyncImplementationFactory<TResult> : ISyncPolicyImplementationFactory<TResult>
    {
        public ISyncPolicyImplementation<TResult> GetImplementation(ISyncPolicy<TResult> policy)
        {
            TimeoutPolicy<TResult> timeout = policy as TimeoutPolicy<TResult> ?? throw new ArgumentException($"policy supplied to {this.GetType().Name} is not a {typeof(TimeoutPolicy<TResult>).Name}", nameof(policy));

            return new TimeoutSyncImplementation<TResult>(timeout);
        }
    }
}
