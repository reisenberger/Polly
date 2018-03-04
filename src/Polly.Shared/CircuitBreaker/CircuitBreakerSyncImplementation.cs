using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using Polly.Execution;
using Polly.Utilities;

namespace Polly.CircuitBreaker
{
    internal class CircuitBreakerSyncImplementation<TResult> : CircuitBreakerImplementation<TResult>, ISyncPolicyImplementation<TResult>
    {
        internal CircuitBreakerSyncImplementation(
            IsPolicy policy,
            IEnumerable<ExceptionPredicate> shouldHandleExceptionPredicates,
            IEnumerable<ResultPredicate<TResult>> shouldHandleResultPredicates,
            ICircuitController breakerController,
            TimeSpan durationOfBreak,
            Action<DelegateResult<TResult>, CircuitState, TimeSpan, Context> onBreak,
            Action<Context> onReset,
            Action onHalfOpen
        ) : base(policy, shouldHandleExceptionPredicates, shouldHandleResultPredicates, breakerController, durationOfBreak, onBreak, onReset, onHalfOpen)
        { }

        public TResult Execute<TExecutable>(in TExecutable action, Context context, in CancellationToken cancellationToken) where TExecutable : ISyncPollyExecutable<TResult>
        {
            cancellationToken.ThrowIfCancellationRequested();

            OnActionPreExecute();

            try
            {
                TResult result = action.Execute(context, cancellationToken);

                if (_shouldHandleResultPredicates.Any(predicate => predicate(result)))
                {
                    using (TimedLock.Lock(_lock))
                    {
                        _lastHandledOutcome = new DelegateResult<TResult>(result);
                        CircuitState transitionTo = _breakerController.OnActionHandledFailure_WithinLock(_circuitState);
                        if (transitionTo != _circuitState)
                        {
                            TransitionTo_NeedsLock(transitionTo, context);
                        }
                    }
                }
                else
                {
                    using (TimedLock.Lock(_lock))
                    {
                        CircuitState transitionTo = _breakerController.OnActionSuccess_WithinLock(_circuitState);
                        if (transitionTo != _circuitState)
                        {
                            TransitionTo_NeedsLock(transitionTo, context);
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Exception handledException = _shouldHandleExceptionPredicates
                    .Select(predicate => predicate(ex))
                    .FirstOrDefault(e => e != null);
                if (handledException == null)
                {
                    throw;
                }

                using (TimedLock.Lock(_lock))
                {
                    _lastHandledOutcome = new DelegateResult<TResult>(handledException);
                    CircuitState transitionTo = _breakerController.OnActionHandledFailure_WithinLock(_circuitState);
                    if (transitionTo != _circuitState)
                    {
                        TransitionTo_NeedsLock(transitionTo, context);
                    }
                }

                if (handledException != ex)
                {
                    ExceptionDispatchInfo.Capture(handledException).Throw();
                }
                throw;
            }
        }

    }
}
