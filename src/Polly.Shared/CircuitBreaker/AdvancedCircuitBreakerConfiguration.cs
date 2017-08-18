using Polly.Utilities;
using System;

namespace Polly.CircuitBreaker
{
    /// <summary>
    /// Describes the configuration parameters of a <see cref="CircuitBreakerPolicy"/>
    /// </summary>
    internal class AdvancedCircuitBreakerConfiguration : CircuitBreakerConfiguration, IAdvancedCircuitBreakerConfiguration
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsecutiveCountCircuitBreakerConfiguration"/> class.
        /// </summary>
        /// <param name="failureThreshold">The failure threshold at which the circuit will break (a number between 0 and 1; eg 0.5 represents breaking if 50% or more of actions result in a handled failure.</param>
        /// <param name="samplingDuration">The duration of the timeslice over which failure ratios are assessed.</param>
        /// <param name="minimumThroughput">The minimum throughput: this many actions or more must pass through the circuit in the timeslice, for statistics to be considered significant and the circuit-breaker to come into action.</param>
        /// <param name="durationOfBreak">The duration of break.</param>
        /// <exception cref="ArgumentOutOfRangeException">failureThreshold;Value must be greater than zero</exception>
        /// <exception cref="ArgumentOutOfRangeException">failureThreshold;Value must be less than or equal to one</exception>
        /// <exception cref="ArgumentOutOfRangeException">samplingDuration;Value must be equal to or greater than the minimum resolution of the CircuitBreaker timer</exception>
        /// <exception cref="ArgumentOutOfRangeException">minimumThroughput;Value must be greater than one</exception>
        public AdvancedCircuitBreakerConfiguration(
            double failureThreshold,
            TimeSpan samplingDuration,
            int minimumThroughput, 
            TimeSpan durationOfBreak)
            : base(durationOfBreak)
        {
            var resolutionOfCircuit = TimeSpan.FromTicks(AdvancedCircuitController<EmptyStruct>.ResolutionOfCircuitTimer);

            if (failureThreshold <= 0) throw new ArgumentOutOfRangeException(nameof(failureThreshold), "Value must be greater than zero.");
            if (failureThreshold > 1) throw new ArgumentOutOfRangeException(nameof(failureThreshold), "Value must be less than or equal to one.");
            if (samplingDuration < resolutionOfCircuit) throw new ArgumentOutOfRangeException(nameof(samplingDuration),
                $"Value must be equal to or greater than {resolutionOfCircuit.TotalMilliseconds} milliseconds. This is the minimum resolution of the CircuitBreaker timer.");
            if (minimumThroughput <= 1) throw new ArgumentOutOfRangeException(nameof(minimumThroughput), "Value must be greater than one.");

            FailureThreshold = failureThreshold;
            SamplingDuration = samplingDuration;
            MinimumThroughput = minimumThroughput;
        }


        /// <summary>
        /// The failure threshold at which the circuit will break (a number between 0 and 1; eg 0.5 represents breaking if 50% or more of actions result in a handled failure.
        /// </summary>
        public double FailureThreshold { get; }

        /// <summary>
        /// The duration of the timeslice over which failure ratios are assessed.
        /// </summary>
        public TimeSpan SamplingDuration { get; }

        /// <summary>
        /// The minimum throughput: this many actions or more must pass through the circuit in the timeslice, for statistics to be considered significant and the circuit-breaker to come into action.
        /// </summary>
        public int MinimumThroughput { get; }

    }
}
