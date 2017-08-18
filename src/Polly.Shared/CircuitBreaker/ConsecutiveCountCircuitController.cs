using System;
using Polly.Utilities;

namespace Polly.CircuitBreaker
{
    internal class ConsecutiveCountCircuitController<TResult> : CircuitStateController<TResult>
    {
        private readonly IConsecutiveCountCircuitBreakerConfiguration _configuration;
        private int _consecutiveFailureCount;

        public ConsecutiveCountCircuitController(
            IConsecutiveCountCircuitBreakerConfiguration configuration, 
            Action<DelegateResult<TResult>, TimeSpan, Context> onBreak, 
            Action<Context> onReset, 
            Action onHalfOpen
            ) : base(configuration, onBreak, onReset, onHalfOpen)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public override void OnCircuitReset(Context context)
        {
            using (TimedLock.Lock(_lock))
            {
                _consecutiveFailureCount = 0;

                ResetInternal_NeedsLock(context);
            }
        }

        public override void OnActionSuccess(Context context)
        {
            using (TimedLock.Lock(_lock))
            {
                switch (_circuitState)
                {
                    case CircuitState.HalfOpen:
                        OnCircuitReset(context);
                        break;

                    case CircuitState.Closed:
                        _consecutiveFailureCount = 0;
                        break;

                    case CircuitState.Open:
                    case CircuitState.Isolated:
                        break; // A successful call result may arrive when the circuit is open, if it was placed before the circuit broke.  We take no action; only time passing governs transitioning from Open to HalfOpen state.

                    default:
                        throw new InvalidOperationException("Unhandled CircuitState.");
                }
            }
        }

        public override void OnActionFailure(DelegateResult<TResult> outcome, Context context)
        {
            using (TimedLock.Lock(_lock))
            {
                _lastOutcome = outcome;

                switch (_circuitState)
                {
                    case CircuitState.HalfOpen:
                        Break_NeedsLock(context);
                        return;

                    case CircuitState.Closed:
                        _consecutiveFailureCount += 1;
                        if (_consecutiveFailureCount >= _configuration.FailuresAllowedBeforeBreaking)
                        {
                            Break_NeedsLock(context);
                        }
                        break;

                    case CircuitState.Open:
                    case CircuitState.Isolated:
                        break; // A failure call result may arrive when the circuit is open, if it was placed before the circuit broke.  We take no action; we do not want to duplicate-signal onBreak; we do not want to extend time for which the circuit is broken.  We do not want to mask the fact that the call executed (as replacing its result with a Broken/IsolatedCircuitException would do).

                    default:
                        throw new InvalidOperationException("Unhandled CircuitState.");
                }


            }
        }
    }
}
