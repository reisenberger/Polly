using System;
using Polly.Utilities;

namespace Polly.Events
{
    /// <summary>
    /// An abstract base class for events that may be emitted by a <see cref="Policy"/>.
    /// </summary>

    public abstract class PolicyEvent : EventArgs, IPolicyEvent
    {
        /// <summary>
        /// Initialises a new instance of <see cref="PolicyEvent"/>.
        /// </summary>
        protected PolicyEvent(Context context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            SourceTimestamp = SystemClock.DateTimeOffsetUtcNow();

            PolicyWrapKey = context.PolicyWrapKey;
            PolicyKey = context.PolicyKey;
            OperationKey = context.OperationKey;
            CorrelationId = context.CorrelationId;
        }

        #region Timing information

        /// <inheritdoc/>
        public DateTimeOffset SourceTimestamp { get; }

        #endregion

        #region ExecutionContext information

        /// <inheritdoc/>
        public string PolicyWrapKey { get; }

        /// <inheritdoc/>
        public string PolicyKey { get; }

        /// <inheritdoc/>
        public string OperationKey { get; }

        /// <inheritdoc/>
        public Guid CorrelationId { get; }

        #endregion
    }
}