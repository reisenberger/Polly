namespace Polly.NoOp
{
    internal class NoOpAsyncImplementationFactory : IAsyncPolicyImplementationFactory
    {
        public IAsyncPolicyImplementation<TResult> GetImplementation<TResult>(IAsyncPolicy policy)
        {
            return new NoOpAsyncImplementation<TResult>();
        }
    }

    internal class NoOpAsyncImplementationFactory<TResult> : IAsyncPolicyImplementationFactory<TResult>
    {
        public IAsyncPolicyImplementation<TResult> GetImplementation(IAsyncPolicy<TResult> policy)
        {
            return new NoOpAsyncImplementation<TResult>();
        }
    }
}
