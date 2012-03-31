using System;
using System.Threading;

namespace Plethora.Timing
{
    /// <summary>
    /// Helper class for tracking the timeout with-in complex operations.
    /// </summary>
    public class OperationTimeout
    {
        private const long INFINITE_TICKS = -1;
        private readonly long endTicks;

        public OperationTimeout(int millisecondsTimeout)
        {
            if ((millisecondsTimeout < 0) && (millisecondsTimeout != Timeout.Infinite))
                throw new ArgumentOutOfRangeException("millisecondsTimeout", millisecondsTimeout,
                    ResourceProvider.ArgTimeout("millisecondsTimeout"));

            if (millisecondsTimeout == Timeout.Infinite)
            {
                endTicks = INFINITE_TICKS;
            }
            else
            {
                long timeoutTicks = TimeSpan.TicksPerMillisecond * millisecondsTimeout;
                endTicks = DateTime.Now.Ticks + timeoutTicks;
            }
        }

        /// <summary>
        /// Gets a flag indicating whether 'millisecondsTimeout' milliseconds have elapsed since
        /// the class was constructed.
        /// </summary>
        public bool HasElapsed
        {
            get
            {
                if (endTicks == INFINITE_TICKS)
                    return false;

                return (endTicks <= DateTime.Now.Ticks);
            }
        }

        /// <summary>
        /// Gets the remaining time in milliseconds.
        /// </summary>
        /// <value>
        /// Returns <see cref="Timeout.Infinite"/> if the class was constructed with
        /// <see cref="Timeout.Infinite"/>
        /// specified for 'millisecondsTimeout'.
        /// - OR -
        /// Returns 'millisecondsTimeout' minus the number of elapsed milliseconds since the class was
        /// constructed. If more than 'millisecondsTimeout' milliseconds has elapsed then zero is returned.
        /// </value>
        public int Remaining
        {
            get
            {
                if (endTicks == INFINITE_TICKS)
                    return Timeout.Infinite;

                long remainingTicks = endTicks - DateTime.Now.Ticks;
                if (remainingTicks <= 0)
                    return 0;

                return (int)((remainingTicks) / TimeSpan.TicksPerMillisecond);
            }
        }

        /// <summary>
        /// Gets the remaining time in milliseconds, and throws a <see cref="TimeoutException"/>
        /// exception if the timeout has elapsed.
        /// </summary>
        public int RemainingThrowIfElapsed
        {
            get
            {
                int remaining = this.Remaining;
                if (remaining == 0)
                    throw new TimeoutException();

                return remaining;
            }
        }

        /// <summary>
        /// Throws a <see cref="TimeoutException"/> exception if the timeout has elapsed.
        /// </summary>
        public void ThrowIfElapsed()
        {
            if (this.Remaining == 0)
                throw new TimeoutException();
        }
    }
}
