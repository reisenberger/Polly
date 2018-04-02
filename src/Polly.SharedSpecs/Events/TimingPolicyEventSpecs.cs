using System;
using Polly.Events;
using FluentAssertions;
using Xunit;

namespace Polly.Specs.Events
{
    public class TimingPolicyEventSpecs
    {
        private class SomeTimingPolicyEvent : TimingPolicyEvent
        {
            public SomeTimingPolicyEvent(TimeSpan elapsed, Context context) : base(elapsed, context) { }
        }

        [Fact]
        public void Should_construct_event_with_passed_duration()
        {
            TimeSpan duration = TimeSpan.FromSeconds(10);

            SomeTimingPolicyEvent ev = new SomeTimingPolicyEvent(duration, new Context());

            ev.Elapsed.Should().Be(duration);
        }
    }
}
