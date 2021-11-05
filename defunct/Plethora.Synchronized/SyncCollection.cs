using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

using JetBrains.Annotations;

using Plethora.ComponentModel;
using Plethora.Synchronized.Change;

namespace Plethora.Synchronized
{
    /// <summary>
    /// A collection which acts as a <see cref="IChangeSink"/> to accept change notifications.
    /// </summary>
    /// <typeparam name="TKey">The data type of the key of the data stored in the collection.</typeparam>
    /// <typeparam name="T">The data type of the data stored in the collection.</typeparam>
    /// <remarks>
    ///   <see cref="SyncCollection{TKey,T}"/>s can be created on the client and server with a channel to pass <see cref="ChangeDescriptor"/>s between them.
    ///   <para>
    ///   This collection is only modified through the <see cref="IChangeSink"/> interface, and can not be modified using the <see cref="ICollection{T}"/> methods.
    ///   </para>
    ///   <para>
    ///   Change notifications are marshalled onto a single thread via the <see cref="ISynchronizeInvoke"/> constructor parameter.
    ///   </para>
    ///   <para>
    ///   Enumerating over this class should be conducted whilst the lock (see <see cref="EnterLock"/>) is held.
    ///   Example:
    ///   <example><code><![CDATA[
    ///         SyncCollection<Guid, Person> syncCollection = new SyncCollection(p => p.Id)
    /// 
    ///         // ...
    /// 
    ///         using (syncCollection.EnterLock())
    ///         {
    ///             foreach (Person person in syncCollection)
    ///             {
    ///                 // Do something with person;
    ///             }
    ///         }
    ///   ]]></code></example>
    ///   Long iterating loops should be avoided. If required, use the enumerator to create a shadow copy of the data and use this copy for the slow loop.
    ///   Other item accessing methods do not require the lock to be held.
    ///   </para>
    /// </remarks>
    public class SyncCollection<TKey, T> : IChangeSink, IChangeSource, INotifyCollectionChanged, INotifyPropertyChanged, IList<T>
    {
        private readonly Func<T, TKey> getKeyFunc;
        private readonly SortedList<TKey, T> innerList;
        private readonly ISynchronizeInvoke synchInvoke;
        private readonly object listLock = new object();

        /// <summary>
        /// Initialise a new instance of the <see cref="SyncCollection{TKey,T}"/> class with the default key comparer, and synchronised on a new thread.
        /// </summary>
        /// <param name="getKeyFunc">The function which gets the key from a data-item.</param>
        public SyncCollection(Func<T, TKey> getKeyFunc)
            : this(getKeyFunc, Comparer<TKey>.Default, new SynchronizeInvoke())
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="SyncCollection{TKey,T}"/> class synchronised on a new thread.
        /// </summary>
        /// <param name="getKeyFunc">The function which gets the key from a data-item.</param>
        /// <param name="keyComparer">The key comparer.</param>
        public SyncCollection(Func<T, TKey> getKeyFunc, IComparer<TKey> keyComparer)
            : this(getKeyFunc, keyComparer, new SynchronizeInvoke())
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="SyncCollection{TKey,T}"/> class.
        /// </summary>
        /// <param name="getKeyFunc">The function which gets the key from a data-item.</param>
        /// <param name="keyComparer">The key comparer.</param>
        /// <param name="synchInvoke">The <see cref="ISynchronizeInvoke"/> to be used to marshal collection updates.</param>
        public SyncCollection(Func<T, TKey> getKeyFunc, IComparer<TKey> keyComparer, ISynchronizeInvoke synchInvoke)
        {
            //Validation
            if (getKeyFunc == null)
                throw new ArgumentNullException(nameof(getKeyFunc));

            if (keyComparer == null)
                throw new ArgumentNullException(nameof(keyComparer));

            if (synchInvoke == null)
                throw new ArgumentNullException(nameof(synchInvoke));


            this.getKeyFunc = getKeyFunc;
            this.synchInvoke = synchInvoke;
            this.innerList = new SortedList<TKey, T>(keyComparer);
        }


        private TKey GetKey(T item)
        {
            TKey key = this.getKeyFunc(item);
            return key;
        }

        /// <summary>
        /// Locks the collection returns an <see cref="IDisposable"/> object which, when disposed, will release the lock.
        /// </summary>
        /// <returns>
        /// The lock on the collection.
        /// </returns>
        [NotNull]
        public IDisposable EnterLock()
        {
            Monitor.Enter(this.listLock);

            return new ActionOnDispose(delegate
            {
                Monitor.Exit(this.listLock);
            });
        }

        /// <summary>
        /// Determines if the lock has already been acquired.
        /// </summary>
        public bool IsLockEntered
        {
            get { return Monitor.IsEntered(this.listLock); }
        }


        #region IChangeSink

        /// <summary>
        /// Applies the change which has been received.
        /// </summary>
        /// <param name="change">
        /// The <see cref="ChangeDescriptor"/> received
        /// </param>
        public void ApplyChange(ChangeDescriptor change)
        {
            if (change == null)
                throw new ArgumentNullException(nameof(change));

            if (this.synchInvoke.InvokeRequired)
            {
                Action<ChangeDescriptor> applyAction = this.ApplyChange_Internal;
                this.synchInvoke.Invoke(applyAction, new object[] { change });
            }
            else
            {
                this.ApplyChange_Internal(change);
            }
        }

        private void ApplyChange_Internal(ChangeDescriptor change)
        {
            switch (change.MemberName)
            {
                case "Add":
                    this.ApplyChange_Add(change);
                    break;

                case "Remove":
                    this.ApplyChange_Remove(change);
                    break;

                case "Clear":
                    this.ApplyChange_Clear(change);
                    break;

                case "Item":
                    this.ApplyChange_Item(change);
                    break;

                default:
                    throw new InvalidOperationException(string.Format("Unknown change type {0} for change {1}.", change.MemberName, change));
            }

            this.OnChangePublished(new ChangePublishedEventArgs(change));
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
            this.AddItem(item);
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
            this.RemoveItem(key);
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


            this.ClearItems();
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
            T item;
            using (this.EnterLock())
            {
                item = this.innerList[key];
            }

            var changeDescriptorApplier = new ChangeDescriptorApplier(item);
            changeDescriptorApplier.Apply((ChangeDescriptor)change.Value);
        }




        private int AddItem(T item)
        {
            TKey key = this.GetKey(item);
            int index;
            using (this.EnterLock())
            {
                this.innerList.Add(key, item);
                index = this.innerList.IndexOfKey(key);
            }

            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            return index;
        }

        private void ClearItems()
        {
            using (this.EnterLock())
            {
                this.innerList.Clear();
            }

            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private bool RemoveItem(TKey key)
        {
            T item;
            int index;
            using (this.EnterLock())
            {
                index = this.innerList.IndexOfKey(key);
                if (index < 0)
                    return false;

                item = this.innerList.Values[index];

                this.innerList.RemoveAt(index);
            }

            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
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
            var handler = this.ChangePublished;
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
            var handler = this.CollectionChanged;
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
            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region Implementation of IEnumerable<out T>

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
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
                if (enumerator == null)
                    throw new ArgumentNullException(nameof(enumerator));

                if (checkLockFunc == null)
                    throw new ArgumentNullException(nameof(checkLockFunc));


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
                if (!this.checkLockFunc())
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
            TKey key = this.GetKey(item);
            using (this.EnterLock())
            {
                bool result = this.innerList.ContainsKey(key);
                return result;
            }
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            using (this.EnterLock())
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
                using (this.EnterLock())
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
            using (this.EnterLock())
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
                using (this.EnterLock())
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
