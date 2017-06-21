using System;
using System.Collections;
using System.Collections.Generic;


namespace Plethora.fqi.Trees
{
    public abstract partial class MultiTree<TKey, TValue> : ITree<TKey, TValue>
    {
        #region Fields

        private readonly ITree<TKey, List<TValue>> innerTree;
        private int count = 0;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="MultiTree{TKey, TValue}"/> class.
        /// </summary>
        protected MultiTree(ITree<TKey, List<TValue>> innerTree)
        {
            //Validation
            if (innerTree == null)
                throw new ArgumentNullException(nameof(innerTree));


            this.innerTree = innerTree;
        }

        #endregion

        #region Abstract Members

        /// <summary>
        /// Gets the tree used internally in this data structure.
        /// </summary>
        protected ITree<TKey, List<TValue>> InnerTree
        {
            get { return this.innerTree; }
        }
        #endregion

        #region Implementation of IEnumerable

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.GetPairEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion

        #region Implementation of ICollection<KeyValuePair<TKey,TValue>>

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            this.InnerTree.Clear();
            this.count = 0;
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            TKey key = item.Key;

            List<TValue> list;
            bool result = this.InnerTree.TryGetValue(key, out list);
            if (!result)
                return false;

            return list.Contains(item.Value);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            //Validation
            if (array == null)
                throw new ArgumentNullException(nameof(array));

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

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            TKey key = item.Key;

            List<TValue> list;
            bool result = this.InnerTree.TryGetValue(key, out list);
            if (!result)
                return false;

            result = list.Remove(item.Value);
            if (result)
                this.count--;

            return result;
        }

        public int Count
        {
            get { return this.count; }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return false; }
        }
        #endregion

        #region Implementation of IDictionary<TKey,TValue>

        public bool ContainsKey(TKey key)
        {
            List<TValue> list;
            this.InnerTree.TryGetValue(key, out list);
            return (list.Count > 0);
        }

        public void Add(TKey key, TValue value)
        {
            //Validation
            if (key == null)
                throw new ArgumentNullException(nameof(key));


            List<TValue> list;
            object locationInfo;
            bool result = this.InnerTree.TryGetValueEx(key, out list, out locationInfo);
            if (!result)
            {
                list = new List<TValue>(1);
                this.InnerTree.AddEx(key, list, locationInfo);
            }

            this.count++;
            list.Add(value);
        }

        public bool Remove(TKey key)
        {
            //Validation
            if (key == null)
                throw new ArgumentNullException(nameof(key));


            List<TValue> list;
            bool result = this.InnerTree.TryGetValue(key, out list);
            if (!result)
                return false;

            this.count -= list.Count;
            return this.InnerTree.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            //Validation
            if (key == null)
                throw new ArgumentNullException(nameof(key));


            List<TValue> list;
            bool result = this.InnerTree.TryGetValue(key, out list);
            if (!result || list.Count == 0)
            {
                value = default(TValue);
                return false;
            }

            value = list[0];
            return true;
        }

        public TValue this[TKey key]
        {
            get
            {
                //Validation
                if (key == null)
                    throw new ArgumentNullException(nameof(key));


                TValue value;
                bool result = this.TryGetValue(key, out value);
                if (!result)
                    throw new KeyNotFoundException(string.Format("Key {0} not found in the tree.", key));

                return value;
            }
            set
            {
                //Validation
                if (key == null)
                    throw new ArgumentNullException(nameof(key));


                List<TValue> list;
                object locationInfo;
                bool result = this.InnerTree.TryGetValueEx(key, out list, out locationInfo);
                if (!result)
                {
                    list = new List<TValue>(1);
                    this.count++;
                    this.InnerTree.AddEx(key, list, locationInfo);
                }

                list.Add(value);
            }
        }

        ICollection<TKey> IDictionary<TKey,TValue>.Keys
        {
            get { return new KeysWrapper(this); }
        }

        ICollection<TValue> IDictionary<TKey,TValue>.Values
        {
            get { return new ValuesWrapper(this); }
        }
        #endregion

        #region Implementation of ITree<TKey,TValue>

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
                throw new ArgumentNullException(nameof(key));


            List<TValue> list;
            object info;
            bool result = this.InnerTree.TryGetValueEx(key, out list, out info);
            if (!result)
            {
                list = new List<TValue>(1);
                this.InnerTree.AddEx(key, list, info);
            }

            if (list.Count > 0)
            {
                //Update
                list[0] = value;
                return false;
            }
            else
            {
                //Add
                list.Add(value);
                return true;
            }
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
                throw new ArgumentNullException(nameof(key));

            if (info == null)
                throw new ArgumentNullException(nameof(info));

            if (!(info is AddExInfo))
                throw new ArgumentException("info not as supplied by TryGetValueEx method.");


            AddExInfo addExInfo = (AddExInfo)info;
            List<TValue> list;
            if (addExInfo.list == null)
            {
                list = new List<TValue>(1);
                this.InnerTree.AddEx(key, list, addExInfo.innerInfo);
            }
            else
            {
                list = addExInfo.list;
            }

            this.count++;
            list.Add(value);
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
            List<TValue> list;
            object innerInfo;
            bool result = this.InnerTree.TryGetValueEx(key, out list, out innerInfo);
            if (!result)
            {
                info = new AddExInfo(innerInfo, null);
                value = default(TValue);
                return false;
            }

            info = new AddExInfo(innerInfo, list);

            if (list.Count == 0)
            {
                value = default(TValue);
                return false;
            }

            value = list[0];
            return true;
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
            get { return true; }
        }
        #endregion

        #region Public Methods

        public IList<TValue> GetAll(TKey key)
        {
            return this.InnerTree[key].AsReadOnly();
        }

        public bool TryGetAll(TKey key, out IList<TValue> values)
        {
            List<TValue> list;
            bool result = this.InnerTree.TryGetValue(key, out list);

            values = list.AsReadOnly();
            return result;
        }
        #endregion

        private struct AddExInfo
        {
            #region Fields

            internal readonly object innerInfo;
            internal readonly List<TValue> list;
            #endregion

            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="AddExInfo"/> class.
            /// </summary>
            public AddExInfo(object innerInfo, List<TValue> list)
            {
                this.innerInfo = innerInfo;
                this.list = list;
            }
            #endregion
        }
    }
}
