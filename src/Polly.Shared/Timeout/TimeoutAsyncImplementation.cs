using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Polly.Execution;
using Polly.Utilities;

#if NET40
using ExceptionDispatchInfo = Polly.Utilities.ExceptionDispatchInfo;
#endif

namespace Polly.Timeout
{
    internal class TimeoutAsyncImplementation<TResult> : IAsyncPolicyImplementation<TResult>
    {
        private ITimeoutPolicyInternal _policy;

        internal TimeoutAsyncImplementation(ITimeoutPolicyInternal policy)
        {
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
        }

        public async Task<TResult> ExecuteAsync<TExecutableAsync>(TExecutableAsync action, Context context, CancellationToken cancellationToken, bool continueOnCapturedContext) where TExecutableAsync : IAsyncPollyExecutable<TResult>
        {
            cancellationToken.ThrowIfCancellationRequested();
            TimeSpan timeout = _policy.TimeoutProvider(context);

            using (CancellationTokenSource timeoutCancellationTokenSource = new CancellationTokenSource())
            {
                using (CancellationTokenSource combinedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCancellationTokenSource.Token))
                {
                    Task<TResult> actionTask = null;
                    CancellationToken combinedToken = combinedTokenSource.Token;

                    try
                    {
                        if (_policy.TimeoutStrategy == TimeoutStrategy.Optimistic)
                        {
                            SystemClock.CancelTokenAfter(timeoutCancellationTokenSource, timeout);
                            return await action.ExecuteAsync(context, combinedToken, continueOnCapturedContext).ConfigureAwait(continueOnCapturedContext);
                        }

                        // else: timeoutStrategy == TimeoutStrategy.Pessimistic

                        Task<TResult> timeoutTask = timeoutCancellationTokenSource.Token.AsTask<TResult>();

                        SystemClock.CancelTokenAfter(timeoutCancellationTokenSource, timeout);

                        actionTask = action.ExecuteAsync(context, combinedToken, continueOnCapturedContext);

                        return await (await
#if NET40
                            TaskEx
#else
                            Task
#endif
                            .WhenAny(actionTask, timeoutTask).ConfigureAwait(continueOnCapturedContext)).ConfigureAwait(continueOnCapturedContext);

                    }
                    catch (Exception e)
                    {
                        if (timeoutCancellationTokenSource.IsCancellationRequested)
                        {
                            await _policy.OnTimeoutAsync(context, timeout, actionTask).ConfigureAwait(continueOnCapturedContext);
                            throw TimeoutSyncImplementation<TResult>.TimeoutRejectedException(e);
                        }

                        throw;
                    }
                }
            }
        }

    }
}
