namespace Polly
{
    /// <summary>
    /// Defines a factory for providing asynchronous implementations for the given non-generic policy.
    /// </summary>
    public interface IAsyncPolicyImplementationFactory
    {
        /// <summary>
        /// Gets an implementation for <typeparamref name="TResult"/>-returning method-generic calls on the given non-generic policy.
        /// </summary>
        /// <typeparam name="TResult">The type of results returned by delegates executed through this implementation</typeparam>
        /// <param name="policy">The owning policy.</param>
        /// <returns>An implementation capable of handling executions returning an instance of <typeparamref name="TResult"/></returns>
        IAsyncPolicyImplementation<TResult> GetImplementation<TResult>(IAsyncPolicy policy);
    }

    /// <summary>
    /// Defines a factory for asynchronous implementations of the given policy, which is generic-typed to execute delegates returning an instance of <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of results returned by delegates executed through the owning policy.</typeparam>
    public interface IAsyncPolicyImplementationFactory<TResult>
    {
        /// <summary>
        /// Gets an implementation for calls on the given policy, which is generic-typed to execute delegates returning an instance of <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="policy">The owning policy.</param>
        /// <returns>An implementation capable of handling executions returning an instance of <typeparamref name="TResult"/></returns>
        IAsyncPolicyImplementation<TResult> GetImplementation(IAsyncPolicy<TResult> policy);
    }
}
