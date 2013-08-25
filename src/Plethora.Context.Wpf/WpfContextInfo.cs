using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;

namespace Plethora.Context.Wpf
{
    public class WpfContextInfo : DependencyObject, IWpfContextSource
    {
        #region Properties

        #region UIElement DependencyProperty

        public UIElement UIElement
        {
            get { return (UIElement)GetValue(UIElementProperty); }
            set { SetValue(UIElementProperty, value); }
        }

        public static readonly DependencyProperty UIElementProperty = DependencyProperty.Register(
            "UIElement",
            typeof(UIElement),
            typeof(WpfContextInfo),
            new PropertyMetadata(default(UIElement), UIElementChangedCallback));

        private static void UIElementChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var wpfContextInfo = (WpfContextInfo)dependencyObject;

            //Rebind if the element data path is specified
            if (wpfContextInfo.ElementDataPath != null)
                wpfContextInfo.BindDataToUIElement();
        }

        #endregion

        #region Name DependencyProperty

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
            "Name",
            typeof(string),
            typeof(WpfContextInfo),
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
            typeof(WpfContextInfo),
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
            typeof(WpfContextInfo),
            new PropertyMetadata(default(object), PropertyChangedCallback));

        #endregion

        #region ElementDataPath DependencyProperty

        public string ElementDataPath
        {
            get { return (string)GetValue(ElementDataPathProperty); }
            set { SetValue(ElementDataPathProperty, value); }
        }

        public static readonly DependencyProperty ElementDataPathProperty = DependencyProperty.Register(
            "ElementDataPath",
            typeof(string),
            typeof(WpfContextInfo),
            new PropertyMetadata(default(string), ElementDataPathChangedCallback));

        private static void ElementDataPathChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var wpfContextInfo = (WpfContextInfo)dependencyObject;
            wpfContextInfo.BindDataToUIElement();
        }

        #endregion

        public ContextInfo Context
        {
            get { return new ContextInfo(this.Name, this.Rank, this.Data); }
        }

        #endregion

        #region Implementation of IWpfContextInfo

        #region ContextChanged Event

        /// <summary>
        /// Raised when <see cref="Context"/> property changes.
        /// </summary>
        public event EventHandler ContextChanged;

        /// <summary>
        /// Raises the <see cref="ContextChanged"/> event.
        /// </summary>
        protected virtual void OnContextChanged(EventArgs e)
        {
            var handler = ContextChanged;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        IEnumerable<ContextInfo> IWpfContextSource.Contexts
        {
            get { return new[] {this.Context}; }
        }

        #endregion

        #region Private Methods

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var wpfContextInfo = (WpfContextInfo)dependencyObject;
            wpfContextInfo.OnContextChanged(EventArgs.Empty);
        }


        private void BindDataToUIElement()
        {
            BindingOperations.ClearBinding(this, DataProperty);

            //Do not re-bind if either the UIElement or the ElementDataPath are null
            if ((ElementDataPath == null) || (UIElement == null))
                return;

            Binding binding = new Binding();
            binding.Source = this.UIElement;
            binding.Path = new PropertyPath(this.ElementDataPath);

            BindingOperations.SetBinding(this, DataProperty, binding);
        }

        #endregion
    }
}
