using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Polly.Throttle
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ThrottlePolicy : Policy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptionPolicy"></param>
        internal ThrottlePolicy(
             Action<Action<CancellationToken>, Context, CancellationToken> exceptionPolicy) : base(exceptionPolicy, Enumerable.Empty<ExceptionPredicate>())
        {

        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public partial class ThrottlePolicy<TResult> : Policy<TResult>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionPolicy"></param>
        internal ThrottlePolicy(
            Func<Func<CancellationToken, TResult>, Context, CancellationToken, TResult> executionPolicy
            ) : base(executionPolicy, Enumerable.Empty<ExceptionPredicate>(), Enumerable.Empty<ResultPredicate<TResult>>())
        {
        }
    }
}
