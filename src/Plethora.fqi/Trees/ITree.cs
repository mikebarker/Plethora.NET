using System;
using System.Collections.Generic;

namespace Plethora.fqi.Trees
{
    /// <summary>
    /// Interface for a tree.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys of this tree.</typeparam>
    /// <typeparam name="TValue">The type of the values of this tree.</typeparam>
    public interface ITree<TKey, TValue> : IDictionary<TKey, TValue>
    {
        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="ITree{TKey, TValue}" />,
        /// or updates the existing value if the key already exists in the tree.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        /// <returns>
        /// true if the item was added; else false.
        /// </returns>
        bool AddOrUpdate(TKey key, TValue value);

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="ITree{TKey, TValue}" />.
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
        ///      ITree<int, Student> students = new BinaryTree<int, Student>();
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
        void AddEx(TKey key, TValue value, object info);

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
        bool TryGetValueEx(TKey key, out TValue value, out object info);

        /// <summary>
        /// Returns an enumerator that iterates through the key-value pairs in the tree. 
        /// </summary>
        /// <returns>
        /// An <see cref="IKeyLimitedEnumerator{TKey, KeyValuePair}"/> for the tree.
        /// </returns>
        IKeyLimitedEnumerator<TKey, KeyValuePair<TKey, TValue>> GetPairEnumerator();

        /// <summary>
        /// Gets a flag indicating whether duplicates are allowed in this <see cref="ITree{TKey,TValue}"/>.
        /// </summary>
        /// <value>
        /// true if duplicates are allowed; else false.
        /// </value>
        bool AreDuplicatesAllowed { get; }
    }
}
