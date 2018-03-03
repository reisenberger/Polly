using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Polly
{
    /// <summary>
    /// Implements elements common to both non-generic <see cref="Policy"/> and generic <see cref="Policy{TResult}"/>
    /// </summary>
    public abstract partial class PolicyBase
    {
        /// <summary>
        /// Predicates indicating which exceptions the policy should handle.
        /// </summary>
        protected internal IEnumerable<ExceptionPredicate> ExceptionPredicates { get; protected set; }

        /// <summary>
        /// Defines a CancellationToken to use, when none is supplied.
        /// </summary>
        protected internal static CancellationToken DefaultCancellationToken = CancellationToken.None;

        /// <summary>
        /// Defines a value to use for continueOnCaptureContext, when none is supplied.
        /// </summary>
        protected internal static bool DefaultContinueOnCapturedContext = false;

        internal Exception NotConfiguredForSyncExecution()
        {
            return new InvalidOperationException($"This {this.GetType().Name} policy is not configured for synchronous executions.");
        }

        internal Exception NotConfiguredForAsyncExecution()
        {
            return new InvalidOperationException($"This {this.GetType().Name} policy is not configured for asynchronous executions.");
        }
    }
}
