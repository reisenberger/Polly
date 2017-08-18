using System;
using System.Collections.Generic;
using System.Text;

namespace Polly.CircuitBreaker
{
    /// <summary>
    /// Describes the configuration parameters of a <see cref="CircuitBreakerPolicy"/>
    /// </summary>
    public interface IConsecutiveCountCircuitBreakerConfiguration : ICircuitBreakerConfiguration
    {
        /// <summary>
        /// Gets the exceptions or handled results allowed before breaking.
        /// </summary>
        int FailuresAllowedBeforeBreaking { get; }
    }
}
