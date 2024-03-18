using Plethora.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Collections.Transformations
{
    /// <summary>
    /// Represents a sorted view of a source collection.
    /// </summary>
    /// <typeparam name="T">The type of objects to in the collection.</typeparam>
    public class SortedTransformedList<T, TProperty> : TransformedList<T>
    {
        private readonly Func<T, TProperty> orderedBy;
        private IList<T> orderedList;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedTransformedList{T, TProperty}"/> class with its
        /// source.
        /// </summary>
        /// <param name="source">
        /// The underlying source, for which this instance presents a view of the contained data.
        /// </param>
        /// <param name="orderedBy">
        /// A function to extract the order-by property from an element.
        /// </param>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public SortedTransformedList(
            IEnumerable<T> source,
            Func<T, TProperty> orderedBy)
            : base(source)
        {
            this.orderedBy = orderedBy;

            this.ResetSource();
        }
#pragma warning restore CS8618

        /// <inheritdoc/>
        public override int Count => this.orderedList.Count;

        /// <inheritdoc/>
        public override T this[int index] => this.orderedList[index];

        /// <inheritdoc/>
        public override IEnumerator<T> GetEnumerator()
        {
            return this.orderedList.GetEnumerator();
        }

        /// <inheritdoc/>
        public override bool Contains(T item)
        {
            return this.orderedList.Contains(item);
        }

        /// <inheritdoc/>
        public override void CopyTo(T[] array, int arrayIndex)
        {
            this.orderedList.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public override int IndexOf(T item)
        {
            return this.orderedList.IndexOf(item);
        }

        /// <inheritdoc/>
        protected override CollectionModifiedResult AddFromSource(int sourceIndex, T item)
        {
            var orderByValue = orderedBy(item);
            var index = this.orderedList.BinarySearch(orderedBy, orderByValue);

            if (index < 0)
            {
                index = ~index;
            }
            else
            {
                index = GetLastIndexOfValue(index, orderByValue);
                index++;
            }

            this.orderedList.Insert(index, item);

            return CollectionModifiedResult.Modified(index);
        }

        /// <inheritdoc/>
        protected override CollectionModifiedResult RemoveFromSource(int sourceIndex, T item)
        {
            var index = this.orderedList.IndexOf(item);
            if (index < 0)
            {
                return CollectionModifiedResult.Unmodified();
            }

            this.orderedList.RemoveAt(index);
            return CollectionModifiedResult.Modified(index);
        }

        /// <inheritdoc/>
        protected override void ResetSource()
        {
            this.orderedList = this.Source
                .OrderBy(this.orderedBy)
                .ToList();
        }

        private int GetLastIndexOfValue(int startIndex, TProperty value)
        {
            for (int i = startIndex + 1; i < this.orderedList.Count; i++)
            {
                if (!Equals(this.orderedBy(this.orderedList[i]), value))
                {
                    return i - 1;
                }
            }

            return this.orderedList.Count - 1;
        }
    }
}
