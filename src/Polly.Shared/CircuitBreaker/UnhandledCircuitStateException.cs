using System;
#if !PORTABLE
using System.Runtime.Serialization;
#endif

namespace Polly.CircuitBreaker
{
    /// <summary>
    /// Exception thrown when a circuit is broken.
    /// </summary>
#if !PORTABLE
    [Serializable]
#endif
    public class UnhandledCircuitStateException : InvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledCircuitStateException"/> class.
        /// </summary>
        public UnhandledCircuitStateException(CircuitState unhandledState) : base(
            $"Unhandled CircuitState: {unhandledState}.")
        {
        }

#if !PORTABLE
        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledCircuitStateException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected UnhandledCircuitStateException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
#endif
    }
}