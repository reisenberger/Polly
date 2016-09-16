using System;
using System.Threading;
using System.Threading.Tasks;
using Polly.Utilities;

namespace Polly.Bulkhead
{
    /// <summary>
    /// 
    /// </summary>
    public partial class BulkheadPolicy
    {
        internal BulkheadPolicy(Func<Func<CancellationToken, Task>, Context, CancellationToken, bool, Task> asyncExceptionPolicy)
           : base(asyncExceptionPolicy, PredicateHelper.EmptyExceptionPredicates)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class BulkheadPolicy<TResult>
    {
        internal BulkheadPolicy(
            Func<Func<CancellationToken, Task<TResult>>, Context, CancellationToken, bool, Task<TResult>> asyncExecutionPolicy
            ) : base(asyncExecutionPolicy, PredicateHelper.EmptyExceptionPredicates, PredicateHelper<TResult>.EmptyResultPredicates)
        {
        }
    }
}