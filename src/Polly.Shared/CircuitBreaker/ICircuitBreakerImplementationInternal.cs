using System;

namespace Polly.CircuitBreaker
{
    internal interface ICircuitBreakerImplementationInternal
    {
        CircuitState CircuitState { get; }

        Exception LastException { get; }

        void Isolate();

        void Reset();

        void OnActionPreExecute();
    }

    internal interface ICircuitBreakerImplementationInternal<out TResult> : ICircuitBreakerImplementationInternal
    {
        TResult LastHandledResult { get; }
    }
}