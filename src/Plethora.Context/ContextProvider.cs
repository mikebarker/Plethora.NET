using System;
using System.Collections.Generic;

namespace Plethora.Context
{
    public abstract class ContextProvider : IContextProvider, IDisposable
    {
        #region Fields

        private readonly object contextLock = new object();
        private bool isContextsCached = false;
        private IEnumerable<ContextInfo> contexts;
        #endregion

        #region Implementation of IContextProvider

        public event EventHandler EnterContext;
        public event EventHandler LeaveContext;
        public event EventHandler ContextChanged;

        public IEnumerable<ContextInfo> Contexts
        {
            get
            {
                lock (contextLock)
                {
                    //Cache the result to prevent thrashing of unnecessary calls to GetContext
                    if (!isContextsCached)
                    {
                        contexts = GetContexts();
                        isContextsCached = true;
                    }

                    return contexts;
                }
            }
        }

        #endregion

        #region Implementation of IDisposable

        // Track whether Dispose has been called.
        private bool disposed = false;

        ~ContextProvider()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources.
                }

                // Clean up unmanaged resources here.


                disposed = true;
            }
        }
        #endregion

        #region Abstract Methods

        public abstract IEnumerable<ContextInfo> GetContexts();
        #endregion

        #region Protected Methods

        protected void OnEnterContext()
        {
            OnEnterContext(this, EventArgs.Empty);
        }

        protected virtual void OnEnterContext(object sender, EventArgs e)
        {
            var handler = this.EnterContext;
            if (handler != null)
                handler(sender, e);

            OnContextChanged();
        }

        protected void OnLeaveContext()
        {
            OnLeaveContext(this, EventArgs.Empty);
        }

        protected virtual void OnLeaveContext(object sender, EventArgs e)
        {
            var handler = this.LeaveContext;
            if (handler != null)
                handler(sender, e);

            OnContextChanged();
        }

        protected void OnContextChanged()
        {
            OnContextChanged(this, EventArgs.Empty);
        }

        protected virtual void OnContextChanged(object sender, EventArgs e)
        {
            lock (contextLock)
            {
                contexts = null;
                isContextsCached = false;
            }

            var handler = this.ContextChanged;
            if (handler != null)
                handler(sender, e);
        }
        #endregion
    }
}
