using System;
using System.Collections.Generic;

namespace Plethora.Context
{
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

        protected override void OnEnterContext(object sender, EventArgs e)
        {
            ClearCache();

            base.OnEnterContext(sender, e);
        }

        protected override void OnLeaveContext(object sender, EventArgs e)
        {
            ClearCache();

            base.OnLeaveContext(sender, e);
        }

        protected override void OnContextChanged(object sender, EventArgs e)
        {
            ClearCache();

            base.OnContextChanged(sender, e);
        }

        #endregion

        #region Abstract Methods

        protected abstract IEnumerable<ContextInfo> GetContexts();
        #endregion
        
        #region Private Methods

        private void ClearCache()
        {
            lock (contextLock)
            {
                contexts = null;
                isContextsCached = false;
            }
        }

        #endregion
    }
}
