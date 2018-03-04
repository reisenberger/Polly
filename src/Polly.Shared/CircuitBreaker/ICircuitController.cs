using System;

namespace Polly.CircuitBreaker
{
    /// <summary>
    /// Defines a circuit controller which can control when a circuit-breaker should transition between closed, half-open and open states, in response to failures and successes experienced by actions executed through the circuit.
    /// <remarks>The interface defines four operations:
    /// - Reset any internal statistics governing the circuit
    /// - Respond to a successful execution through the circuit
    /// - Respond to an execution through the circuit resulting in a handled failure
    /// - In half-open state, indicate whether another trial execution should be permitted
    /// </remarks>
    /// </summary>
    public interface ICircuitController
    {
        /// <summary>
        /// Called when the circuit is reset. The <see cref="ICircuitController"/> implementation should reset any internal statistics.
        /// This method is called in three circumstances: at circuit instantiation; at the typical transition from HalfOpen back to Closed state; when the circuit is manually reset by an external call to <see cref="M:ICircuitBreakerPolicy.Reset()"/>
        /// <remarks>A lock is held externally while this method is called.</remarks>
        /// </summary>
        void ResetCircuitStatistics_WithinLock();

        /// <summary>
        /// Called when an action has executed successfully through the circuit.  
        /// The <see cref="ICircuitController"/> implmentation may trigger a change of circuit state by returning a <see cref="CircuitState"/> different from the passed <paramref name="currentState"/>.
        /// <remarks>A lock is held externally while this method is called.</remarks>
        /// </summary>
        /// <param name="currentState">The state of the circuit at the time the action succeeded.</param>
        /// <returns>A <see cref="CircuitState"/> to transition to (if different from the passed <paramref name="currentState"/>). </returns>
        CircuitState OnActionSuccess_WithinLock(CircuitState currentState);

        /// <summary>
        /// Called when an action executed through the circuit has resulted in a failure handled by the circuit.  
        /// The <see cref="ICircuitController"/> implmentation may trigger a change of circuit state by returning a <see cref="CircuitState"/> different from the passed <paramref name="currentState"/>.
        /// <remarks>A lock is held externally while this method is called.</remarks>
        /// </summary>
        /// <param name="currentState">The state of the circuit at the time the action resulted in a handled failure.</param>
        /// <returns>A <see cref="CircuitState"/> to transition to (if different from the passed <paramref name="currentState"/>). </returns>
        CircuitState OnActionHandledFailure_WithinLock(CircuitState currentState);

        /// <summary>
        /// Called when an action is requested to be executed, in half-open state.  The method may return true or false to indicate the actioned should be attempted, or not.
        /// </summary>
        /// <param name="durationOfBreak">The duration of break configured in the circuit, ticks.
        /// <remarks>Circuit controllers may choose to allow 1, or a small number, of executions per break duration; this parameter allows them to calculate that.</remarks></param>
        /// <returns>true, if the action should be permitted; false, if it should not.</returns>
        bool PermitHalfOpenCircuitTest(TimeSpan durationOfBreak);
    }
}
