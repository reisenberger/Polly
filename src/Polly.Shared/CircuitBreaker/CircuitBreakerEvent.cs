using System;
using System.Collections.Generic;
using System.Text;

namespace Polly.CircuitBreaker
{
    /// <summary>
    /// Defines events broadcast by <see cref="CircuitBreakerPolicy"/>s.
    /// </summary>
    public static class CircuitBreakerEvent
    {
        // Events are strings rather than enums, because, with an enum, if we ever publish an IPolicy interface, and then let users define their own Policys, they would not be able to extend an enum to add custom events for their custom policy.  And enums would effectively get serialized to strings anyway when events emitted eg by json.

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member
        public const String CircuitHalfOpened = "CircuitBreakerEvent_CircuitHalfOpened";
        public const String CircuitOpened     = "CircuitBreakerEvent_CircuitOpened";
        public const String CircuitClosed     = "CircuitBreakerEvent_CircuitClosed";
#pragma warning restore 1591 // Missing XML comment for publicly visible type or member
    }
}
