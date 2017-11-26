using System;
using System.Globalization;

using JetBrains.Annotations;

using Plethora.Xaml.Properties;

namespace Plethora.Xaml
{
    /// <summary>
    /// Class which provides access to the standard resources with substitutions made.
    /// </summary>
    public static class ResourceProvider
    {
        #region Public Static Methods

        /// <summary>
        /// Returns the resource string 'ResourceIsNotOfType' with substitutions made.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="expectedType">The expected type of the resource.</param>
        [NotNull]
        public static string ResourceIsNotOfType([NotNull] object resourceKey, [NotNull] Type expectedType)
        {
            return StringFormat(Resources.ResourceIsNotOfType, resourceKey, expectedType.Name);
        }

        /// <summary>
        /// Returns the resource string 'ResourceKeysNotSpecified'.
        /// </summary>
        [NotNull]
        public static string ResourceKeysNotSpecified()
        {
            return Resources.ResourceKeysNotSpecified;
        }

        /// <summary>
        /// Returns the resource string 'ResourceNotLocated' with substitutions made.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        [NotNull]
        public static string ResourceNotLocated([NotNull] object resourceKey)
        {
            return StringFormat(Resources.ResourceIsNotOfType, resourceKey);
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
        [StringFormatMethod("format")]
        [NotNull]
        private static string StringFormat([NotNull] string format, [NotNull] params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, format, args);
        }
        #endregion
    }
}