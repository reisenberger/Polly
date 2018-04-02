using System;
using Polly.Events;

namespace Polly
{
    public partial class PolicyBase : IPolicyEventSource
    {
        /// <inheritdoc/>
        public event EventHandler<IPolicyEvent> ExecutionEventRaised;

        /// <inheritdoc/>
        /// <remarks>
        /// This property is indicative that no event sinks are subscribed to this <see cref="Policy"/>.  
        /// It is intended to allow policies to avoid the overhead of creating events when there are no subscribers, with code such as:
        ///    <code>if (EventsEnabled) { OnExecutionEvent(new SomeEvent(/* params*/)); }</code>
        /// This approach is useful because the default pattern with Polly is likely to be that event sinks are attached at application startup; or certainly before each execution.
        /// However, the actual code emitting events should still be thread-safe for the concurrent addition or removal of subscribers.
        /// </remarks>
        public bool EventsEnabled => ExecutionEventRaised != null;

        /// <summary>
        /// A method for raising an <see cref="IPolicyEvent"/> thread-safely, regardless of whether there are subscribers.
        /// </summary>
        /// <param name="event">The event to raise.</param>
        internal void OnExecutionEvent(IPolicyEvent @event)
        {
            if (@event == null) throw new NullReferenceException(nameof(@event));

            ExecutionEventRaised?.Invoke(this, @event); // Raise event safely: see https://codeblog.jonskeet.uk/2015/01/30/clean-event-handlers-invocation-with-c-6/ . We need to be null-ref-exc safe, but we intentionally don't use the more complex patterns discussed by Jon Skeet to get the "absolute latest" set of subscribers, as expect the default pattern with Polly to be that event sinks are attached before Policy use.
        }

    }
}
