using System.Collections.Generic;

namespace Plethora.fqi.Trees
{
    public class AvlTree<TKey, TValue> : BinaryTree<TKey, TValue>
    {
        private enum RotationDirection
        {
            Left,
            Right
        }

        protected internal class AvlNode : Node
        {
            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="AvlNode"/> class.
            /// </summary>
            public AvlNode(TKey key, TValue value)
                : base(key, value)
            {
            }
            #endregion

            #region Properties

            public new AvlNode Left
            {
                get { return (AvlNode)base.Left; }
                set { base.Left = value; }
            }

            public new AvlNode Right
            {
                get { return (AvlNode)base.Right; }
                set { base.Right = value; }
            }

            public new AvlNode Parent
            {
                get { return (AvlNode)base.Parent; }
            }

            public int BalanceFactor
            {
                get
                {
                    int leftHeight = (this.Left == null) ? -1 : this.Left.Height;
                    int rightHeight = (this.Right == null) ? -1 : this.Right.Height;

                    return rightHeight - leftHeight;
                }
            }
            #endregion
        }

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="BinaryTree{TKey, TValue}"/> class.
        /// </summary>
        public AvlTree()
            : base()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BinaryTree{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="comparer">
        /// The <see cref="IComparer{T}"/> implementation to use when comparing keys.
        /// </param>
        public AvlTree(IComparer<TKey> comparer)
            : base(comparer)
        {
        }
        #endregion

        #region Override Methods

        protected internal new AvlNode Root
        {
            get { return (AvlNode)base.Root; }
            set { base.Root = value; }
        }

        protected override Node AddNode(TKey key, TValue value, Node parent, Edge? edge)
        {
            AvlNode node = (AvlNode)base.AddNode(key, value, parent, edge);

            this.BalanceNode(node, true);

            return node;
        }

        protected override void RemoveNode(Node node)
        {
            AvlNode parent = ((AvlNode)node).Parent;

             base.RemoveNode(node);

            if (parent == null)
                this.BalanceNode(this.Root, false);
            else
                this.BalanceNode(parent, false);
        }

        protected override Node CreateNode(TKey key, TValue value)
        {
            return new AvlNode(key, value);
        }
        #endregion

        #region Private Methods

        private void Rotate(AvlNode rotationRoot, RotationDirection direction)
        {
            AvlNode parent = rotationRoot.Parent;
            Edge? edge = rotationRoot.RelationToParent;

            AvlNode pivot = (direction == RotationDirection.Right) ?
                rotationRoot.Left :
                rotationRoot.Right;


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
                if (edge.Value == Edge.Left)
                    parent.Left = pivot;
                else
                    parent.Right = pivot;
            }
        }

        private void BalanceNode(AvlNode node, bool isInsertion)
        {
            bool balanceParent = true;

            //Test for balance
            int balanceFactor = node.BalanceFactor;
            if (balanceFactor == 2)
            {
                if (node.Right.BalanceFactor == 1)
                {
                    this.Rotate(node, RotationDirection.Left);
                }
                else
                {
                    this.Rotate(node.Right, RotationDirection.Right);
                    this.Rotate(node, RotationDirection.Left);
                }

                //Insertion requires at most one single or double rotation
                if (isInsertion)
                    balanceParent = false;
            }
            else if (balanceFactor == -2)
            {
                if (node.Left.BalanceFactor == -1)
                {
                    this.Rotate(node, RotationDirection.Right);
                }
                else
                {
                    this.Rotate(node.Left, RotationDirection.Left);
                    this.Rotate(node, RotationDirection.Right);
                }

                //Insertion requires at most one single or double rotation
                if (isInsertion)
                    balanceParent = false;
            }
            else if ((balanceFactor == 1) || (balanceFactor == -1))
            {
                //For a delete (i.e. not insert), no further rebalance is 
                // required if the sub tree hieght is unchanged.
                // i.e. a balance factor of 1 or -1
                if (!isInsertion)
                    balanceParent = false;
            }

            //Test if the parent must be rebalanced
            if ((balanceParent) && (node.Parent != null))
                this.BalanceNode(node.Parent, isInsertion);
        }
        #endregion
    }
}
