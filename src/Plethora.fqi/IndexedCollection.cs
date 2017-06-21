using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Plethora.fqi.Trees;

namespace Plethora.fqi
{
    internal class IndexedCollection<T> : IIndexedCollection<T>
    {
        #region Fields

        private readonly IDeepTreeLayer<T> innerIndexedCollection;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="IndexedCollection{T}"/> class.
        /// </summary>
        public IndexedCollection(bool unique, params LambdaExpression[] propertyExpressions)
        {
            //Validation
            if (propertyExpressions == null)
                throw new ArgumentNullException(nameof(propertyExpressions));

            if (propertyExpressions.Length == 0)
                throw new ArgumentException("propertyExpressions may not be empty.", nameof(propertyExpressions));


            this.innerIndexedCollection = DeepTree.CreateDeepTree<T>(unique, propertyExpressions);
        }
        #endregion

        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.innerIndexedCollection.GetEnumerator();
        }

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

        #endregion

        #region Implementation of ICollection<T>

        /// <summary>
        /// Adds an item to the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="ICollection{T}"/>.</param>
        /// <exception cref="NotSupportedException">The <see cref="ICollection{T}"/> is read-only.</exception>
        public void Add(T item)
        {
            this.innerIndexedCollection.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <exception cref="NotSupportedException">The <see cref="ICollection{T}"/> is read-only. </exception>
        public void Clear()
        {
            this.innerIndexedCollection.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="ICollection{T}"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="ICollection{T}"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="ICollection{T}"/>.</param>
        public bool Contains(T item)
        {
            return this.innerIndexedCollection.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="ICollection{T}"/> to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="ICollection{T}"/>. The <see cref="Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="array"/> is multidimensional.
        ///   -or-
        ///   <paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.
        ///   -or-
        ///   The number of elements in the source <see cref="ICollection{T}"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.
        ///   -or-
        ///   Type <typeparamref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.
        /// </exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.innerIndexedCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="ICollection{T}"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="ICollection{T}"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="ICollection{T}"/>.</param>
        /// <exception cref="NotSupportedException">The <see cref="ICollection{T}"/> is read-only.</exception>
        public bool Remove(T item)
        {
            return this.innerIndexedCollection.Remove(item);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="ICollection{T}"/>.
        /// </returns>
        public int Count
        {
            get { return this.innerIndexedCollection.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ICollection{T}"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="ICollection{T}"/> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly
        {
            get { return this.innerIndexedCollection.IsReadOnly; }
        }

        #endregion

        #region Implementation of IIndexedEnumerable<T>

        /// <summary>
        /// Gets the list of indexed members, in order by which elements are sorted.
        /// </summary>
        public IEnumerable<string> IndexedMembers
        {
            get { return this.innerIndexedCollection.IndexedMembers; }
        }

        /// <summary>
        /// Gets a <see cref="IIndexedEnumerable{T}"/> which restricts its returned values
        /// by a minimum and maximum range.
        /// </summary>
        IIndexedEnumerable<T> IIndexedEnumerable<T>.FilterBy(Expression<Func<T, bool>> expr)
        {
            var memberRanges = ExpressionAnalyser.GetMemberRestrictions(expr);

            return this.FilterBy(expr, memberRanges);
        }

        /// <summary>
        /// Get an enumerator filtered by the specified predicate.
        /// </summary>
        /// <param name="predicate">The filtering predicate to be applied.</param>
        /// <param name="ranges">The keys' ranges to restrict by.</param>
        /// <returns>
        /// The required enumerator.
        /// </returns>
        /// <remarks>
        ///  <para>
        ///   The <paramref name="ranges"/> pertains to the members returned
        ///   by <see cref="IIndexedEnumerable{T}.IndexedMembers"/>.
        ///  </para>
        ///  <para>
        ///   The parameter <paramref name="ranges"/> is a helper array which identifies
        ///   the minimum and maximum values of the available keys.
        ///  </para>
        /// </remarks>
        public IEnumerator<T> GetIndexedEnumerator(Func<T, bool> predicate, IDictionary<string, ILateRange> ranges)
        {
            return this.innerIndexedCollection.GetIndexedEnumerator(predicate, ranges);
        }

        /// <summary>
        /// Gets a flag indicating whether the <see cref="IIndexedEnumerable{T}"/>
        /// will support the usage of out-of-order indexing.
        /// </summary>
        public bool SupportsOutOfOrderIndexing
        {
            get { return this.innerIndexedCollection.SupportsOutOfOrderIndexing; }
        }
        #endregion

        #region Implementation of IIndexedCollection<T>

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="IIndexedCollection{T}" />,
        /// or updates the existing value if the key already exists.
        /// </summary>
        /// <param name="item">The object to add.</param>
        /// <returns>
        /// true if the item was added; else false.
        /// </returns>
        public bool AddOrUpdate(T item)
        {
            return this.innerIndexedCollection.AddOrUpdate(item);
        }
        #endregion

        #region Public Members

        /// <summary>
        /// Gets a <see cref="IIndexedEnumerable{T}"/> which restricts its returned values
        /// by a minimum and maximum range.
        /// </summary>
        public IIndexedEnumerable<T> FilterBy(Expression<Func<T, bool>> expr, IDictionary<string, INamedLateRange> memberRanges)
        {
            return new IndexedEnumerable<T>(this, expr, memberRanges);
        }
        #endregion
    }
}
