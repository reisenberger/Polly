using System;

namespace Polly.Events
{
    /// <summary>
    /// Defines an event that may be emitted by a Polly <see cref="Policy"/>.
    /// </summary>
    public interface IPolicyEvent : IExecutionContext
    {
        /// <summary>
        /// A UTC timestamp stamped at source at the moment the event was raised.
        /// </summary>
        DateTimeOffset SourceTimestamp { get; }
    }
}