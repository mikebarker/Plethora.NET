using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Plethora.Context
{
    internal class XamlContextProvider : CachedContextProvider
    {
        private readonly UIElement element;
        private XamlContextSourceCollection contextSource;

        public XamlContextProvider([NotNull] UIElement element)
        {
            //Validation
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            if (!(element is DependencyObject))
                throw new ArgumentException(ResourceProvider.ArgMustBeOfType(nameof(element), typeof(DependencyObject)), nameof(element));

            this.element = element;
            this.RegisterUIElement(this.element);
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
        public UIElement Element
        {
            get { return this.element; }
        }

        private void RegisterUIElement([NotNull] UIElement element)
        {
            //Do not hook up the event model in design mode
            if (DesignMode.DesignModeEnabled)
                return;

            element.GotFocus += this.element_GotKeyboardFocus;
            element.LostFocus += this.element_LostKeyboardFocus;

            ContextManager contextManager = XamlContext.GetContextManagerForElement((DependencyObject)element);
            contextManager.RegisterProvider(this);
        }

        private void element_GotKeyboardFocus(object sender, RoutedEventArgs e)
        {
            this.OnEnterContext();
        }

        private void element_LostKeyboardFocus(object sender, RoutedEventArgs e)
        {
            UIElement source = (UIElement)sender;
            if (!ReferenceEquals(source, this.Element))
                source.LostFocus -= this.element_LostKeyboardFocus;

            UIElement activeControl = FocusManager.GetFocusedElement() as UIElement;
            if (activeControl == null)
            {
                this.OnLeaveContext();
                return;
            }

            if (this.IsActivityElement(activeControl))
            {
                if (!ReferenceEquals(activeControl, this.Element))
                    activeControl.LostFocus += this.element_LostKeyboardFocus;
            }
            else
            {
                this.OnLeaveContext();
            }

            // Helper for debugging
            // Debug.WriteLine($"Leaving {sender.GetType().Name} entering {activeControl.GetType().Name}");
        }

        private bool IsActivityElement([CanBeNull] UIElement element)
        {
            // Special cases
            // These types allow the right-click menus to behave as expected.
            if (element is MenuFlyoutPresenter)
                return true;

            if (element is MenuFlyoutItemBase)
                return true;

            if (element is AppBarButton)
                return true;

            if (element is Popup)
                return true;


            DependencyObject obj = element;
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
