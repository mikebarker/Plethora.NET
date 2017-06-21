using System;
using System.Threading;

namespace Plethora.Threading
{
    public class LiteLock
    {
        private class Disposable : IDisposable
        {
            #region Field

            private readonly Action onDispose;

            #endregion

            #region Constructors

            public Disposable(Action onDispose)
            {
                this.onDispose = onDispose;
            }

            #endregion

            #region Implementation of IDisposable

            public void Dispose()
            {
                this.onDispose();
            }

            #endregion
        }

        private readonly object @lock = new object();
        private int lockCount = 0;
        private int lockThreadId = -1;

        public IDisposable AcquireLock()
        {
            Monitor.Enter(this.@lock);
            if (this.lockCount == 0)
                this.lockThreadId = Thread.CurrentThread.ManagedThreadId;
            this.lockCount++;

            return new Disposable(delegate
            {
                this.lockCount--;
                if (this.lockCount == 0)
                    this.lockThreadId = -1;
                Monitor.Exit(this.@lock);
            });
        }

        public bool IsLockAcquired
        {
            get { return (this.lockThreadId == Thread.CurrentThread.ManagedThreadId); }
        }
    }
}
