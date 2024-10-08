using System;
using System.Collections;
using System.Collections.Generic;

namespace Plethora.Collections.Trees
{
    public partial class BinaryTree<TKey, TValue>
    {
        private abstract class BaseWrapper<T> : ICollection<T>
        {
            #region Fields

            private readonly BinaryTree<TKey, TValue> innerTree;
            #endregion

            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="ValuesWrapper"/> class.
            /// </summary>
            /// <param name="tree">
            /// The <see cref="BinaryTree{TKey,TValue}"/> to be wrapped in this collection.
            /// </param>
            protected BaseWrapper(BinaryTree<TKey, TValue> tree)
            {
                this.innerTree = tree;
            }
            #endregion

            #region ICollection<T> Members

            public void Add(T item)
            {
                throw new NotSupportedException();
            }

            public void Clear()
            {
                throw new NotSupportedException();
            }

            public abstract bool Contains(T item);

            public void CopyTo(T[] array, int arrayIndex)
            {
                //Validation
                ArgumentNullException.ThrowIfNull(array);

                if (array.Rank != 1)
                    throw new ArgumentException(ResourceProvider.ArgArrayMultiDimensionNotSupported());

                if ((arrayIndex < 0) || ((array.Length - arrayIndex) < this.Count))
                    throw new ArgumentException(ResourceProvider.ArgInvalidOffsetLength(nameof(arrayIndex), nameof(this.Count)));


                int i = arrayIndex;
                foreach (var t in this)
                {
                    array[i++] = t;
                }
            }

            public int Count
            {
                get { return this.innerTree.Count; }
            }

            bool ICollection<T>.IsReadOnly
            {
                get { return true; }
            }

            public bool Remove(T item)
            {
                throw new NotSupportedException();
            }
            #endregion

            #region IEnumerable<TValue> Members

            public abstract IEnumerator<T> GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
            #endregion

            #region Properties

            protected BinaryTree<TKey, TValue> InnerTree
            {
                get { return this.innerTree; }
            }
            #endregion
        }

        private class ValuesWrapper : BaseWrapper<TValue>
        {
            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="ValuesWrapper"/> class.
            /// </summary>
            /// <param name="tree">
            /// The <see cref="BinaryTree{TKey,TValue}"/> to be wrapped as a Value collection.
            /// </param>
            public ValuesWrapper(BinaryTree<TKey, TValue> tree)
                : base(tree)
            {
            }
            #endregion

            #region ICollection<TValue> Members

            public override bool Contains(TValue item)
            {
                foreach (var value in this)
                {
                    if (item is null)
                    {
                        if (value is null)
                            return true;
                    }
                    else
                    {
                        if (item.Equals(value))
                            return true;
                    }
                }

                return false;
            }
            #endregion

            #region IEnumerable<TValue> Members

            public override IEnumerator<TValue> GetEnumerator()
            {
                return new InOrderValueEnumerator(this.InnerTree);
            }
            #endregion
        }

        private class KeysWrapper : BaseWrapper<TKey>
        {
            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="KeysWrapper"/> class.
            /// </summary>
            /// <param name="tree">
            /// The <see cref="BinaryTree{TKey,TValue}"/> to be wrapped as a Key collection.
            /// </param>
            public KeysWrapper(BinaryTree<TKey, TValue> tree)
                : base(tree)
            {
            }
            #endregion

            #region ICollection<TKey> Members

            public override bool Contains(TKey item)
            {
                return base.InnerTree.ContainsKey(item);
            }
            #endregion

            #region IEnumerable<TKey> Members

            public override IEnumerator<TKey> GetEnumerator()
            {
                return new InOrderKeyEnumerator(this.InnerTree);
            }
            #endregion
        }
    }
}
