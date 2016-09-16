using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Polly.Utilities;

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
             Action<Action<CancellationToken>, Context, CancellationToken> exceptionPolicy) : base(exceptionPolicy, PredicateHelper.EmptyExceptionPredicates)
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
            ) : base(executionPolicy, PredicateHelper.EmptyExceptionPredicates, PredicateHelper<TResult>.EmptyResultPredicates)
        {
        }
    }
}
