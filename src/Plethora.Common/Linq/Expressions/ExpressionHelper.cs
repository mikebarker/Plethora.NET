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
        ///     ExpressionHelper.GetPropertyName(() => this.GetType());             // throws an exception. GetType() is not a property
        /// ]]></code>
        /// </example>
        /// <para>
        /// Notice that the expression is not executed.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="propertyExpression"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="propertyExpression"/> is not an expression to get the value of a property.
        /// </exception>
        public static string GetPropertyName<TResult>(Expression<Func<TResult>> propertyExpression)
        {
            return GetPropertyOrFieldName(propertyExpression, true, false, ResourceProvider.ExpressionMustBeProperty);
        }

        /// <see cref="GetPropertyName{T}(Expression{Func{T}})"/>
        public static string GetPropertyName<T, TResult>(Expression<Func<T, TResult>> propertyExpression)
        {
            return GetPropertyOrFieldName(propertyExpression, true, false, ResourceProvider.ExpressionMustBeProperty);
        }

        /// <see cref="GetPropertyName{T}(Expression{Func{T}})"/>
        public static string GetPropertyName(LambdaExpression expression)
        {
            return GetPropertyOrFieldName(expression, true, false, ResourceProvider.ExpressionMustBeProperty);
        }


        /// <see cref="GetPropertyName{T}(Expression{Func{T}})"/>
        public static string GetFieldName<TResult>(Expression<Func<TResult>> fieldExpression)
        {
            return GetPropertyOrFieldName(fieldExpression, false, true, ResourceProvider.ExpressionMustBeField);
        }

        /// <see cref="GetPropertyName{T}(Expression{Func{T}})"/>
        public static string GetFieldName<T, TResult>(Expression<Func<T, TResult>> fieldExpression)
        {
            return GetPropertyOrFieldName(fieldExpression, false, true, ResourceProvider.ExpressionMustBeField);
        }

        /// <see cref="GetPropertyName{T}(Expression{Func{T}})"/>
        public static string GetFieldName(LambdaExpression expression)
        {
            return GetPropertyOrFieldName(expression, false, true, ResourceProvider.ExpressionMustBeField);
        }


        /// <see cref="GetPropertyName{T}(Expression{Func{T}})"/>
        public static string GetPropertyOrFieldName<TResult>(Expression<Func<TResult>> expression)
        {
            return GetPropertyOrFieldName(expression, true, true, ResourceProvider.ExpressionMustBePropertyOrField);
        }

        /// <see cref="GetPropertyName{T}(Expression{Func{T}})"/>
        public static string GetPropertyOrFieldName<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            return GetPropertyOrFieldName(expression, true, true, ResourceProvider.ExpressionMustBePropertyOrField);
        }

        /// <see cref="GetPropertyName{T}(Expression{Func{T}})"/>
        public static string GetPropertyOrFieldName(LambdaExpression expression)
        {
            return GetPropertyOrFieldName(expression, true, true, ResourceProvider.ExpressionMustBePropertyOrField);
        }



        private static string GetPropertyOrFieldName(
            LambdaExpression expression,
            bool canBeProperty,
            bool canBeField,
            Func<string> errorTextFunc)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var body = expression.Body as MemberExpression;
            if (body == null)
                throw new ArgumentException(errorTextFunc());

            if (canBeProperty)
            {
                var property = body.Member as PropertyInfo;
                if (property != null)
                    return property.Name;
            }

            if (canBeField)
            {
                var field = body.Member as FieldInfo;
                if (field != null)
                    return field.Name;
            }

            throw new ArgumentException(errorTextFunc());
        }
    }
}
