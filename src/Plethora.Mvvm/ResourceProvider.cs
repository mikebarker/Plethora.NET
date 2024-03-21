using System;
using System.Globalization;

using Plethora.Mvvm.Properties;

namespace Plethora.Mvvm
{
    /// <summary>
    /// Class which provides access to the standard resources with substitutions made.
    /// </summary>
    public static class ResourceProvider
    {
        #region Public Static Methods

        /// <summary>
        /// Returns the resource string 'CantFindStaticMember' with substitutions made.
        /// </summary>
        /// <param name="memberName">The name of the member of the type.</param>
        /// <param name="type">The type.</param>
        public static string CantFindStaticMember(string memberName, Type type)
        {
            return StringFormat(Resources.CantFindStaticMember,
                memberName,
                type.Name);
        }

        /// <summary>
        /// Returns the resource string 'DefaultPropertyValueNotOfType' with substitutions made.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="propertyType">The type of the property.</param>
        public static string DefaultPropertyValueNotOfType(string propertyName, Type propertyType, object? defaultValue)
        {
            return StringFormat(Resources.DefaultPropertyValueNotOfType,
                propertyName,
                propertyType.Name,
                defaultValue ?? "<null>");
        }

        /// <summary>
        /// Returns the resource string 'PropertyExpression' with substitutions made.
        /// </summary>
        /// <param name="propertyExpressionArgumentName">The name of the property expression argument.</param>
        /// <param name="type">The type.</param>
        public static string PropertyExpression(string propertyExpressionArgumentName, Type type)
        {
            return StringFormat(Resources.PropertyExpression,
                propertyExpressionArgumentName,
                type.Name);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns the format string with substitutions made, according to the current culture.
        /// </summary>
        /// <param name="format">
        /// A string containing zero or more format items.
        /// </param>
        /// <param name="args">
        /// An object array containing zero or more objects to format.
        /// </param>
        /// <returns>
        /// The format string with substitutions made, according to the current UI
        /// culture.
        /// </returns>
        private static string StringFormat(string format, params object?[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, format, args);
        }
        #endregion
    }
}