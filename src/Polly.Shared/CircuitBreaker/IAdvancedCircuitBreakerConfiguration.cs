using System;

namespace Polly.CircuitBreaker
{
    /// <summary>
    /// Describes the configuration parameters for an advanced <see cref="CircuitBreakerPolicy"/>
    /// </summary>

    public interface IAdvancedCircuitBreakerConfiguration : ICircuitBreakerConfiguration
    {
        /// <summary>
        /// The failure threshold at which the circuit will break (a number between 0 and 1; eg 0.5 represents breaking if 50% or more of actions result in a handled failure.
        /// </summary>
        double FailureThreshold { get; }

        /// <summary>
        /// The duration of the timeslice over which failure ratios are assessed.
        /// </summary>
        TimeSpan SamplingDuration { get; }

        /// <summary>
        /// The minimum throughput: this many actions or more must pass through the circuit in the timeslice, for statistics to be considered significant and the circuit-breaker to come into action.
        /// </summary>
        int MinimumThroughput { get; }
    }
}
