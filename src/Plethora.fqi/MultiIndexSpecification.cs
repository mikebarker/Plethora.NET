using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Plethora.fqi
{
    /// <summary>
    /// A fluid interface for defining multiple indices.
    /// </summary>
    /// <typeparam name="T">The type of the data to be indexed by this declaration.</typeparam>
    /// <remarks>
    /// <example>
    /// This fluid interface allows indices to be defined as follows:
    /// <code>
    /// <![CDATA[
    /// var indexSpecification = new MultiIndexSpecification<DateTime>()
    ///     .AddIndex(true, dt => dt.Year).Then(dt => dt.Month).Then(dt => dt.Day)
    ///     .AddIndex(false, dt => dt.Hour).Then(dt => dt.Minute).Then (dt=> dt.Second);
    /// ]]>
    /// </code>
    /// In this example the index specification states that a unique index is declared on the
    /// Year, Month and Day of the underlying data, and a second non-unique index is declared
    /// on the Hour, Minute and Second values.
    /// </example>
    /// </remarks>
    /// <seealso cref="SingleIndexSpecification{T}"/>
    public class MultiIndexSpecification<T> : IEnumerable<IIndexSpecification>
    {
        /// <seealso cref="MultiIndexSpecification{T}"/>
        public class IndexSpecification : IIndexSpecification
        {
            #region Fields

            private readonly MultiIndexSpecification<T> parent;
            private readonly List<LambdaExpression> indexExpressions = new List<LambdaExpression>();
            private readonly bool isUnique;
            #endregion

            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="IndexSpecification"/> class.
            /// </summary>
            internal IndexSpecification(bool unique, MultiIndexSpecification<T> parent, LambdaExpression expression)
            {
                this.parent = parent;
                this.isUnique = unique;

                this.indexExpressions.Add(expression);
            }
            #endregion

            #region Public Methods

            /// <summary>
            /// Declares a new index, with top level property.
            /// </summary>
            /// <typeparam name="TProperty">The return type of the indexed property.</typeparam>
            /// <param name="propertyExpressions">The expression usde to retrieve the property value.</param>
            /// <returns>A fluid index declaration interface.</returns>
            public IndexSpecification AddIndex<TProperty>(Expression<Func<T, TProperty>> propertyExpressions)
            {
                return this.parent.AddIndex(propertyExpressions);
            }

            /// <summary>
            /// Declares a new index, with top level property.
            /// </summary>
            /// <typeparam name="TProperty">The return type of the indexed property.</typeparam>
            /// <param name="unique">true if the index is to be unique across all properties in the specification; else false.</param>
            /// <param name="propertyExpressions">The expression usde to retrieve the property value.</param>
            /// <returns>A fluid index declaration interface.</returns>
            public IndexSpecification AddIndex<TProperty>(bool unique, Expression<Func<T, TProperty>> propertyExpressions)
            {
                return this.parent.AddIndex(unique, propertyExpressions);
            }

            /// <summary>
            /// Identifies the next indexed property in an index.
            /// </summary>
            /// <typeparam name="TProperty">The return type of the indexed property.</typeparam>
            /// <param name="propertyExpressions">The expression usde to retrieve the property value.</param>
            /// <returns>A fluid index declaration interface.</returns>
            public IndexSpecification Then<TProperty>(Expression<Func<T, TProperty>> propertyExpressions)
            {
                if (!this.indexExpressions.Contains(propertyExpressions))
                    this.indexExpressions.Add(propertyExpressions);

                return this;
            }
            #endregion

            #region Implementation of IIndexSpecification<T>

            bool IIndexSpecification.IsUnique
            {
                get { return this.isUnique; }
            }

            IEnumerable<LambdaExpression> IIndexSpecification.IndexExpressions
            {
                get { return this.indexExpressions.AsReadOnly(); }
            }
            #endregion
        }

        #region Constants

        private const bool DEFAULT_UNIQUE = false;
        #endregion

        #region Fields

        private readonly List<IndexSpecification> indices = new List<IndexSpecification>();
        #endregion

        #region Public Methods

        /// <summary>
        /// Declares a new index, with top level property.
        /// </summary>
        /// <typeparam name="TProperty">The return type of the indexed property.</typeparam>
        /// <param name="propertyExpressions">The expression usde to retrieve the property value.</param>
        /// <returns>A fluid index declaration interface.</returns>
        /// <remarks>The index created is not unique.</remarks>
        public IndexSpecification AddIndex<TProperty>(Expression<Func<T, TProperty>> propertyExpressions)
        {
            var index = new IndexSpecification(DEFAULT_UNIQUE, this, propertyExpressions);
            this.indices.Add(index);

            return index;
        }

        /// <summary>
        /// Declares a new index, with top level property.
        /// </summary>
        /// <typeparam name="TProperty">The return type of the indexed property.</typeparam>
        /// <param name="unique">true if the index is to be unique across all properties in the specification; else false.</param>
        /// <param name="propertyExpressions">The expression usde to retrieve the property value.</param>
        /// <returns>A fluid index declaration interface.</returns>
        public IndexSpecification AddIndex<TProperty>(bool unique, Expression<Func<T, TProperty>> propertyExpressions)
        {
            var index = new IndexSpecification(unique, this, propertyExpressions);
            this.indices.Add(index);

            return index;
        }
        #endregion

        #region Implementation of IEnumerable<IIndexSpecification>

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}" /> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<IIndexSpecification> IEnumerable<IIndexSpecification>.GetEnumerator()
        {
            if (this.indices.Count == 0)
                throw new InvalidOperationException("Indices have not been added.");

            return this.indices.OfType<IIndexSpecification>().GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<IIndexSpecification>)this).GetEnumerator();
        }
        #endregion
    }
}
