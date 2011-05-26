using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Plethora.fqi.Trees
{
    partial class DeepTree
    {
        /// <summary>
        /// A middle layer of a deep-tree.
        /// </summary>
        /// <typeparam name="TKey">The type of the key for this level of the deep-tree.</typeparam>
        /// <typeparam name="T">The type of the data stored at the leaf level of the tree.</typeparam>
        /// <typeparam name="TSubTree">The data-type of the next layer in the deep-tree.</typeparam>
        private class DeepTreeMidLayer<TKey, T, TSubTree> : IDeepTreeLayer<T>
        {
            #region Fields

            private readonly bool leafUnique;
            private readonly IEnumerable<string> indexNames;
            private readonly IEnumerable<Delegate> indexFuncs;

            private readonly Func<T, TKey> indexFunc;
            private readonly ITree<TKey, IDeepTreeLayer<T>> innerTree;
            private int count = 0;
            #endregion

            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="DeepTreeMidLayer{TKey, T, TSubTree}"/> class.
            /// </summary>
            public DeepTreeMidLayer(bool unique, IEnumerable<string> indexNames, IEnumerable<Delegate> indexFuncs)
            {
                this.leafUnique = unique;
                this.indexNames = indexNames;
                this.indexFuncs = indexFuncs;

                this.indexFunc = (Func<T, TKey>)indexFuncs.First();

                if (unique)
                    this.innerTree = new AvlTree<TKey, IDeepTreeLayer<T>>();
                else
                    this.innerTree = new AvlMultiTree<TKey, IDeepTreeLayer<T>>();
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
                return new Enumerator(this);
            }

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="IEnumerator" /> object that can be used to iterate through the collection.
            /// </returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
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
                var key = indexFunc(item);

                IDeepTreeLayer<T> subtree;
                object locationInfo;
                bool result = this.innerTree.TryGetValueEx(key, out subtree, out locationInfo);
                if (!result)
                {
                    subtree = this.CreateSubTree();
                    this.innerTree.AddEx(key, subtree, locationInfo);
                }

                subtree.Add(item);

                this.count++;
            }

            /// <summary>
            /// Removes all items from the <see cref="ICollection{T}" />.
            /// </summary>
            /// <exception cref="NotSupportedException">The <see cref="ICollection{T}" /> is read-only. </exception>
            public void Clear()
            {
                this.innerTree.Clear();
                this.count = 0;
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
                var key = indexFunc(item);

                IDeepTreeLayer<T> subtree;
                bool result = this.innerTree.TryGetValue(key, out subtree);

                return result && subtree.Contains(item);
            }

            /// <summary>
            /// Copies the elements of the <see cref="ICollection{T}" /> to an <see cref="Array" />, starting at a particular <see cref="Array" /> index.
            /// </summary>
            /// <param name="array">The one-dimensional <see cref="Array" /> that is the destination of the elements copied from <see cref="ICollection{T}" />. The <see cref="Array" /> must have zero-based indexing.</param>
            /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
            /// <exception cref="ArgumentNullException"><paramref name="array" /> is null.</exception>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex" /> is less than 0.</exception>
            /// <exception cref="ArgumentException">
            ///  <paramref name="array" /> is multidimensional.
            ///  -or-
            ///  <paramref name="arrayIndex" /> is equal to or greater than the length of <paramref name="array" />.
            ///  -or-
            ///  The number of elements in the source <see cref="ICollection{T}" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.
            /// </exception>
            public void CopyTo(T[] array, int arrayIndex)
            {
                throw new NotImplementedException();
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
                var key = indexFunc(item);

                IDeepTreeLayer<T> subtree;
                bool result = this.innerTree.TryGetValue(key, out subtree);
                if (!result)
                    return false;

                result = subtree.Remove(item);
                if (result)
                    this.count--;

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
                get { return this.count; }
            }

            /// <summary>
            /// Gets a value indicating whether the <see cref="ICollection{T}" /> is read-only.
            /// </summary>
            /// <returns>
            /// true if the <see cref="ICollection{T}" /> is read-only; otherwise, false.
            /// </returns>
            public bool IsReadOnly
            {
                get { return false; }
            }

            #endregion

            #region Implementation of IIndexedEnumerable<T>

            /// <summary>
            /// Gets the list of indexed members, in order by which elements are sorted.
            /// </summary>
            public IEnumerable<string> IndexedMembers
            {
                get { return this.indexNames; }
            }

            /// <summary>
            /// Gets a <see cref="IIndexedEnumerable{T}"/> which restricts its returned values
            /// by a minimum and maximum range.
            /// </summary>
            public IIndexedEnumerable<T> FilterBy(Expression<Func<T, bool>> expr)
            {
                var memberRanges = ExpressionAnalyser.GetMemberRestrictions(expr);

                return FilterBy(expr, memberRanges);
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
                return new Enumerator(this, predicate, ranges);
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
                var key = indexFunc(item);

                IDeepTreeLayer<T> subtree;
                object locationInfo;
                bool result = this.innerTree.TryGetValueEx(key, out subtree, out locationInfo);
                if (!result)
                {
                    subtree = this.CreateSubTree();
                    this.innerTree.AddEx(key, subtree, locationInfo);
                }

                result = subtree.AddOrUpdate(item);
                if (result)
                    this.count++;

                return result;
            }
            #endregion

            #region Public Methods

            /// <summary>
            /// Gets a <see cref="IIndexedEnumerable{T}"/> which restricts its returned values
            /// by a minimum and maximum range.
            /// </summary>
            public IIndexedEnumerable<T> FilterBy(Expression<Func<T, bool>> expr, IDictionary<string, INamedLateRange> memberRanges)
            {
                return new IndexedEnumerable<T>(this, expr, memberRanges);
            }
            #endregion

            #region Private Methods

            private IDeepTreeLayer<T> CreateSubTree()
            {
                return CreateTreeInstance<T>(typeof(TSubTree),
                                             this.leafUnique, this.indexNames.Skip(1), this.indexFuncs.Skip(1));
            }
            #endregion

            private class Enumerator : IEnumerator<T>
            {
                #region Fields


                private readonly IKeyLimitedEnumerator<TKey, KeyValuePair<TKey, IDeepTreeLayer<T>>> treeEnumerator;
                private IEnumerator<T> subTreeEnumerator;
                private readonly IDictionary<string, ILateRange> namedRanges;
                private readonly Func<T, bool> predicate;

                private readonly string indexedMember;
                private bool initialised;
                #endregion

                #region Constructors

                /// <summary>
                /// Initialises a new instance of the <see cref="Enumerator"/> class.
                /// </summary>
                public Enumerator(DeepTreeMidLayer<TKey, T, TSubTree> deepTree)
                {
                    this.indexedMember = deepTree.IndexedMembers.First();

                    this.treeEnumerator = deepTree.innerTree.GetPairEnumerator();
                }

                /// <summary>
                /// Initialises a new instance of the <see cref="Enumerator"/> class.
                /// </summary>
                public Enumerator(DeepTreeMidLayer<TKey, T, TSubTree> deepTree, Func<T, bool> predicate, IDictionary<string, ILateRange> namedRanges)
                    : this(deepTree)
                {
                    this.predicate = predicate;
                    this.namedRanges = namedRanges;

                    this.initialised = false;
                }
                #endregion

                #region Implementation of IDisposable

                /// <summary>
                /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
                /// </summary>
                public void Dispose()
                {
                    this.Reset();
                }
                #endregion

                #region Implementation of IEnumerator

                /// <summary>
                /// Advances the enumerator to the next element of the collection.
                /// </summary>
                /// <returns>
                /// true if the enumerator was successfully advanced to the next element; 
                /// false if the enumerator has passed the end of the collection.
                /// </returns>
                /// <exception cref="InvalidOperationException">
                /// The collection was modified after the enumerator was created. 
                /// </exception>
                public bool MoveNext()
                {
                    if (!this.initialised)
                        Reset();

                    bool result = false;
                    while (!result)
                    {
                        //Allow for the first itteration (where subTreeEnumeration is null)
                        result = (subTreeEnumerator == null)
                            ? false
                            : subTreeEnumerator.MoveNext();

                        if (!result)
                        {
                            bool treeResult = treeEnumerator.MoveNext();
                            if (!treeResult)
                                return false;

                            subTreeEnumerator = GetLayerEnumerator(treeEnumerator.Current.Value);
                        }
                    }

                    return true;
                }

                private IEnumerator<T> GetLayerEnumerator(IDeepTreeLayer<T> layer)
                {
                    if (this.predicate == null)
                        return layer.GetEnumerator();
                    else
                        return layer.GetIndexedEnumerator(this.predicate, this.namedRanges);
                }

                /// <summary>
                /// Sets the enumerator to its initial position, which is before the first element in the collection.
                /// </summary>
                /// <exception cref="InvalidOperationException">
                /// The collection was modified after the enumerator was created. 
                /// </exception>
                public void Reset()
                {
                    if (namedRanges != null)
                    {
                        ILateRange range;
                        bool result = namedRanges.TryGetValue(this.indexedMember, out range);
                        if (result)
                        {
                            if (range.HasMin)
                                this.treeEnumerator.Min = (TKey)range.MinFunc();

                            if (range.HasMax)
                                this.treeEnumerator.Max = (TKey)range.MaxFunc();
                        }
                    }

                    subTreeEnumerator = null;
                    treeEnumerator.Reset();

                    this.initialised = true;
                }

                /// <summary>
                /// Gets the element in the collection at the current position of the enumerator.
                /// </summary>
                /// <returns>
                /// The element in the collection at the current position of the enumerator.
                /// </returns>
                public T Current
                {
                    get { return subTreeEnumerator.Current; }
                }

                /// <summary>
                /// Gets the current element in the collection.
                /// </summary>
                /// <returns>
                /// The current element in the collection.
                /// </returns>
                /// <exception cref="InvalidOperationException">
                /// The enumerator is positioned before the first element of the collection or after the last element.
                /// -or-
                /// The collection was modified after the enumerator was created.
                /// </exception>
                object IEnumerator.Current
                {
                    get { return Current; }
                }
                #endregion
            }
        }
    }
}
