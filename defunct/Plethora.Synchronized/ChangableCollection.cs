using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Plethora.ComponentModel;
using Plethora.Synchronized.Change;

namespace Plethora.Synchronized
{
    public class ChangableCollection<TKey, T> : INotifyCollectionChanged, INotifyPropertyChanged, IBindingList, IList<T>
    {
        #region Fields

        private readonly FacadeCollection<TKey, T> facadeCollection;
        #endregion

        #region Constructors

        public ChangableCollection(IChangeSource inBoundChannel, IChangeSink outBoundChannel, Func<T, TKey> getKeyFunc, Func<T, T> cloneFunc)
            : this(inBoundChannel, outBoundChannel, getKeyFunc, cloneFunc, Comparer<TKey>.Default, new SynchronizeInvoke())
        {
        }

        public ChangableCollection(IChangeSource inBoundChannel, IChangeSink outBoundChannel, Func<T, TKey> getKeyFunc, Func<T, T> cloneFunc, Comparer<TKey> keyComparer)
            : this(inBoundChannel, outBoundChannel, getKeyFunc, cloneFunc, keyComparer, new SynchronizeInvoke())
        {
        }

        public ChangableCollection(IChangeSource inBoundChannel, IChangeSink outBoundChannel, Func<T, TKey> getKeyFunc, Func<T, T> cloneFunc, Comparer<TKey> keyComparer, ISynchronizeInvoke syncInvoke)
        {
            if (inBoundChannel == null)
                throw new ArgumentNullException(nameof(inBoundChannel));

            if (outBoundChannel == null)
                throw new ArgumentNullException(nameof(outBoundChannel));

            if (getKeyFunc == null)
                throw new ArgumentNullException(nameof(getKeyFunc));

            if (cloneFunc == null)
                throw new ArgumentNullException(nameof(cloneFunc));

            if (keyComparer == null)
                throw new ArgumentNullException(nameof(keyComparer));

            if (syncInvoke == null)
                throw new ArgumentNullException(nameof(syncInvoke));


            SyncCollection<TKey, T> syncCollection = new SyncCollection<TKey, T>(getKeyFunc, keyComparer, syncInvoke);
            this.facadeCollection = new FacadeCollection<TKey, T>(syncCollection, cloneFunc, new ChangeSourceIdProvider());


            //Bind the sources and sinks
            this.facadeCollection.ChangePublished += (sender, e) => outBoundChannel.ApplyChange(e.Change);
            inBoundChannel.ChangePublished += (sender, e) => syncCollection.ApplyChange(e.Change);
        }

        #endregion

        public bool IsLocalChange(TKey key)
        {
            return this.facadeCollection.IsLocalChange(key);
        }

        #region Implementation of INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { this.facadeCollection.CollectionChanged += value; }
            remove { this.facadeCollection.CollectionChanged -= value; }
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { this.facadeCollection.PropertyChanged += value; }
            remove { this.facadeCollection.PropertyChanged -= value; }
        }

        #endregion

        #region Implementation of IEnumerable<out T>

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.facadeCollection.GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<T>

        public void Add(T item)
        {
            this.facadeCollection.Add(item);
        }

        public void Clear()
        {
            this.facadeCollection.Clear();
        }

        public bool Contains(T item)
        {
            return this.facadeCollection.Contains(item);
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            ((ICollection<T>)this.facadeCollection).CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return this.facadeCollection.Remove(item);
        }

        public int Count
        {
            get { return this.facadeCollection.Count; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return ((ICollection<T>)this.facadeCollection).IsReadOnly; }
        }

        #endregion

        #region Implementation of ICollection

        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)this.facadeCollection).CopyTo(array, index);
        }

        object ICollection.SyncRoot
        {
            get { return ((ICollection)this.facadeCollection).SyncRoot; }
        }

        bool ICollection.IsSynchronized
        {
            get { return ((ICollection)this.facadeCollection).IsSynchronized; }
        }

        #endregion
        
        #region Implementation of IList<T>

        public int IndexOf(T item)
        {
            return this.facadeCollection.IndexOf(item);
        }

        void IList<T>.Insert(int index, T item)
        {
            ((IList<T>)this.facadeCollection).Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this.facadeCollection.RemoveAt(index);
        }

        public T this[int index]
        {
            get { return this.facadeCollection[index]; }
        }

        T IList<T>.this[int index]
        {
            get { return this[index]; }
            set { ((IList<T>)this.facadeCollection)[index] = value; }
        }

        #endregion

        #region Implementation of IList

        int IList.Add(object value)
        {
            return ((IList)this.facadeCollection).Add(value);
        }

        bool IList.Contains(object value)
        {
            return ((IList)this.facadeCollection).Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return ((IList)this.facadeCollection).IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            ((IList)this.facadeCollection).Insert(index, value);
        }

        void IList.Remove(object value)
        {
            ((IList)this.facadeCollection).Remove(value);
        }

        object IList.this[int index]
        {
            get { return ((IList)this.facadeCollection)[index]; }
            set { ((IList)this.facadeCollection)[index] = value; }
        }

        bool IList.IsReadOnly
        {
            get { return ((IList)this.facadeCollection).IsReadOnly; }
        }

        bool IList.IsFixedSize
        {
            get { return ((IList)this.facadeCollection).IsFixedSize; }
        }

        #endregion

        #region Implementation of IBindingList

        object IBindingList.AddNew()
        {
            return ((IBindingList)this.facadeCollection).AddNew();
        }

        void IBindingList.AddIndex(PropertyDescriptor property)
        {
            ((IBindingList)this.facadeCollection).AddIndex(property);
        }

        void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {
            ((IBindingList)this.facadeCollection).ApplySort(property, direction);
        }

        int IBindingList.Find(PropertyDescriptor property, object key)
        {
            return ((IBindingList)this.facadeCollection).Find(property, key);
        }

        void IBindingList.RemoveIndex(PropertyDescriptor property)
        {
            ((IBindingList)this.facadeCollection).RemoveIndex(property);
        }

        void IBindingList.RemoveSort()
        {
            ((IBindingList)this.facadeCollection).RemoveSort();
        }

        bool IBindingList.AllowNew
        {
            get { return ((IBindingList)this.facadeCollection).AllowNew; }
        }

        bool IBindingList.AllowEdit
        {
            get { return ((IBindingList)this.facadeCollection).AllowEdit; }
        }

        bool IBindingList.AllowRemove
        {
            get { return ((IBindingList)this.facadeCollection).AllowEdit; }
        }

        bool IBindingList.SupportsChangeNotification
        {
            get { return ((IBindingList)this.facadeCollection).SupportsChangeNotification; }
        }

        bool IBindingList.SupportsSearching
        {
            get { return ((IBindingList)this.facadeCollection).SupportsSearching; }
        }

        bool IBindingList.SupportsSorting
        {
            get { return ((IBindingList)this.facadeCollection).SupportsSorting; }
        }

        bool IBindingList.IsSorted
        {
            get { return ((IBindingList)this.facadeCollection).IsSorted; }
        }

        PropertyDescriptor IBindingList.SortProperty
        {
            get { return ((IBindingList)this.facadeCollection).SortProperty; }
        }

        ListSortDirection IBindingList.SortDirection
        {
            get { return ((IBindingList)this.facadeCollection).SortDirection; }
        }


        /// <summary>
        /// Raised when list has changed.
        /// </summary>
        public event ListChangedEventHandler ListChanged
        {
            add { this.facadeCollection.ListChanged += value; }
            remove { this.facadeCollection.ListChanged -= value; }
        }

        #endregion
    }
}
