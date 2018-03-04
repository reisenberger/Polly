using System;
using System.Collections.Generic;
using System.Threading;
using Polly.Utilities;

namespace Polly.CircuitBreaker
{
    /// <summary>
    /// Represents behaviour common to circuit breaker implementations.
    /// </summary>
    internal abstract class CircuitBreakerImplementation<TResult> : ICircuitBreakerImplementationInternal<TResult>
    {
        protected readonly IsPolicy _policy;
        protected readonly IEnumerable<ExceptionPredicate> _shouldHandleExceptionPredicates;
        protected readonly IEnumerable<ResultPredicate<TResult>> _shouldHandleResultPredicates;

        protected readonly Action<DelegateResult<TResult>, CircuitState, TimeSpan, Context> _onBreak;
        protected readonly Action<Context> _onReset;
        protected readonly Action _onHalfOpen;

        protected readonly TimeSpan _durationOfBreak;
        protected long _blockedTill;
        protected CircuitState _circuitState;

        protected readonly ICircuitController _breakerController;

        protected readonly object _lock = new object();

        protected CircuitBreakerImplementation(IsPolicy policy,
            IEnumerable<ExceptionPredicate> shouldHandleExceptionPredicates,
            IEnumerable<ResultPredicate<TResult>> shouldHandleResultPredicates,
            ICircuitController breakerController,
            TimeSpan durationOfBreak,
            Action<DelegateResult<TResult>, CircuitState, TimeSpan, Context> onBreak,
            Action<Context> onReset,
            Action onHalfOpen
        )
        {
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
            _breakerController = breakerController ?? throw new ArgumentNullException(nameof(breakerController));
            _shouldHandleExceptionPredicates = shouldHandleExceptionPredicates ?? PredicateHelper.EmptyExceptionPredicates;
            _shouldHandleResultPredicates = shouldHandleResultPredicates ?? PredicateHelper<TResult>.EmptyResultPredicates;
            _durationOfBreak = durationOfBreak;
            _onBreak = onBreak;
            _onReset = onReset;
            _onHalfOpen = onHalfOpen;

            _circuitState = CircuitState.Closed;
            Reset();
        }

        public CircuitState CircuitState
        {
            get
            {
                if (_circuitState != CircuitState.Open)
                {
                    return _circuitState;
                }

                using (TimedLock.Lock(_lock))
                {
                    if (_circuitState == CircuitState.Open && !IsInAutomatedBreak_NeedsLock)
                    {
                        TransitionTo_NeedsLock(CircuitState.HalfOpen, Context.None);
                    }
                    return _circuitState;
                }
            }
        }

        protected bool IsInAutomatedBreak_NeedsLock => SystemClock.DateTimeOffsetUtcNow().Ticks < _blockedTill;

        protected DelegateResult<TResult> _lastHandledOutcome;

        public Exception LastException
        {
            get
            {
                using (TimedLock.Lock(_lock))
                {
                    return _lastHandledOutcome?.Exception;
                }
            }
        }
        public TResult LastHandledResult
        {
            get
            {
                using (TimedLock.Lock(_lock))
                {
                    return _lastHandledOutcome != null
                        ? _lastHandledOutcome.Result : default(TResult);
                }
            }
        }

        public void OnActionPreExecute()
        {
            switch (CircuitState)
            {
                case CircuitState.Closed:
                    break;
                case CircuitState.HalfOpen:
                    if (!_breakerController.PermitHalfOpenCircuitTest(_durationOfBreak)) { throw GetBreakingException(); }
                    break;
                case CircuitState.Open:
                    throw GetBreakingException();
                case CircuitState.Isolated:
                    throw new IsolatedCircuitException();
                default:
                    throw new UnhandledCircuitStateException(CircuitState);
            }
        }
        
        protected void TransitionTo_NeedsLock(CircuitState transitionToState, Context context)
        {
            switch (transitionToState)
            {
                case CircuitState.Closed:
                    ResetInternal_NeedsLock(context);
                    break;

                case CircuitState.HalfOpen:
                    HalfOpen_NeedsLock();
                    break;

                case CircuitState.Open:
                    Break_NeedsLock(context);
                    break;

                case CircuitState.Isolated:
                    Isolate(context);
                    break;
                default:
                    throw new UnhandledCircuitStateException(CircuitState);
            }
        }

        private void HalfOpen_NeedsLock()
        {
            _circuitState = CircuitState.HalfOpen;
            _onHalfOpen();
        }

        private void Break_NeedsLock(Context context)
        {
            BreakFor_NeedsLock(_durationOfBreak, context);
        }

        private void BreakFor_NeedsLock(TimeSpan durationOfBreak, Context context)
        {
            bool willDurationTakeUsPastDateTimeMaxValue = durationOfBreak > DateTime.MaxValue - SystemClock.DateTimeOffsetUtcNow();
            _blockedTill = willDurationTakeUsPastDateTimeMaxValue
                ? DateTime.MaxValue.Ticks
                : (SystemClock.DateTimeOffsetUtcNow() + durationOfBreak).Ticks;

            var transitionedState = _circuitState;
            _circuitState = CircuitState.Open;

            _onBreak(_lastHandledOutcome, transitionedState, durationOfBreak, context);
        }

        public void Isolate() => Isolate(Context.None);

        public void Isolate(Context context)
        {
            using (TimedLock.Lock(_lock))
            {
                _lastHandledOutcome = new DelegateResult<TResult>(new IsolatedCircuitException());
                BreakFor_NeedsLock(TimeSpan.MaxValue, context);
                _circuitState = CircuitState.Isolated;
            }
        }

        public void Reset()
        {
            using (TimedLock.Lock(_lock))
            {
                ResetInternal_NeedsLock(Context.None);
            }
        }

        protected void ResetInternal_NeedsLock(Context context)
        {
            _blockedTill = DateTime.MinValue.Ticks;
            _lastHandledOutcome = null;

            CircuitState priorState = _circuitState;
            _circuitState = CircuitState.Closed;
            _breakerController.ResetCircuitStatistics_WithinLock();

            if (priorState != CircuitState.Closed)
            {
                _onReset(context);
            }
        }

        protected BrokenCircuitException GetBreakingException()
        {
            return _lastHandledOutcome.Exception != null
                ? new BrokenCircuitException("The circuit is now open and is not allowing calls.", _lastHandledOutcome.Exception)
                : new BrokenCircuitException<TResult>("The circuit is now open and is not allowing calls.", _lastHandledOutcome.Result);
        }

    }
}
