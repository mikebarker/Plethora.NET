using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Plethora.Linq;

namespace Plethora.fqi.Trees
{
    partial class DeepTree
    {
        /// <summary>
        /// The leaf layer of a deep-tree.
        /// </summary>
        /// <typeparam name="TKey">The type of the key for this level of the deep-tree.</typeparam>
        /// <typeparam name="T">The type of the data stored at the leaf level of the deep-tree.</typeparam>
        private class DeepTreeLeafLayer<TKey, T> : IDeepTreeLayer<T>
        {
            #region Fields

            private readonly Func<T, TKey> indexFunc;
            private readonly string indexedMember;
            private readonly ITree<TKey, T> innerTree;
            #endregion

            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="DeepTreeLeafLayer{TKey, T}"/> class.
            /// </summary>
            public DeepTreeLeafLayer(bool unique, IEnumerable<string> indexNames, IEnumerable<Delegate> indexFuncs)
            {
                this.indexedMember = indexNames.Single();
                this.indexFunc = (Func<T, TKey>)indexFuncs.Single();

                if (unique)
                    this.innerTree = new AvlTree<TKey, T>();
                else
                    this.innerTree = new AvlMultiTree<TKey, T>();

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
                var enumerator = new Enumerator(this.innerTree, null);

                return enumerator;
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
                var key = this.indexFunc(item);
                this.innerTree.Add(key, item);
            }

            /// <summary>
            /// Removes all items from the <see cref="ICollection{T}" />.
            /// </summary>
            /// <exception cref="NotSupportedException">The <see cref="ICollection{T}" /> is read-only. </exception>
            public void Clear()
            {
                this.innerTree.Clear();
            }

            /// <summary>
            /// Determines whether the <see cref="ICollection{T}" /> contains a specific value.
            /// </summary>
            /// <returns>
            /// true if <paramref name="item" /> is found in the <see cref="ICollection{T}" />; otherwise, false.
            /// </returns>
            /// <param name="item">The object to locate in the <see cref="ICollection{T}" />.</param>
            public bool Contains(T item)
            {
                var key = this.indexFunc(item);
                return this.innerTree.ContainsKey(key);
            }

            /// <summary>
            /// Copies the elements of the <see cref="ICollection{T}" /> to an <see cref="Array" />, starting at a particular <see cref="Array" /> index.
            /// </summary>
            /// <param name="array">The one-dimensional <see cref="Array" /> that is the destination of the elements copied from <see cref="ICollection{T}" />. The <see cref="Array" /> must have zero-based indexing.</param>
            /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
            /// <exception cref="ArgumentNullException"><paramref name="array" /> is null.</exception>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex" /> is less than 0.</exception>
            /// <exception cref="ArgumentException">
            /// <paramref name="array" /> is multidimensional.
            /// -or-
            /// <paramref name="arrayIndex" /> is equal to or greater than the length of <paramref name="array" />.
            /// -or-
            /// The number of elements in the source <see cref="ICollection{T}" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.
            /// -or-
            /// Type <typeparamref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array" />.
            /// </exception>
            public void CopyTo(T[] array, int arrayIndex)
            {
                this.innerTree.Values.CopyTo(array, arrayIndex);
            }

            /// <summary>
            /// Removes the first occurrence of a specific object from the <see cref="ICollection{T}" />.
            /// </summary>
            /// <returns>
            /// true if <paramref name="item" /> was successfully removed from the <see cref="ICollection{T}" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="ICollection{T}" />.
            /// </returns>
            /// <param name="item">The object to remove from the <see cref="ICollection{T}" />.</param>
            /// <exception cref="NotSupportedException">The <see cref="ICollection{T}" /> is read-only.</exception>
            public bool Remove(T item)
            {
                var key = this.indexFunc(item);
                return this.innerTree.Remove(key);
            }

            /// <summary>
            /// Gets the number of elements contained in the <see cref="ICollection{T}" />.
            /// </summary>
            /// <returns>
            /// The number of elements contained in the <see cref="ICollection{T}" />.
            /// </returns>
            public int Count
            {
                get { return this.innerTree.Count; }
            }

            /// <summary>
            /// Gets a value indicating whether the <see cref="ICollection{T}" /> is read-only.
            /// </summary>
            /// <returns>
            /// true if the <see cref="ICollection{T}" /> is read-only; otherwise, false.
            /// </returns>
            public bool IsReadOnly
            {
                get { return this.innerTree.IsReadOnly; }
            }

            #endregion

            #region Implementation of IIndexedEnumerable<T>

            /// <summary>
            /// Gets the list of indexed members, in order by which elements are sorted.
            /// </summary>
            public IEnumerable<string> IndexedMembers
            {
                get { return Enumerable.Repeat(this.indexedMember, 1); }
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
            /// <param name="ranges">The maximum values to restrict by.</param>
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
                ILateRange range;
                ranges.TryGetValue(this.indexedMember, out range);
                var enumerator = new Enumerator(this.innerTree, range);

                var enumerable = enumerator.AsEnumerable();
                var filteredEnumerable = enumerable.Where(predicate);
                var filteredEnumerator = filteredEnumerable.GetEnumerator();

                return filteredEnumerator;
            }

            /// <summary>
            /// Gets a flag indicating whether the <see cref="IIndexedEnumerable{T}"/>
            /// will support the usage of out-of-order indexing.
            /// </summary>
            public bool SupportsOutOfOrderIndexing
            {
                get { return true; }
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
                var key = this.indexFunc(item);
                return this.innerTree.AddOrUpdate(key, item);
            }
            #endregion

            #region Public Methods

            /// <summary>
            /// Gets a <see cref="IIndexedEnumerable{T}"/> which restricts its returned values
            /// by a mniimum and maximum range.
            /// </summary>
            public IIndexedEnumerable<T> FilterBy(Expression<Func<T, bool>> expr, IDictionary<string, INamedLateRange> memberRanges)
            {
                return new IndexedEnumerable<T>(this, expr, memberRanges);
            }
            #endregion

            private class Enumerator : IKeyLimitedEnumerator<TKey, T>
            {
                #region Fields

                private readonly IKeyLimitedEnumerator<TKey, KeyValuePair<TKey, T>> innerEnumerator;
                private readonly ILateRange range;
                private bool initialised;
                #endregion

                #region Constructors

                /// <summary>
                /// Initialises a new instance of the <see cref="Enumerator"/> class.
                /// </summary>
                /// <param name="tree">
                /// </param>
                /// <param name="range">
                ///  <para>
                ///   The <see cref="ILateRange"/> which will be used to evaluate the maximum and
                ///   minimum values of the key for this enumerator.
                ///  </para>
                ///  <para>
                ///   May be null, if no constraints are required.
                ///  </para>
                /// </param>
                public Enumerator(ITree<TKey, T> tree, ILateRange range)
                {
                    //Validation
                    if (tree == null)
                        throw new ArgumentNullException(nameof(tree));


                    this.innerEnumerator = tree.GetPairEnumerator();
                    this.range = range;
                    this.initialised = false;
                }
                #endregion

                #region Implementation of IDisposable

                public void Dispose()
                {
                    this.innerEnumerator.Dispose();
                }
                #endregion

                #region Implementation of IEnumerator

                public bool MoveNext()
                {
                    if (!this.initialised)
                        this.Reset();

                    return this.innerEnumerator.MoveNext();
                }

                public void Reset()
                {
                    if (this.range != null)
                    {
                        if (this.range.HasMin)
                            this.Min = (TKey)this.range.MinFunc();

                        if (this.range.HasMax)
                            this.Max = (TKey)this.range.MaxFunc();
                    }

                    this.innerEnumerator.Reset();

                    this.initialised = true;
                }

                public T Current
                {
                    get { return this.innerEnumerator.Current.Value; }
                }

                object IEnumerator.Current
                {
                    get { return this.Current; }
                }
                #endregion

                #region Implementation of IKeyLimitedEnumerator<TKey,T>

                public TKey Min
                {
                    get { return this.innerEnumerator.Min; }
                    set { this.innerEnumerator.Min = value; }
                }

                public TKey Max
                {
                    get { return this.innerEnumerator.Max; }
                    set { this.innerEnumerator.Max = value; }
                }
                #endregion
            }
        }
    }
}
