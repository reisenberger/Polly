
namespace System.Threading
{
    using System;
    using System.Runtime.Serialization;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Class SemaphoreRejectedException.
    /// </summary>
#if !PORTABLE
    [Serializable()]
#endif
    [ComVisibleAttribute(false)]
    public class SemaphoreRejectedException :
#if PORTABLE
        Exception
#else        
        SystemException
#endif
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SemaphoreRejectedException" /> class.
        /// </summary>
        public SemaphoreRejectedException() : this("The semaphore is full and the call was rejected.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SemaphoreRejectedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public SemaphoreRejectedException(String message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SemaphoreRejectedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public SemaphoreRejectedException(String message, Exception innerException) : base(message, innerException)
        {
        }

#if !PORTABLE
        /// <summary>
        /// Initializes a new instance of the <see cref="SemaphoreRejectedException"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="context">The context.</param>
        protected SemaphoreRejectedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }
}
