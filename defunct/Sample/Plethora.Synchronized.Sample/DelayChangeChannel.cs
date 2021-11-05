using System;
using System.Threading;

using Plethora.ComponentModel;
using Plethora.Synchronized.Change;

namespace Plethora.Synchronized.Sample
{
    /// <summary>
    /// Mock change channel which behaves in a way to simulate network latency.
    /// </summary>
    class DelayedChangeChannel<TKey, T> : IChangeSink, IChangeSource
    {
        #region Fields

        private readonly SynchronizeInvoke synchronizeInvoke = new SynchronizeInvoke();
        private readonly SyncCollection<TKey, T> serverCollection;

        #endregion

        public DelayedChangeChannel(SyncCollection<TKey, T> serverCollection)
        {
            this.serverCollection = serverCollection;
        }

        #region Implementation of IChangeSink and IChangeSource

        public void ApplyChange(ChangeDescriptor change)
        {
            Thread.Sleep(500);

            this.serverCollection.ApplyChange(change);

            Action<ChangeDescriptor> publishChange = this.PublishChange;
            this.synchronizeInvoke.BeginInvoke(publishChange, new object[] { change });
        }

        private void PublishChange(ChangeDescriptor change)
        {
            Thread.Sleep(1000);
            var handler = this.ChangePublished;
            if (handler != null)
                handler(this, new ChangePublishedEventArgs(change));
        }

        public event ChangePublishedEventHandler ChangePublished;

        #endregion
    }
}
