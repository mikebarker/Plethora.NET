using System;
using System.Threading;

namespace Plethora.Threading
{
    /// <summary>
    /// Represents the result of a TryLock operation of <see cref="AsyncLock"/>.
    /// </summary>
    /// <seealso cref="AsyncLock.TryLock(TimeSpan, CancellationToken)"/>
    /// <seealso cref="AsyncLock.TryLockAsync(TimeSpan, CancellationToken)"/>
    public sealed class LockResult : IDisposable
    {
        private LockObject? lockObject;

        private LockResult(
            bool isLockAcquired,
            LockObject? lockObject)
        {
            this.IsLockAcquired = isLockAcquired;
            this.lockObject = lockObject;
        }

        #region Implementation of IDisposable

        private bool disposed;

        ~LockResult()
        {
            this.Dispose(false);
        }

        /// <inheritdoc/>
        void IDisposable.Dispose()
        {
            // Dispose of unmanaged resources.
            this.Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
                return;

            if (disposing)
            {
                ((IDisposable?)this.lockObject)?.Dispose();
            }

            this.disposed = true;
        }

        #endregion

        /// <summary>
        /// True if the lock was acquired; otherwise false.
        /// </summary>
        public bool IsLockAcquired { get; }

        /// <summary>
        /// An instance of <see cref="LockObject"/> if the lock was acquired; otherwise null.
        /// </summary>
        public LockObject? LockObject => this.lockObject;

        /// <summary>
        /// Extract ownership of the <see cref="LockObject"/>.
        /// </summary>
        /// <returns>
        /// The content of <see cref="LockObject"/>. Will be null if <see cref="IsLockAcquired"/> is false, or if
        /// <see cref="ExtractLockObject"/> has previously been called.
        /// </returns>
        /// <remarks>
        /// This method allows this <see cref="LockResult"/> to be disposed without disposing the
        /// contained <see cref="Plethora.Threading.LockObject"/>.
        /// </remarks>
        public LockObject? ExtractLockObject()
        {
            var lockObj = Interlocked.Exchange(ref this.lockObject, null);
            return lockObj;
        }

        internal static LockResult Acquired(Action releaseLockAction)
        {
            ArgumentNullException.ThrowIfNull(releaseLockAction);

            LockObject lockObject = new(releaseLockAction);
            LockResult lockResult = new(true, lockObject);
            return lockResult;
        }

        internal static LockResult NotAcquired()
        {
            LockResult lockResult = new(false, null);
            return lockResult;
        }
    }
}
