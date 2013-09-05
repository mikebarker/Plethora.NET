using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Plethora.Context.Wpf
{
    internal class WpfContextProvider : IContextProvider
    {
        public WpfContextProvider(UIElement element)
        {
            //Validation
            if (element == null)
                throw new ArgumentNullException("element");


            this.internalContextProvider = new InternalContextProvider(this);
            this.uiElement = element;
            RegisterUIElement(uiElement);
        }

        #region Implementation of IContextProvider

        //Uses containment to provide inheritance.
        private sealed class InternalContextProvider : ContextProvider
        {
            private readonly WpfContextProvider parent;

            public InternalContextProvider(WpfContextProvider parent)
            {
                this.parent = parent;
            }

            public override IEnumerable<ContextInfo> GetContexts()
            {
                return parent.Contexts;
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
            get
            {
                return (ContextSource != null) 
                    ? ContextSource.Contexts
                    : null;
            }
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



        private IWpfContextSource contextSource;

        public IWpfContextSource ContextSource
        {
            get { return contextSource; }
            set 
            {
                if (ReferenceEquals(contextSource, value))
                    return;


                if (contextSource != null)
                {
                    contextSource.UIElement = null;
                    contextSource.ContextChanged -= contextSource_ContextChanged;
                }

                contextSource = value;

                if (contextSource != null)
                {
                    contextSource.UIElement = this.uiElement;
                    contextSource.ContextChanged += contextSource_ContextChanged;
                }
            }
        }

        void contextSource_ContextChanged(object sender, EventArgs e)
        {
            this.OnContextChanged();
        }





        private readonly UIElement uiElement;

        public UIElement UIElement
        {
            get { return uiElement; }
        }

        private void RegisterUIElement(UIElement element)
        {
            //Do not hook up the event model in design mode
            if (DesignerProperties.GetIsInDesignMode(element))
                return;

            element.GotKeyboardFocus += element_GotKeyboardFocus;
            element.LostKeyboardFocus += element_LostKeyboardFocus;

            var contextManager = WpfContext.GetManagerForElement(element);
            contextManager.RegisterProvider(this);

            if (ContextSource != null)
                contextSource.UIElement = uiElement;
        }

        private void element_GotKeyboardFocus(object sender, RoutedEventArgs e)
        {
            OnEnterContext();
        }

        private void element_LostKeyboardFocus(object sender, RoutedEventArgs e)
        {
            UIElement source = (UIElement)sender;
            if (!ReferenceEquals(source, this.UIElement))
                source.LostKeyboardFocus -= element_LostKeyboardFocus;

            UIElement activeControl = Keyboard.FocusedElement as UIElement;
            if (activeControl == null)
            {
                this.OnLeaveContext();
                return;
            }

            bool isActivityControl = false; // TODO: method to determine if active control can provide context activity
            if (isActivityControl)
            {
                if (!ReferenceEquals(activeControl, this.UIElement))
                    activeControl.LostKeyboardFocus += element_LostKeyboardFocus;
            }
            else
            {
                this.OnLeaveContext();
            }

            
            OnLeaveContext();
        }

    }
}
