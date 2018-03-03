namespace Polly.CircuitBreaker
{
    internal class ConsecutiveCountCircuitController : ICircuitController
    {
        private readonly int _exceptionsAllowedBeforeBreaking;
        private int _consecutiveFailureCount;

        public ConsecutiveCountCircuitController(int exceptionsAllowedBeforeBreaking) 
        {
            _exceptionsAllowedBeforeBreaking = exceptionsAllowedBeforeBreaking;
        }

        public void ResetCircuitStatistics_WithinLock()
        {
            _consecutiveFailureCount = 0;
        }

        public CircuitState OnActionSuccess_WithinLock(CircuitState currentState)
        {
                switch (currentState)
                {
                    case CircuitState.HalfOpen:
                        // A single success in half-open state closes the circuit again.
                        return CircuitState.Closed;

                    case CircuitState.Closed:
                        _consecutiveFailureCount = 0;
                        break;

                    case CircuitState.Open:
                    case CircuitState.Isolated:
                        break; // A successful call result may arrive when the circuit is open, if it was placed before the circuit broke.  We take no action; only time passing governs transitioning from Open to HalfOpen state.

                    default:
                        throw new UnhandledCircuitStateException(currentState);
                }

            return currentState;
        }

        public CircuitState OnActionHandledFailure_WithinLock(CircuitState currentState)
        {
                switch (currentState)
                {
                    case CircuitState.HalfOpen:
                    // A single failure in HalfOpen causes reversion to Open.
                        return CircuitState.Open;

                    case CircuitState.Closed:
                        // Too many failures in Closed cause breaking, to Open.
                        _consecutiveFailureCount += 1;
                        if (_consecutiveFailureCount >= _exceptionsAllowedBeforeBreaking)
                        {
                            return CircuitState.Open;
                        }
                    break;

                    case CircuitState.Open:
                    case CircuitState.Isolated:
                        break; // A failure call result may arrive when the circuit is open, if it was placed before the circuit broke.  We take no action; we do not want to duplicate-signal onBreak; we do not want to extend time for which the circuit is broken.  We do not want to mask the fact that the call executed (as replacing its result with a Broken/IsolatedCircuitException would do).

                    default:
                        throw new UnhandledCircuitStateException(currentState);
                }

            return currentState;
        }

    }
}
