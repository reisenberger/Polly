using System;

namespace Polly.Events
{
    /// <summary>
    /// An abstract base class for events which measure the elapsed of something.
    /// </summary>
    public abstract class TimingPolicyEvent : PolicyEvent
    {
        /// <summary>
        /// Creates a new instance of a <see cref="TimingPolicyEvent"/>
        /// </summary>
        /// <param name="elapsed">The elapsed time recorded in this event.</param>
        /// <param name="context">The execution context.</param>
        protected TimingPolicyEvent(TimeSpan elapsed, Context context) : base(context)
        {
            Elapsed = elapsed;
        }

        /// <summary>
        /// The elapsed time recorded in this event.
        /// </summary>
        public TimeSpan Elapsed { get; }
    }
}