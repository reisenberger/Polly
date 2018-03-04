using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Polly.Execution;
using Polly.Utilities;

namespace Polly.Fallback
{
    internal class FallbackAsyncImplementation<TResult> : IAsyncPolicyImplementation<TResult>
    {
        private IsPolicy _policy;
        IEnumerable<ExceptionPredicate> _shouldHandleExceptionPredicates;
        IEnumerable<ResultPredicate<TResult>> _shouldHandleResultPredicates;
        Func<DelegateResult<TResult>, Context, Task> _onFallbackAsync;
        Func<DelegateResult<TResult>, Context, CancellationToken, bool, Task<TResult>> _fallbackActionAsync;

        internal FallbackAsyncImplementation(IsPolicy policy,
            IEnumerable<ExceptionPredicate> shouldHandleExceptionPredicates,
            IEnumerable<ResultPredicate<TResult>> shouldHandleResultPredicates,
            Func<DelegateResult<TResult>, Context, Task> onFallbackAsync,
            Func<DelegateResult<TResult>, Context, CancellationToken, bool, Task<TResult>> fallbackActionAsync)
        {
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
            _shouldHandleExceptionPredicates = shouldHandleExceptionPredicates ?? PredicateHelper.EmptyExceptionPredicates;
            _shouldHandleResultPredicates = shouldHandleResultPredicates ?? PredicateHelper<TResult>.EmptyResultPredicates;
            _onFallbackAsync = onFallbackAsync ?? throw new ArgumentNullException(nameof(onFallbackAsync));
            _fallbackActionAsync = fallbackActionAsync ?? throw new ArgumentNullException(nameof(fallbackActionAsync));
        }

        public async Task<TResult> ExecuteAsync<TExecutableAsync>(TExecutableAsync action, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext) where TExecutableAsync : IAsyncPollyExecutable<TResult>
        {
            DelegateResult<TResult> delegateOutcome;

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                TResult result = await action.ExecuteAsync(context, cancellationToken, continueOnCapturedContext).ConfigureAwait(continueOnCapturedContext);

                if (!_shouldHandleResultPredicates.Any(predicate => predicate(result)))
                {
                    return result;
                }

                delegateOutcome = new DelegateResult<TResult>(result);
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

                delegateOutcome = new DelegateResult<TResult>(handledException);
            }

            await _onFallbackAsync(delegateOutcome, context).ConfigureAwait(continueOnCapturedContext);

            return await _fallbackActionAsync(delegateOutcome, context, cancellationToken, continueOnCapturedContext).ConfigureAwait(continueOnCapturedContext);
        }
        
    }
}
