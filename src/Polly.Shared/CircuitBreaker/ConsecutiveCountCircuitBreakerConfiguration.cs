using System;

namespace Polly.CircuitBreaker
{
    /// <summary>
    /// Describes the configuration parameters for a <see cref="CircuitBreakerPolicy"/>
    /// </summary>
    internal class ConsecutiveCountCircuitBreakerConfiguration : CircuitBreakerConfiguration, IConsecutiveCountCircuitBreakerConfiguration
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsecutiveCountCircuitBreakerConfiguration"/> class.
        /// </summary>
        /// <param name="failuresAllowedBeforeBreaking">The exceptions or handled results allowed before breaking.</param>
        /// <param name="durationOfBreak">The duration of break.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">failuresAllowedBeforeBreaking;Value must be greater than zero.</exception>
        public ConsecutiveCountCircuitBreakerConfiguration(int failuresAllowedBeforeBreaking, TimeSpan durationOfBreak)
            : base(durationOfBreak)
        {
            if (failuresAllowedBeforeBreaking <= 0) throw new ArgumentOutOfRangeException(nameof(failuresAllowedBeforeBreaking), "Value must be greater than zero.");

            FailuresAllowedBeforeBreaking = failuresAllowedBeforeBreaking;
        }

        /// <summary>
        /// The exceptions or handled results allowed before breaking
        /// </summary>
        public int FailuresAllowedBeforeBreaking { get; }

    }
}
