using System.Globalization;

namespace Plethora.Globalization
{
    /// <summary>
    /// Culture extension class for the current UI culture.
    /// </summary>
    public sealed class CultureExtensionUI : ICultureExtension
    {
        #region Singleton Implementation

        public static readonly CultureExtensionUI Instance = new CultureExtensionUI();

        /// <summary>
        /// Initialise a new instance of the <see cref="CultureExtensionUI"/> class.
        /// </summary>
        private CultureExtensionUI()
        {
        }
        #endregion

        #region Implementation of ICultureExtension

        /// <summary>
        /// Gets the CultureInfo for which this extension is written. 
        /// </summary>
        public CultureInfo Culture
        {
            get { return InnerExtension.Culture; }
        }

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
        public string GetOrdinalSuffix(int number)
        {
            return InnerExtension.GetOrdinalSuffix(number);
        }
        #endregion

        #region Private Members

        private static ICultureExtension InnerExtension
        {
            get { return CultureExtensionProvider.Instance.GetCulturalExtension(CultureInfo.CurrentUICulture); }
        }
        #endregion
    }
}
