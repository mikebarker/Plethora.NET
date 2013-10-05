using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Plethora.Context.Wpf
{
    /// <summary>
    /// A collection of context sources.
    /// </summary>
    public class WpfContextSourceCollection : FrameworkElement, IList<IWpfContextSource>, IList, IWpfContextSource
    {
        private readonly List<IWpfContextSource> innerList = new List<IWpfContextSource>();

        #region IEnumerable<IWpfContextSource>

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<IWpfContextSource> GetEnumerator()
        {
            return innerList.GetEnumerator();
        }

        #endregion

        #region ICollection<IWpfContextSource>

        public void Add(IWpfContextSource item)
        {
            innerList.Add(item);
            item.ContextChanged += item_ContextChanged;
        }

        public void Clear()
        {
            foreach (IWpfContextSource item in innerList)
            {
                item.ContextChanged -= item_ContextChanged;
            }
            innerList.Clear();
        }

        public bool Contains(IWpfContextSource item)
        {
            return innerList.Contains(item);
        }

        public void CopyTo(IWpfContextSource[] array, int arrayIndex)
        {
            innerList.CopyTo(array, arrayIndex);
        }

        public bool Remove(IWpfContextSource item)
        {
            item.ContextChanged -= item_ContextChanged;
            return innerList.Remove(item);
        }

        public int Count
        {
            get { return innerList.Count; }
        }

        bool ICollection<IWpfContextSource>.IsReadOnly
        {
            get { return ((ICollection<IWpfContextSource>)innerList).IsReadOnly; }
        }

        #endregion

        #region IList<IWpfContextSource>

        public int IndexOf(IWpfContextSource item)
        {
            return innerList.IndexOf(item);
        }

        public void Insert(int index, IWpfContextSource item)
        {
            innerList.Insert(index, item);
            item.ContextChanged += item_ContextChanged;
        }

        public void RemoveAt(int index)
        {
            var item = innerList[index];
            item.ContextChanged -= item_ContextChanged;
            innerList.RemoveAt(index);
        }

        public IWpfContextSource this[int index]
        {
            get { return innerList[index]; }
            set
            {
                var oldValue = innerList[index];
                if (oldValue == value)
                    return;

                oldValue.ContextChanged -= item_ContextChanged;

                innerList[index] = value;

                value.ContextChanged += item_ContextChanged;
            }
        }

        #endregion

        #region ICollection

        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)innerList).CopyTo(array, index);
        }

        object ICollection.SyncRoot
        {
            get { return ((ICollection)innerList).SyncRoot; }
        }

        bool ICollection.IsSynchronized
        {
            get { return ((ICollection)innerList).IsSynchronized; }
        }

        #endregion

        #region IList

        int IList.Add(object value)
        {
            var item = (IWpfContextSource)value;
            this.Add(item);
            return innerList.Count - 1;
        }

        bool IList.Contains(object value)
        {
            var item = (IWpfContextSource)value; 
            return this.Contains(item);
        }

        int IList.IndexOf(object value)
        {
            var item = (IWpfContextSource)value; 
            return this.IndexOf(item);
        }

        void IList.Insert(int index, object value)
        {
            var item = (IWpfContextSource)value;
            this.Insert(index, item);
        }

        void IList.Remove(object value)
        {
            var item = (IWpfContextSource)value; 
            this.Remove(item);
        }

        object IList.this[int index]
        {
            get { return this[index]; }
            set
            {
                var item = (IWpfContextSource)value;
                this[index] = item;
            }
        }

        bool IList.IsReadOnly
        {
            get { return ((IList)innerList).IsReadOnly; }
        }

        bool IList.IsFixedSize
        {
            get { return ((IList)innerList).IsFixedSize; }
        }

        #endregion

        #region Implementation of IWpfContextInfo

        #region UIElement DependencyProperty

        public UIElement UIElement
        {
            get { return (UIElement)GetValue(UIElementProperty); }
            set { SetValue(UIElementProperty, value); }
        }

        public static readonly DependencyProperty UIElementProperty = DependencyProperty.Register(
            "UIElement",
            typeof(UIElement),
            typeof(WpfContextSourceCollection),
            new PropertyMetadata(default(UIElement), UIElementChanged));

        private static void UIElementChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue)
                return;

            var uiElement = (UIElement)e.NewValue;
            var collection = (WpfContextSourceCollection)dependencyObject;
            foreach (IWpfContextSource contextSource in collection)
            {
                contextSource.UIElement = uiElement;
            }
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
            var handler = ContextChanged;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        IEnumerable<ContextInfo> IWpfContextSource.Contexts
        {
            get { return this.SelectMany(item => item.Contexts); }
        }

        #endregion

        void item_ContextChanged(object sender, EventArgs e)
        {
            this.OnContextChanged(EventArgs.Empty);
        }

    }

}
