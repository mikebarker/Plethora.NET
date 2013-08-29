using System;
using System.Collections.Generic;
using System.Windows;

namespace Plethora.Context.Wpf
{
    public class WpfContextSource : Freezable, IWpfContextSource
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
            typeof(WpfContextSource),
            new PropertyMetadata(default(UIElement)));

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
            typeof(WpfContextSource),
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
            typeof(WpfContextSource),
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
            typeof(WpfContextSource),
            new PropertyMetadata(default(object), PropertyChangedCallback));

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
            var wpfContextInfo = (WpfContextSource)dependencyObject;
            wpfContextInfo.OnContextChanged(EventArgs.Empty);
        }

        #endregion


        protected override Freezable CreateInstanceCore()
        {
            return new WpfContextSource();
        }
    }
}
