using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Windows.ApplicationModel;

namespace Plethora.Context
{
    /// <summary>
    /// A collection of context sources.
    /// </summary>
    public class XamlContextSourceCollection : ObservableCollection<XamlContextSource>
    {
        public XamlContextSourceCollection()
        {
            ((INotifyCollectionChanged)this).CollectionChanged += this.HandleCollectionChanged;
        }

        private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //Do not hook up the event model in design mode
            if (DesignMode.DesignModeEnabled)
                return;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (XamlContextSource newItem in e.NewItems)
                    {
                        newItem.ContextChanged += this.ItemContextChanged;
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (XamlContextSource oldItem in e.OldItems)
                    {
                        oldItem.ContextChanged -= this.ItemContextChanged;
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    foreach (XamlContextSource oldItem in e.OldItems)
                    {
                        oldItem.ContextChanged -= this.ItemContextChanged;
                    }
                    foreach (XamlContextSource newItem in e.NewItems)
                    {
                        newItem.ContextChanged += this.ItemContextChanged;
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                    throw new InvalidOperationException("Reset operations are not supported.");
            }
        }

        #region ContextChanged Event

        /// <summary>
        /// Raised when <see cref="Context"/> property changes.
        /// </summary>
        public event EventHandler ContextChanged;

        /// <summary>
        /// Raises the <see cref="ContextChanged"/> event.
        /// </summary>
        protected virtual void OnContextChanged(EventArgs e)
        {
            var handler = this.ContextChanged;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        /// <summary>
        /// Gets the <see cref="ContextInfo"/> from the <see cref="XamlContextSource"/> elements in this list.
        /// </summary>
        [NotNull, ItemNotNull]
        public IEnumerable<ContextInfo> Contexts
        {
            get { return this.Select(item => item.Context); }
        }

        private void ItemContextChanged(object sender, EventArgs e)
        {
            this.OnContextChanged(EventArgs.Empty);
        }
    }
}
