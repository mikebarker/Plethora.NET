using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Plethora.Context.Wpf
{
    public class WpfContextSourceCollection : FreezableCollection<WpfContextSource>, IWpfContextSource //ObservableCollection<IWpfContextSource>, IWpfContextSource
    {
        public WpfContextSourceCollection()
        {
            ((INotifyCollectionChanged)this).CollectionChanged += this_CollectionChanged;
        }


        #region Implementation Of IWpfContextSource

        #region ContextChanged Event

        /// <summary>
        /// Raised when <see cref="Contexts"/> property changes.
        /// </summary>
        public event EventHandler ContextChanged;

        /// <summary>
        /// Raises the <see cref="ContextChanged"/> event.
        /// </summary>
        private void OnContextChanged()
        {
            OnContextChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="ContextChanged"/> event.
        /// </summary>
        protected virtual void OnContextChanged(EventArgs e)
        {
            var handler = ContextChanged;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        public IEnumerable<ContextInfo> Contexts
        {
            get
            {
                var contexts = this
                    .Cast<IWpfContextSource>()
                    .SelectMany(item => item.Contexts)
                    .ToArray();

                return contexts;
            }
        }

        #endregion


        private UIElement uiElement;

        public UIElement UIElement
        {
            get { return uiElement; }
            set
            {
                if (uiElement != null)
                    DeregisterElement(uiElement);

                uiElement = value;

                if (uiElement != null)
                    RegisterUIElement(uiElement);
            }
        }

        private void RegisterUIElement(UIElement element)
        {
            //Do not hook up the event model in design mode
            if (DesignerProperties.GetIsInDesignMode(element))
                return;

            foreach (var wpfContextSource in this)
            {
                wpfContextSource.UIElement = element;
            }
        }

        private void DeregisterElement(UIElement element)
        {
            //Do not hook up the event model in design mode
            if (DesignerProperties.GetIsInDesignMode(element))
                return;

            foreach (var wpfContextSource in this)
            {
                wpfContextSource.UIElement = null;
            }
        }


        void contextSource_ContextChanged(object sender, EventArgs e)
        {
            this.OnContextChanged();
        }

        private void this_CollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var newItem in e.NewItems)
                    {
                        var wpfContextSource = (IWpfContextSource)newItem;
                        wpfContextSource.UIElement = this.UIElement;
                        wpfContextSource.ContextChanged += contextSource_ContextChanged;
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (var oldItem in e.OldItems)
                    {
                        var wpfContextSource = (IWpfContextSource)oldItem;
                        wpfContextSource.ContextChanged -= contextSource_ContextChanged;
                        wpfContextSource.UIElement = null;
                    }
                    break;
            }
        }


    }
}
