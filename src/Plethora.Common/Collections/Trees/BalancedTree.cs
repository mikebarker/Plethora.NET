using System.Collections.Generic;

namespace Plethora.Collections.Trees
{
    public abstract class BalancedTree<TKey, TValue> : BinaryTree<TKey, TValue>
    {
        protected enum RotationDirection
        {
            Left,
            Right
        }

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="BalancedTree{TKey, TValue}"/> class.
        /// </summary>
        protected BalancedTree()
            : base()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BalancedTree{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="comparer">
        /// The <see cref="IComparer{T}"/> implementation to use when comparing keys.
        /// </param>
        protected BalancedTree(IComparer<TKey> comparer)
            : base(comparer)
        {
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Rotates a sub-tree from the root node <paramref name="rotationRoot"/>, in the direction <paramref name="direction"/>.
        /// </summary>
        /// <param name="rotationRoot">The rotation root.</param>
        /// <param name="direction">The rotation direction.</param>
        /// <returns>
        /// The new root node of the sub-tree.
        /// </returns>
        /// <remarks>
        /// The return value is determined by the <paramref name="direction"/> as follows:
        /// if <see cref="RotationDirection.Left"/> then <paramref name="rotationRoot"/>.<see cref="BinaryTree{TKey,TValue}.Node.Right"/>,
        /// if <see cref="RotationDirection.Right"/> then <paramref name="rotationRoot"/>.<see cref="BinaryTree{TKey,TValue}.Node.Left"/>,
        /// prior to the rotation taking place.
        /// </remarks>
        protected void Rotate(Node rotationRoot, RotationDirection direction)
        {
            Node? parent = rotationRoot.Parent;
            Edge? edge = rotationRoot.RelationToParent;

            Node pivot = (direction == RotationDirection.Right)
                ? rotationRoot.Left!
                : rotationRoot.Right!;


            if (direction == RotationDirection.Right)
            {
                rotationRoot.Left = pivot.Right;
                pivot.Right = rotationRoot;
            }
            else
            {
                rotationRoot.Right = pivot.Left;
                pivot.Left = rotationRoot;
            }


            //Swap the parent of the nodes
            if (parent == null)
            {
                this.Root = pivot;
            }
            else
            {
                if (edge!.Value == Edge.Left)
                    parent.Left = pivot;
                else
                    parent.Right = pivot;
            }
        }

        protected static int BalanceFactor(Node? node)
        {
            if (node is null)
                return 0;

            int leftHeight = (node.Left == null) ? -1 : node.Left.Height;
            int rightHeight = (node.Right == null) ? -1 : node.Right.Height;

            return rightHeight - leftHeight;
        }

        #endregion
    }
}
