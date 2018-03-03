using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.Fallback
{
    interface IFallbackPolicyInternal<TResult>
    {
        Action<DelegateResult<TResult>, Context> OnFallback { get; }
        Func<DelegateResult<TResult>, Context, CancellationToken, TResult> FallbackAction { get; }

        Func<DelegateResult<TResult>, Context, Task> OnFallbackAsync { get; }
        Func<DelegateResult<TResult>, Context, CancellationToken, Task<TResult>> FallbackActionAsync { get; }
    }
}
