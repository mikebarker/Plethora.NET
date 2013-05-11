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
            Monitor.Enter(@lock);
            if (lockCount == 0)
                lockThreadId = Thread.CurrentThread.ManagedThreadId;
            lockCount++;

            return new Disposable(delegate
            {
                lockCount--;
                if (lockCount == 0)
                    lockThreadId = -1;
                Monitor.Exit(@lock);
            });
        }

        public bool IsLockAcquired
        {
            get { return (lockThreadId == Thread.CurrentThread.ManagedThreadId); }
        }
    }
}
