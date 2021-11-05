using System;
using System.Diagnostics;
using System.Threading;

namespace Plethora.Timing
{
    /// <summary>
    /// Helper class for tracking the timeout with-in complex operations.
    /// </summary>
    public class OperationTimeout
    {
        private readonly Stopwatch stopwatch;
        private readonly TimeSpan timeout;

        public OperationTimeout(TimeSpan timeout)
        {
            if ((timeout < TimeSpan.Zero) && (timeout != Timeout.InfiniteTimeSpan))
                throw new ArgumentOutOfRangeException(nameof(timeout), timeout,
                    ResourceProvider.ArgTimeout(nameof(timeout)));

            this.stopwatch = Stopwatch.StartNew();
            this.timeout = timeout;
        }

        /// <summary>
        /// Gets a flag indicating whether 'timeout' has elapsed since the class was constructed.
        /// </summary>
        public bool HasElapsed
        {
            get
            {
                return (this.Remaining == TimeSpan.Zero);
            }
        }

        /// <summary>
        /// Gets the remaining time.
        /// </summary>
        /// <value>
        /// Returns <see cref="Timeout.InfiniteTimeSpan"/> if the class was constructed with
        /// <see cref="Timeout.InfiniteTimeSpan"/> specified for 'timeout'.
        /// - OR -
        /// Returns 'timeout' minus the elapsed time since the class was constructed.
        /// If more than 'timeout' has elapsed then TimeSpan.Zero is returned.
        /// </value>
        public TimeSpan Remaining
        {
            get
            {
                if (this.timeout == Timeout.InfiniteTimeSpan)
                    return Timeout.InfiniteTimeSpan;

                TimeSpan remaining = this.timeout - this.stopwatch.Elapsed;
                if (remaining < TimeSpan.Zero)
                    return TimeSpan.Zero;

                return remaining;
            }
        }

        /// <summary>
        /// Gets the remaining time, and throws a <see cref="TimeoutException"/>
        /// exception if the timeout has elapsed.
        /// </summary>
        public TimeSpan RemainingThrowIfElapsed
        {
            get
            {
                TimeSpan remaining = this.Remaining;
                if (remaining == TimeSpan.Zero)
                    throw new TimeoutException();

                return remaining;
            }
        }

        /// <summary>
        /// Throws a <see cref="TimeoutException"/> exception if the timeout has elapsed.
        /// </summary>
        public void ThrowIfElapsed()
        {
            if (this.HasElapsed)
                throw new TimeoutException();
        }
    }
}
