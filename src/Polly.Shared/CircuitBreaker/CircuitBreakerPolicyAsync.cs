
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Polly.Utilities;

namespace Polly.CircuitBreaker
{
    public partial class CircuitBreakerPolicy : ICircuitBreakerPolicy
    {
        internal CircuitBreakerPolicy(
            PolicyBuilder builder,
            Func<IAsyncPolicy, IAsyncPolicyImplementation<Object>> factory
        ) : base(builder, factory)
        {
            _implementation = (ICircuitBreakerImplementationInternal)_nonGenericAsyncImplementation;
        }
    }

    public partial class CircuitBreakerPolicy<TResult> : ICircuitBreakerPolicy<TResult>
    {
        internal CircuitBreakerPolicy(
            PolicyBuilder<TResult> builder,
            Func<IAsyncPolicy<TResult>, IAsyncPolicyImplementation<TResult>> factory
        ) : base(builder, factory)
        {
            _implementation = (ICircuitBreakerImplementationInternal<TResult>) _genericAsyncImplementation;
        }
    }
}
