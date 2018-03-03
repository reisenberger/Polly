namespace Polly
{
    internal interface IAsyncPolicyImplementationFactory
    {
        IAsyncPolicyImplementation<TResult> GetImplementation<TResult>(IAsyncPolicy policy);
    }

    internal interface IAsyncPolicyImplementationFactory<TResult>
    {
        IAsyncPolicyImplementation<TResult> GetImplementation(IAsyncPolicy<TResult> policy);
    }
}
