﻿using System;
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
                throw new ArgumentNullException("element");


            this.uiElement = element;
            RegisterUIElement(uiElement);
        }

        #region Implementation of IContextProvider

        protected override IEnumerable<ContextInfo> GetContexts()
        {
            return (ContextSource != null) 
                ? ContextSource.Contexts
                : null;
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

            element.GotFocus += element_GotFocus;
            element.LostFocus += element_LostFocus;

            var contextManager = WpfContext.GetManagerForElement(element);
            contextManager.RegisterProvider(this);

            if (ContextSource != null)
                contextSource.UIElement = element;
        }

        private void element_GotFocus(object sender, RoutedEventArgs e)
        {
            OnEnterContext();
        }

        private void element_LostFocus(object sender, RoutedEventArgs e)
        {
            UIElement source = (UIElement)sender;
            if (!ReferenceEquals(source, this.UIElement))
                source.LostFocus -= element_LostFocus;

            UIElement activeControl = Keyboard.FocusedElement as UIElement;
            if (activeControl == null)
            {
                this.OnLeaveContext();
                return;
            }

            if (IsActivityElement(activeControl))
            {
                if (!ReferenceEquals(activeControl, this.UIElement))
                    activeControl.LostFocus += element_LostFocus;
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
