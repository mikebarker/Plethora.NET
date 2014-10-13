using System;
using System.Collections.Generic;

namespace Plethora.Context
{
    /// <summary>
    /// An implementation of the <see cref="IContextProvider"/> interface which
    /// provides common methods to call the context change events.
    /// </summary>
    /// <seealso cref="IContextProvider"/>
    public abstract class ContextProviderBase : IContextProvider, IDisposable
    {
        #region Implementation of IContextProvider

        public event EventHandler EnterContext;
        public event EventHandler LeaveContext;
        public event EventHandler ContextChanged;

        public abstract IEnumerable<ContextInfo> Contexts
        {
            get;
        }

        #endregion

        #region Implementation of IDisposable

        // Track whether Dispose has been called.
        private bool disposed = false;

        ~ContextProviderBase()
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
        }

        protected void OnContextChanged()
        {
            OnContextChanged(this, EventArgs.Empty);
        }

        protected virtual void OnContextChanged(object sender, EventArgs e)
        {
            var handler = this.ContextChanged;
            if (handler != null)
                handler(sender, e);
        }

        #endregion
    }
}
