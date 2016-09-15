using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Polly.Bulkhead
{
    /// <summary>
    /// 
    /// </summary>
    public partial class BulkheadPolicy : Policy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptionPolicy"></param>
        internal BulkheadPolicy(
             Action<Action<CancellationToken>, Context, CancellationToken> exceptionPolicy) : base(exceptionPolicy, Enumerable.Empty<ExceptionPredicate>())
        {

        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public partial class BulkheadPolicy<TResult> : Policy<TResult>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionPolicy"></param>
        internal BulkheadPolicy(
            Func<Func<CancellationToken, TResult>, Context, CancellationToken, TResult> executionPolicy
            ) : base(executionPolicy, Enumerable.Empty<ExceptionPredicate>(), Enumerable.Empty<ResultPredicate<TResult>>())
        {
        }
    }
}
