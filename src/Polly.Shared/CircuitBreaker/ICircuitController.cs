using System;
using System.Reactive.Subjects;
using Polly.Events;

namespace Polly.CircuitBreaker
{
    internal interface ICircuitController<TResult>
    {
        CircuitState GetCircuitState(Context context);
        Exception LastException { get; }
        TResult LastHandledResult { get; }
        ISubject<PolicyEvent> EventSubject { get; set; }
        void Isolate();
        void Reset();
        void OnCircuitReset(Context context);
        void OnActionPreExecute(Context context);
        void OnActionSuccess(Context context);
        void OnActionFailure(DelegateResult<TResult> outcome, Context context);
    }
}
