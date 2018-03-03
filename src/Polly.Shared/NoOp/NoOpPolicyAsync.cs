namespace Polly.NoOp
{
    /// <summary>
    /// A no op policy that can be applied to delegates.
    /// </summary>
    public partial class NoOpPolicy : Policy, INoOpPolicy
    {
        internal NoOpPolicy(NoOpAsyncImplementationFactory factory) : base(PolicyBuilder.Empty, factory)
        {
        }
    }

    public partial class NoOpPolicy<TResult> : INoOpPolicy<TResult>
    {
        internal NoOpPolicy(NoOpAsyncImplementationFactory<TResult> factory) : base(PolicyBuilder<TResult>.Empty, factory)
        {
        }
    }
}
