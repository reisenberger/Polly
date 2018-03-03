using System;

namespace Polly.CircuitBreaker
{
    internal class AdvancedCircuitController : ICircuitController
    {
        private const short NumberOfWindows = 10;
        internal static readonly long ResolutionOfCircuitTimer = TimeSpan.FromMilliseconds(20).Ticks;

        private readonly IHealthMetrics _metrics;
        private readonly double _failureThreshold;
        private readonly int _minimumThroughput;

        public AdvancedCircuitController(
            double failureThreshold, 
            TimeSpan samplingDuration, 
            int minimumThroughput
            )
        {
            _metrics = samplingDuration.Ticks < ResolutionOfCircuitTimer * NumberOfWindows
                ? (IHealthMetrics)new SingleHealthMetrics(samplingDuration)
                : (IHealthMetrics)new RollingHealthMetrics(samplingDuration, NumberOfWindows);

            _failureThreshold = failureThreshold;
            _minimumThroughput = minimumThroughput;
        }

        public void ResetCircuitStatistics_WithinLock()
        {
            _metrics?.Reset_NeedsLock();
            // Is only null during initialization of the current class
            // as the variable is not set, before the base class calls
            // current method from constructor.
        }

        public CircuitState OnActionSuccess_WithinLock(CircuitState currentState)
        {
            switch (currentState)
            {
                case CircuitState.HalfOpen:
                    // A single success in half-open state closes the circuit again.
                    return CircuitState.Closed;

                case CircuitState.Closed:
                    break;

                case CircuitState.Open:
                case CircuitState.Isolated:
                    break; // A successful call result may arrive when the circuit is open, if it was placed before the circuit broke.  We take no action; only time passing governs transitioning from Open to HalfOpen state.

                default:
                    throw new UnhandledCircuitStateException(currentState);
            }

            _metrics.IncrementSuccess_NeedsLock();

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
                    _metrics.IncrementFailure_NeedsLock();
                    var healthCount = _metrics.GetHealthCount_NeedsLock();

                    int throughput = healthCount.Total;
                    if (throughput >= _minimumThroughput && ((double)healthCount.Failures) / throughput >= _failureThreshold)
                    {
                        return CircuitState.Open;
                    }
                    break;

                case CircuitState.Open:
                case CircuitState.Isolated:
                    _metrics.IncrementFailure_NeedsLock();
                    break; // A failure call result may arrive when the circuit is open, if it was placed before the circuit broke.  We take no action; we do not want to duplicate-signal onBreak; we do not want to extend time for which the circuit is broken.  We do not want to mask the fact that the call executed (as replacing its result with a Broken/IsolatedCircuitException would do).

                default:
                    throw new UnhandledCircuitStateException(currentState);
            }

            return currentState;
        }

    }
}
