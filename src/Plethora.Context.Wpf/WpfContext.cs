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
                typeof (WpfContext),
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
            typeof(WpfContext),
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

        #region ActivityItemRegister Attached Property

        public static readonly DependencyProperty ActivityItemRegisterProperty =
            DependencyProperty.RegisterAttached(
                "ActivityItemRegister",
                typeof(ActivityItemRegister),
                typeof(WpfContext),
                new PropertyMetadata(default(ActivityItemRegister)));

        public static void SetActivityItemRegister(DependencyObject dependencyObject, ActivityItemRegister value)
        {
            dependencyObject.SetValue(ActivityItemRegisterProperty, value);
        }

        public static ActivityItemRegister GetActivityItemRegister(DependencyObject dependencyObject)
        {
            return (ActivityItemRegister)dependencyObject.GetValue(ActivityItemRegisterProperty);
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

        public static ActivityItemRegister GetActivityItemRegisterForElement(UIElement element)
        {
            bool isDefault;
            return GetActivityItemRegisterForElement(element, out isDefault);
        }

        public static ActivityItemRegister GetActivityItemRegisterForElement(UIElement element, out bool isDefault)
        {
            DependencyObject obj = element;

            isDefault = false;
            ActivityItemRegister activityItemRegister = null;
            while (obj != null)
            {
                activityItemRegister = GetActivityItemRegister(obj);

                if (activityItemRegister != null)
                    break;

                obj = LogicalTreeHelper.GetParent(obj);
            }

            if (activityItemRegister == null)
                isDefault = true;

            return activityItemRegister;
        }
    }
}
