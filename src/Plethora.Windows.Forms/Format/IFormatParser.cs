using System;

namespace Plethora.Format
{
    /// <summary>
    /// Interface defining parsing and formatting functionality.
    /// </summary>
    /// <typeparam name="T">
    /// The type for which this interface defines the parsing and formatting
    /// functionality.
    /// </typeparam>
    public interface IFormatParser<T>
    {
        #region Events

        /// <summary>
        /// Sends a notification when the properties of the <see cref="IFormatParser{T}"/>
        /// have changed.
        /// </summary>
        event EventHandler Changed;
        #endregion

        #region Methods

        /// <summary>
        /// Converts the value to its equivalent string representation
        /// for display purposes.
        /// </summary>
        /// <param name="value">
        /// The value of type <typeparamref name="T"/> to be converted.
        /// </param>
        /// <returns>
        /// The string equivalent of 'value'.
        /// </returns>
        string Format(T value);

        /// <summary>
        /// Converts the string representation of a number to its equivalent
        /// value of type <typeparamref name="T"/>.
        /// A return value indicates whether the operation succeeded.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <param name="result">
        ///  <para>
        ///   When this method returns, contains the value of type
        ///   <typeparamref name="T"/> equivalent to the string contained in
        ///   's', if the conversion succeeded.
        ///  </para>
        ///  <para>
        ///   Each implementation defines its own return value for 'result' in the
        ///   case where the conversion was not successful. Usually this will be 0
        ///   for numeric values, and 'null' for reference types.
        ///  </para>
        /// </param>
        /// <returns>
        /// 'true' if 's' was converted successfully; otherwise, 'false'.
        /// </returns>
        bool TryParse(string s, out T result);

        #endregion
    }
}