namespace Polly
{
    internal interface ISyncPolicyImplementationFactory
    {
        ISyncPolicyImplementation<TResult> GetImplementation<TResult>(ISyncPolicy policy);
    }

    internal interface ISyncPolicyImplementationFactory<TResult>
    {
        ISyncPolicyImplementation<TResult> GetImplementation(ISyncPolicy<TResult> policy);
    }

}
