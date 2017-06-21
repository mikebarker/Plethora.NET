using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using Plethora.Collections;
using Plethora.ComponentModel;
using Plethora.Synchronized.Change;

namespace Plethora.Synchronized
{
    internal class FacadeCollection<TKey, T> : IChangeSource, INotifyCollectionChanged, INotifyPropertyChanged, IBindingList, IList<T>
    {
        private readonly SyncCollection<TKey, T> syncCollection;
        private readonly Func<T, T> cloneFunc;

        private readonly ChangeDescriptorFactory changeFactory;

        private readonly object listLock = new object();
        private SortedList<TKey, T> localItemList;
        private Dictionary<TKey, ItemState> localStateList;

        private readonly List<ChangeDescriptor> localChanges = new List<ChangeDescriptor>();

        private readonly KeyedCollection<string, ChangeHandler> changeHandlers;

        #region Constructors

        public FacadeCollection(SyncCollection<TKey, T> syncCollection, Func<T, T> cloneFunc, ChangeSourceIdProvider changeSource)
        {
            //Validation
            if (syncCollection == null)
                throw new ArgumentNullException(nameof(syncCollection));

            if (cloneFunc == null)
                throw new ArgumentNullException(nameof(cloneFunc));

            if (changeSource == null)
                throw new ArgumentNullException(nameof(changeSource));


            this.cloneFunc = cloneFunc;
            this.changeFactory = new ChangeDescriptorFactory(changeSource);

            this.changeHandlers = new KeyedCollection<string, ChangeHandler>(handler => handler.MemberNameHandled);
            this.changeHandlers.Add(new AddHandler(this));
            this.changeHandlers.Add(new RemoveHandler(this));
            this.changeHandlers.Add(new ItemHandler(this));
            this.changeHandlers.Add(new ClearHandler(this));


            this.syncCollection = syncCollection;
            this.syncCollection.ChangePublished += this.SyncCollection_ChangePublished;

            bool didErrorOccur;
            this.BuildLocalList(out this.localItemList, out this.localStateList, out didErrorOccur);
        }

        #endregion

        private void SyncCollection_ChangePublished(object sender, ChangePublishedEventArgs e)
        {
            ChangeDescriptor change = e.Change;

            //Validate the change
            ChangeHandler handler = this.GetChangeHandler(change);
            handler.Validate(change);


            //Remove the change for the local change set if it originated from this source
            if (this.changeFactory.SourceIdProvider.ChangeSourceId == change.SourceId)
            {
                lock (this.localChanges)
                {
                    this.localChanges.Remove(change);
                }
            }


            //Rebuild the item list
            SortedList<TKey, T> oldItemList = this.localItemList;

            SortedList<TKey, T> newItemList;
            Dictionary<TKey, ItemState> newStateList;
            bool didErrorOccur;
            this.BuildLocalList(out newItemList, out newStateList, out didErrorOccur);

            lock (this.listLock)
            {
                this.localStateList = newStateList;
                this.localItemList = newItemList;
            }

            
            //Raise the events
            handler.RaiseEvents(change, oldItemList, newItemList, didErrorOccur);
        }

        public bool IsLocalChange(TKey key)
        {
            ItemState itemState;
            if (!this.localStateList.TryGetValue(key, out itemState))
                return false;

            return (itemState != ItemState.Server);
        }


        private TKey GetKey(T item)
        {
            var key = this.syncCollection.GetKeyFunc(item);
            return key;
        }


        private void BuildLocalList(out SortedList<TKey, T> itemList, out Dictionary<TKey, ItemState> stateList, out bool didErrorOccur)
        {
            stateList = new Dictionary<TKey, ItemState>();

            //Get the list from the server cache
            using (this.syncCollection.EnterLock())
            {
                itemList = new SortedList<TKey, T>(this.syncCollection.Count, this.syncCollection.KeyComparer);
                foreach (T item in this.syncCollection)
                {
                    itemList.Add(this.GetKey(item), item);
                }
            }

            //Apply local changes
            lock (this.localChanges)
            {
                didErrorOccur = false;
                List<ChangeDescriptor> errorChanges = new List<ChangeDescriptor>();
                foreach (ChangeDescriptor change in this.localChanges)
                {
                    try
                    {
                        this.ApplyChange(change, itemList, stateList, false, true);
                    }
                    catch (Exception ex)
                    {
                        this.OnConflictingChange(new ConflictingChangeEventArgs(change, ex));
                        errorChanges.Add(change);
                        didErrorOccur = true;
                    }
                }

                //Remove conflicting changes
                foreach (var errorChange in errorChanges)
                {
                    this.localChanges.Remove(errorChange);
                }
            }
        }

        private ChangeHandler GetChangeHandler(ChangeDescriptor change)
        {
            ChangeHandler changeHandler;
            if (!this.changeHandlers.TryGetValue(change.MemberName, out changeHandler))
                throw new InvalidOperationException(string.Format("Unknown change type {0} for change {1}.",
                                                                  change.MemberName, change));

            return changeHandler;
        }

        private void ApplyChange(ChangeDescriptor change, SortedList<TKey, T> itemList, Dictionary<TKey, ItemState> stateList, bool raiseEvents, bool setItemState)
        {
            ChangeHandler changeHandler = this.GetChangeHandler(change);

            changeHandler.ApplyChange(change, itemList, stateList, raiseEvents, setItemState);
        }



        #region Implementation of IChangeSource

        /// <summary>
        /// Raised when a change is published.
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

        #region ApplyLocalChange

        private readonly ISynchronizeInvoke publishChangeSynchInvoke = new SynchronizeInvoke();
        private void PublishChange(ChangeDescriptor change, Action<Exception> onException)
        {
            if (this.publishChangeSynchInvoke.InvokeRequired)
            {
                Action<ChangeDescriptor, Action<Exception>> internal_PublishChangeAction = this.Internal_PublishChange;
                this.publishChangeSynchInvoke.BeginInvoke(internal_PublishChangeAction, new object[] { change, onException });
            }
            else
            {
                this.Internal_PublishChange(change, onException);
            }
        }

        private void Internal_PublishChange(ChangeDescriptor change, Action<Exception> onException)
        {
            try
            {
                this.OnChangePublished(new ChangePublishedEventArgs(change));
            }
            catch (Exception ex)
            {
                onException(ex);
            }
        }

        private void DropChange(ChangeDescriptor change)
        {
            //Drop the change which caused the error, and rebuild the local lists.
            lock (this.listLock)
            {
                lock (this.localChanges)
                {
                    this.localChanges.Remove(change);
                }

                bool didErrorOccur;
                this.BuildLocalList(out this.localItemList, out this.localStateList, out didErrorOccur);
            }

            //Raise the reset events
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        private void ApplyLocalChange(ChangeDescriptor change)
        {
            Action<Exception> onException = exception => this.DropChange(change);

            //Apply the change
            lock (this.listLock)
            {
                lock (this.localChanges)
                {
                    this.localChanges.Add(change);
                }

                try
                {
                    this.ApplyChange(change, this.localItemList, this.localStateList, true, true);
                }
                catch (Exception ex)
                {
                    onException(ex);
                    throw;
                }
            }

            //Publish the change
            this.PublishChange(change, onException);
        }

        #endregion

        #region ConflictingChange Event

        /// <summary>
        /// Raised when a local change can not be applied due to a conflict with a server change.
        /// </summary>
        public event ConflictingChangeEventHandler ConflictingChange;

        /// <summary>
        /// Raises the <see cref="ConflictingChange"/> event.
        /// </summary>
        protected virtual void OnConflictingChange(ConflictingChangeEventArgs e)
        {
            var handler = this.ConflictingChange;
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
            return this.localItemList.Values.GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<T>

        public void Add(T item)
        {
            var change = this.changeFactory.CallMethod(-1, "Add", new object[] {item});

            //Apply the change
            this.ApplyLocalChange(change);
        }

        public void Clear()
        {
            var change = this.changeFactory.CallMethod(-1, "Clear", new object[0]);

            //Apply the change
            this.ApplyLocalChange(change);
        }

        public bool Contains(T item)
        {
            TKey key = this.GetKey(item);
            lock (this.listLock)
            {
                bool result = this.localItemList.ContainsKey(key);
                return result;
            }
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            this.localItemList.Values.CopyTo(array, arrayIndex);
        }

        private bool Remove(TKey key)
        {
            var change = this.changeFactory.CallMethod(-1, "Remove", new object[] { key });

            lock (this.listLock)
            {
                if (!this.localItemList.ContainsKey(key))
                    return false;
            }

            //Apply the change
            this.ApplyLocalChange(change);
            return true;
        }

        public bool Remove(T item)
        {
            TKey key = this.GetKey(item);
            return this.Remove(key);
        }

        public int Count
        {
            get { return this.localItemList.Count; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region Implementation of ICollection

        void ICollection.CopyTo(Array array, int index)
        {
            lock (this.listLock)
            {
                ((ICollection)this.localItemList).CopyTo(array, index);
            }
        }

        object ICollection.SyncRoot
        {
            get { return this; }
        }

        bool ICollection.IsSynchronized
        {
            get { return true; }
        }

        #endregion

        #region Implementation of IList<T>

        public int IndexOf(T item)
        {
            TKey key = this.GetKey(item);
            lock(this.listLock)
            {
                return this.localItemList.IndexOfKey(key);
            }
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            TKey key;
            lock(this.listLock)
            {
                key = this.localItemList.Keys[index];
            }
            this.Remove(key);
        }

        public T this[int index]
        {
            get
            { 
                lock(this.listLock)
                {
                    return this.localItemList.Values[index];
                }
            }
        }

        T IList<T>.this[int index]
        {
            get { return this[index]; }
            set { throw new NotSupportedException(); }
        }

        #endregion

        #region Implementation of IList

        int IList.Add(object value)
        {
            T item = (T)value;
            lock (this.listLock)
            {
                this.Add(item);
                var index = this.localItemList.IndexOfKey(this.GetKey(item));
                return index;
            }
        }

        bool IList.Contains(object value)
        {
            T item = (T)value;
            return this.Contains(item);
        }

        int IList.IndexOf(object value)
        {
            T item = (T)value;
            return this.IndexOf(item);
        }

        void IList.Insert(int index, object value)
        {
            throw new NotSupportedException();
        }

        void IList.Remove(object value)
        {
            T item = (T)value;
            this.Remove(item);
        }

        object IList.this[int index]
        {
            get { return this[index]; }
            set { throw new NotSupportedException(); }
        }

        bool IList.IsReadOnly
        {
            get { return false; }
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        #endregion

        #region Implementation of IBindingList

        object IBindingList.AddNew()
        {
            throw new NotSupportedException();
        }

        void IBindingList.AddIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {
            throw new NotSupportedException();
        }

        int IBindingList.Find(PropertyDescriptor property, object key)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveSort()
        {
            throw new NotSupportedException();
        }

        bool IBindingList.AllowNew
        {
            get { return false; }
        }

        bool IBindingList.AllowEdit
        {
            get { return false; }
        }

        bool IBindingList.AllowRemove
        {
            get { return false; }
        }

        bool IBindingList.SupportsChangeNotification
        {
            get { return true; }
        }

        bool IBindingList.SupportsSearching
        {
            get { return false; }
        }

        bool IBindingList.SupportsSorting
        {
            get { return false; }
        }

        bool IBindingList.IsSorted
        {
            get { throw new NotSupportedException(); }
        }

        PropertyDescriptor IBindingList.SortProperty
        {
            get { throw new NotSupportedException(); }
        }

        ListSortDirection IBindingList.SortDirection
        {
            get { throw new NotSupportedException(); }
        }


        /// <summary>
        /// Raised when list has changed.
        /// </summary>
        public event ListChangedEventHandler ListChanged;

        /// <summary>
        /// Raises the <see cref="ListChanged"/> event.
        /// </summary>
        protected virtual void OnListChanged(ListChangedEventArgs e)
        {
            var handler = this.ListChanged;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        [Flags]
        private enum ItemState
        {
            Server = 0,
            Added = 1,
            Removed = 2,
            Changed = 4
        }


        private abstract class ChangeHandler
        {
            #region Fields

            private readonly FacadeCollection<TKey, T> parent;
            private readonly Type valueType;
            private readonly Type[] argumentTypes;

            #endregion

            #region Constructors

            protected ChangeHandler(FacadeCollection<TKey, T> parent,
                Type valueType, Type[] argumentTypes)
            {
                this.parent = parent;
                this.valueType = valueType;
                this.argumentTypes = argumentTypes;
            }

            #endregion

            #region Properties

            protected FacadeCollection<TKey, T> Parent
            {
                get { return this.parent; }
            }

            #endregion

            #region Public Methods

            public void Validate(ChangeDescriptor change)
            {
                if (change == null)
                    throw new ArgumentNullException(nameof(change));

                
                if (change.MemberName != this.MemberNameHandled)
                    throw new ArgumentException(string.Format("Expected MemberName to be {0}.", this.MemberNameHandled));


                //Validate the value
                if (this.valueType == null)
                {
                    if (change.Value != null)
                        throw new ArgumentException(string.Format("The {0} change expects Value to be null.", this.MemberNameHandled));
                }
                else
                {
                    if (!this.valueType.IsInstanceOfType(change.Value))
                        throw new ArgumentException(string.Format("The {0} change expects Value to be of type {1}", this.MemberNameHandled, this.valueType.Name));
                }


                //Validate the arguments
                if (this.argumentTypes == null)
                {
                    if (change.Arguments != null)
                        throw new ArgumentException(string.Format("The {0} change expects Arguments to be null.", this.MemberNameHandled));
                }
                else
                {
                    if (change.Arguments.Length != this.argumentTypes.Length)
                        throw new ArgumentException(string.Format("The {0} change expects Arguments to be of type {1}.", this.MemberNameHandled, this.TypesString(this.argumentTypes)));


                    for (int i = 0; i < this.argumentTypes.Length; i++)
                    {
                        object changeArgument = change.Arguments[i];
                        Type argumentType = this.argumentTypes[i];
                        if (!argumentType.IsInstanceOfType(changeArgument))
                            throw new ArgumentException(string.Format("The {0} change expects Arguments to be of type {1}.", this.MemberNameHandled, this.TypesString(this.argumentTypes)));
                    }
                }
            }
            
            public abstract string MemberNameHandled { get; }

            public abstract void ApplyChange(ChangeDescriptor change, SortedList<TKey, T> itemList, Dictionary<TKey, ItemState> stateList, bool raiseEvents, bool setItemState);

            public void RaiseEvents(ChangeDescriptor change, SortedList<TKey, T> oldList, SortedList<TKey, T> newList, bool didChangeCauseError)
            {
                if (didChangeCauseError)
                {
                    this.Parent.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                    this.Parent.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                    this.Parent.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    this.Parent.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
                }
                else
                {
                    this.RaiseEvents(change, oldList, newList);
                }
            }

            protected abstract void RaiseEvents(ChangeDescriptor change, SortedList<TKey, T> oldList, SortedList<TKey, T> newList);

            #endregion

            protected static void SetItemStatus(Dictionary<TKey, ItemState> stateList, TKey key, ItemState itemState)
            {
                ItemState state;
                if (stateList.TryGetValue(key, out state))
                {
                    state |= itemState;
                    stateList[key] = state;
                }
                else
                {
                    stateList.Add(key, itemState);
                }
            }

            private string TypesString(IEnumerable<Type> types)
            {
                StringBuilder sb = new StringBuilder();
                bool isFirst = true;
                foreach (var type in types)
                {
                    if (isFirst)
                        isFirst = false;
                    else
                        sb.Append(",");

                    sb.Append(type.Name);
                }
                return sb.ToString();
            }
        }

        private class AddHandler : ChangeHandler
        {
            #region Constructors

            public AddHandler(FacadeCollection<TKey, T> parent)
                : base(parent, null, new[] { typeof(T) })
            {
            }

            #endregion

            #region Implementation of IChangeDescriptorHandler

            public override string MemberNameHandled
            {
                get { return "Add"; }
            }

            public override void ApplyChange(ChangeDescriptor change, SortedList<TKey, T> itemList, Dictionary<TKey, ItemState> stateList, bool raiseEvents, bool setItemState)
            {
                T item = (T)change.Arguments[0];

                TKey key = this.Parent.GetKey(item);
                itemList.Add(key, item);

                if (raiseEvents)
                {
                    int index = itemList.IndexOfKey(key);
                    this.RaiseEvents(item, index);
                }

                if (setItemState)
                {
                    SetItemStatus(stateList, key, ItemState.Added);
                }
            }

            protected override void RaiseEvents(ChangeDescriptor change, SortedList<TKey, T> oldItemList, SortedList<TKey, T> newItemList)
            {
                T newItem = (T)change.Arguments[0];
                TKey key = this.Parent.GetKey(newItem);
                int newIndex = newItemList.IndexOfKey(key);

                if (newIndex < 0) //local change has prevented Add
                    return;

                this.RaiseEvents(newItem, newIndex);
            }

            private void RaiseEvents(T newItem, int newIndex)
            {
                this.Parent.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                this.Parent.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                this.Parent.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem, newIndex));
                this.Parent.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, newIndex));
            }

            #endregion
        }

        private class RemoveHandler : ChangeHandler
        {
            #region Constructors

            public RemoveHandler(FacadeCollection<TKey, T> parent)
                : base(parent, null, new[] { typeof(TKey) })
            {
            }

            #endregion

            #region Implementation of IChangeDescriptorHandler

            public override string MemberNameHandled
            {
                get { return "Remove"; }
            }

            public override void ApplyChange(ChangeDescriptor change, SortedList<TKey, T> itemList, Dictionary<TKey, ItemState> stateList, bool raiseEvents, bool setItemState)
            {
                TKey key = (TKey)change.Arguments[0];

                int index = itemList.IndexOfKey(key);
                if (index < 0)
                    return;

                T item = itemList.Values[index];

                itemList.RemoveAt(index);

                if (raiseEvents)
                {
                    this.RaiseEvents(item, index);
                }

                if (setItemState)
                {
                    SetItemStatus(stateList, key, ItemState.Removed);
                }
            }

            protected override void RaiseEvents(ChangeDescriptor change, SortedList<TKey, T> oldList, SortedList<TKey, T> newList)
            {
                TKey key = (TKey)change.Arguments[0];
                T oldItem = oldList[key];
                int oldIndex = oldList.IndexOfKey(key);

                if (oldIndex < 0) //local change has prevented Remove
                    return;

                this.RaiseEvents(oldItem, oldIndex);
            }

            private void RaiseEvents(T oldItem, int oldIndex)
            {
                this.Parent.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                this.Parent.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                this.Parent.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItem, oldIndex));
                this.Parent.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, oldIndex));
            }

            #endregion
        }

        private class ItemHandler : ChangeHandler
        {
            #region Constructors

            public ItemHandler(FacadeCollection<TKey, T> parent)
                : base(parent, typeof(ChangeDescriptor), new[] { typeof(TKey) })
            {
            }

            #endregion

            #region Implementation of IChangeDescriptorHandler

            public override string MemberNameHandled
            {
                get { return "Item"; }
            }

            public override void ApplyChange(ChangeDescriptor change, SortedList<TKey, T> itemList, Dictionary<TKey, ItemState> stateList, bool raiseEvents, bool setItemState)
            {
                TKey key = (TKey)change.Arguments[0];
                T item = itemList[key];

                T clone = this.Parent.cloneFunc(item);

                var changeDescriptorApplier = new ChangeDescriptorApplier(clone);
                changeDescriptorApplier.Apply((ChangeDescriptor)change.Value);

                if (raiseEvents)
                {
                    int index = itemList.IndexOfKey(key);
                    this.RaiseEvents(clone, item, index);
                }

                if (setItemState)
                {
                    SetItemStatus(stateList, key, ItemState.Changed);
                }
            }

            protected override void RaiseEvents(ChangeDescriptor change, SortedList<TKey, T> oldList, SortedList<TKey, T> newList)
            {
                TKey key = (TKey)change.Arguments[0];

                int oldIndex = oldList.IndexOfKey(key);
                if (oldIndex < 0) // local change prevent ammend
                    return;
                T oldItem = oldList.Values[oldIndex];

                int newIndex = newList.IndexOfKey(key);
                if (newIndex < 0) // local change prevent ammend
                    return;
                T newItem = newList.Values[newIndex];

                this.RaiseEvents(newItem, oldItem, newIndex);
            }

            private void RaiseEvents(T newItem, T oldItem, int index)
            {
                this.Parent.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                this.Parent.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem, index));
                this.Parent.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
            }

            #endregion
        }

        private class ClearHandler : ChangeHandler
        {
            #region Constructors

            public ClearHandler(FacadeCollection<TKey, T> parent)
                : base(parent, null, new Type[0])
            {
            }

            #endregion

            #region Implementation of IChangeDescriptorHandler

            public override string MemberNameHandled
            {
                get { return "Clear"; }
            }

            public override void ApplyChange(ChangeDescriptor change, SortedList<TKey, T> itemList, Dictionary<TKey, ItemState> stateList, bool raiseEvents, bool setItemState)
            {
                List<TKey> keys = new List<TKey>(itemList.Keys);

                itemList.Clear();

                if (raiseEvents)
                {
                    this.RaiseEvents();
                }

                if (setItemState)
                {
                    foreach (var key in keys)
                    {
                        SetItemStatus(stateList, key, ItemState.Removed);
                    }
                }
            }

            protected override void RaiseEvents(ChangeDescriptor change, SortedList<TKey, T> oldList, SortedList<TKey, T> newList)
            {
                this.RaiseEvents();
            }

            private void RaiseEvents()
            {
                this.Parent.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                this.Parent.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                this.Parent.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                this.Parent.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }

            #endregion
        }

    }
}
