using System;
using System.Collections.Generic;
using System.Text;

namespace Polly.CircuitBreaker
{
    /// <summary>
    /// Describes the configuration parameters common to all members of the <see cref="CircuitBreakerPolicy"/> family.
    /// </summary>
    public interface ICircuitBreakerConfiguration
    {
        /// <summary>
        /// The duration the circuit will stay open before resetting.
        /// </summary>
        TimeSpan DurationOfBreak { get; }
    }
}
