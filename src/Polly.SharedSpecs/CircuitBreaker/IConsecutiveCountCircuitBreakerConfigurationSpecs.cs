using System;
using FluentAssertions;
using Polly.CircuitBreaker;
using Polly.Specs.Helpers;
using Xunit;

namespace Polly.Specs.CircuitBreaker
{
    public class IConsecutiveCountCircuitBreakerConfigurationSpecs
    {
        /// <summary>
        /// An intentionally naive dynamic implementation of IConsecutiveCountCircuitBreakerConfiguration, intended to be the simplest possible thing to support testing.  Not intended as a production implementation. Intended production implementations might update the configuration, eg from some external config source.
        /// </summary>
        public class ConsecutiveCountCircuitBreakerConfiguration : IConsecutiveCountCircuitBreakerConfiguration
        {
            /// <summary>
            /// The exceptions or handled results allowed before breaking
            /// </summary>
            public int FailuresAllowedBeforeBreaking { get; set; }

            /// <summary>
            /// The duration of break
            /// </summary>
            public TimeSpan DurationOfBreak { get; set; }
        }

        [Fact]
        public void Can_use_custom_IConsecutiveCountCircuitBreakerConfiguration()
        {
            var customConfiguration = new ConsecutiveCountCircuitBreakerConfiguration
            {
                FailuresAllowedBeforeBreaking = 1,
                DurationOfBreak = TimeSpan.FromMinutes(1)
            };

            CircuitBreakerPolicy breaker = Policy
                .Handle<DivideByZeroException>()
                .CircuitBreaker(customConfiguration, (_,__,___) => {}, _ => {}, () => {} );

            breaker.Invoking(x => x.RaiseException<DivideByZeroException>())
                .ShouldThrow<DivideByZeroException>();
            breaker.CircuitState.Should().Be(CircuitState.Open);
        }

        [Fact]
        public void Can_use_custom_IConsecutiveCountCircuitBreakerConfiguration_for_dynamic_reconfiguration_of_ExceptionsAllowedBeforeBreaking()
        {
            var customConfiguration = new ConsecutiveCountCircuitBreakerConfiguration
            {
                FailuresAllowedBeforeBreaking = 1,
                DurationOfBreak = TimeSpan.FromMinutes(1)
            };

            CircuitBreakerPolicy breaker = Policy
                .Handle<DivideByZeroException>()
                .CircuitBreaker(customConfiguration, (_, __, ___) => { }, _ => { }, () => { });

            breaker.Invoking(x => x.RaiseException<DivideByZeroException>())
                .ShouldThrow<DivideByZeroException>();
            breaker.CircuitState.Should().Be(CircuitState.Open); // A single exception should break breaker.

            // Now update the configuration dynamically
            customConfiguration.FailuresAllowedBeforeBreaking = 2;

            breaker.Reset(); // (no need to reset breaker when changing configuration; it just makes this test clearer)

            breaker.Invoking(x => x.RaiseException<DivideByZeroException>())
                .ShouldThrow<DivideByZeroException>();
            breaker.CircuitState.Should().Be(CircuitState.Closed); // A single exception should no longer break breaker.

            breaker.Invoking(x => x.RaiseException<DivideByZeroException>())
                .ShouldThrow<DivideByZeroException>();
            breaker.CircuitState.Should().Be(CircuitState.Open); // A second exception is now required to break breaker.
        }


        [Fact]
        public void Can_use_custom_IConsecutiveCountCircuitBreakerConfiguration_for_dynamic_reconfiguration_of_DurationOfBreak()
        {
            TimeSpan initialDurationOfBreak = TimeSpan.FromMinutes(1);
            var customConfiguration = new ConsecutiveCountCircuitBreakerConfiguration
            {
                FailuresAllowedBeforeBreaking = 1,
                DurationOfBreak = initialDurationOfBreak
            };

            TimeSpan? passedBreakTimespan = null;
            Action<Exception, TimeSpan, Context> onBreak = (_, timespan, __) => { passedBreakTimespan = timespan; };

            CircuitBreakerPolicy breaker = Policy
                .Handle<DivideByZeroException>()
                .CircuitBreaker(customConfiguration, onBreak, _ => { }, () => { });

            breaker.Invoking(x => x.RaiseException<DivideByZeroException>())
                .ShouldThrow<DivideByZeroException>();
            breaker.CircuitState.Should().Be(CircuitState.Open);

            passedBreakTimespan.Should().Be(initialDurationOfBreak);

            // Now update the configuration dynamically
            TimeSpan reconfiguredDurationOfBreak = TimeSpan.FromMinutes(2);
            customConfiguration.DurationOfBreak = reconfiguredDurationOfBreak;

            breaker.Reset(); // (no need to reset breaker when changing configuration; it just makes this test clearer)

            breaker.Invoking(x => x.RaiseException<DivideByZeroException>())
                .ShouldThrow<DivideByZeroException>();
            breaker.CircuitState.Should().Be(CircuitState.Open);

            passedBreakTimespan.Should().Be(reconfiguredDurationOfBreak);
        }
    }

}
