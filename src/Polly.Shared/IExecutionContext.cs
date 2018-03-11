using System;
using Polly.Wrap;

namespace Polly
{
    /// <summary>
    /// Represents context that carries with a single execution through a Policy or PolicyWrap.   
    /// </summary>
    public interface IExecutionContext
    {
        /// <summary>
        /// When execution is through a <see cref="PolicyWrap"/>, identifies the PolicyWrap executing the current delegate by returning the <see cref="PolicyBase.PolicyKey"/> of the outermost layer in the PolicyWrap; otherwise, null.
        /// </summary>
        String PolicyWrapKey { get; }

        /// <summary>
        /// The <see cref="PolicyBase.PolicyKey"/> of the <see cref="Policy"/> instance executing the current delegate.
        /// </summary>
        String PolicyKey { get; }

        /// <summary>
        /// A key unique to the operation or call site of this usage of the policy. 
        /// <remarks><see cref="Policy"/> instances are commonly reused across multiple call sites.  An OperationKey is set to distinguish the different operations that policy instances are being used for in logging and metrics.</remarks>
        /// </summary>
        String OperationKey { get; }

        /// <summary>
        /// A Guid guaranteed to be unique to each execution.
        /// <remarks>Acts as a correlation id so that events specific to a single execution can be identified in logging and telemetry.</remarks>
        /// </summary>
        Guid CorrelationId { get; }
    }
}