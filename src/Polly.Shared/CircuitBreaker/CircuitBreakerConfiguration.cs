using System;
using System.Collections.Generic;
using System.Text;

namespace Polly.CircuitBreaker
{
    /// <summary>
    /// Describes the configuration parameters for any member of the circuit-breaker family of policies
    /// </summary>
    public abstract class CircuitBreakerConfiguration : ICircuitBreakerConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CircuitBreakerConfiguration"/> class.
        /// </summary>
        /// <param name="durationOfBreak">The duration of break.</param>
        /// <exception cref="ArgumentOutOfRangeException">durationOfBreak;Value must be greater than zero</exception>
        protected CircuitBreakerConfiguration(TimeSpan durationOfBreak)
        {
            if (durationOfBreak < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(durationOfBreak), "Value must be greater than zero.");

            DurationOfBreak = durationOfBreak;
        }

        /// <summary>
        /// The duration of break
        /// </summary>
        public TimeSpan DurationOfBreak { get; }
    }
}
