using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Plethora.Threading
{
    /// <summary>
    /// Represents a register of awaited and acquired locks.
    /// </summary>
    public class LockRegister
    {
        /// <summary>
        /// Gets and sets the default instance of the <see cref="LockRegister"/>.
        /// </summary>
        /// <remarks>
        /// By default, the <see cref="DefaultInstance"/> is null, and so no locks will
        /// participate in long-wait detection. Setting the <see cref="DefaultInstance"/>
        /// prior to creating locks will allow all instances to participate in long-wait
        /// detection.
        /// </remarks>
        public static LockRegister DefaultInstance { get; set; } = null;

        private readonly HashSet<LockContext> lockContexts = new HashSet<LockContext>();
        private readonly TimeSpan longWaitDetectionTimeout;

        /// <summary>
        /// Initialise a new instance of the <see cref="LockRegister"/> class.
        /// </summary>
        /// <param name="longWaitDetectionTimeout">The amount of time to elapse before a long-wait detection is triggered.</param>
        public LockRegister(TimeSpan longWaitDetectionTimeout)
        {
            long totalMilliseconds = (long)longWaitDetectionTimeout.TotalMilliseconds;
            if (totalMilliseconds < -1L || totalMilliseconds > (long)int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(longWaitDetectionTimeout), (object)longWaitDetectionTimeout, ResourceProvider.TimeoutInvalid());

            this.longWaitDetectionTimeout = longWaitDetectionTimeout;
        }

        #region LongWaitDetected event

        /// <summary>
        /// Event raised when a long-wait is detected.
        /// </summary>
        /// <remarks>
        /// A long-wait is a defined by the value of the longWaitDetectionTimeout
        /// parameter during construction.
        /// </remarks>
        public event EventHandler<LongWaitDetectedEventArgs> LongWaitDetected;

        /// <summary>
        /// Triggers the <see cref="LongWaitDetected"/> event.
        /// </summary>
        /// <param name="lockContext">
        /// The <see cref="LockContext"/> for which the long-wait was detected.
        /// </param>
        protected void OnLongWaitDetected(LockContext lockContext)
        {
            // Raise the LongWaitDetected 
            LockContext[] otherLockContexts;
            lock (this.lockContexts)
            {
                otherLockContexts = this.lockContexts
                    .Where(c => !ReferenceEquals(lockContext, c))
                    .ToArray();
            }

            var e = new LongWaitDetectedEventArgs(lockContext, otherLockContexts);
            this.OnLongWaitDetected(e);
        }

        /// <summary>
        /// Triggers the <see cref="LongWaitDetected"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LongWaitDetectedEventArgs"/> event arguments.</param>
        protected virtual void OnLongWaitDetected(LongWaitDetectedEventArgs e)
        {
            this.LongWaitDetected?.Invoke(this, e);
        }

        #endregion

        /// <summary>
        /// Register an await on a lock.
        /// </summary>
        /// <param name="lock">The lock being awited.</param>
        /// <param name="originMemberName">The caller member name of the original wait.</param>
        /// <param name="originSourceFilePath">The caller source file path of the original wait.</param>
        /// <param name="originSourceLineNumber">The caller source line number of the original wait.</param>
        /// <returns>
        /// An <see cref="IDisposable"/> which will deregister the await when disposed.
        /// </returns>
        public IDisposable RegisterAwaitingLock(
            object @lock,
            string originMemberName,
            string originSourceFilePath,
            int originSourceLineNumber)
        {
            var lockContext = new LockContext(
                @lock,
                LockRequestStatus.Awaiting,
                originMemberName,
                originSourceFilePath,
                originSourceLineNumber);

            lock (this.lockContexts)
            {
                this.lockContexts.Add(lockContext);
            }

            var cts = new CancellationTokenSource();

            var token = cts.Token;
            _ = Task.Run(async () =>
            {
                await Task.Delay(this.longWaitDetectionTimeout).ConfigureAwait(false);

                if (!token.IsCancellationRequested)
                {
                    // Raise the LongWaitDetected event
                    this.OnLongWaitDetected(lockContext);
                }
            });

            return new ActionOnDispose(() =>
            {
                lock (this.lockContexts)
                {
                    this.lockContexts.Remove(lockContext);
                }

                cts.Cancel();
                cts.Dispose();
            });
        }

        /// <summary>
        /// Register an acquired on a lock.
        /// </summary>
        /// <param name="lock">The lock acquired.</param>
        /// <param name="originMemberName">The caller member name of the original acquisition.</param>
        /// <param name="originSourceFilePath">The caller source file path of the original acquisition.</param>
        /// <param name="originSourceLineNumber">The caller source line number of the original acquisition.</param>
        /// <returns>
        /// An <see cref="IDisposable"/> which will deregister the acquisition when disposed.
        /// </returns>
        public IDisposable RegisterAcquiredLock(
            object @lock,
            string originMemberName,
            string originSourceFilePath,
            int originSourceLineNumber)
        {
            var lockContext = new LockContext(
                @lock,
                LockRequestStatus.Acquired,
                originMemberName,
                originSourceFilePath,
                originSourceLineNumber);

            lock (this.lockContexts)
            {
                this.lockContexts.Add(lockContext);
            }

            return new ActionOnDispose(() =>
            {
                lock (this.lockContexts)
                {
                    this.lockContexts.Remove(lockContext);
                }
            });
        }

        /// <summary>
        /// Gets a collection of the currently registered lock contexts.
        /// </summary>
        /// <returns>
        /// A <see cref="IReadOnlyCollection{LockContext}"/> of the currently registered lock contexts.
        /// </returns>
        public IReadOnlyCollection<LockContext> GetLockContexts()
        {
            LockContext[] array;
            lock (this.lockContexts)
            {
                array = new LockContext[this.lockContexts.Count];
                this.lockContexts.CopyTo(array);
            }

            return array;
        }
    }
}
