using System;
using System.Collections.Generic;
using System.Windows;

namespace Plethora.Context.Wpf
{
    public abstract class WpfContextSourceBase : FrameworkElement, IWpfContextSource
    {
        #region Implementation of IWpfContextInfo

        #region UIElement DependencyProperty

        public UIElement UIElement
        {
            get { return (UIElement)this.GetValue(UIElementProperty); }
            set { this.SetValue(UIElementProperty, value); }
        }

        public static readonly DependencyProperty UIElementProperty = DependencyProperty.Register(
            nameof(UIElement),
            typeof(UIElement),
            typeof(WpfContextSourceBase),
            new PropertyMetadata(default(UIElement), UIElementChanged));

        private static void UIElementChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue)
                return;

            var uiElement = (UIElement)e.NewValue;
            var contextSource = (WpfContextSourceBase)dependencyObject;

            contextSource.UIElementChanged(uiElement);
        }

        protected virtual void UIElementChanged(UIElement uiElement)
        {
        }

        #endregion

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
            var handler = this.ContextChanged;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        public abstract IEnumerable<ContextInfo> Contexts { get; }

        #endregion
    }
}
