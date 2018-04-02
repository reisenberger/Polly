using System;
using Polly.Events;
using FluentAssertions;
using Polly.Utilities;
using Xunit;

namespace Polly.Specs.Events
{
    [Collection(Polly.Specs.Helpers.Constants.AbstractedTimeDependentTestCollection)]
    public class PolicyEventSpecs : IDisposable
    {
        private class SomePolicyEvent : PolicyEvent
        {
            public SomePolicyEvent(Context context) : base(context) { }
        }

        [Fact]
        public void Should_construct_event_with_SourceTimestamp_from_SystemClock()
        {
            DateTimeOffset now = DateTimeOffset.Now;
            SystemClock.DateTimeOffsetUtcNow = () => now;

            SomePolicyEvent ev = new SomePolicyEvent(new Context());

            ev.SourceTimestamp.Should().Be(now);
        }

        [Fact]
        public void Should_construct_with_passed_Context()
        {
            string operationKey = "someOpKey";
            string policyKey = "somePolicyKey";
            string policyWrapKey = "somePolicyWrapKey";

            Context context = new Context(operationKey)
            {
                PolicyKey = policyKey,
                PolicyWrapKey = policyWrapKey,
            };

            SomePolicyEvent ev = new SomePolicyEvent(context);

            ev.PolicyKey.Should().Be(context.PolicyKey);
            ev.PolicyWrapKey.Should().Be(context.PolicyWrapKey);
            ev.OperationKey.Should().Be(context.OperationKey);
            ev.CorrelationId.Should().Be(context.CorrelationId);
        }

        public void Dispose()
        {
            SystemClock.Reset();
            SystemStopwatch.Reset();
        }

    }
}
