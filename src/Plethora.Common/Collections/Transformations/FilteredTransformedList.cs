using System;
using System.Collections.Generic;

namespace Plethora.Collections.Transformations
{
    /// <summary>
    /// Represents a filtered view of a source collection.
    /// </summary>
    /// <typeparam name="T">The type of objects to in the collection.</typeparam>
    public class FilteredTransformedList<T> : TransformedList<T>, IList<T>
    {
        private readonly Func<T, bool> filterBy;

        private readonly IndexItemPairList filteredList = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="FilteredTransformedList{T}"/> class with its
        /// source.
        /// </summary>
        /// <param name="source">
        /// The underlying source, for which this instance presents a view of the contained data.
        /// </param>
        /// <param name="filterBy">
        /// The predicate which filters items from the source collection.
        /// </param>
        public FilteredTransformedList(
            IEnumerable<T> source,
            Func<T, bool> filterBy)
            : base(source)
        {
            this.filterBy = filterBy;

            this.ResetSource();
        }

        /// <inheritdoc/>
        public override int Count => this.filteredList.Count;

        public override T this[int index] => this.filteredList.ItemAt(index);

        /// <inheritdoc/>
        public override IEnumerator<T> GetEnumerator() => this.filteredList.GetItemsEnumerator();

        /// <inheritdoc/>
        public override bool Contains(T item)
        {
            return this.filteredList.ContainsItem(item);
        }

        /// <inheritdoc/>
        public override void CopyTo(T[] array, int arrayIndex)
        {
            this.filteredList.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        protected override CollectionModifiedResult AddFromSource(int sourceIndex, T item)
        {
            int insertTargetIndex = this.filteredList.BinarySearchSourceIndex(sourceIndex);
            if (insertTargetIndex < 0)
            {
                insertTargetIndex = ~insertTargetIndex;
            }

            // Add if required
            CollectionModifiedResult result;
            if (!this.filterBy(item))
            {
                result = CollectionModifiedResult.Unmodified();
            }
            else
            {
                this.filteredList.Insert(insertTargetIndex, sourceIndex, item);
                result = CollectionModifiedResult.Modified(insertTargetIndex);

                insertTargetIndex++;
            }

            // Update indices
            this.filteredList.IncrementSourceIndexFrom(insertTargetIndex);

            return result;
        }

        /// <inheritdoc/>
        protected override CollectionModifiedResult RemoveFromSource(int sourceIndex, T item)
        {
            CollectionModifiedResult result;

            // Remove if required
            int removeTargetIndex = this.filteredList.BinarySearchSourceIndex(sourceIndex);
            if (removeTargetIndex < 0)
            {
                result = CollectionModifiedResult.Unmodified();
                removeTargetIndex = ~removeTargetIndex;
            }
            else
            {
                this.filteredList.RemoveAt(removeTargetIndex);
                result = CollectionModifiedResult.Modified(removeTargetIndex);
            }

            // Update indices
            this.filteredList.DecrementSourceIndexFrom(removeTargetIndex);

            return result;
        }

        /// <inheritdoc/>
        protected override void ResetSource()
        {
            this.filteredList.Clear();

            int sourceIndex = 0;
            foreach (var item in this.Source)
            {
                if (this.filterBy(item))
                {
                    this.filteredList.Add(sourceIndex, item);
                }

                sourceIndex++;
            }
        }

        public override int IndexOf(T item)
        {
            return this.filteredList.IndexOf(item);
        }

        private sealed class IndexItemPairList
        {
            private readonly List<T> filteredItems = new();
            private readonly List<int> filteredIndices = new();

            public int Count => this.filteredItems.Count;

            public int BinarySearchSourceIndex(int sourceIndex) => this.filteredIndices.BinarySearch(sourceIndex);

            public bool ContainsItem(T item) => this.filteredItems.Contains(item);

            public void CopyTo(T[] array, int arrayIndex) => this.filteredItems.CopyTo(array, arrayIndex);

            public IEnumerator<T> GetItemsEnumerator() => this.filteredItems.GetEnumerator();

            public T ItemAt(int index) => this.filteredItems[index];

            public void Clear()
            {
                this.filteredIndices.Clear();
                this.filteredItems.Clear();
            }

            public void Add(int sourceIndex, T item)
            {
                this.filteredIndices.Add(sourceIndex);
                this.filteredItems.Add(item);
            }


            public void Insert(int index, int sourceIndex, T item)
            {
                this.filteredIndices.Insert(index, sourceIndex);
                this.filteredItems.Insert(index, item);
            }

            public int IndexOf(T item) => this.filteredItems.IndexOf(item);

            public void RemoveAt(int index)
            {
                this.filteredIndices.RemoveAt(index);
                this.filteredItems.RemoveAt(index);
            }

            public void IncrementSourceIndexFrom(int index)
            {
                for (int targetIndex = index; targetIndex < this.filteredIndices.Count; targetIndex++)
                {
                    this.filteredIndices[targetIndex]++;
                }
            }

            public void DecrementSourceIndexFrom(int index)
            {
                for (int targetIndex = index; targetIndex < this.filteredIndices.Count; targetIndex++)
                {
                    this.filteredIndices[targetIndex]--;
                }
            }
        }
    }
}
