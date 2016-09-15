﻿using System;
using System.Threading;

namespace Polly.Bulkhead
{
    /// <summary>
    /// Synchronous Bulkhead Engine
    /// </summary>
    internal static partial class BulkheadEngine
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action"></param>
        /// <param name="maxParallelizationSemaphore"></param>
        /// <param name="maxQueuedActionsSemaphore"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        internal static TResult Implementation<TResult>(
            Func<CancellationToken, TResult> action,
            SemaphoreSlim maxParallelizationSemaphore,
            SemaphoreSlim maxQueuedActionsSemaphore,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!maxQueuedActionsSemaphore.Wait(TimeSpan.Zero, cancellationToken)) throw new SemaphoreRejectedException();
            
            try
            {
                maxParallelizationSemaphore.Wait(cancellationToken);
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    DelegateResult<TResult> delegateOutcome = new DelegateResult<TResult>(action(cancellationToken));
                    return delegateOutcome.Result;
                }
                finally
                {
                    maxParallelizationSemaphore.Release();
                }
            }
            finally
            {
                maxQueuedActionsSemaphore.Release();
            }
        }
    }
}
