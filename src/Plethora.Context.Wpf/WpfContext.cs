using System.Windows;
using System.Windows.Data;
using Plethora.Context.Action;

namespace Plethora.Context.Wpf
{
    /// <summary>
    /// Helper class which defines the attached properties required for using context within WPF XAML mark-up.
    /// </summary>
    public static class WpfContext
    {
        #region ContextSourceTemplate Attached Property

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

        #region ContextManager Attached Property

        public static readonly DependencyProperty ContextManagerProperty =
            DependencyProperty.RegisterAttached(
                "ContextManager",
                typeof(ContextManager),
                typeof(WpfContext),
                new PropertyMetadata(default(ContextManager)));

        public static void SetContextManager(DependencyObject dependencyObject, ContextManager value)
        {
            dependencyObject.SetValue(ContextManagerProperty, value);
        }

        public static ContextManager GetContextManager(DependencyObject dependencyObject)
        {
            return (ContextManager)dependencyObject.GetValue(ContextManagerProperty);
        }

        #endregion

        #region ActionManager Attached Property

        public static readonly DependencyProperty ActionManagerProperty =
            DependencyProperty.RegisterAttached(
                "ActionManager",
                typeof(ActionManager),
                typeof(WpfContext),
                new PropertyMetadata(default(ActionManager)));

        public static void SetActionManager(DependencyObject dependencyObject, ActionManager value)
        {
            dependencyObject.SetValue(ActionManagerProperty, value);
        }

        public static ActionManager GetActionManager(DependencyObject dependencyObject)
        {
            return (ActionManager)dependencyObject.GetValue(ActionManagerProperty);
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

        /// <summary>
        /// Get the <see cref="ContextManager"/> from the <see cref="UIElement"/> or one of its parents (from the logical tree).
        /// </summary>
        /// <param name="element">
        /// The <see cref="UIElement"/> for which the <see cref="ContextManager"/> is required.
        /// </param>
        /// <returns>
        /// Returns the value of the <see cref="ContextManagerProperty"/> defined against the <paramref name="element"/>.
        /// If <paramref name="element"/> does not have a value for <see cref="ContextManagerProperty"/> then its logical 
        /// parent is tested. This process is repeated until an value is located.
        /// If no value is assigned to <see cref="ContextManagerProperty"/> within the logical tree, then 
        /// <see cref="ContextManager.GlobalInstance"/> is returned.
        /// </returns>
        public static ContextManager GetContextManagerForElement(UIElement element)
        {
            ContextManager contextManager = GetDependencyPropertyFromLogicalTree(element, ContextManagerProperty) as ContextManager;

            if (contextManager == null)
                return ContextManager.GlobalInstance;

            return contextManager;
        }


        /// <summary>
        /// Get the <see cref="ActionManager"/> from the <see cref="UIElement"/> or one of its parents (from the logical tree).
        /// </summary>
        /// <param name="element">
        /// The <see cref="UIElement"/> for which the <see cref="ActionManager"/> is required.
        /// </param>
        /// <returns>
        /// Returns the value of the <see cref="ActionManagerProperty"/> defined against the <paramref name="element"/>.
        /// If <paramref name="element"/> does not have a value for <see cref="ActionManagerProperty"/> then its logical 
        /// parent is tested. This process is repeated until an value is located.
        /// If no value is assigned to <see cref="ActionManagerProperty"/> within the logical tree, then 
        /// <see cref="ActionManager.GlobalInstance"/> is returned.
        /// </returns>
        public static ActionManager GetActionManagerForElement(UIElement element)
        {
            ActionManager actionManager = GetDependencyPropertyFromLogicalTree(element, ActionManagerProperty) as ActionManager;

            if (actionManager == null)
                return ActionManager.GlobalInstance;

            return actionManager;
        }


        /// <summary>
        /// Searches up the logical tree to retrieve the first instance of a <see cref="DependencyProperty"/>
        /// assigned to a <see cref="DependencyObject"/> or one of its parents.
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> from which the searched is initiated.</param>
        /// <param name="dependencyProperty">The <see cref="DependencyProperty"/> for which the value is required.</param>
        /// <returns>
        /// The value of the first instance of the <see cref="DependencyProperty"/> located in the logical tree.
        /// </returns>
        private static object GetDependencyPropertyFromLogicalTree(
            DependencyObject obj,
            DependencyProperty dependencyProperty)
        {
            object item = null;
            while (obj != null)
            {
                item = obj.GetValue(dependencyProperty);

                if (item != null)
                    break;

                obj = LogicalTreeHelper.GetParent(obj);
            }

            return item;
        }

    }
}
