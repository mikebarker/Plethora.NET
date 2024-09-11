using System;
using System.Threading;

namespace Plethora.Threading
{
    /// <summary>
    /// Represents a lock as acquired by <see cref="AsyncLock"/>.
    /// </summary>
    /// <remarks>
    /// Disposing this object will release the acquired lock.
    /// </remarks>
    public sealed class LockObject : IDisposable
    {
        private Action? releaseLockAction;

        internal LockObject(Action releaseLockAction)
        {
            ArgumentNullException.ThrowIfNull(releaseLockAction);

            this.releaseLockAction = releaseLockAction;
        }

        #region Implementation of IDisposable

        ~LockObject()
        {
            this.Dispose(false);
        }

        /// <inheritdoc/>
        void IDisposable.Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            this.Release();
        }

        #endregion

        /// <summary>
        /// Releases the lock represented by this <see cref="LockObject"/>.
        /// </summary>
        /// <remarks>
        /// This method is re-entrant safe, and the lock will only be released the first
        /// time the method is called.
        /// </remarks>
        public void Release()
        {
            var releaseAction = Interlocked.Exchange(ref this.releaseLockAction, null);

            releaseAction?.Invoke();
        }
    }
}
