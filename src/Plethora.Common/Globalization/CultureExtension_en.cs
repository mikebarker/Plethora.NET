namespace Plethora.Globalization
{
    /// <summary>
    /// Culture extension class for English.
    /// </summary>
    /// <remarks>
    /// Culture code 'en'.
    /// </remarks>
    /// <seealso cref="NumericHelper"/>
    public sealed class CultureExtension_en : CultureExtensionBase
    {
        #region Constants

        private const string CULTURE_NAME = "en";
        #endregion

        #region Singleton Implementation

        public static readonly CultureExtension_en Instance = new CultureExtension_en();

        /// <summary>
        /// Initialise a new instance of the <see cref="CultureExtension_en"/> class.
        /// </summary>
        private CultureExtension_en()
            : base(CULTURE_NAME)
        {
        }

        #endregion

        #region CultureExtensionBase Overrides

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
        /// <example>
        /// The number 1 should take the suffix 'st' (of first).
        /// </example>
        public override string GetOrdinalSuffix(int number)
        {
            int unitDigit = number % 10;
            int tensDigit = (number / 10) % 10;

            //For most numbers the ordinal ends in "th",
            // except for those where the unit is 1, 2 or 3,
            //The exception to this rule is 11, 12 and 13.
            string ordinal = "th";
            if (tensDigit != 1)
            {
                switch (unitDigit)
                {
                    case 1:
                        ordinal = "st";
                        break;

                    case 2:
                        ordinal = "nd";
                        break;

                    case 3:
                        ordinal = "rd";
                        break;
                }
            }

            return ordinal;
        }
        #endregion
    }
}
