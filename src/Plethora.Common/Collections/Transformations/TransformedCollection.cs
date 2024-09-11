using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Plethora.Collections.Transformations
{
    /// <summary>
    /// Represents a strongly typed collection of objects which projects a modified view of a
    /// source collection.
    /// </summary>
    /// <typeparam name="T">The type of objects to in the collection.</typeparam>
    public abstract class TransformedCollection<T> : IEnumerable, IEnumerable<T>, IReadOnlyCollection<T>, ICollection, ICollection<T>, INotifyCollectionChanged
    {
        private readonly IEnumerable<T> source;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformedCollection{T}"/> class with its
        /// source.
        /// </summary>
        /// <param name="source">
        /// The underlying source, for which this instance presents a view of the contained data.
        /// </param>
        public TransformedCollection(
            IEnumerable<T> source)
        {
            this.source = source;
            ((INotifyCollectionChanged)source).CollectionChanged += Source_CollectionChanged;
        }

        /// <summary>
        /// The underlying source, for which this instance presents a view of the contained data.
        /// </summary>
        protected IEnumerable<T> Source => this.source;

        #region Implementation of INotifyCollectionChanged

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        private void OnCollectionChanged_Add(IEnumerable<ContiguousIndexGroup> groups)
        {
            foreach (var group in groups)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add,
                    group.Items,
                    group.StartingIndex));
            }
        }

        private void OnCollectionChanged_Remove(IEnumerable<ContiguousIndexGroup> groups)
        {
            foreach (var group in groups)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove,
                    group.Items,
                    group.StartingIndex));
            }
        }

        private void OnCollectionChanged_Reset()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Raises the <see cref="CollectionChanged"/> event with the provided arguments.
        /// </summary>
        /// <param name="e">Arguments of the event being raised.</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            this.CollectionChanged?.Invoke(this, e);
        }

        #endregion

        #region Implementaiton of IEnumerable<T>

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator{T}"/> object that can be used to iterate through the collection.
        /// </returns>
        public abstract IEnumerator<T> GetEnumerator();

        #endregion

        #region Implementaiton of IReadOnlyCollection<T>

        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        public abstract int Count { get; }

        #endregion

        #region Implementaiton of ICollection

        /// <inheritdoc/>
        bool ICollection.IsSynchronized
        {
            get
            {
                if (this.source is ICollection collection)
                {
                    return collection.IsSynchronized;
                }

                return false;
            }
        }

        /// <inheritdoc/>
        object ICollection.SyncRoot
        {
            get
            {
                if (this.source is ICollection collection)
                {
                    return collection.SyncRoot;
                }

                return this.source;
            }
        }

        /// <inheritdoc/>
        void ICollection.CopyTo(Array array, int index)
        {
            this.CopyTo((T[])array, index);
        }

        #endregion

        #region Implementaiton of ICollection<T>

        private static ICollection<T> AsCollection(IEnumerable enumerable)
        {
            if (enumerable is ICollection<T> collection)
            {
                return collection;
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ICollection{T}"/> is read-only.
        /// </summary>
        bool ICollection<T>.IsReadOnly
        {
            get
            {
                if (this.source is ICollection<T> collection)
                {
                    return collection.IsReadOnly;
                }

                return false;
            }
        }

        /// <summary>
        /// Adds an item to the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <param name="item">The object to add.</param>
        public void Add(T item)
        {
            AsCollection(this.source).Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="ICollection{T}"/>.
        /// </summary>
        public void Clear()
        {
            AsCollection(this.source).Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="ICollection{T}"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate.</param>
        /// <returns>
        /// true if item is found in the <see cref="ICollection{T}"/>; otherwise, false.
        /// </returns>
        public abstract bool Contains(T item);

        /// <summary>
        /// Copies the elements of the <see cref="ICollection{T}"/> to an array, starting at a
        /// particular index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional array that is the destination of the elements copied from
        /// <see cref="ICollection{T}"/>. The array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public abstract void CopyTo(T[] array, int arrayIndex);

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <param name="item">The object to remove.</param>
        /// <returns>
        /// true if item was successfully removed from the <see cref="ICollection{T}"/>;
        /// otherwise, false. This method also returns false if item is not found in the
        /// original <see cref="ICollection{T}"/>.
        /// </returns>
        public bool Remove(T item)
        {
            return AsCollection(this.source).Remove(item);
        }

        #endregion

        /// <summary>
        /// Called when an item is added to the underlying source.
        /// </summary>
        /// <param name="sourceIndex">The index where the item was added; or -1.</param>
        /// <param name="item">The item which was added.</param>
        /// <returns>
        /// A <see cref="CollectionModifiedResult"/> indicating whether the item was added to this
        /// <see cref="TransformedCollection{T}"/>.
        /// </returns>
        protected abstract CollectionModifiedResult AddFromSource(int sourceIndex, T item);

        /// <summary>
        /// Called when an item is removed from the underlying source.
        /// </summary>
        /// <param name="sourceIndex">The index where the item was removed; or -1.</param>
        /// <param name="item">The item which was removed.</param>
        /// <returns>
        /// A <see cref="CollectionModifiedResult"/> indicating whether the item was removed from this
        /// <see cref="TransformedCollection{T}"/>.
        /// </returns>
        protected abstract CollectionModifiedResult RemoveFromSource(int sourceIndex, T item);

        /// <summary>
        /// Called when the underlying source has been reset.
        /// </summary>
        protected abstract void ResetSource();

        private static IReadOnlyCollection<ContiguousIndexGroup> GroupContiguousIndices(IEnumerable<Tuple<int, object>> pairs)
        {
            var orderedPairs = pairs
                .OrderBy(tuple => tuple.Item1);

            List<ContiguousIndexGroup> groups = new();

            int prevIndex = default;
            List<object>? groupList = null;
            foreach (var (index, item) in orderedPairs)
            {
                bool includeInGroup =
                    ((index == -1) && (prevIndex == -1)) ||
                    (index == prevIndex + 1);

                if (groupList is null || !includeInGroup)
                {
                    groupList = new();
                    groups.Add(new ContiguousIndexGroup(index, groupList));
                }

                groupList.Add(item);

                prevIndex = index;
            }

            return groups;
        }

        private void Source_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        if (e.NewItems is null)
                        {
                            return;
                        }

                        List<Tuple<int, object>> itemIndexPairs = new();
                        int index = e.NewStartingIndex;
                        foreach (T newItem in e.NewItems)
                        {
                            var result = this.AddFromSource(index, newItem);
                            if (result.IsModified)
                            {
                                itemIndexPairs.Add(new Tuple<int, object>(result.Index, newItem));
                            }
                            index++;
                        }

                        var groups = GroupContiguousIndices(itemIndexPairs);
                        this.OnCollectionChanged_Add(groups);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    {
                        if (e.OldItems is null)
                        {
                            return;
                        }

                        List<Tuple<int, object>> itemIndexPairs = new();
                        int index = e.OldStartingIndex;
                        foreach (T oldItem in e.OldItems)
                        {
                            var result = this.RemoveFromSource(index, oldItem);
                            if (result.IsModified)
                            {
                                itemIndexPairs.Add(new Tuple<int, object>(result.Index, oldItem));
                            }
                            index++;
                        }

                        var groups = GroupContiguousIndices(itemIndexPairs);
                        this.OnCollectionChanged_Remove(groups);
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    this.ResetSource();
                    this.OnCollectionChanged_Reset();
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// A struct containing the result of a collection modification.
        /// </summary>
        protected readonly struct CollectionModifiedResult
        {
            /// <summary>
            /// Creates a <see cref="CollectionModifiedResult"/> indicating that the
            /// <see cref="TransformedCollection{T}"/> was unmodified.
            /// </summary>
            /// <returns>
            /// A <see cref="CollectionModifiedResult"/> indicating that the
            /// <see cref="TransformedCollection{T}"/> was unmodified.
            /// </returns>
            public static CollectionModifiedResult Unmodified()
            {
                return new CollectionModifiedResult(false, -1);
            }

            /// <summary>
            /// Creates a <see cref="CollectionModifiedResult"/> indicating that the
            /// <see cref="TransformedCollection{T}"/> was modified.
            /// </summary>
            /// <param name="index">The index of the modification.</param>
            /// <returns>
            /// A <see cref="CollectionModifiedResult"/> indicating that the
            /// <see cref="TransformedCollection{T}"/> was modified.
            /// </returns>
            public static CollectionModifiedResult Modified(int index)
            {
                return new CollectionModifiedResult(true, index);
            }

            private CollectionModifiedResult(bool isModified, int index)
            {
                this.IsModified = isModified;
                this.Index = index;
            }

            /// <summary>
            /// True if the <see cref="TransformedCollection{T}"/> was modified by the operation.
            /// </summary>
            public bool IsModified { get; }

            /// <summary>
            /// The index in the <see cref="TransformedCollection{T}"/> where the modified was realised.
            /// </summary>
            public int Index { get; }
        }

        private class ContiguousIndexGroup
        {
            public ContiguousIndexGroup(int startingIndex, IList items)
            {
                this.StartingIndex = startingIndex;
                this.Items = items;
            }

            public int StartingIndex { get; }
            public IList Items { get; }
        }
    }
}
