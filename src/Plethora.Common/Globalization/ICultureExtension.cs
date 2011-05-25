using System.Globalization;

namespace Plethora.Globalization
{
    /// <summary>
    /// Interface which provides a custom extension for a culture.
    /// </summary>
    /// <seealso cref="CultureExtensionBase"/>
    /// <seealso cref="CultureExtensionHandler{T}"/>
    public interface ICultureExtension
    {
        /// <summary>
        /// Gets the CultureInfo for which this extension is written. 
        /// </summary>
        CultureInfo Culture { get; }


        /// <summary>
        /// Gets the suffix of an ordinal, which can be appended to a numeric
        /// value, to provide a human readable form.
        /// </summary>
        /// <param name="number">
        /// The number for which a ordinal suffix is required.
        /// </param>
        /// <returns>
        /// The suffix for the numer provided.
        /// </returns>
        string GetOrdinalSuffix(int number);

        /// <summary>
        /// Gets the number in a written, human readable form.
        /// </summary>
        /// <param name="number">
        /// The number for which the written form is required.
        /// </param>
        /// <returns>
        /// The written form of the number provided.
        /// </returns>
        string GetWordForm(int number);
    }
}