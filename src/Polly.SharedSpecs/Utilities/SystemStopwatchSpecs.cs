using System;
using System.Threading;
using FluentAssertions;
using Polly.Utilities;
using Xunit;

namespace Polly.Specs.Utilities
{
    [Collection(Polly.Specs.Helpers.Constants.AbstractedTimeDependentTestCollection)]
    public class SystemStopwatchSpecs : IDisposable
    {
        [Fact]
        public void Should_give_realistic_elapsed_value_with_default_implementation()
        {
            long start = SystemStopwatch.StopwatchTimestamp();

            TimeSpan expectedDuration = TimeSpan.FromSeconds(1);
            SystemClock.Sleep(expectedDuration, CancellationToken.None);

            SystemStopwatch.ElapsedSince(start).Should().BeCloseTo(expectedDuration, precision: 250); // Somewhat broad precision, to allow for variability in running, in CI environments.
        }

        [Fact]
        public void Should_give_custom_elapsed_value_with_overridden_implementation()
        {
            SystemStopwatch.StopwatchFrequency = TimeSpan.TicksPerSecond / TimeSpan.TicksPerMillisecond; // Override with a custom implementation scaled in Milliseconds.

            long start = 0;
            SystemStopwatch.StopwatchTimestamp = () => start + SystemStopwatch.StopwatchFrequency; // Manually advance the exact units for one second.

            SystemStopwatch.ElapsedSince(0).Should().Be(TimeSpan.FromSeconds(1));
        }



        public void Dispose()
        {
            SystemStopwatch.Reset();
        }

    }
}
