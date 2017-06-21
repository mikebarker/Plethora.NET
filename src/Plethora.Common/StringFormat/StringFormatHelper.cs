using System;

namespace Plethora.StringFormat
{
    /// <summary>
    /// Helper class which assist with string formatting.
    /// </summary>
    public static class StringFormatHelper
    {
        /// <summary>
        /// Helper method to allow named format items to be replaced by an index within a format string.
        /// </summary>
        /// <param name="format">The format string containing named format items.</param>
        /// <param name="name">The named format item.</param>
        /// <param name="index">The index which is to replace the named format item.</param>
        /// <returns>
        /// The format string with the named format item replaced by the <paramref name="index"/>.
        /// </returns>
        /// <example>
        /// The following example shows how the named format items <c>Name</c> and <c>Value</c>
        /// may be replaced in a format string:
        ///   <code>
        ///     string format = "Name: {Name}; Value: {Value:N8}";
        ///     format = format
        ///         .ReplaceNamedFormat("Name", 0)
        ///         .ReplaceNamedFormat("Value", 1);
        ///     Console.WriteLine(format);
        /// 
        ///     // Returns:
        ///     //   Name: {0}; Value: {1:N8}
        ///   </code>
        /// </example>
        /// <remarks>
        /// The format string will only replace named items which adhere to
        ///     {name[,alignment][:formatString]}
        /// 
        /// TODO: Known issues: 
        ///  <list>
        ///   <item>Whitespace may not occur within the format item identifier.</item>
        ///   <item>Named items within doubled braces are still replaced, when they should not be.</item>
        ///  </list>
        /// </remarks>
        public static string ReplaceNamedFormat(this string format, string name, int index)
        {
            if (format == null)
                throw new ArgumentNullException(nameof(format));

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), index,
                    ResourceProvider.ArgMustBeGreaterThanEqualToZero(nameof(index)));


            string namePrefix = "{" + name;
            string indexPrefix = "{" + index.ToString();

            // Allow for format items adhering to
            //   {name[,alignment][:formatString]}
            return format
                .Replace(namePrefix + "}", indexPrefix + "}")
                .Replace(namePrefix + ":", indexPrefix + ":")
                .Replace(namePrefix + ",", indexPrefix + ",");
        }

    }
}
