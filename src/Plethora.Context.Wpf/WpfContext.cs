using System.Windows;

namespace Plethora.Context.Wpf
{
    public static class WpfContext
    {
        #region ContextSource Attached Property

        public static readonly DependencyProperty ContextSourceProperty =
            DependencyProperty.RegisterAttached(
                "ContextSource",
                typeof (IWpfContextSource),
                typeof (UIElement),
                new PropertyMetadata(null, ContextSourceChangedCallback));

        private static void ContextSourceChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (ReferenceEquals(e.OldValue, e.NewValue))
                return;

            UIElement element = (UIElement)dependencyObject;
            IWpfContextSource contextSource = (IWpfContextSource)e.NewValue;

            var provider = GetContextProvider(element);
            provider.ContextSource = contextSource;
        }

        public static void SetContextSource(UIElement element, IWpfContextSource value)
        {
            element.SetValue(ContextSourceProperty, value);
        }

        public static IWpfContextSource GetContextSource(UIElement element)
        {
            return (IWpfContextSource)element.GetValue(ContextSourceProperty);
        }

        #endregion


        #region ContextProvider DependencyProperty

        internal static readonly DependencyProperty ContextProviderProperty = DependencyProperty.RegisterAttached(
            "ContextProvider_", //Intentionally renamed to force WPF to use the Get method below
            typeof(WpfContextProvider),
            typeof(UIElement),
            new PropertyMetadata(default(WpfContextProvider)));

        private static void SetContextProvider(UIElement element, WpfContextProvider value)
        {
            element.SetValue(ContextProviderProperty, value);
        }

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
                typeof(UIElement),
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
