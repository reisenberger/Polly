namespace Polly.CircuitBreaker
{
    /// <summary>
    /// Describes the configuration parameters for a <see cref="CircuitBreakerPolicy"/>
    /// </summary>
    public interface IConsecutiveCountCircuitBreakerConfiguration : ICircuitBreakerConfiguration
    {
        /// <summary>
        /// Gets the exceptions or handled results allowed before breaking.
        /// </summary>
        int FailuresAllowedBeforeBreaking { get; }
    }
}
