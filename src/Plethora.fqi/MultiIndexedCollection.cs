using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.fqi
{
    /// <summary>
    /// Multi-columned, multi-indexed collection of objects.
    /// </summary>
    /// <typeparam name="T">The type of the objects to be enumerated.</typeparam>
    public class MultiIndexedCollection<T> : IMultiIndexedCollection<T>, ISetCollection<T>
    {
        #region Fields

        private readonly List<IIndexedCollection<T>> indices = new List<IIndexedCollection<T>>();
        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="MultiIndexedCollection{T}"/> class.
        /// </summary>
        public MultiIndexedCollection(MultiIndexSpecification<T> multiIndexSpec)
            : this((IEnumerable<IIndexSpecification>)multiIndexSpec)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="MultiIndexedCollection{T}"/> class.
        /// </summary>
        public MultiIndexedCollection(SingleIndexSpecification<T> indexSpec)
            : this(Enumerable.Repeat((IIndexSpecification)indexSpec, 1))
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="MultiIndexedCollection{T}"/> class.
        /// </summary>
        private MultiIndexedCollection(IEnumerable<IIndexSpecification> indexSpecifications)
        {
            if (indexSpecifications == null)
                throw new ArgumentNullException(nameof(indexSpecifications));

            if (!indexSpecifications.Any())
                throw new ArgumentException("indexSpecifications must contain at least one index.", nameof(indexSpecifications));


            foreach (var index in indexSpecifications)
            {
                this.indices.Add(new IndexedCollection<T>(index.IsUnique, index.IndexExpressions.ToArray()));
            }
        }
        #endregion

        #region Implementation of IEnumerable<T>

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return (this.indices.Count == 0)
                       ? Enumerable.Empty<T>().GetEnumerator()
                       : this.indices[0].GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion

        #region Implementation of ICollection<T>

        /// <summary>
        /// Adds an item to the <see cref="ICollection{T}" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="ICollection{T}" />.</param>
        /// <exception cref="NotSupportedException">The <see cref="ICollection{T}" /> is read-only.</exception>
        public void Add(T item)
        {
            foreach (var index in this.indices)
            {
                index.Add(item);
            }
        }

        /// <summary>
        /// Removes all items from the <see cref="ICollection{T}" />.
        /// </summary>
        /// <exception cref="NotSupportedException">The <see cref="ICollection{T}" /> is read-only. </exception>
        public void Clear()
        {
            foreach (var index in this.indices)
            {
                index.Clear();
            }
        }

        /// <summary>
        /// Determines whether the <see cref="ICollection{T}" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="ICollection{T}" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> is found in the <see cref="ICollection{T}" />; otherwise, false.
        /// </returns>
        public bool Contains(T item)
        {
            return (this.indices.Count == 0)
                       ? false
                       : this.indices[0].Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="ICollection{T}" /> to an <see cref="Array" />, starting at a particular <see cref="Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array" /> that is the destination of the elements copied from <see cref="ICollection{T}" />. The <see cref="Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array" /> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex" /> is less than 0.</exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="array" /> is multidimensional.
        ///   -or-
        ///   <paramref name="arrayIndex" /> is equal to or greater than the length of <paramref name="array" />.
        ///   -or-
        ///   The number of elements in the source <see cref="ICollection{T}" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.
        ///   -or-
        ///   Type <typeparamref name="T" /> cannot be cast automatically to the type of the destination <paramref name="array" />.
        /// </exception>
        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            //Validation
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), arrayIndex, "arrayIndex may not be less than 0.");

            if (array.Rank > 1)
                throw new ArgumentException("array may not be multidimensional.");

            if (arrayIndex >= array.Length)
                throw new ArgumentException("arrayIndex is equal to or greater than the length of array.");

            if (this.indices.Count == 0)
                return;

            if (array.Length < arrayIndex + this.indices[0].Count)
                throw new ArgumentException(
                    "The number of elements in the source ICollection{T} is greater than the available space from arrayIndex to the end of the destination array.");


            //Copy the elements to the array
            int i = arrayIndex;
            foreach (T t in this.indices[0])
            {
                array[i++] = t;
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="ICollection{T}" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="ICollection{T}" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> was successfully removed from the <see cref="ICollection{T}" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="ICollection{T}" />.
        /// </returns>
        /// <exception cref="NotSupportedException">The <see cref="ICollection{T}" /> is read-only.</exception>
        public bool Remove(T item)
        {
            bool result = true;
            foreach (var index in this.indices)
            {
                result &= index.Remove(item);
            }
            return result;
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="ICollection{T}" />.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="ICollection{T}" />.
        /// </returns>
        public int Count
        {
            get
            {
                return (this.indices.Count == 0)
                           ? 0
                           : this.indices[0].Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ICollection{T}" /> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="ICollection{T}" /> is read-only; otherwise, false.
        /// </returns>
        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }
        #endregion

        #region Implementation of ISetCollection<T>

        public bool AddOrUpdate(T item)
        {
            bool result = true;
            foreach (var index in this.indices)
            {
                result &= index.AddOrUpdate(item);
            }
            return result;
        }
        #endregion

        #region Implementation of IMultiIndexedCollection<T>

        /// <summary>
        /// Gets the list of <see cref="IIndexedCollection{T}"/>s.
        /// </summary>
        public IEnumerable<IIndexedCollection<T>> IndexedCollections
        {
            get { return this.indices; }
        }
        #endregion

        #region Implementation of IMultiIndexedEnumerable<T>

        /// <summary>
        /// Gets the list of <see cref="IIndexedEnumerable{T}"/>s.
        /// </summary>
        IEnumerable<IIndexedEnumerable<T>> IMultiIndexedEnumerable<T>.IndexedEnumerables
        {
            get { return this.IndexedCollections.OfType<IIndexedEnumerable<T>>(); }
        }
        #endregion
    }
}
