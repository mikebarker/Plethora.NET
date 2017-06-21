using System;
using System.Globalization;

namespace Plethora.Globalization
{
    /// <summary>
    /// Base implementation of the <see cref="ICultureExtension"/> interface.
    /// </summary>
    /// <seealso cref="ICultureExtension"/>
    public abstract class CultureExtensionBase : ICultureExtension
    {
        #region Fields

        private readonly CultureInfo culture;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialise an instance of the <see cref="CultureExtensionBase"/>
        /// class, utilising the provided language name.
        /// </summary>
        /// <param name="cultureName">
        /// The name of the culture of this extension.
        /// </param>
        protected CultureExtensionBase(string cultureName)
            : this(CultureInfo.GetCultureInfo(cultureName))
        {
        }

        /// <summary>
        /// Initialise an instance of the <see cref="CultureExtensionBase"/>
        /// class, utilising the provided culture.
        /// </summary>
        /// <param name="culture">
        /// The culture of this extension.
        /// </param>
        protected CultureExtensionBase(CultureInfo culture)
        {
            //Validation
            if (culture == null)
                throw new ArgumentNullException(nameof(culture));

            this.culture = culture;
        }
        #endregion

        #region ICulturalExtension Members

        /// <summary>
        /// Gets the CultureInfo for which this extension is written. 
        /// </summary>
        public CultureInfo Culture
        {
            get { return this.culture; }
        }

        /// <summary>
        /// Gets the suffix of an ordinal, which can be appended to a numeric
        /// value, to provide a human readable form.
        /// </summary>
        public abstract string GetOrdinalSuffix(int number);


        /// <summary>
        /// Gets the number in a written, human readable form.
        /// </summary>
        public abstract string GetWordForm(int number);
        #endregion
    }
}