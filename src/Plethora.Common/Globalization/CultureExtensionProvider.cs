using System;
using System.Collections.Generic;
using System.Globalization;

namespace Plethora.Globalization
{
    /// <summary>
    /// Class for providing culture extensions.
    /// </summary>
    /// <seealso cref="ICultureExtension"/>
    public sealed class CultureExtensionProvider
    {
        #region Fields

        private ICultureExtension defaultExtension;

        /// <summary>
        /// The table of extensions defined.
        /// </summary>
        /// <remarks>
        /// The key is the LCID of the culture.
        /// </remarks>
        private readonly Dictionary<int, ICultureExtension> cultureExtensions;
        #endregion

        #region Singleton Implementation

        public static readonly CultureExtensionProvider Instance = new CultureExtensionProvider();

        /// <summary>
        /// Initialise a new instance of the <see cref="CultureExtensionProvider"/> class.
        /// </summary>
        private CultureExtensionProvider()
        {
            this.cultureExtensions = new Dictionary<int, ICultureExtension>();
            this.DefaultExtension = CultureExtension_en.Instance;
        }
        #endregion

        #region Properties

        public ICultureExtension DefaultExtension
        {
            get
            {
                return this.defaultExtension;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this.defaultExtension = value;
                RegisterCulturalExtension(value);
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Registers an <see cref="ICultureExtension"/> with the
        /// <see cref="CultureExtensionProvider"/>.
        /// </summary>
        /// <param name="cultureExtension">
        /// The <see cref="ICultureExtension"/> to be registered.
        /// </param>
        /// <remarks>
        /// Add or updates the culture with the culture extension.
        /// </remarks>
        public void RegisterCulturalExtension(ICultureExtension cultureExtension)
        {
            //Validation
            if (cultureExtension == null)
                throw new ArgumentNullException("cultureExtension");

            CultureInfo culture = cultureExtension.Culture;
            if (culture == null)
                throw new ArgumentException(
                  ResourceProvider.ArgPropertyInvalid("cultureExtension", "Culture"), "cultureExtension");


            //Add or update the culture extension
            int lcid = culture.LCID;
            cultureExtensions[lcid] = cultureExtension;
        }

        /// <summary>
        /// Gets the culture extension for the culture provided.
        /// </summary>
        /// <param name="culture">
        /// The culture for which the extension is required.
        /// </param>
        /// <returns>
        /// The extension matching the culture provided.
        /// </returns>
        /// <remarks>
        /// The culture is searched for by the specific culture first, then by the
        /// neutral culture. The default extension is returned if no match was
        /// found.
        /// </remarks>
        public ICultureExtension GetCulturalExtension(CultureInfo culture)
        {
            //Validation
            if (culture == null)
                throw new ArgumentNullException("culture");


            CultureInfo matchCulture = culture;
            int invariantLCID = CultureInfo.InvariantCulture.LCID;

            while (matchCulture.LCID != invariantLCID)
            {
                int lcid = matchCulture.LCID;
                if (cultureExtensions.ContainsKey(lcid))
                    return cultureExtensions[lcid];

                matchCulture = matchCulture.Parent;
            }

            // return the default extension if no other match is found.
            return defaultExtension;
        }
        #endregion
    }
}