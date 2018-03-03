namespace Polly.NoOp
{
    internal class NoOpSyncImplementationFactory : ISyncPolicyImplementationFactory
    {
        public ISyncPolicyImplementation<TResult> GetImplementation<TResult>(ISyncPolicy policy)
        {
            return new NoOpSyncImplementation<TResult>();
        }
    }

    internal class NoOpSyncImplementationFactory<TResult> : ISyncPolicyImplementationFactory<TResult>
    {
        public ISyncPolicyImplementation<TResult> GetImplementation(ISyncPolicy<TResult> policy)
        {
            return new NoOpSyncImplementation<TResult>();
        }
    }
}
