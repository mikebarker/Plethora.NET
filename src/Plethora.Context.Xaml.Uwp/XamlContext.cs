using Plethora.Context.Action;
using Windows.UI.Xaml;

namespace Plethora.Context
{
    /// <summary>
    /// Helper class which defines the attached properties required for using context within WPF XAML mark-up.
    /// </summary>
    public static class XamlContext
    {
        #region ContextSource Attached Property

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.RegisterAttached(
                "Source_", //Intentionally misnamed to force WPF to use the Get method below
                typeof(XamlContextSourceCollection),
                typeof(XamlContext),
                new PropertyMetadata(null, SourceChangedCallback));

        private static void SourceChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (ReferenceEquals(e.OldValue, e.NewValue))
                return;

            XamlContextSourceCollection contextSourceCollection = (XamlContextSourceCollection)e.NewValue;

            UIElement element = (UIElement)dependencyObject;
            XamlContextProvider provider = GetContextProvider(element);

            provider.ContextSourceCollection = contextSourceCollection;
        }

        public static XamlContextSourceCollection GetSource(UIElement element)
        {
            var collection = (XamlContextSourceCollection)element.GetValue(SourceProperty);
            if (collection == null)
            {
                collection = new XamlContextSourceCollection();
                element.SetValue(SourceProperty, collection);
            }
            return collection;
        }

        #endregion

        #region ContextProvider Attached Property

        internal static readonly DependencyProperty ContextProviderProperty = DependencyProperty.RegisterAttached(
            "ContextProvider_", //Intentionally misnamed to force WPF to use the Get method below
            typeof(XamlContextProvider),
            typeof(XamlContext),
            new PropertyMetadata(default(XamlContextProvider)));

        private static XamlContextProvider GetContextProvider(UIElement element)
        {
            var provider = (XamlContextProvider)element.GetValue(ContextProviderProperty);
            if (provider == null)
            {
                provider = new XamlContextProvider(element);
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
                typeof(XamlContext),
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
                typeof(XamlContext),
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

        /// <summary>
        /// Identifies the IsActivityItem property.
        /// </summary>
        public static readonly DependencyProperty IsActivityItemProperty =
            DependencyProperty.RegisterAttached(
                "IsActivityItem",
                typeof(bool),
                typeof(XamlContext),
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

        /// <summary>
        /// Sets a flag indicating whether the <see cref="DependencyObject"/> should skip causing the context to
        /// change when it receives the keyboard focus.
        /// </summary>
        /// <param name="dependencyObject">The <see cref="DependencyObject"/> for which the property is to be set.</param>
        /// <param name="value">
        /// True if the the <see cref="DependencyObject"/> should skip causing the context to
        /// change when it receives the keyboard focus; otherwise false.
        /// </param>
        public static void SetIsActivityItem(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(IsActivityItemProperty, value);
        }

        /// <summary>
        /// Gets a flag indicating whether the <see cref="DependencyObject"/> should skip causing the context to
        /// change when it receives the keyboard focus.
        /// </summary>
        /// <param name="dependencyObject">The <see cref="DependencyObject"/> for which the property is required.</param>
        /// <returns>
        /// True if the the <see cref="DependencyObject"/> should skip causing the context to
        /// change when it receives the keyboard focus; otherwise false.
        /// </returns>
        public static bool GetIsActivityItem(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(IsActivityItemProperty);
        }

        #endregion

        /// <summary>
        /// Get the <see cref="ContextManager"/> from the <see cref="DependencyObject"/> or one of its parents (from the logical tree).
        /// </summary>
        /// <param name="obj">
        /// The <see cref="DependencyObject"/> for which the <see cref="ContextManager"/> is required.
        /// </param>
        /// <returns>
        /// Returns the value of the <see cref="ContextManagerProperty"/> defined against the <paramref name="obj"/>.
        /// If <paramref name="obj"/> does not have a value for <see cref="ContextManagerProperty"/> then its logical 
        /// parent is tested. This process is repeated until an value is located.
        /// If no value is assigned to <see cref="ContextManagerProperty"/> within the logical tree, then 
        /// <see cref="ContextManager.GlobalInstance"/> is returned.
        /// </returns>
        public static ContextManager GetContextManagerForElement(DependencyObject obj)
        {
            ContextManager contextManager = GetDependencyPropertyFromLogicalTree(obj, ContextManagerProperty) as ContextManager;

            if (contextManager == null)
                return ContextManager.GlobalInstance;

            return contextManager;
        }


        /// <summary>
        /// Get the <see cref="ActionManager"/> from the <see cref="DependencyObject"/> or one of its parents (from the logical tree).
        /// </summary>
        /// <param name="obj">
        /// The <see cref="DependencyObject"/> for which the <see cref="ActionManager"/> is required.
        /// </param>
        /// <returns>
        /// Returns the value of the <see cref="ActionManagerProperty"/> defined against the <paramref name="obj"/>.
        /// If <paramref name="obj"/> does not have a value for <see cref="ActionManagerProperty"/> then its logical 
        /// parent is tested. This process is repeated until an value is located.
        /// If no value is assigned to <see cref="ActionManagerProperty"/> within the logical tree, then 
        /// <see cref="ActionManager.GlobalInstance"/> is returned.
        /// </returns>
        public static ActionManager GetActionManagerForElement(DependencyObject obj)
        {
            ActionManager actionManager = GetDependencyPropertyFromLogicalTree(obj, ActionManagerProperty) as ActionManager;

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
            FrameworkElement element = obj as FrameworkElement;

            object item = null;
            while (element != null)
            {
                item = element.GetValue(dependencyProperty);

                if (item != null)
                    break;

                element = element.Parent as FrameworkElement;
            }

            return item;
        }

    }
}
