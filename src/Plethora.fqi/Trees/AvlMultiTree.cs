using System.Collections.Generic;

using Plethora.Collections.Trees;

namespace Plethora.fqi.Trees
{
    public class AvlMultiTree<TKey, TValue> : MultiTree<TKey, TValue>
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="AvlMultiTree{TKey,TValue}"/> class.
        /// </summary>
        public AvlMultiTree()
            : base(new AvlTree<TKey, List<TValue>>())
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="AvlMultiTree{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="comparer">
        /// The <see cref="IComparer{T}"/> implementation to use when comparing keys.
        /// </param>
        public AvlMultiTree(IComparer<TKey> comparer)
            : base(new AvlTree<TKey, List<TValue>>(comparer))
        {
        }
        #endregion
    }
}
