using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Plethora
{
    /// <summary>
    /// Helper class for working with enumerations.
    /// </summary>
    public static class EnumHelper
    {
        #region Constants

        private const string DEFAULT_SEPARATOR = ", ";
        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the defined description for an enum.
        /// </summary>
        /// <param name="value">
        /// The Enum value for which the description is required.
        /// </param>
        /// <returns>
        /// The description for an enum.
        /// </returns>
        public static string Description(Enum value)
        {
            return Description(value, DEFAULT_SEPARATOR);
        }

        /// <summary>
        /// Returns the defined description for an enum.
        /// </summary>
        /// <param name="value">
        /// The Enum value for which the description is required.
        /// </param>
        /// <param name="separator">
        /// The string used to interleave multiple values if required.
        /// </param>
        /// <returns>
        /// The description for an enum. If the enum has the FlagAttribute set
        /// multiple values are return separated by 'separator', if required.
        /// </returns>
        public static string Description(Enum value, string separator)
        {
            //Validation
            if (separator == null)
                throw new ArgumentNullException("separator");


            Type enumType = value.GetType();

            object[] flagsAttribs = enumType.GetCustomAttributes(typeof(FlagsAttribute), false);

            if (flagsAttribs.Length == 0)
            {
                return DescriptionNoFlags(value);
            }
            else
            {
                string[] descriptions = DescriptionFlags(value);
                return string.Join(separator, descriptions);
            }
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Returns the defined description for an Enum where the Enum type does
        /// have the FlagsAttribute.
        /// </summary>
        /// <param name="value">
        /// The Enum value for which the description is required.
        /// </param>
        /// <returns>
        /// The descriptions for an Enum.
        /// </returns>
        /// <remarks>
        /// If value is a combination of multiple Enum values, each value is
        /// returned.
        /// </remarks>
        private static string[] DescriptionFlags(Enum value)
        {
            Type enumType = value.GetType();

            long lngValue = Convert.ToInt64(value, CultureInfo.CurrentCulture);

            Array flagValues = Enum.GetValues(enumType);
            List<string> rtnList = new List<string>(flagValues.Length);
            foreach (ValueType flagValue in flagValues)
            {
                long lngFlagValue = Convert.ToInt64(flagValue, CultureInfo.CurrentCulture);

                if (lngValue == lngFlagValue)
                {
                    string description = DescriptionNoFlags((Enum)flagValue);
                    return new string[] { description };
                }
                else if ((lngFlagValue != 0) && ((lngValue & lngFlagValue) == lngFlagValue))
                {
                    string description = DescriptionNoFlags((Enum)flagValue);
                    rtnList.Add(description);
                }
            }

            if (rtnList.Count == 0)
                return new string[] { value.ToString() };
            else
                return rtnList.ToArray();
        }

        /// <summary>
        /// Returns the defined description for an enum where the Enum type does
        /// not have the FlagsAttribute.
        /// </summary>
        /// <param name="value">
        /// The Enum value for which the description is required.
        /// </param>
        /// <returns>
        /// The description for an enum.
        /// </returns>
        private static string DescriptionNoFlags(Enum value)
        {
            Type enumType = value.GetType();

            string enumString = value.ToString();

            FieldInfo field = enumType.GetField(enumString);
            if (field == null)
                return enumString;

            object[] attribs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if ((attribs == null) || (attribs.Length == 0))
                return enumString;

            return ((DescriptionAttribute)attribs[0]).Description;
        }
        #endregion
    }
}
