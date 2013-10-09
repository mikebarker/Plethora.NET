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
    public class WpfContextSourceCollection : WpfContextSourceBase, IList<WpfContextSourceBase>, IList
    {
        private readonly List<WpfContextSourceBase> innerList = new List<WpfContextSourceBase>();

        #region IEnumerable<WpfContextSourceBase>

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<WpfContextSourceBase> GetEnumerator()
        {
            return innerList.GetEnumerator();
        }

        #endregion

        #region ICollection<WpfContextSourceBase>

        public void Add(WpfContextSourceBase item)
        {
            innerList.Add(item);
            item.ContextChanged += item_ContextChanged;
        }

        public void Clear()
        {
            foreach (WpfContextSourceBase item in innerList)
            {
                item.ContextChanged -= item_ContextChanged;
            }
            innerList.Clear();
        }

        public bool Contains(WpfContextSourceBase item)
        {
            return innerList.Contains(item);
        }

        public void CopyTo(WpfContextSourceBase[] array, int arrayIndex)
        {
            innerList.CopyTo(array, arrayIndex);
        }

        public bool Remove(WpfContextSourceBase item)
        {
            item.ContextChanged -= item_ContextChanged;
            return innerList.Remove(item);
        }

        public int Count
        {
            get { return innerList.Count; }
        }

        bool ICollection<WpfContextSourceBase>.IsReadOnly
        {
            get { return ((ICollection<WpfContextSourceBase>)innerList).IsReadOnly; }
        }

        #endregion

        #region IList<WpfContextSourceBase>

        public int IndexOf(WpfContextSourceBase item)
        {
            return innerList.IndexOf(item);
        }

        public void Insert(int index, WpfContextSourceBase item)
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

        public WpfContextSourceBase this[int index]
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
            var item = (WpfContextSourceBase)value;
            this.Add(item);
            return innerList.Count - 1;
        }

        bool IList.Contains(object value)
        {
            var item = (WpfContextSourceBase)value; 
            return this.Contains(item);
        }

        int IList.IndexOf(object value)
        {
            var item = (WpfContextSourceBase)value; 
            return this.IndexOf(item);
        }

        void IList.Insert(int index, object value)
        {
            var item = (WpfContextSourceBase)value;
            this.Insert(index, item);
        }

        void IList.Remove(object value)
        {
            var item = (WpfContextSourceBase)value; 
            this.Remove(item);
        }

        object IList.this[int index]
        {
            get { return this[index]; }
            set
            {
                var item = (WpfContextSourceBase)value;
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

        #region Implementation of WpfContextSourceBase

        protected override void UIElementChanged(UIElement uiElement)
        {
            base.UIElementChanged(uiElement);

            foreach (WpfContextSourceBase contextSource in this.innerList)
            {
                contextSource.UIElement = uiElement;
            }
        }

        public override IEnumerable<ContextInfo> Contexts
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
