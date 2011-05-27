namespace Plethora.Format
{
    /// <summary>
    /// Interface defining parsing and formatting functionality for partially
    /// complete strings.
    /// </summary>
    /// <typeparam name="T">
    /// The type for which this interface defines the parsing and formatting
    /// functionality.
    /// </typeparam>
    /// <seealso cref="IFormatParser{T}"/>
    public interface IFormatParserPartial<T> : IFormatParser<T>
    {
        #region Methods

        /// <summary>
        /// Determines whether a string representation of a number can be converted
        /// to its equivalent value of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <param name="partial">
        ///  <para>
        ///   'true' if the string is only a partial representation of
        ///   type <typeparamref name="T"/>; else, 'false'.
        ///  </para>
        ///  <para>
        ///   This parameter maybe 'true' to represent a text value still being
        ///   entered by a user, indicating that <paramref name="s"/> is not yet
        ///   the final representation to be parsed.
        ///  </para>
        /// </param>
        /// <returns>
        /// 'true' if <paramref name="s"/> cen be converted successfully;
        /// otherwise, 'false'.
        /// </returns>
        bool CanParse(string s, bool partial);

        #endregion
    }
}