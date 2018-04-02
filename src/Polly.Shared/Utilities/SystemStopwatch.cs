using System;

namespace Polly.Utilities
{
    /// <summary>
    /// Elapsed time related delegates used to support testability of the code.
    /// </summary>
    public static class SystemStopwatch
    {
        /// <summary>
        /// Initializes the <see cref="SystemStopwatch"/> class.
        /// </summary>
        static SystemStopwatch()
        {
            Stopwatch.Start();
            Initialise();
        }

        private static readonly System.Diagnostics.Stopwatch Stopwatch = new System.Diagnostics.Stopwatch();

        /// <summary>
        /// Allows the setting of a custom StopwatchTimestamp value, for testing.
        /// By default this will use the <see cref="System.Diagnostics.Stopwatch.GetTimestamp()"/> method.
        /// </summary>
        public static Func<long> StopwatchTimestamp;

        /// <summary>
        /// Allows the setting of a custom StopwatchFrequency value, for testing.
        /// By default this will use <see cref="m:Stopwatch.Frequency"/>
        /// </summary>
        public static long StopwatchFrequency;

        /// <summary>
        /// Gets the elapsed time since an earlier StopwatchTimestamp.
        /// </summary>
        /// <param name="startStopwatchTimestamp">The StopwatchTimestamp </param>
        public static TimeSpan ElapsedSince(long startStopwatchTimestamp)
        {
            return new TimeSpan((long)((StopwatchTimestamp() - startStopwatchTimestamp) * (TimeSpan.TicksPerSecond / (double)StopwatchFrequency)));
        }

        /// <summary>
        /// Sets the implementations. 
        /// </summary>
        public static void Initialise()
        {
            StopwatchTimestamp = System.Diagnostics.Stopwatch.GetTimestamp;
            StopwatchFrequency = System.Diagnostics.Stopwatch.Frequency;
        }

        /// <summary>
        /// Resets the custom implementations to their defaults. 
        /// Should be called during test teardowns.
        /// </summary>
        public static void Reset()
        {
            Initialise();
        }
    }
}