using System.Collections.Generic;
using System.Diagnostics;

namespace Plethora.Collections.Trees
{
    public class AvlTree<TKey, TValue> : BalancedTree<TKey, TValue>
    {
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

        protected override Node AddNode(TKey key, TValue value, Node? parent, Edge? edge)
        {
            Node node = base.AddNode(key, value, parent, edge);

            this.BalanceNodeAfterAdd(node);

            return node;
        }

        protected override bool RemoveNode(Node node)
        {
            Node? parent = node.Parent;

            bool rebalanceRequired = base.RemoveNode(node);

            if (rebalanceRequired)
            {
                if (parent is null)
                    this.BalanceNodeAfterDelete(this.Root!); // Root can't be null if we are deleting a node.
                else
                    this.BalanceNodeAfterDelete(parent);
            }

            return rebalanceRequired;
        }

        protected override Node CreateNode(TKey key, TValue value)
        {
            return new Node(key, value);
        }

        #endregion

        #region Private Methods

        private void BalanceNodeAfterAdd(Node node)
        {
            Node? _node = node;

            while (_node is not null)
            {
                //Test for balance
                int balanceFactor = BalanceFactor(_node);
                if (balanceFactor == 2)
                {
                    Debug.Assert(_node.Right is not null);

                    if (BalanceFactor(_node.Right) == 1)
                    {
                        this.Rotate(_node, RotationDirection.Left);
                    }
                    else
                    {
                        this.Rotate(_node.Right, RotationDirection.Right);
                        this.Rotate(_node, RotationDirection.Left);
                    }

                    //Insertion requires at most one single or double rotation
                    return;
                }
                else if (balanceFactor == -2)
                {
                    Debug.Assert(_node.Left is not null);

                    if (BalanceFactor(_node.Left) == -1)
                    {
                        this.Rotate(_node, RotationDirection.Right);
                    }
                    else
                    {
                        this.Rotate(_node.Left, RotationDirection.Left);
                        this.Rotate(_node, RotationDirection.Right);
                    }

                    //Insertion requires at most one single or double rotation
                    return;
                }

                _node = _node.Parent;
            }
        }

        private void BalanceNodeAfterDelete(Node node)
        {
            Node? _node = node;

            while (_node is not null)
            {
                //Test for balance
                int balanceFactor = BalanceFactor(_node);
                if (balanceFactor == 2)
                {
                    Debug.Assert(_node.Right is not null);

                    if (BalanceFactor(_node.Right) >= 0)
                    {
                        this.Rotate(_node, RotationDirection.Left);
                    }
                    else
                    {
                        this.Rotate(_node.Right, RotationDirection.Right);
                        this.Rotate(_node, RotationDirection.Left);
                    }

                    //Insertion requires at most one single or double rotation
                }
                else if (balanceFactor == -2)
                {
                    Debug.Assert(_node.Left is not null);

                    if (BalanceFactor(_node.Left) <= 0)
                    {
                        this.Rotate(_node, RotationDirection.Right);
                    }
                    else
                    {
                        this.Rotate(_node.Left, RotationDirection.Left);
                        this.Rotate(_node, RotationDirection.Right);
                    }

                    //Insertion requires at most one single or double rotation
                }
                else if ((balanceFactor == 1) || (balanceFactor == -1))
                {
                    //For a delete (i.e. not insert), no further rebalance is 
                    // required if the sub tree height is unchanged.
                    // i.e. a balance factor of 1 or -1
                    //return;
                }

                //Test if the parent must be rebalanced
                _node = _node.Parent;
            }
        }

        #endregion
    }
}
