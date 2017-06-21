using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Plethora.Context
{
    /// <summary>
    /// An implementation of the <see cref="IContextProvider"/> interface which 
    /// caches the context results until the context has changed.
    /// </summary>
    public abstract class CachedContextProvider : ContextProviderBase
    {
        #region Fields

        private readonly object contextLock = new object();
        private bool isContextsCached = false;
        private IEnumerable<ContextInfo> contexts;
        #endregion

        #region Overrides of ContextProviderBase

        public override IEnumerable<ContextInfo> Contexts
        {
            get
            {
                lock (this.contextLock)
                {
                    //Cache the result to prevent thrashing of unnecessary calls to GetContext
                    if (!this.isContextsCached)
                    {
                        this.contexts = this.GetContexts();
                        this.isContextsCached = true;
                    }

                    return this.contexts;
                }
            }
        }

        protected override void OnEnterContext(object sender, EventArgs e)
        {
            this.ClearCache();

            base.OnEnterContext(sender, e);
        }

        protected override void OnLeaveContext(object sender, EventArgs e)
        {
            this.ClearCache();

            base.OnLeaveContext(sender, e);
        }

        protected override void OnContextChanged(object sender, EventArgs e)
        {
            this.ClearCache();

            base.OnContextChanged(sender, e);
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Abstract method which acquires the context from the underlying source.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> of <see cref="ContextInfo"/> objects which
        /// represents the context of the underlying source.
        /// </returns>
        [CanBeNull]
        protected abstract IEnumerable<ContextInfo> GetContexts();

        #endregion
        
        #region Private Methods

        private void ClearCache()
        {
            lock (this.contextLock)
            {
                this.contexts = null;
                this.isContextsCached = false;
            }
        }

        #endregion
    }
}
