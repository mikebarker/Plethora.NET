using System.Collections.Generic;
using System.Diagnostics;

namespace Plethora.Collections.Trees
{
    public class RedBlackTree<TKey, TValue> : BalancedTree<TKey, TValue>
    {
        [DebuggerDisplay("{GetType().Name} - {Color} [ {Key}, {Value} ]")]
        protected internal class RedBlackNode : Node, IHasColor
        {
            private Color color = Color.Black;

            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="RedBlackNode"/> class.
            /// </summary>
            public RedBlackNode(TKey key, TValue value)
                : base(key, value)
            {
            }

            #endregion

            #region Properties

            public new RedBlackNode? Left
            {
                get { return (RedBlackNode?)base.Left; }
                protected internal set { base.Left = value; }
            }

            public new RedBlackNode? Right
            {
                get { return (RedBlackNode?)base.Right; }
                protected internal set { base.Right = value; }
            }

            public new RedBlackNode? Parent
            {
                get { return (RedBlackNode?)base.Parent; }
            }

            public new RedBlackNode? Sibling
            {
                get { return (RedBlackNode?)base.Sibling; }
            }

            internal Color Color
            {
                get { return this.color; }
                set { this.color = value; }
            }

            Color IHasColor.Color
            {
                get { return this.Color; }
            }

            #endregion
        }

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="BinaryTree{TKey, TValue}"/> class.
        /// </summary>
        public RedBlackTree()
            : base()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BinaryTree{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="comparer">
        /// The <see cref="IComparer{T}"/> implementation to use when comparing keys.
        /// </param>
        public RedBlackTree(IComparer<TKey> comparer)
            : base(comparer)
        {
        }

        #endregion

        #region Override Methods

        protected internal new RedBlackNode? Root
        {
            get { return (RedBlackNode?)base.Root; }
            set { base.Root = value; }
        }

        protected override Node AddNode(TKey key, TValue value, Node? parent, Edge? edge)
        {
            RedBlackNode node = (RedBlackNode)base.AddNode(key, value, parent, edge);

            node.Color = Color.Red;
            this.BalanceAfterAdd(node);

            return node;
        }

        protected override bool RemoveNode(Node node)
        {
            RedBlackNode redBlackNode = ((RedBlackNode)node);

            RedBlackNode? parent = redBlackNode.Parent;
            Edge? edge = redBlackNode.RelationToParent;
            Color nodeColor = redBlackNode.Color;

            bool skipRebalance = false;
            if ((redBlackNode.Left == null) &&
                (redBlackNode.Right == null))
            {
                if (redBlackNode.Color == Color.Black)
                {
                    this.DeleteCase1(redBlackNode);
                }

                skipRebalance = true;
            }

            bool rebalanceRequired = base.RemoveNode(redBlackNode);

            if (!skipRebalance & rebalanceRequired)
            {
                RedBlackNode? child;
                if (parent == null)
                    child = this.Root;
                else if (edge == Edge.Left)
                    child = parent.Left;
                else
                    child = parent.Right;

                if (nodeColor == Color.Black)
                {
                    if (child?.Color == Color.Red)
                    {
                        child.Color = Color.Black;
                    }
                    else
                    {
                        this.DeleteCase1(child!);
                    }
                }
            }

            return rebalanceRequired;
        }

        protected override Node CreateNode(TKey key, TValue value)
        {
            return new RedBlackNode(key, value);
        }


        #endregion

        #region Private Methods

        private void BalanceAfterAdd(RedBlackNode node)
        {
            if (node.Parent == null)
            {
                // This is the root node.
                // No re-balance required
                node.Color = Color.Black;
                return;
            }

            RedBlackNode? grandParent = node.Parent.Parent;
            if (grandParent == null)
            {
                // Parent is the root node.
                // No re-balance required
                return;
            }

            if (node.Parent.Color == Color.Black)
            {
                // No re-balance required
                return;
            }

            RedBlackNode? uncle = node.Parent.Sibling;

            if ((uncle != null) && (uncle.Color == Color.Red))  // uncle is red
            {
                node.Parent.Color = Color.Black;
                uncle.Color = Color.Black;
                grandParent.Color = Color.Red;

                this.BalanceAfterAdd(grandParent);
            }
            else
            {
                RedBlackNode step2Node = node;
                RedBlackNode step2Parent = node.Parent;
                RedBlackNode step2GrandParent = node.Parent.Parent!;

                //Step 1
                if ((grandParent.Left != null) && (node == grandParent.Left.Right))
                {
                    this.Rotate(node.Parent, RotationDirection.Left);
                    step2Node = node.Left!;
                    step2Parent = node;
                    step2GrandParent = node.Parent;
                }
                else if ((grandParent.Right != null) && (node == grandParent.Right.Left))
                {
                    this.Rotate(node.Parent, RotationDirection.Right);
                    step2Node = node.Right!;
                    step2Parent = node;
                    step2GrandParent = node.Parent;
                }

                //Step 2
                if (ReferenceEquals(step2Node, step2Parent.Left))
                    this.Rotate(step2GrandParent, RotationDirection.Right);
                else
                    this.Rotate(step2GrandParent, RotationDirection.Left);

                step2Parent.Color = Color.Black;
                step2GrandParent.Color = Color.Red;
            }
        }


        private void DeleteCase1(RedBlackNode node)
        {
            if (node.Parent != null)
                this.DeleteCase2(node);
        }

        private void DeleteCase2(RedBlackNode node)
        {
            RedBlackNode? sibling = node.Sibling;

            if (sibling?.Color == Color.Red)
            {
                node.Parent!.Color = Color.Red;
                sibling.Color = Color.Black;

                if (ReferenceEquals(node.Parent.Left, node))
                    this.Rotate(node.Parent, RotationDirection.Left);
                else
                    this.Rotate(node.Parent, RotationDirection.Right);
            }

            this.DeleteCase3(node);
        }

        private void DeleteCase3(RedBlackNode node)
        {
            RedBlackNode sibling = node.Sibling!;

            if ((node.Parent!.Color == Color.Black) &&
                (sibling.Color == Color.Black) &&
                (sibling.Left!.SafeColor() == Color.Black) &&
                (sibling.Right!.SafeColor() == Color.Black))
            {
                sibling.Color = Color.Red;
                this.DeleteCase1(node.Parent);
            }
            else
            {
                this.DeleteCase4(node);
            }
        }

        private void DeleteCase4(RedBlackNode node)
        {
            RedBlackNode sibling = node.Sibling!;


            if ((node.Parent!.Color == Color.Red) &&
                (sibling.Color == Color.Black) &&
                (sibling.Left!.SafeColor() == Color.Black) &&
                (sibling.Right!.SafeColor() == Color.Black))
            {
                sibling.Color = Color.Red;
                node.Parent.Color = Color.Black;
            }
            else
            {
                this.DeleteCase5(node);
            }
        }

        private void DeleteCase5(RedBlackNode node)
        {
            RedBlackNode sibling = node.Sibling!;

            if (sibling.Color == Color.Black)
            {
                /* this if statement is trivial,
                 * due to case 2 (even though case 2 changed the sibling to a sibling's child,
                 * the sibling's child can't be red, since no red parent can have a red child).
                 * the following statements just force the red to be on the left of the left of the parent,
                 * or right of the right, so case six will rotate correctly.
                 */
                if ((node == node.Parent!.Left) &&
                    (sibling.Right!.SafeColor() == Color.Black) &&
                    (sibling.Left!.SafeColor() == Color.Red))
                {
                    // this last test is trivial too due to cases 2-4.
                    sibling.Color = Color.Red;
                    sibling.Left!.Color = Color.Black;

                    this.Rotate(sibling, RotationDirection.Right);
                }
                else if ((node == node.Parent.Right) &&
                    (sibling.Left!.SafeColor() == Color.Black) &&
                    (sibling.Right!.SafeColor() == Color.Red))
                {
                    // this last test is trivial too due to cases 2-4.
                    sibling.Color = Color.Red;
                    sibling.Right!.Color = Color.Black;

                    this.Rotate(sibling, RotationDirection.Left);
                }
            }

            this.DeleteCase6(node);
        }

        private void DeleteCase6(RedBlackNode node)
        {
            RedBlackNode sibling = node.Sibling!;

            sibling!.Color = node.Parent!.Color;
            node.Parent.Color = Color.Black;

            if (node == node.Parent.Left)
            {
                sibling.Right!.Color = Color.Black;
                this.Rotate(node.Parent, RotationDirection.Left);
            }
            else
            {
                sibling.Left!.Color = Color.Black;
                this.Rotate(node.Parent, RotationDirection.Right);
            }
        }


        #endregion
    }

    internal enum Color
    {
        Red,
        Black
    }

    internal interface IHasColor
    {
        Color Color { get; }
    }

    internal static class RedBlackNodeHelper
    {
        public static Color SafeColor(this IHasColor node)
        {
            if (ReferenceEquals(node, null))
                return Color.Black;

            return node.Color;
        }
    }
}
