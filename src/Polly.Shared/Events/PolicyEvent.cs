using System;
using System.Collections.Generic;
using System.Text;
using Polly.Wrap;

namespace Polly.Events
{
    /// <summary>
    /// Represents an event broadcast during execution of a Policy.
    /// </summary>
    public struct PolicyEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyEvent"/> struct.
        /// </summary>
        /// <param name="event">The policy event.</param>
        /// <param name="context">The context.</param>
        internal PolicyEvent(string @event, Context context)
        {
            Event = @event;
            PolicyWrapKey = context.PolicyWrapKey;
            PolicyKey = context.PolicyKey;
            ExecutionKey = context.ExecutionKey;
            ExecutionGuid = context.ExecutionGuid;
        }

        /// <summary>
        /// The policy event
        /// </summary>
        public String Event; // Events are strings rather than enums, because if we ever publish an IPolicy interface, and then let users define their own Policys, they would not be able to extend an enum to add custom events for their custom policy.  Enums would effectively get serialized to strings anyway when events emitted eg by json.

        /// <summary>
        /// A key unique to the outermost <see cref="PolicyWrap"/> instance involved in the current PolicyWrap execution.
        /// </summary>
        public String PolicyWrapKey;

        /// <summary>
        /// A key unique to the <see cref="Policy"/> instance executing the current delegate.
        /// </summary>
        public String PolicyKey;

        /// <summary>
        /// A key unique to the call site of the current execution. 
        /// <remarks>The value is set </remarks>
        /// </summary>
        public String ExecutionKey;

        /// <summary>
        /// A Guid guaranteed to be unique to each execution.
        /// </summary>
        public Guid ExecutionGuid;
    }
}
