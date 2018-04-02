using System;
using Polly;
using Polly.Events;
using FluentAssertions;
using Polly.NoOp;
using Xunit;

namespace Polly.Specs
{
    public class PolicyBaseEventsSpecs
    {
        private class SomePolicyEvent : PolicyEvent
        {
            public SomePolicyEvent(Context context) : base(context) { }
        }

        [Fact]
        public void Should_indicate_events_are_enabled_if_and_only_we_have_subscribers()
        {
            IPolicyEventSource policy = Policy.NoOp();
            policy.EventsEnabled.Should().BeFalse();

            EventHandler<IPolicyEvent> eventSink = (sender, ev) => { };

            policy.ExecutionEventRaised += eventSink;
            policy.EventsEnabled.Should().BeTrue();

            policy.ExecutionEventRaised -= eventSink;
            policy.EventsEnabled.Should().BeFalse();

        }

        [Fact]
        public void Should_be_able_to_call_ExecutionEventRaised_safely_even_if_no_subscribers()
        {
            NoOpPolicy policy = Policy.NoOp();
            policy.EventsEnabled.Should().BeFalse();

            policy.Invoking(p => p.OnExecutionEvent(new SomePolicyEvent(new Context()))).ShouldNotThrow();
        }
    }
}
