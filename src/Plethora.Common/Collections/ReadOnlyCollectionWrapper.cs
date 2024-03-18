using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Plethora.Collections
{
    /// <summary>
    /// Implementation of <see cref="ICollection{T}"/> which wraps a collection in a readonly interface.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the collection's elements.
    /// </typeparam>
    /// <remarks>
    /// Unlike <see cref="System.Collections.ObjectModel.ReadOnlyCollection{T}"/> this class can wrap any collection, not just a list.
    /// </remarks>
    [Serializable]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class ReadOnlyCollectionWrapper<T> : IEnumerable<T>, IReadOnlyCollection<T>, ICollection<T>, ICollection
    {
        private readonly ICollection<T> innerCollection;
        [NonSerialized]
        private object? syncRoot;

        /// <summary>
        /// Initialises a new instance of the <see cref="ReadOnlyCollectionWrapper{T}"/> class.
        /// </summary>
        /// <param name="collection">
        /// The collection to be wrapped.
        /// </param>
        public ReadOnlyCollectionWrapper(ICollection<T> collection)
        {
            ArgumentNullException.ThrowIfNull(collection);

            this.innerCollection = collection;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.innerCollection.GetEnumerator();
        }

        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        /// <remarks>
        /// The number of elements in the collection.
        /// </remarks>
        public int Count
        {
            get { return this.innerCollection.Count; }
        }

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException(ResourceProvider.CollectionReadOnly());
        }

        void ICollection<T>.Clear()
        {
            throw new NotSupportedException(ResourceProvider.CollectionReadOnly());
        }

        /// <summary>
        /// Determines whether the <see cref="ICollection{T}"/> contains a specific value.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the <see cref="ICollection{T}"/>.
        /// </param>
        /// <returns>
        /// true if item is found in the <see cref="ICollection{T}"/> otherwise, false.
        /// </returns>
        public bool Contains(T item)
        {
            return this.innerCollection.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="ICollection{T}"/> to an <see cref="Array"/>, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional System.Array that is the destination of the elements copied from <see cref="ICollection{T}"/>. The <see cref="Array"/> must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.innerCollection.CopyTo(array, arrayIndex);
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException(ResourceProvider.CollectionReadOnly());
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return true; }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ArgumentNullException.ThrowIfNull(array);

            if (array.Rank != 1)
                throw new ArgumentException(ResourceProvider.ArgArrayMultiDimensionNotSupported());

            if (array.GetLowerBound(0) != 0)
                throw new ArgumentException(ResourceProvider.ArgArrayNonZeroLowerBound());

            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), index, ResourceProvider.ArgMustBeGreaterThanZero(nameof(index)));

            if ((array.Length - index) < this.Count)
                throw new ArgumentException(ResourceProvider.ArgInvalidOffsetLength(nameof(index), "count"));

            if (array is T[] arrayT)
            {
                this.innerCollection.CopyTo(arrayT, index);
            }
            else
            {
                Type? elementType = array.GetType().GetElementType();
                if (elementType is null)
                    throw new ArgumentException(ResourceProvider.ArgArrayInvalidType());

                Type c = typeof(T);
                if (!elementType.IsAssignableFrom(c) && !c.IsAssignableFrom(elementType))
                    throw new ArgumentException(ResourceProvider.ArgArrayInvalidType());

                if (array is not object?[] objArray)
                    throw new ArgumentException(ResourceProvider.ArgArrayInvalidType());

                try
                {
                    int i = 0;
                    foreach (T item in this.innerCollection)
                    {
                        objArray[index + i] = item;
                        i++;
                    }
                }
                catch (ArrayTypeMismatchException)
                {
                    throw new ArgumentException(ResourceProvider.ArgArrayInvalidType());
                }
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                if (this.syncRoot == null)
                {
                    if (this.innerCollection is ICollection collection)
                        this.syncRoot = collection.SyncRoot;
                    else
                        Interlocked.CompareExchange<object?>(ref this.syncRoot, new object(), null);
                }
                return this.syncRoot;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                if (this.innerCollection is ICollection collection)
                {
                    return collection.IsSynchronized;
                }
                return false;
            }
        }
    }
}
