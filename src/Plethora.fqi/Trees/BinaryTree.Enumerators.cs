using System;
using System.Collections;
using System.Collections.Generic;

namespace Plethora.fqi.Trees
{
    public partial class BinaryTree<TKey, TValue>
    {
        public abstract class BaseEnumerator<T> : IKeyLimitedEnumerator<TKey, T>
        {
            #region Fields

            private readonly BinaryTree<TKey, TValue> tree;
            private bool hasStart = false;
            private bool hasStop = false;
            private TKey start;
            private TKey stop;
            private Node currentNode;
            #endregion

            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="BaseEnumerator{T}"/> class.
            /// </summary>
            /// <param name="tree">The tree to be enumerated.</param>
            protected BaseEnumerator(BinaryTree<TKey, TValue> tree)
            {
                this.tree = tree;
                Reset();
            }
            #endregion

            #region Implementation of IDisposable

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing,
            /// or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                Reset();
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
            public virtual bool MoveNext()
            {
                if (this.currentNode == null)
                {
                    //Begin enumeration at left most node.

                    currentNode = FindFirstNode();
                }
                else
                {
                    if (currentNode.Right != null)
                    {
                        currentNode = LeftMostSubNode(currentNode.Right);
                    }
                    else
                    {
                        switch (currentNode.RelationToParent)
                        {
                            case null:
                                currentNode = null;
                                return false;

                            case Edge.Left:
                                currentNode = currentNode.Parent;
                                break;

                            case Edge.Right:
                                currentNode = FirstRightParent(currentNode);
                                break;
                        }
                    }
                }

                // null indicates the end of the enumeration
                bool result = (currentNode != null);
                if (result && hasStop)
                    result &= (this.tree.comparer.Compare(currentNode.Key, this.stop) <= 0);

                return result;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="InvalidOperationException">
            /// The collection was modified after the enumerator was created.
            /// </exception>
            public virtual void Reset()
            {
                this.currentNode = null;
            }

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <returns>
            /// The element in the collection at the current position of the enumerator.
            /// </returns>
            public abstract T Current
            {
                get;
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

            #region Implementation of IKeyLimitedEnumerator<Tkey, T>

            /// <summary>
            /// Lower limit to be placed on the key of the enumerator.
            /// </summary>
            public TKey Min
            {
                get
                {
                    if (!this.hasStart)
                        throw new InvalidOperationException("No start value defined.");

                    return this.start;
                }
                set
                {
                    this.hasStart = true;
                    this.start = value;
                }
            }

            /// <summary>
            /// Upper limit to be placed on the key of the enumerator.
            /// </summary>
            public TKey Max
            {
                get
                {
                    if (!this.hasStop)
                        throw new InvalidOperationException("No stop value defined.");

                    return this.stop;
                }
                set
                {
                    this.hasStop = true;
                    this.stop = value;
                }
            }
            #endregion

            #region Properties

            protected Node CurrentNode
            {
                get { return this.currentNode; }
            }
            #endregion

            #region Private Methods

            private Node FindFirstNode()
            {
                if (!this.hasStart)
                {
                    if (this.tree.Root == null)
                        return null;

                    return LeftMostSubNode(this.tree.Root);
                }
                else
                {
                    Node n = this.tree.Root;
                    Node startNode = null;

                    while (n != null)
                    {
                        int result = tree.comparer.Compare(n.Key, this.start);
                        if (result == 0) // n.Key == this.start
                        {
                            return n;
                        }
                        else if (result < 0) // n.Key < this.start
                        {
                            n = n.Right;
                        }
                        else if (result > 0) // n.Key > this.start
                        {
                            startNode = n;
                            n = n.Left;
                        }
                    }

                    return startNode;
                }
            }

            private static Node LeftMostSubNode(Node node)
            {
                //Search down to the left most sub-node
                while (node.Left != null)
                {
                    node = node.Left;
                }
                return node;
            }

            private static Node FirstRightParent(Node node)
            {
                while (node.RelationToParent == Edge.Right)
                {
                    node = node.Parent;
                }

                return (node.RelationToParent == null)
                    ? null
                    : node.Parent;
            }
            #endregion
        }

        public class InOrderValueEnumerator : BaseEnumerator<TValue>
        {
            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="InOrderValueEnumerator"/> class.
            /// </summary>
            /// <param name="tree">The tree to be enumerated.</param>
            public InOrderValueEnumerator(BinaryTree<TKey, TValue> tree)
                : base(tree)
            {
            }
            #endregion

            #region BaseEnumerator Overrides

            public override TValue Current
            {
                get { return this.CurrentNode.Value; }
            }
            #endregion
        }

        public class InOrderKeyEnumerator : BaseEnumerator<TKey>
        {
            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="InOrderKeyEnumerator"/> class.
            /// </summary>
            /// <param name="tree">The tree to be enumerated.</param>
            public InOrderKeyEnumerator(BinaryTree<TKey, TValue> tree)
                : base(tree)
            {
            }
            #endregion

            #region BaseEnumerator Overrides

            public override TKey Current
            {
                get { return this.CurrentNode.Key; }
            }
            #endregion
        }

        public class InOrderPairEnumerator : BaseEnumerator<KeyValuePair<TKey, TValue>>
        {
            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="InOrderPairEnumerator"/> class.
            /// </summary>
            /// <param name="tree">The tree to be enumerated.</param>
            public InOrderPairEnumerator(BinaryTree<TKey, TValue> tree)
                : base(tree)
            {
            }
            #endregion

            #region BaseEnumerator Overrides

            public override KeyValuePair<TKey, TValue> Current
            {
                get { return new KeyValuePair<TKey, TValue>(this.CurrentNode.Key, this.CurrentNode.Value); }
            }
            #endregion
        }

    }
}
