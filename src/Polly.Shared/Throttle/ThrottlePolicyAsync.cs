using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.Throttle
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ThrottlePolicy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="asyncExceptionPolicy"></param>
        internal ThrottlePolicy(Func<Func<CancellationToken, Task>, Context, CancellationToken, bool, Task> asyncExceptionPolicy)
           : base(asyncExceptionPolicy, Enumerable.Empty<ExceptionPredicate>())
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ThrottlePolicy<TResult>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="asyncExecutionPolicy"></param>
        internal ThrottlePolicy(
            Func<Func<CancellationToken, Task<TResult>>, Context, CancellationToken, bool, Task<TResult>> asyncExecutionPolicy
            ) : base(asyncExecutionPolicy, Enumerable.Empty<ExceptionPredicate>(), Enumerable.Empty<ResultPredicate<TResult>>())
        {
        }
    }
}