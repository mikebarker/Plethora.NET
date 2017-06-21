using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Plethora.Context.Wpf
{
    internal class WpfContextProvider : CachedContextProvider
    {
        public WpfContextProvider(UIElement element)
        {
            //Validation
            if (element == null)
                throw new ArgumentNullException(nameof(element));


            this.uiElement = element;
            this.RegisterUIElement(this.uiElement);
        }

        #region Implementation of IContextProvider

        protected override IEnumerable<ContextInfo> GetContexts()
        {
            return (this.ContextSource != null) 
                ? this.ContextSource.Contexts
                : null;
        }

        #endregion



        private IWpfContextSource contextSource;

        public IWpfContextSource ContextSource
        {
            get { return this.contextSource; }
            set 
            {
                if (ReferenceEquals(this.contextSource, value))
                    return;


                if (this.contextSource != null)
                {
                    this.contextSource.UIElement = null;
                    this.contextSource.ContextChanged -= this.contextSource_ContextChanged;
                }

                this.contextSource = value;

                if (this.contextSource != null)
                {
                    this.contextSource.UIElement = this.uiElement;
                    this.contextSource.ContextChanged += this.contextSource_ContextChanged;
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
            get { return this.uiElement; }
        }

        private void RegisterUIElement(UIElement element)
        {
            //Do not hook up the event model in design mode
            if (DesignerProperties.GetIsInDesignMode(element))
                return;

            element.GotKeyboardFocus += this.element_GotKeyboardFocus;
            element.LostKeyboardFocus += this.element_LostKeyboardFocus;

            var contextManager = WpfContext.GetContextManagerForElement(element);
            contextManager.RegisterProvider(this);

            if (this.ContextSource != null)
                this.contextSource.UIElement = element;
        }

        private void element_GotKeyboardFocus(object sender, RoutedEventArgs e)
        {
            this.OnEnterContext();
        }

        private void element_LostKeyboardFocus(object sender, RoutedEventArgs e)
        {
            UIElement source = (UIElement)sender;
            if (!ReferenceEquals(source, this.UIElement))
                source.LostKeyboardFocus -= this.element_LostKeyboardFocus;

            UIElement activeControl = Keyboard.FocusedElement as UIElement;
            if (activeControl == null)
            {
                this.OnLeaveContext();
                return;
            }

            if (this.IsActivityElement(activeControl))
            {
                if (!ReferenceEquals(activeControl, this.UIElement))
                    activeControl.LostKeyboardFocus += this.element_LostKeyboardFocus;
            }
            else
            {
                this.OnLeaveContext();
            }
        }

        private bool IsActivityElement(UIElement element)
        {
            DependencyObject obj = element;
            while (obj != null)
            {
                if (ActivityItemRegister.Instance.IsActivityItem(obj))
                    return true;

                obj = LogicalTreeHelper.GetParent(obj);
            }

            return false;
        }

    }
}
