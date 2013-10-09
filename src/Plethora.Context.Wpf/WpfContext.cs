using System.Windows;
using System.Windows.Data;

namespace Plethora.Context.Wpf
{
    public static class WpfContext
    {
        #region ContextSource Attached Property

        public static readonly DependencyProperty ContextSourceTemplateProperty =
            DependencyProperty.RegisterAttached(
                "ContextSourceTemplate",
                typeof(IWpfContextSourceTemplate),
                typeof (WpfContext),
                new PropertyMetadata(null, ContextSourceChangedCallback));

        private static void ContextSourceChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (ReferenceEquals(e.OldValue, e.NewValue))
                return;

            UIElement element = (UIElement)dependencyObject;
            IWpfContextSourceTemplate contextTemplate = (IWpfContextSourceTemplate)e.NewValue;
            WpfContextSourceBase contextSource = contextTemplate.CreateContent();
            contextSource.UIElement = element;

            if (element is FrameworkElement)
            {
                Binding dataContextBinding = new Binding();
                dataContextBinding.Source = element;
                dataContextBinding.Path = new PropertyPath("DataContext");
                BindingOperations.SetBinding(contextSource, FrameworkElement.DataContextProperty, dataContextBinding);
            }

            var provider = GetContextProvider(element);
            provider.ContextSource = contextSource;
        }

        public static void SetContextSourceTemplate(UIElement element, IWpfContextSourceTemplate value)
        {
            element.SetValue(ContextSourceTemplateProperty, value);
        }

        public static IWpfContextSourceTemplate GetContextSourceTemplate(UIElement element)
        {
            return (IWpfContextSourceTemplate)element.GetValue(ContextSourceTemplateProperty);
        }

        #endregion

        #region ContextProvider Attached Property

        internal static readonly DependencyProperty ContextProviderProperty = DependencyProperty.RegisterAttached(
            "ContextProvider_", //Intentionally renamed to force WPF to use the Get method below
            typeof(WpfContextProvider),
            typeof(WpfContext),
            new PropertyMetadata(default(WpfContextProvider)));

        private static WpfContextProvider GetContextProvider(UIElement element)
        {
            var provider = (WpfContextProvider)element.GetValue(ContextProviderProperty);
            if (provider == null)
            {
                provider = new WpfContextProvider(element);
                element.SetValue(ContextProviderProperty, provider);
            }

            return provider;
        }

        #endregion

        #region Manager Attached Property

        public static readonly DependencyProperty ManagerProperty =
            DependencyProperty.RegisterAttached(
                "Manager",
                typeof(ContextManager),
                typeof(WpfContext),
                new PropertyMetadata(default(ContextManager)));

        public static void SetManager(DependencyObject dependencyObject, ContextManager value)
        {
            dependencyObject.SetValue(ManagerProperty, value);
        }

        public static ContextManager GetManager(DependencyObject dependencyObject)
        {
            return (ContextManager)dependencyObject.GetValue(ManagerProperty);
        }

        #endregion

        #region IsActivityItem Attached Property

        public static readonly DependencyProperty IsActivityItemProperty =
            DependencyProperty.RegisterAttached(
                "IsActivityItem",
                typeof(bool),
                typeof(WpfContext),
                new PropertyMetadata(false, IsActivityItemChangedCallback));

        private static void IsActivityItemChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;

            if (((bool)e.NewValue) == true)
                ActivityItemRegister.Instance.RegisterActivityItem(dependencyObject);
            else
                ActivityItemRegister.Instance.DeregisterActivityItem(dependencyObject);
        }

        public static void SetIsActivityItem(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(IsActivityItemProperty, value);
        }

        public static bool GetIsActivityItem(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(IsActivityItemProperty);
        }

        #endregion

        public static ContextManager GetManagerForElement(UIElement element)
        {
            bool isDefault;
            return GetManagerForElement(element, out isDefault);
        }

        public static ContextManager GetManagerForElement(UIElement element, out bool isDefault)
        {
            DependencyObject obj = element;

            isDefault = false;
            ContextManager contextManager = null;
            while (obj != null)
            {
                contextManager = GetManager(obj);

                if (contextManager != null)
                    break;

                obj = LogicalTreeHelper.GetParent(obj);
            }

            if (contextManager == null)
            {
                isDefault = true;
                contextManager = ContextManager.DefaultInstance;
            }

            return contextManager;
        }

    }
}
