using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Plethora.ComponentModel;
using Plethora.Synchronized.Change;
using Plethora.Threading;

namespace Plethora.Synchronized
{
    internal class SyncCollection<TKey, T> : IChangeSink, IChangeSource, INotifyCollectionChanged, INotifyPropertyChanged, IList<T>
    {
        private readonly Func<T, TKey> getKeyFunc;
        private readonly SortedList<TKey, T> innerList;
        private readonly ISynchronizeInvoke synchInvoke;

        public SyncCollection(Func<T, TKey> getKeyFunc)
            : this(getKeyFunc, Comparer<TKey>.Default, new SynchronizeInvoke())
        {
        }

        public SyncCollection(Func<T, TKey> getKeyFunc, IComparer<TKey> keyComparer)
            : this(getKeyFunc, keyComparer, new SynchronizeInvoke())
        {
        }

        public SyncCollection(Func<T, TKey> getKeyFunc, IComparer<TKey> keyComparer, ISynchronizeInvoke synchInvoke)
        {
            //Validation
            if (getKeyFunc == null)
                throw new ArgumentNullException("getKeyFunc");

            if (keyComparer == null)
                throw new ArgumentNullException("keyComparer");

            if (synchInvoke == null)
                throw new ArgumentNullException("synchInvoke");


            this.getKeyFunc = getKeyFunc;
            this.synchInvoke = synchInvoke;
            this.innerList = new SortedList<TKey, T>(keyComparer);
        }


        private TKey GetKey(T item)
        {
            TKey key = this.getKeyFunc(item);
            return key;
        }


        private readonly LiteLock listLock = new LiteLock();
        internal IDisposable EnterLock()
        {
            return listLock.AcquireLock();
        }

        internal bool IsLockEntered
        {
            get { return listLock.IsLockAcquired; }
        }


        #region IChangeSink

        public void ApplyChange(ChangeDescriptor change)
        {
            if (change == null)
                throw new ArgumentNullException("change");

            if (synchInvoke.InvokeRequired)
            {
                Action<ChangeDescriptor> applyAction = ApplyChange_Internal;
                synchInvoke.Invoke(applyAction, new object[] { change });
            }
            else
            {
                ApplyChange_Internal(change);
            }
        }

        private void ApplyChange_Internal(ChangeDescriptor change)
        {
            switch (change.MemberName)
            {
                case "Add":
                    ApplyChange_Add(change);
                    break;

                case "Remove":
                    ApplyChange_Remove(change);
                    break;

                case "Clear":
                    ApplyChange_Clear(change);
                    break;

                case "Item":
                    ApplyChange_Item(change);
                    break;

                default:
                    throw new InvalidOperationException(string.Format("Unknown change type {0} for change {1}.", change.MemberName, change));
            }

            OnChangePublished(new ChangePublishedEventArgs(change));
        }

        #endregion

        private void ApplyChange_Add(ChangeDescriptor change)
        {
            if (change == null)
                throw new ArgumentNullException();

            if (change.MemberName != "Add")
                throw new ArgumentException();

            if (change.Value != null)
                throw new ArgumentException();

            if ((change.Arguments == null) || (change.Arguments.Length != 1) || !(change.Arguments[0] is T))
                throw new ArgumentException();


            T item = (T)change.Arguments[0];
            this.Add(item);
        }

        private void ApplyChange_Remove(ChangeDescriptor change)
        {
            if (change == null)
                throw new ArgumentNullException();

            if (change.MemberName != "Remove")
                throw new ArgumentException();

            if (change.Value != null)
                throw new ArgumentException();

            if ((change.Arguments == null) || (change.Arguments.Length != 1) || !(change.Arguments[0] is TKey))
                throw new ArgumentException();


            TKey key = (TKey)change.Arguments[0];
            this.Remove(key);
        }

        private void ApplyChange_Clear(ChangeDescriptor change)
        {
            if (change == null)
                throw new ArgumentNullException();

            if (change.MemberName != "Clear")
                throw new ArgumentException();

            if (change.Value != null)
                throw new ArgumentException();

            if ((change.Arguments == null) || (change.Arguments.Length != 0))
                throw new ArgumentException();


            this.Clear();
        }

        private void ApplyChange_Item(ChangeDescriptor change)
        {
            if (change == null)
                throw new ArgumentNullException();

            if (change.MemberName != "Item")
                throw new ArgumentException();

            if ((change.Value == null) || !(change.Value is ChangeDescriptor))
                throw new ArgumentException();

            if ((change.Arguments == null) || (change.Arguments.Length != 1) || !(change.Arguments[0] is TKey))
                throw new ArgumentException();


            TKey key = (TKey)change.Arguments[0];
            int index;
            T item;
            using (EnterLock())
            {
                index = this.innerList.IndexOfKey(key);
                item = this.innerList[key];
            }

            var changeDescriptorApplier = new ChangeDescriptorApplier(item);
            changeDescriptorApplier.Apply((ChangeDescriptor)change.Value);

            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, item, index)); //TODO: this should possibly clone the object??
        }




        private void Add(T item)
        {
            TKey key = GetKey(item);
            using(EnterLock())
            {
                this.innerList.Add(key, item);
            }

            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        private void Clear()
        {
            using (EnterLock())
            {
                this.innerList.Clear();
            }

            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private bool Remove(TKey key)
        {
            T item;
            using (EnterLock())
            {
                int index = this.innerList.IndexOfKey(key);
                if (index < 0)
                    return false;

                item = this.innerList.Values[index];

                this.innerList.RemoveAt(index);
            }

            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            return true;
        }

        #region Implementation of IChangeSource

        /// <summary>
        /// Raised when a change has been applied.
        /// </summary>
        public event ChangePublishedEventHandler ChangePublished;

        /// <summary>
        /// Raises the <see cref="ChangePublished"/> event.
        /// </summary>
        protected virtual void OnChangePublished(ChangePublishedEventArgs e)
        {
            var handler = ChangePublished;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region Implementation of INotifyCollectionChanged

        /// <summary>
        /// Raised when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Raises the <see cref="CollectionChanged"/> event.
        /// </summary>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            var handler = CollectionChanged;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        /// <summary>
        /// Raised when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region Implementation of IEnumerable<out T>

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new CheckLockEnumerator(this.innerList.Values.GetEnumerator(), () => this.IsLockEntered);
        }

        /// <summary>
        /// Helper class which assists the user in checking that the lock is held when the
        /// enumerator is utilised.
        /// </summary>
        /// <remarks>
        /// This will not prevent the user from bad threading techniques, but should assist
        /// in detecting incorrect usage, by failing early if the lock is not held when the
        /// MoveNext() method is called.
        /// </remarks>
        private class CheckLockEnumerator : IEnumerator<T>
        {
            #region Fields

            private readonly IEnumerator<T> enumerator;
            private readonly Func<bool> checkLockFunc;

            #endregion

            #region Constructor

            public CheckLockEnumerator(IEnumerator<T> enumerator, Func<bool> checkLockFunc)
            {
                //Validation
                if (enumerator==null)
                    throw new ArgumentNullException("enumerator");

                if (checkLockFunc == null)
                    throw new ArgumentNullException("checkLockFunc");


                this.enumerator = enumerator;
                this.checkLockFunc = checkLockFunc;
            }

            #endregion

            #region Implementation of IEnumerator<out T>

            public T Current
            {
                get { return this.enumerator.Current; }
            }

            #endregion

            #region Implementation of IEnumerator

            public bool MoveNext()
            {
                if (!checkLockFunc())
                    throw new InvalidOperationException("The lock must be acquired before the enumerator is used. Call the SyncCollection.EnterLock() method.");

                return this.enumerator.MoveNext();
            }

            public void Reset()
            {
                this.enumerator.Reset();
            }

            object IEnumerator.Current
            {
                get { return ((IEnumerator)this.enumerator).Current; }
            }

            #endregion

            #region Implementation of IDisposable

            public void Dispose()
            {
                this.enumerator.Dispose();
            }

            #endregion
        }

        #endregion

        #region Implementation of ICollection<T>

        void ICollection<T>.Add(T item)
        {
            throw new InvalidOperationException("The server cached collection can not be modified except by the parent source.");
        }

        void ICollection<T>.Clear()
        {
            throw new InvalidOperationException("The server cached collection can not be modified except by the parent source.");
        }

        public bool Contains(T item)
        {
            TKey key = GetKey(item);
            using (EnterLock())
            {
                bool result = this.innerList.ContainsKey(key);
                return result;
            }
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            using (EnterLock())
            {
                this.innerList.Values.CopyTo(array, arrayIndex);
            }
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new InvalidOperationException("The server cached collection can not be modified except by the parent source.");
        }

        public int Count
        {
            get
            {
                using (EnterLock())
                {
                    return this.innerList.Count;
                }
            }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        #endregion

        #region Implementation of IList<T>

        public int IndexOf(T item)
        {
            TKey key = this.GetKey(item);
            using (EnterLock())
            {
                int index = this.innerList.IndexOfKey(key);
                return index;
            }
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new InvalidOperationException("The server cached collection can not be modified except by the parent source.");
        }

        void IList<T>.RemoveAt(int index)
        {
            throw new InvalidOperationException("The server cached collection can not be modified except by the parent source.");
        }

        public T this[int index]
        {
            get
            {
                using (EnterLock())
                {
                    TKey key = this.innerList.Keys[index];
                    T item = this.innerList[key];
                    return item;
                }
            }
        }

        T IList<T>.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                throw new InvalidOperationException("The server cached collection can not be modified except by the parent source.");
            }
        }

        #endregion

        public IComparer<TKey> KeyComparer
        {
            get { return this.innerList.Comparer; }
        }

        public Func<T, TKey> GetKeyFunc
        {
            get { return this.getKeyFunc; }
        }
    }
}
