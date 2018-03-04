using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.Utilities
{
    /// <summary>
    /// Task helpers.
    /// </summary>
    public static class TaskHelper
    {
        /// <summary>
        /// Defines a completed Task for use as a completed, empty asynchronous delegate.
        /// </summary>
        public static Task EmptyTask = Task.FromResult(true);

        internal static Task<TResult> AsTask<TResult>(this CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<TResult>();

            // A generalised version of this method would include a hotpath returning a canceled task (rather than setting up a registration) if (cancellationToken.IsCancellationRequested) on entry.  For Polly, this is currently omitted, since the only consumer of this method, TimeoutAsyncImplementation, only starts the timeout countdown in the token _after calling this method.

            IDisposable registration = null;
            registration = cancellationToken.Register(() =>
            {
                tcs.TrySetCanceled();
                registration?.Dispose();
            }, useSynchronizationContext: false);

            return tcs.Task;
        }
    }
}
