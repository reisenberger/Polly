using System;

namespace Polly.CircuitBreaker
{
    /// <summary>
    /// A circuit-breaker policy that can be applied to delegates.
    /// </summary>
    public partial class CircuitBreakerPolicy : Policy, ICircuitBreakerPolicy
    {
        internal readonly ICircuitBreakerImplementationInternal _implementation;

        internal CircuitBreakerPolicy(
            PolicyBuilder builder, 
            Func<ISyncPolicy, ISyncPolicyImplementation<Object>> factory
            ) : base(builder, factory)
        {
            _implementation = (ICircuitBreakerImplementationInternal) _nonGenericSyncImplementation;
        }

        /// <summary>
        /// Gets the state of the underlying circuit.
        /// </summary>
        public CircuitState CircuitState => _implementation.CircuitState;

        /// <summary>
        /// Gets the last exception handled by the circuit-breaker.
        /// <remarks>This will be null if no exceptions have been handled by the circuit-breaker since the circuit last closed.</remarks>
        /// </summary>
        public Exception LastException => _implementation.LastException;

        /// <summary>
        /// Isolates (opens) the circuit manually, and holds it in this state until a call to <see cref="Reset()"/> is made.
        /// </summary>
        public void Isolate() => _implementation.Isolate();

        /// <summary>
        /// Closes the circuit, and resets any statistics controlling automated circuit-breaking.
        /// </summary>
        public void Reset() => _implementation.Reset();
    }

    /// <summary>
    /// A circuit-breaker policy that can be applied to delegates returning a value of type <typeparamref name="TResult"/>.
    /// </summary>
    public partial class CircuitBreakerPolicy<TResult> : Policy<TResult>, ICircuitBreakerPolicy<TResult>
    {
        internal readonly ICircuitBreakerImplementationInternal<TResult> _implementation;

        internal CircuitBreakerPolicy(
            PolicyBuilder<TResult> builder,
            Func<ISyncPolicy<TResult>, ISyncPolicyImplementation<TResult>> factory
            ) : base(builder, factory)
        {
            _implementation = (ICircuitBreakerImplementationInternal<TResult>)_genericImplementation;
        }

        /// <summary>
        /// Gets the state of the underlying circuit.
        /// </summary>
        public CircuitState CircuitState => _implementation.CircuitState;

        /// <summary>
        /// Gets the last exception handled by the circuit-breaker.
        /// <remarks>This will be null if no exceptions have been handled by the circuit-breaker since the circuit last closed, or if the last event handled by the circuit was a handled <typeparamref name="TResult"/> value.</remarks>
        /// </summary>
        public Exception LastException => _implementation.LastException;

        /// <summary>
        /// Gets the last result returned from a user delegate which the circuit-breaker handled.
        /// <remarks>This will be default(<typeparamref name="TResult"/>) if no results have been handled by the circuit-breaker since the circuit last closed, or if the last event handled by the circuit was an exception.</remarks>
        /// </summary>
        public TResult LastHandledResult => _implementation.LastHandledResult;

        /// <summary>
        /// Isolates (opens) the circuit manually, and holds it in this state until a call to <see cref="Reset()"/> is made.
        /// </summary>
        public void Isolate() => _implementation.Isolate();

        /// <summary>
        /// Closes the circuit, and resets any statistics controlling automated circuit-breaking.
        /// </summary>
        public void Reset() => _implementation.Reset();
    }

}
