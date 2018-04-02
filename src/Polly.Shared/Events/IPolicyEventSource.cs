using System;

namespace Polly.Events
{
    /// <summary>
    /// Defines properties and methods for being a source of <see cref="IPolicyEvent"/>s.
    /// </summary>
    public interface IPolicyEventSource
    {
        /// <summary>
        /// An event that will be raised for each <see cref="IPolicyEvent"/>
        /// </summary>
        event EventHandler<IPolicyEvent> ExecutionEventRaised;
        
        /// <summary>
        /// Whether the <see cref="IPolicyEventSource"/> is enabled to emit events.
        /// </summary>
        bool EventsEnabled { get; }
    }
}
