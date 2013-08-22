using System.Collections;
using System.ComponentModel;
using System.Windows;

namespace Plethora.Context.Wpf
{
    public class WpfContext : DependencyObject, INotifyPropertyChanged
    {
        #region Implementation Of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        #region Name DependencyProperty

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
            "Name",
            typeof(string),
            typeof(WpfContext),
            new PropertyMetadata(default(string), PropertyChangedCallback));

        #endregion

        #region Rank DependencyProperty

        public int Rank
        {
            get { return (int)GetValue(RankProperty); }
            set { SetValue(RankProperty, value); }
        }

        public static readonly DependencyProperty RankProperty = DependencyProperty.Register(
            "Rank",
            typeof(int),
            typeof(WpfContext),
            new PropertyMetadata(default(int), PropertyChangedCallback));

        #endregion

        #region Data DependencyProperty

        public object Data
        {
            get { return GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            "Data",
            typeof(object),
            typeof(WpfContext),
            new PropertyMetadata(default(object), PropertyChangedCallback));

        #endregion

        #endregion

        #region Private Methods

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var wpfContext = (WpfContext)dependencyObject;
            wpfContext.OnPropertyChanged(e.Property.Name);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


        #region Providers DependencyProperty

        internal static readonly DependencyProperty ContextCollectionProperty = DependencyProperty.RegisterAttached(
            "ContextCollection_", //Intentionally rename to force WPF to use the Get method below
            typeof(IList),
            typeof(UIElement),
            new PropertyMetadata(default(IList)));

        public static void SetContextCollection(UIElement element, IList value)
        {
            element.SetValue(ContextCollectionProperty, value);
        }

        public static IList GetContextCollection(UIElement element)
        {
            var list = (IList)element.GetValue(ContextCollectionProperty);
            if (list == null)
            {
                list = new WpfContextCollection(element);
                element.SetValue(ContextCollectionProperty, list);
            }

            return list;
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
