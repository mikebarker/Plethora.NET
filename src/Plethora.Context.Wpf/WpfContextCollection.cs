using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Plethora.Context.Wpf
{
    public class WpfContextCollection : ObservableCollection<WpfContext>, IContextProvider
    {
        public WpfContextCollection()
        {
            this.internalContextProvider = new InternalContextProvider(this);
        }

        public WpfContextCollection(UIElement uiElement)
            : this()
        {
            SetUIElement(uiElement);
        }

        #region Implementation of IContextProvider

        //Uses containment to provide inheritance.
        private sealed class InternalContextProvider : ContextProvider
        {
            private readonly WpfContextCollection parent;

            public InternalContextProvider(WpfContextCollection parent)
            {
                this.parent = parent;
            }

            public override IEnumerable<ContextInfo> GetContexts()
            {
                return parent.GetContextInfos();
            }

            public new void OnContextChanged()
            {
                base.OnContextChanged(parent, EventArgs.Empty);
            }

            public new void OnEnterContext()
            {
                base.OnEnterContext(parent, EventArgs.Empty);
            }

            public new void OnLeaveContext()
            {
                base.OnLeaveContext(parent, EventArgs.Empty);
            }
        }


        private readonly InternalContextProvider internalContextProvider;

        public event EventHandler EnterContext
        {
            add { internalContextProvider.EnterContext += value; }
            remove { internalContextProvider.EnterContext -= value; }
        }

        public event EventHandler LeaveContext
        {
            add { internalContextProvider.LeaveContext += value; }
            remove { internalContextProvider.LeaveContext -= value; }
        }

        public event EventHandler ContextChanged
        {
            add { internalContextProvider.ContextChanged += value; }
            remove { internalContextProvider.ContextChanged -= value; }
        }

        public IEnumerable<ContextInfo> Contexts
        {
            get { return internalContextProvider.Contexts; }
        }


        private void OnContextChanged()
        {
            internalContextProvider.OnContextChanged();
        }

        private void OnEnterContext()
        {
            internalContextProvider.OnEnterContext();
        }

        private void OnLeaveContext()
        {
            internalContextProvider.OnLeaveContext();
        }

        #endregion


        private IEnumerable<ContextInfo> GetContextInfos()
        {
            var contextInfos = this.Items
                .Select(wpfContext => new ContextInfo(wpfContext.Name, wpfContext.Rank, wpfContext.Data))
                .ToArray();

            return contextInfos;
        }


        private bool isUIElementSet = false;

        internal void SetUIElement(UIElement element)
        {
            //Do not hook up the event model in design mode
            if (DesignerProperties.GetIsInDesignMode(element))
                return;

            if (isUIElementSet)
                throw new InvalidOperationException("UIElement is already set.");

            isUIElementSet = true;

            element.GotFocus += element_GotFocus;
            element.LostFocus += element_LostFocus;

            var contextManager = WpfContext.GetManagerForElement(element);
            contextManager.RegisterProvider(this);
        }


        private void element_GotFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            this.OnEnterContext();
        }

        private void element_LostFocus(object sender, RoutedEventArgs e)
        {
            this.OnLeaveContext();
        }

        void context_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
                        var context = (WpfContext)newItem;
                        context.PropertyChanged += context_PropertyChanged;
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (var oldItem in e.OldItems)
                    {
                        var context = (WpfContext)oldItem;
                        context.PropertyChanged -= context_PropertyChanged;
                    }
                    break;
            }
        }
    }
}
