using System;
using System.Threading.Tasks;

namespace Polly.Timeout
{
    internal interface ITimeoutPolicyInternal
    {
        Func<Context, TimeSpan> TimeoutProvider { get; }
        TimeoutStrategy TimeoutStrategy { get; }
        Action<Context, TimeSpan, Task> OnTimeout { get; }
        Func<Context, TimeSpan, Task, Task> OnTimeoutAsync { get; }
    }
}