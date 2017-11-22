using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using JetBrains.Annotations;

namespace Plethora.Context
{
    internal class XamlContextProvider : CachedContextProvider
    {
        private readonly IInputElement inputElement;
        private XamlContextSourceCollection contextSource;

        public XamlContextProvider([NotNull] IInputElement element)
        {
            //Validation
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            if (!(element is DependencyObject))
                throw new ArgumentException(ResourceProvider.ArgMustBeOfType(nameof(element), typeof(DependencyObject)), nameof(element));


            this.inputElement = element;
            this.RegisterUIElement(this.inputElement);
        }

        [CanBeNull]
        protected override IEnumerable<ContextInfo> GetContexts()
        {
            return (this.ContextSourceCollection != null)
                ? this.ContextSourceCollection.Contexts
                : null;
        }

        [CanBeNull]
        public XamlContextSourceCollection ContextSourceCollection
        {
            get { return this.contextSource; }
            set 
            {
                if (ReferenceEquals(this.contextSource, value))
                    return;


                if (this.contextSource != null)
                {
                    this.contextSource.ContextChanged -= this.contextSource_ContextChanged;
                }

                this.contextSource = value;

                if (this.contextSource != null)
                {
                    this.contextSource.ContextChanged += this.contextSource_ContextChanged;
                }
            }
        }

        void contextSource_ContextChanged(object sender, EventArgs e)
        {
            this.OnContextChanged();
        }

        [NotNull]
        public IInputElement InputElement
        {
            get { return this.inputElement; }
        }

        private void RegisterUIElement([NotNull] IInputElement element)
        {
            //Do not hook up the event model in design mode
            if (DesignerProperties.GetIsInDesignMode((DependencyObject)element))
                return;

            element.GotKeyboardFocus += this.element_GotKeyboardFocus;
            element.LostKeyboardFocus += this.element_LostKeyboardFocus;

            ContextManager contextManager = XamlContext.GetContextManagerForElement((DependencyObject)element);
            contextManager.RegisterProvider(this);
        }

        private void element_GotKeyboardFocus(object sender, RoutedEventArgs e)
        {
            this.OnEnterContext();
        }

        private void element_LostKeyboardFocus(object sender, RoutedEventArgs e)
        {
            IInputElement source = (IInputElement)sender;
            if (!ReferenceEquals(source, this.InputElement))
                source.LostKeyboardFocus -= this.element_LostKeyboardFocus;

            IInputElement activeControl = Keyboard.FocusedElement as UIElement;
            if (activeControl == null)
            {
                this.OnLeaveContext();
                return;
            }

            if (this.IsActivityElement(activeControl))
            {
                if (!ReferenceEquals(activeControl, this.InputElement))
                    activeControl.LostKeyboardFocus += this.element_LostKeyboardFocus;
            }
            else
            {
                this.OnLeaveContext();
            }
        }

        private bool IsActivityElement([CanBeNull] IInputElement element)
        {
            DependencyObject obj = element as DependencyObject;
            while (obj != null)
            {
                if (ActivityItemRegister.Instance.IsActivityItem(obj))
                    return true;

                obj = VisualTreeHelper.GetParent(obj);
            }

            return false;
        }

    }
}
