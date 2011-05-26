using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Plethora.fqi
{
    /// <summary>
    /// A fluid interface for defining indices.
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
    /// In this example the index specification states that a unique index in declared on the
    /// Year, Month and Day of the underlying data, and a second non-unique index is declared
    /// on the Hour, Minute and Second values.
    /// </example>
    /// </remarks>
    public interface IFluidIndexDeclaration<T> : IMultiIndexDeclaration<T>
    {
        /// <summary>
        /// Identifies the next indexed property in an index.
        /// </summary>
        /// <typeparam name="TProperty">The return type of the indexed property.</typeparam>
        /// <param name="propertyExpressions">The expression usde to retrieve the property value.</param>
        /// <returns>A fluid index declaration interface.</returns>
        IFluidIndexDeclaration<T> Then<TProperty>(Expression<Func<T, TProperty>> propertyExpressions);
    }

    /// <summary>
    /// Top level interface of the fluid index definition.
    /// </summary>
    /// <typeparam name="T">The type of the data to be indexed by this declaration.</typeparam>
    public interface IMultiIndexDeclaration<T>
    {
        /// <summary>
        /// Declares a new index, with top level property.
        /// </summary>
        /// <typeparam name="TProperty">The return type of the indexed property.</typeparam>
        /// <param name="propertyExpressions">The expression usde to retrieve the property value.</param>
        /// <returns>A fluid index declaration interface.</returns>
        IFluidIndexDeclaration<T> AddIndex<TProperty>(Expression<Func<T, TProperty>> propertyExpressions);

        /// <summary>
        /// Declares a new index, with top level property.
        /// </summary>
        /// <typeparam name="TProperty">The return type of the indexed property.</typeparam>
        /// <param name="unique">true if the index is to be unique across all properties in the specification; else false.</param>
        /// <param name="propertyExpressions">The expression usde to retrieve the property value.</param>
        /// <returns>A fluid index declaration interface.</returns>
        IFluidIndexDeclaration<T> AddIndex<TProperty>(bool unique, Expression<Func<T, TProperty>> propertyExpressions);
    }

    /// <summary>
    /// Index specifiaction for a single index.
    /// </summary>
    public interface IIndexSpecification
    {
        /// <summary>
        /// A list of expressions which retrieve the indexed properties from the underlying data.
        /// </summary>
        IEnumerable<LambdaExpression> IndexExpressions { get; }

        /// <summary>
        /// true if the index is unique across the properties given by <see cref="IndexExpressions"/>.
        /// </summary>
        bool IsUnique { get; }
    }


    public class MultiIndexSpecification<T> : IEnumerable<IIndexSpecification>, IMultiIndexDeclaration<T>
    {
        private class IndexSpecification : IIndexSpecification, IFluidIndexDeclaration<T>
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
            internal IndexSpecification(MultiIndexSpecification<T> parent, LambdaExpression expression)
                : this(DEFAULT_UNIQUE, parent, expression)
            {
            }

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

            #region Implementation of IIndexSpecification<T>

            bool IIndexSpecification.IsUnique
            {
                get { return this.isUnique; }
            }

            IEnumerable<LambdaExpression> IIndexSpecification.IndexExpressions
            {
                get { return indexExpressions.AsReadOnly(); }
            }
            #endregion

            #region Implementation of IFluidIndexDeclaration<T>

            IFluidIndexDeclaration<T> IMultiIndexDeclaration<T>.AddIndex<TProperty>(Expression<Func<T, TProperty>> propertyExpressions)
            {
                return this.parent.AddIndex(propertyExpressions);
            }

            IFluidIndexDeclaration<T> IMultiIndexDeclaration<T>.AddIndex<TProperty>(bool unique, Expression<Func<T, TProperty>> propertyExpressions)
            {
                return this.parent.AddIndex(unique, propertyExpressions);
            }
            #endregion

            #region Implementation of IFluidIndexDeclaration<T>

            IFluidIndexDeclaration<T> IFluidIndexDeclaration<T>.Then<TProperty>(Expression<Func<T, TProperty>> propertyExpressions)
            {
                if (!this.indexExpressions.Contains(propertyExpressions))
                    this.indexExpressions.Add(propertyExpressions);

                return this;
            }
            #endregion
        }

        #region Constants

        private const bool DEFAULT_UNIQUE = false;
        #endregion

        #region Fields

        private readonly List<IndexSpecification> indices = new List<IndexSpecification>();
        #endregion

        #region Implementation of IMultiIndexDeclaration<T>

        public IFluidIndexDeclaration<T> AddIndex<TProperty>(Expression<Func<T, TProperty>> propertyExpressions)
        {
            var index = new IndexSpecification(this, propertyExpressions);
            this.indices.Add(index);

            return index;
        }

        public IFluidIndexDeclaration<T> AddIndex<TProperty>(bool unique, Expression<Func<T, TProperty>> propertyExpressions)
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
