using System;

namespace Polly.CircuitBreaker
{
    internal class ConsecutiveCountCircuitBreakerConfigurationValidator : IConsecutiveCountCircuitBreakerConfiguration
    {
        private IConsecutiveCountCircuitBreakerConfiguration _wrappedConfigurationProvider;

        public ConsecutiveCountCircuitBreakerConfigurationValidator(IConsecutiveCountCircuitBreakerConfiguration configurationProvider)
        {
            _wrappedConfigurationProvider = configurationProvider;
        }

        public int FailuresAllowedBeforeBreaking
        {
            get
            {
                int wrappedValue = _wrappedConfigurationProvider.FailuresAllowedBeforeBreaking;
                if (wrappedValue <= 0) throw new InvalidOperationException("IConsecutiveCountCircuitBreakerConfiguration.FailuresAllowedBeforeBreaking must provide values greater than zero.");
                return wrappedValue;
            }
        }

        public TimeSpan DurationOfBreak
        {
            get
            {
                TimeSpan wrappedValue = _wrappedConfigurationProvider.DurationOfBreak;
                if (wrappedValue < TimeSpan.Zero) throw new InvalidOperationException("IConsecutiveCountCircuitBreakerConfiguration.DurationOfBreak must provide values greater than or equal to zero.");
                return wrappedValue;
            }
        }
    }
}
