using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Plethora.Context.Wpf
{
    public class WpfContextSourceCollection : ObservableCollection<IWpfContextSource>, IWpfContextSource
    {
        public WpfContextSourceCollection()
        {
        }

        public WpfContextSourceCollection(UIElement uiElement)
        {
            UIElement = uiElement;
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
                var contexts = this.Items
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

            foreach (var wpfContextSource in Items)
            {
                wpfContextSource.UIElement = element;
            }
        }

        private void DeregisterElement(UIElement element)
        {
            //Do not hook up the event model in design mode
            if (DesignerProperties.GetIsInDesignMode(element))
                return;

            foreach (var wpfContextSource in Items)
            {
                wpfContextSource.UIElement = null;
            }
        }


        void contextSource_ContextChanged(object sender, EventArgs e)
        {
            this.OnContextChanged();
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);

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
                        wpfContextSource.UIElement = null;
                        wpfContextSource.ContextChanged -= contextSource_ContextChanged;
                    }
                    break;
            }
        }


    }
}
