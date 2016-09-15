using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.Bulkhead
{
    /// <summary>
    /// 
    /// </summary>
    public partial class BulkheadPolicy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="asyncExceptionPolicy"></param>
        internal BulkheadPolicy(Func<Func<CancellationToken, Task>, Context, CancellationToken, bool, Task> asyncExceptionPolicy)
           : base(asyncExceptionPolicy, Enumerable.Empty<ExceptionPredicate>())
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class BulkheadPolicy<TResult>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="asyncExecutionPolicy"></param>
        internal BulkheadPolicy(
            Func<Func<CancellationToken, Task<TResult>>, Context, CancellationToken, bool, Task<TResult>> asyncExecutionPolicy
            ) : base(asyncExecutionPolicy, Enumerable.Empty<ExceptionPredicate>(), Enumerable.Empty<ResultPredicate<TResult>>())
        {
        }
    }
}