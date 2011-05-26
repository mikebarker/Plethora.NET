using System;
using System.Collections;
using System.Collections.Generic;

namespace Plethora.fqi.Trees
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
        public class Node
        {
            #region Fields

            private Node parent;

            private Node left;
            private Node right;

            private bool ignorSetHeight = false;
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
            public Node Left
            {
                get { return left; }
                set
                {
                    if (left != null)
                        left.Parent = null;

                    left = value;

                    if (left != null)
                        left.Parent = this;

                    SetHeight();
                }
            }

            /// <summary>
            /// Gets the right child of this <see cref="Node"/>.
            /// </summary>
            /// <remarks>
            /// The right child will have a key value less than the
            /// this <see cref="Node"/>'s key.
            /// </remarks>
            public Node Right
            {
                get { return right; }
                set
                {
                    if (right != null)
                        right.Parent = null;

                    right = value;

                    if (right != null)
                        right.Parent = this;

                    SetHeight();
                }
            }

            /// <summary>
            /// Gets the parent <see cref="Node"/> of this <see cref="Node"/>.
            /// </summary>
            public Node Parent
            {
                get { return parent; }
                private set
                {
                    //Ensure the previous parent no longer points to
                    // this node.
                    if (parent != null)
                    {
                        if (parent.left == this)
                            parent.left = null;
                        if (parent.right == this)
                            parent.right = null;

                        parent.SetHeight();
                    }

                    parent = value;
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
            /// <see cref="Edge.Left"/> if this <see cref="Node"/> has no parent.
            /// </value>
            public Edge? RelationToParent
            {
                get
                {
                    if (Parent == null)
                        return null;
                    else if (Parent.Left == this)
                        return Edge.Left;
                    else
                        return Edge.Right;
                }
            }

            /// <summary>
            /// Separates this <see cref="Node"/> from its <see cref="Parent"/>.
            /// </summary>
            public void RemoveFromParent()
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
                    int leftCount  = ((this.Left == null)  ? 0 : this.Left.SubTreeCount  + 1);
                    int rightCount = ((this.Right == null) ? 0 : this.Right.SubTreeCount + 1);

                    return leftCount + rightCount;
                }
            }
            #endregion

            #region Private Methods

            private void SetHeight()
            {
                if (ignorSetHeight)
                    return;

                //Prevent infinite recursion (incase of circular references)
                ignorSetHeight = true;

                int leftHeight = (left == null) ? -1 : left.Height;
                int rightHeight = (right == null) ? -1 : right.Height;

                int prevHeight = this.Height;
                this.Height = Math.Max(leftHeight, rightHeight) + 1;

                if ((Parent != null) && (prevHeight != this.Height))
                    Parent.SetHeight();

                ignorSetHeight = false;
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

            public override string ToString()
            {
                return string.Format("{0} {{ {1}, {2} }}", this.GetType().Name, this.Key, this.Value);
            }
        }

        private struct LocationInfo
        {
            #region Fields

            internal readonly Node node;
            internal readonly Edge? edge;
            #endregion

            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="LocationInfo"/> class.
            /// </summary>
            internal LocationInfo(Node node, Edge? edge)
            {
                this.node = node;
                this.edge = edge;
            }
            #endregion
        }

        #region Fields

        private Node root;
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
            if (comparer == null)
                throw new ArgumentNullException("comparer");


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
            if (tree == null)
                throw new ArgumentNullException("tree");


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
            AddNodeInternal(key, value);
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
            Node node;
            Edge? edge;
            return Find(key, out node, out edge);
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
            if (key == null)
                throw new ArgumentNullException("key");


            Node node;
            Edge? edge;
            bool result = Find(key, out node, out edge);

            if (!result)
                return false;

            RemoveNode(node);

            return true;
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
        public bool TryGetValue(TKey key, out TValue value)
        {
            //Validation
            if (key == null)
                throw new ArgumentNullException("key");


            Node node;
            Edge? edge;
            bool result = Find(key, out node, out edge);

            value = result
                ? node.Value
                : default(TValue);
            return result;
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
                if (key == null)
                    throw new ArgumentNullException("key");


                Node node;
                Edge? edge;
                bool result = Find(key, out node, out edge);
                if (!result)
                    throw new KeyNotFoundException();
                else
                    return node.Value;
            }
            set
            {
                //Validation
                if (key == null)
                    throw new ArgumentNullException("key");


                Node node;
                Edge? edge;
                bool result = Find(key, out node, out edge);
                if (!result)
                    throw new KeyNotFoundException();
                else
                    node.Value = value;
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
            if (array == null)
                throw new ArgumentNullException("array");

            if (array.Rank != 1)
                throw new ArgumentException("Only single dimensional arrays are supported for the requested action.");

            if ((arrayIndex < 0) || ((array.Length - arrayIndex) < this.Count))
                throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");


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
                if (this.Root == null)
                    return 0;
                else
                    return Root.SubTreeCount + 1;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ICollection{T}" /> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="ICollection{T}" /> is read-only; otherwise, false.
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
        /// true if the item was added; else false.
        /// </returns>
        public bool AddOrUpdate(TKey key, TValue value)
        {
            //Validation
            if (key == null)
                throw new ArgumentNullException("key");

            Node node;
            Edge? edge;
            bool result = this.Find(key, out node, out edge);
            if (result)
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
        /// or null if <see cref="key"/> was not located.
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
        public bool TryGetValueEx(TKey key, out TValue value, out object info)
        {
            //Validation
            if (key == null)
                throw new ArgumentNullException("key");


            Node node;
            Edge? edge;
            bool result = this.Find(key, out node, out edge);

            info = new LocationInfo(node, edge);
            value = (!result) ? default(TValue) : node.Value;
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
            if (key == null)
                throw new ArgumentNullException("key");

            if (info == null)
                throw new ArgumentNullException("info");

            if (!(info is LocationInfo))
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

        protected internal Node Root
        {
            get { return root; }
            set
            {
                root = value;

                if (root != null)
                    root.RemoveFromParent();
            }
        }

        protected virtual Node AddNode(TKey key, TValue value, Node parent, Edge? edge)
        {
            //Insert node
            Node node = CreateNode(key, value);
            if (parent == null)
            {
                //Tree is empty
                this.Root = node;
            }
            else
            {
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

        protected virtual void RemoveNode(Node node)
        {
            RemoveNodeInternal(node);
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
            if (key == null)
                throw new ArgumentNullException("key");


            Node parentNode;
            Edge? edge;
            bool result = Find(key, out parentNode, out edge);

            //Duplicate key found.
            if (result)
                throw new ArgumentException(string.Format("Item already exists with key {0}", key));


            return AddNode(key, value, parentNode, edge);
        }

        private void RemoveNodeInternal(Node node)
        {
            //Validation
            if (node == null)
                throw new ArgumentNullException("node");


            if ((node.Left == null) && (node.Right == null))
            {
                //No children
                node.RemoveFromParent();
            }
            else if ((node.Left == null) || (node.Right == null))
            {
                Node childNode = node.Left ?? node.Right;

                //Swap parents
                if (node.Parent == null)
                    this.Root = childNode;
                else if (node.RelationToParent == Edge.Left)
                    node.Parent.Left = childNode;
                else
                    node.Parent.Right = childNode;
            }
            else
            {
                Node current = node.Left;
                //Find largest node in left sub tree
                while (current.Right != null)
                {
                    current = current.Right;
                }
                Node.SwapNodes(node, current);
                RemoveNode(current);
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
        /// Output. Indicates the edge, relavtive to the parent returned by <paramref name="node"/>, where
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
        private bool Find(TKey key, out Node node, out Edge? edge)
        {
            edge = null;
            Node parent = null;
            Node current = this.Root;
            while(current != null)
            {
                int result = comparer.Compare(current.Key, key);
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

            if (current != null)
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
