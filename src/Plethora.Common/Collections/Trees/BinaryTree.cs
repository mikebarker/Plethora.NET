using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Plethora.Collections.Trees
{
    public enum Edge
    {
        Left = 0,
        Right,
    }

    /// <summary>
    /// Implementation of a binary search tree.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the tree.</typeparam>
    /// <typeparam name="TValue">The type of the values in the tree.</typeparam>
    public partial class BinaryTree<TKey, TValue> : ITree<TKey, TValue>
    {
        /// <summary>
        /// Class which denotes a node of a tree.
        /// </summary>
        [DebuggerDisplay("{GetType().Name} [ {Key}, {Value} ]")]
        public class Node
        {
            #region Fields

            private Node? parent;

            private Node? left;
            private Node? right;

            private bool ignoreSetHeight = false;
            #endregion

            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="Node"/> class.
            /// </summary>
            /// <param name="key">The key of this node.</param>
            /// <param name="value">The value of this node.</param>
            public Node(TKey key, TValue value)
            {
                this.Key = key;
                this.Value = value;
                this.Height = 0;
            }
            #endregion

            #region Properties

            /// <summary>
            /// Gets the key of this <see cref="Node"/>.
            /// </summary>
            public TKey Key { get; private set; }

            /// <summary>
            /// Gets and sets the value of this <see cref="Node"/>.
            /// </summary>
            public TValue Value { get; set; }

            /// <summary>
            /// Gets the left child of this <see cref="Node"/>.
            /// </summary>
            /// <remarks>
            /// The left child will have a key value less than the
            /// this <see cref="Node"/>'s key.
            /// </remarks>
            public Node? Left
            {
                get { return this.left; }
                protected internal set
                {
                    if (this.left is not null)
                        this.left.Parent = null;

                    this.left = value;

                    if (this.left is not null)
                        this.left.Parent = this;

                    this.SetHeight();
                }
            }

            /// <summary>
            /// Gets the right child of this <see cref="Node"/>.
            /// </summary>
            /// <remarks>
            /// The right child will have a key value less than the
            /// this <see cref="Node"/>'s key.
            /// </remarks>
            public Node? Right
            {
                get { return this.right; }
                protected internal set
                {
                    if (this.right is not null)
                        this.right.Parent = null;

                    this.right = value;

                    if (this.right is not null)
                        this.right.Parent = this;

                    this.SetHeight();
                }
            }

            /// <summary>
            /// Gets the parent <see cref="Node"/> of this <see cref="Node"/>.
            /// </summary>
            public Node? Parent
            {
                get { return this.parent; }
                private set
                {
                    //Ensure the previous parent no longer points to
                    // this node.
                    if (this.parent is not null)
                    {
                        if (this.parent.left == this)
                            this.parent.left = null;
                        if (this.parent.right == this)
                            this.parent.right = null;

                        this.parent.SetHeight();
                    }

                    this.parent = value;
                }
            }

            /// <summary>
            /// Gets the sibling <see cref="Node"/> of this <see cref="Node"/>.
            /// </summary>
            /// <value>
            /// <see cref="Parent"/>.<see cref="Right"/> if this <see cref="Node"/> is the left child of its <see cref="Parent"/>.
            /// <see cref="Parent"/>.<see cref="Left"/> if this <see cref="Node"/> is the right child of its <see cref="Parent"/>.
            /// <see langword="null"/> if this <see cref="Node"/> has no parent.
            /// </value>
            public Node? Sibling
            {
                get
                {
                    if (this.Parent is null)
                        return null;
                    else if (this.Parent.Left == this)
                        return this.Parent.Right;
                    else
                        return this.Parent.Left;
                }
            }

            /// <summary>
            /// Gets the height of this <see cref="Node"/>'s sub-tree.
            /// </summary>
            public int Height { get; private set; }

            /// <summary>
            /// Gets the relation of this <see cref="Node"/> to its <see cref="Parent"/>.
            /// </summary>
            /// <value>
            /// <see cref="Edge.Left"/> if this <see cref="Node"/> is the left child of its <see cref="Parent"/>.
            /// <see cref="Edge.Right"/> if this <see cref="Node"/> is the right child of its <see cref="Parent"/>.
            /// <see langword="null"/> if this <see cref="Node"/> has no parent.
            /// </value>
            public Edge? RelationToParent
            {
                get
                {
                    if (this.Parent is null)
                        return null;
                    else if (this.Parent.Left == this)
                        return Edge.Left;
                    else
                        return Edge.Right;
                }
            }

            /// <summary>
            /// Separates this <see cref="Node"/> from its <see cref="Parent"/>.
            /// </summary>
            protected internal void RemoveFromParent()
            {
                this.Parent = null;
            }

            /// <summary>
            /// Gets the number of nodes which occur under this node.
            /// </summary>
            public int SubTreeCount
            {
                get
                {
                    int leftCount  = ((this.Left is null)  ? 0 : this.Left.SubTreeCount  + 1);
                    int rightCount = ((this.Right is null) ? 0 : this.Right.SubTreeCount + 1);

                    return leftCount + rightCount;
                }
            }
            #endregion

            #region Private Methods

            private void SetHeight()
            {
                if (this.ignoreSetHeight)
                    return;

                //Prevent infinite recursion (incase of circular references)
                this.ignoreSetHeight = true;

                int leftHeight = (this.left is null) ? -1 : this.left.Height;
                int rightHeight = (this.right is null) ? -1 : this.right.Height;

                int prevHeight = this.Height;
                this.Height = Math.Max(leftHeight, rightHeight) + 1;

                if ((this.Parent is not null) && (prevHeight != this.Height))
                    this.Parent.SetHeight();

                this.ignoreSetHeight = false;
            }
            #endregion

            #region Static Methods

            /// <summary>
            /// Swaps the key and value of this node with another.
            /// </summary>
            /// <param name="node1">The first node to be swapped.</param>
            /// <param name="node2">The second node to be swapped.</param>
            /// <remarks>
            /// WARNING: Calling this method will result in an out-of-order tree.
            /// </remarks>
            internal static void SwapNodes(Node node1, Node node2)
            {
                if (node1 == node2)
                    return;

                TKey node1Key = node1.Key;
                TValue node1Value = node1.Value;

                node1.Key = node2.Key;
                node1.Value = node2.Value;
                node2.Key = node1Key;
                node2.Value = node1Value;
            }
            #endregion

            //public override string ToString()
            //{
            //    return $"{this.GetType().Name} {{ {this.Key}, {this.Value} }}";
            //}
        }

        private readonly struct LocationInfo
        {
            #region Fields

            internal readonly Node? node;
            internal readonly Edge? edge;
            #endregion

            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="LocationInfo"/> class.
            /// </summary>
            internal LocationInfo(Node? node, Edge? edge)
            {
                this.node = node;
                this.edge = edge;
            }
            #endregion
        }

        #region Fields

        private Node? root;
        private readonly IComparer<TKey> comparer;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="BinaryTree{TKey, TValue}"/> class.
        /// </summary>
        public BinaryTree()
            : this(Comparer<TKey>.Default)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BinaryTree{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="comparer">
        /// The <see cref="IComparer{T}"/> implementation to use when comparing keys.
        /// </param>
        public BinaryTree(IComparer<TKey> comparer)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(comparer);


            this.comparer = comparer;
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="tree">
        /// The <see cref="BinaryTree{TKey, TValue}"/> which is to be copied.
        /// </param>
        protected BinaryTree(BinaryTree<TKey, TValue> tree)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(tree);


            this.comparer = tree.comparer;
            this.Root = tree.Root;
        }
        #endregion

        #region Implementation of IDictionary<TKey, TValue>

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="BinaryTree{TKey, TValue}" />.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        public void Add(TKey key, TValue value)
        {
            this.AddNodeInternal(key, value);
        }


        /// <summary>
        /// Determines whether the <see cref="IDictionary{TKey, TValue}" /> contains an element
        /// with the specified key.
        /// </summary>
        /// <returns>
        /// true if the <see cref="IDictionary{TKey, TValue}" /> contains an element with the key; 
        /// otherwise, false.
        /// </returns>
        /// <param name="key">The key to locate in the <see cref="IDictionary{TKey, TValue}" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="key" /> is null.</exception>
        public bool ContainsKey(TKey key)
        {
            return this.Find(key, out _, out _);
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="BinaryTree{TKey, TValue}" />.
        /// </summary>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.
        /// This method also returns false if <paramref name="key" /> was not found in the original
        /// <see cref="BinaryTree{TKey, TValue}" />.
        /// </returns>
        /// <param name="key">The key of the element to remove.</param>
        /// <exception cref="ArgumentNullException"><paramref name="key" /> is null.</exception>
        public bool Remove(TKey key)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(key);


            if (this.Find(key, out var node, out _))
            {
                this.RemoveNode(node);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <returns>
        /// true if the object that implements <see cref="IDictionary{TKey, TValue}" /> contains an
        /// element with the specified key; otherwise, false.
        /// </returns>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">
        /// When this method returns, the value associated with the specified key, if the key is found;
        /// otherwise, the default value for the type of the <paramref name="value" /> parameter. 
        /// This parameter is passed uninitialized.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="key" /> is null.</exception>
        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(key);


            if (this.Find(key, out var node, out _))
            {
                value = node.Value;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        /// <summary>
        /// Gets the element with the specified key.
        /// </summary>
        /// <returns>
        /// The element with the specified key.
        /// </returns>
        /// <param name="key">The key of the element to get or set.</param>
        /// <exception cref="ArgumentNullException"><paramref name="key" /> is null.</exception>
        /// <exception cref="KeyNotFoundException">The property is retrieved and <paramref name="key" /> is not found.</exception>
        public TValue this[TKey key]
        {
            get
            {
                //Validation
                ArgumentNullException.ThrowIfNull(key);


                if (this.Find(key, out var node, out _))
                {
                    return node.Value;
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            set
            {
                //Validation
                ArgumentNullException.ThrowIfNull(key);


                if (this.Find(key, out var node, out _))
                {
                    node.Value = value;
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
        }

        /// <summary>
        /// Gets an <see cref="ICollection{T}" /> containing the keys of the <see cref="IDictionary{TKey, TValue}" />.
        /// </summary>
        /// <returns>
        /// An <see cref="ICollection{T}" /> containing the keys of the object that implements <see cref="IDictionary{TKey, TValue}" />.
        /// </returns>
        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            get { return new KeysWrapper(this); }
        }

        /// <summary>
        /// Gets an <see cref="ICollection{T}" /> containing the values in the <see cref="IDictionary{TKey, TValue}" />.
        /// </summary>
        /// <returns>
        /// An <see cref="ICollection{T}" /> containing the values in the object that implements <see cref="IDictionary{TKey, TValue}" />.
        /// </returns>
        ICollection<TValue> IDictionary<TKey, TValue>.Values
        {
            get { return new ValuesWrapper(this); }
        }
        #endregion

        #region Implementation of IEnumerable<KeyValuePair<TKey, TValue>>

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator{T}" /> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return new InOrderPairEnumerator(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, TValue>>)this).GetEnumerator();
        }
        #endregion

        #region Implementation of ICollection<KeyValuePair<TKey, TValue>>

        /// <summary>
        /// Adds an item to the <see cref="ICollection{T}" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="ICollection{T}" />.</param>
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        /// <summary>
        /// Removes all items from the <see cref="ICollection{T}" />.
        /// </summary>
        public void Clear()
        {
            this.Root = null;
        }

        /// <summary>
        /// Determines whether the <see cref="ICollection{T}" /> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item" /> is found in the <see cref="ICollection{T}" />; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="ICollection{T}" />.</param>
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.ContainsKey(item.Key);
        }

        /// <summary>
        /// Copies the elements of the <see cref="ICollection{T}" /> to an <see cref="Array" />, starting at a particular <see cref="Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array" /> that is the destination of the elements copied from <see cref="ICollection{T}" />. The <see cref="Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="ArgumentNullException">
        ///  <paramref name="array" /> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="arrayIndex" /> is less than 0.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///  <paramref name="arrayIndex" /> is equal to or greater than the length of <paramref name="array" />.
        ///  -or-
        ///  The number of elements in the source <see cref="ICollection{T}" /> is greater than the available
        ///  space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.
        /// </exception>
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
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

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="ICollection{T}" />.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item" /> was successfully removed from the <see cref="ICollection{T}" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="ICollection{T}" />.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="ICollection{T}" />.</param>
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.Remove(item.Key);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="ICollection{T}" />.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="ICollection{T}" />.
        /// </returns>
        public int Count
        {
            get
            {
                if (this.Root is null)
                    return 0;
                else
                    return this.Root.SubTreeCount + 1;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ICollection{T}" /> is read-only.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the <see cref="ICollection{T}" /> is read-only; otherwise <see langword="false"/>.
        /// </returns>
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return false; }
        }
        #endregion

        #region Implementation of ITree<TKey, TValue>

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="ITree{TKey, TValue}" />,
        /// or updates the existing value if the key already exists in the tree.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        /// <returns>
        /// <see langword="true"/> if the item was added; otherwise <see langword="false"/>.
        /// </returns>
        public bool AddOrUpdate(TKey key, TValue value)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(key);

            if (this.Find(key, out var node, out var edge))
            {
                //Update
                node.Value = value;
                return false;
            }
            else
            {
                //Add
                this.AddNode(key, value, node, edge);
                return true;
            }
        }

        /// <summary>
        /// Gets the value associated with the specified key, providing additional information
        /// to provide performant conditional "addition" operations.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">
        /// Output. When this method returns, the value associated with the specified key, if the key is found;
        /// otherwise, the default value for the type of the <paramref name="value" /> parameter. 
        /// This parameter is passed uninitialized.
        /// </param>
        /// <param name="info">
        /// Output. Object containing location information of <see cref="key"/> if located;
        /// or <see langword="null"/> if <see cref="key"/> was not located.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        /// true if the object that implements <see cref="IDictionary{TKey, TValue}" /> contains an
        /// element with the specified key; otherwise, false.
        /// </returns>
        /// <remarks>
        /// Code should not depend on <paramref name="info"/>. This output parameter should only be
        /// passed, as is, to the <see cref="AddEx"/> method.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="key" /> is null.</exception>
        /// <seealso cref="AddEx"/>
        public bool TryGetValueEx(TKey key, [MaybeNullWhen(false)] out TValue value, [MaybeNullWhen(false)] out object info)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(key);


            bool result = this.Find(key, out var node, out var edge);

            info = new LocationInfo(node, edge);
            value = (!result) ? default : node!.Value;
            return result;
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="BinaryTree{TKey, TValue}" />.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        /// <param name="info">The location information for where the element is to be added.</param>
        /// <remarks>
        ///  <para>
        ///   The parameter <paramref name="info"/> should be provided by the <see cref="TryGetValueEx"/> method.
        ///  </para>
        ///  <para>
        ///   This method is supplied to allow code to be executed depending on the result of conducting a
        ///   "TryGetValue" operation before adding an element, without loosing the performance benefit derived
        ///   from the knowledge gained during the "TryGetValue" operation. Example:
        ///   <code>
        ///   <![CDATA[
        ///      BinaryTree<int, Student> students = new BinaryTree<int, Student>();
        ///      public Student FindOrCreate(int studentId)
        ///      {
        ///          Student student;
        ///          object locationInfo;
        ///          bool result = tree.TryGetValueEx(studentId, out student, out locationInfo);
        ///          if (!result)
        ///          {
        ///              student = new Student();
        ///              tree.AddEx(studentId, student, locationInfo);
        ///          }
        /// 
        ///          return student;
        ///      }
        ///   ]]>
        ///   </code>
        ///  </para>
        /// </remarks>
        /// <seealso cref="TryGetValueEx"/>
        public void AddEx(TKey key, TValue value, object info)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(key);
            ArgumentNullException.ThrowIfNull(info);

            if (info is not LocationInfo)
                throw new ArgumentException("info not as supplied by TryGetValueEx method.");


            LocationInfo locationInfo = (LocationInfo)info;
            this.AddNode(key, value, locationInfo.node, locationInfo.edge);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the values in the tree. 
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator{TValue}"/> for the tree.
        /// </returns>
        public IKeyLimitedEnumerator<TKey, KeyValuePair<TKey, TValue>> GetPairEnumerator()
        {
            return new InOrderPairEnumerator(this);
        }

        /// <summary>
        /// Gets a flag indicating whether duplicates are allowed in this <see cref="ITree{TKey,TValue}"/>.
        /// </summary>
        /// <value>
        /// true if duplicates are allowed; else false.
        /// </value>
        public bool AreDuplicatesAllowed
        {
            get { return false; }
        }
        #endregion

        #region Protected Members

        public Node? Root
        {
            get { return this.root; }
            protected internal set
            {
                this.root = value;

                this.root?.RemoveFromParent();
            }
        }

        protected virtual Node AddNode(TKey key, TValue value, Node? parent, Edge? edge)
        {
            //Insert node
            Node node = this.CreateNode(key, value);
            if (parent is null)
            {
                //Tree is empty
                this.Root = node;
            }
            else
            {
                Debug.Assert(edge.HasValue);

                switch (edge.Value)
                {
                    case Edge.Left:
                        parent.Left = node;
                        break;
                    case Edge.Right:
                        parent.Right = node;
                        break;
                }
            }
            return node;
        }

        protected virtual bool RemoveNode(Node node)
        {
            return this.RemoveNodeInternal(node);
        }

        protected virtual Node CreateNode(TKey key, TValue value)
        {
            return new Node(key, value);
        }
        #endregion

        #region Private Methods

        private Node AddNodeInternal(TKey key, TValue value)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(key);


            bool result = this.Find(key, out var parentNode, out var edge);

            //Duplicate key found.
            if (result)
                throw new ArgumentException($"Item already exists with key {key}");


            return this.AddNode(key, value, parentNode, edge);
        }

        private bool RemoveNodeInternal(Node node)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(node);


            if ((node.Left is null) && (node.Right is null))
            {
                //No children
                node.RemoveFromParent();

                return true;
            }
            else if ((node.Left is null) || (node.Right is null))
            {
                Node? childNode = node.Left ?? node.Right;

                //Swap parents
                if (node.Parent is null)
                    this.Root = childNode;
                else if (node.RelationToParent == Edge.Left)
                    node.Parent.Left = childNode;
                else
                    node.Parent.Right = childNode;

                return true;
            }
            else
            {
                Node current = node.Left;
                //Find largest node in left sub tree
                while (current.Right is not null)
                {
                    current = current.Right;
                }
                Node.SwapNodes(node, current);
                this.RemoveNode(current);

                return false;
            }
        }

        /// <summary>
        /// Finds the node with a specified key in the tree, or the location where the key
        /// should be inserted.
        /// </summary>
        /// <param name="key">
        /// The key of the node to be located.
        /// </param>
        /// <param name="node">
        /// Output. If the return value is true, <paramref name="node"/> returns the node containing key;
        /// else returns the parent node for an insert operation, 'null' if the tree is empty.
        /// </param>
        /// <param name="edge">
        /// Output. Indicates the edge, relative to the parent returned by <paramref name="node"/>, where
        /// the key be inserted. 
        /// Invalid if <paramref name="node"/> is null, or the return value is true.
        /// </param>
        /// <returns>
        /// true if key was found in the tree; else false
        /// </returns>
        /// <remarks>
        /// The following table defines the return values:
        /// <![CDATA[
        ///  +====================+========+===============+=============+
        ///  | STATE              | result |     node      |    edge     |
        ///  +--------------------+--------+---------------+-------------+
        ///  | Tree is empty.     | false  |      null     |      X      |
        ///  | Key was not found. | false  | insert parent | insert edge |
        ///  | Key found.         | true   | located node  |      X      |
        ///  +====================+========+===============+=============+
        /// ]]>
        /// X = Undefined, possibly null.
        /// </remarks>
        private bool Find(TKey key, [MaybeNullWhen(false)] out Node node, out Edge? edge)
        {
            edge = null;
            Node? parent = null;
            Node? current = this.Root;
            while(current is not null)
            {
                int result = this.comparer.Compare(current.Key, key);
                if (result == 0)        // current.Key == key
                {
                    break;
                }
                else if (result < 0)    // current.Key < key
                {
                    // Traverse right subtree
                    parent = current;
                    edge = Edge.Right;
                    current = current.Right;
                }
                else if (result > 0)    // current.Key > key
                {
                    // Traverse left subtree
                    parent = current;
                    edge = Edge.Left;
                    current = current.Left;
                }
            }

            if (current is not null)
            {
                //Node found
                edge = null;
                node = current;
                return true;
            }
            else
            {
                //Node not found.
                // 'edge' will contain insert direction.
                node = parent;
                return false;
            }
        }

        #endregion
    }
}
