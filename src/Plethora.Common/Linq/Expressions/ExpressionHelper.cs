using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Plethora.Linq.Expressions
{
    /// <summary>
    /// Helper class for working with <see cref="Expression{TDelegate}"/> instances.
    /// </summary>
    public static class ExpressionHelper
    {
        /// <summary>
        /// Gets the name of a property from an expression to get the value of the property.
        /// </summary>
        /// <typeparam name="TResult">The return type of the property.</typeparam>
        /// <param name="propertyExpression">The get property value expression.</param>
        /// <returns>
        /// The name of the property to which this expression points.
        /// </returns>
        /// <remarks>
        /// The <paramref name="propertyExpression"/> argument should be defined to the to get the value of a property.
        /// <example><code><![CDATA[
        ///     ExpressionHelper.GetPropertyName(() => this.Name);                  // returns "Name"
        ///     ExpressionHelper.GetPropertyName(() => this.DateOfBirth.Year);      // returns "Year"
        ///     ExpressionHelper.GetPropertyName(() => this.GetType().Name);        // returns "Name"
        ///     ExpressionHelper.GetPropertyName<Person, string>((person) => person.Name);  // returns "Name"
        /// 
        ///     ExpressionHelper.GetPropertyName(() => this.GetType());             // throws an exception
        /// ]]></code>
        /// </example>
        /// <para>
        /// Notice that the expression is not executed.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="propertyExpression"/> is null.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// <paramref name="propertyExpression"/> is not an expression to get the value of a property.
        /// </exception>
        public static string GetPropertyName<TResult>(Expression<Func<TResult>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
            PropertyInfo propertyInfo = (PropertyInfo)memberExpression.Member;
            return propertyInfo.Name;
        }

        /// <see cref="GetPropertyName{T}(Expression{Func{T}})"/>
        public static string GetPropertyName<T, TResult>(Expression<Func<T, TResult>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
            PropertyInfo propertyInfo = (PropertyInfo)memberExpression.Member;
            return propertyInfo.Name;
        }
    }
}
