using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
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
        /// Presents all elements of a enumeration as a list.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> containing the elements of the enum.
        /// </returns>
        public static IEnumerable<T> GetValues<T>()
        {
            //Validation
            if (typeof(T).IsSubclassOf(typeof(Enum)))
                throw new ArgumentException(ResourceProvider.ArgMustBeOfType(nameof(T), typeof(Enum)), nameof(T));


            Array flagValues = Enum.GetValues(typeof(T));
            return flagValues.OfType<T>();
        }

        /// <summary>
        /// Returns the defined description for an enum.
        /// </summary>
        /// <param name="value">The Enum value for which the description is required.</param>
        /// <returns>
        /// The description for an enum.
        /// </returns>
        public static string Description(this Enum value)
        {
            return Description(value, DEFAULT_SEPARATOR);
        }

        /// <summary>
        /// Returns the defined description for an enum.
        /// </summary>
        /// <param name="value">The Enum value for which the description is required.</param>
        /// <param name="separator">The string used to interleave multiple values if required.</param>
        /// <returns>
        /// The description for an enum. If the enum has the FlagAttribute set
        /// multiple values are return separated by 'separator', if required.
        /// </returns>
        public static string Description(this Enum value, string separator)
        {
            return Description(value, separator, typeof(DescriptionAttribute), "Description");
        }

        /// <summary>
        /// Returns the defined description for an enum.
        /// </summary>
        /// <param name="value">The Enum value for which the description is required.</param>
        /// <param name="separator">The string used to interleave multiple values if required.</param>
        /// <param name="attributeType">The attribute type which contains the enum's description.</param>
        /// <param name="attributeProperty">The attribute property which returns the enums description.</param>
        /// <returns>
        /// The description for an enum. If the enum has the FlagAttribute set
        /// multiple values are return separated by 'separator', if required.
        /// </returns>
        public static string Description(this Enum value, string separator, Type attributeType, string attributeProperty)
        {
            //Validation
            if (separator == null)
                throw new ArgumentNullException(nameof(separator));

            if (attributeType == null)
                throw new ArgumentNullException(nameof(attributeType));

            if (attributeProperty == null)
                throw new ArgumentNullException(nameof(attributeProperty));


            var enumToDescription = CreateEnumToDescrptionFunc(attributeType, attributeProperty);

            Type enumType = value.GetType();

            object[] flagsAttribs = enumType.GetCustomAttributes(typeof(FlagsAttribute), false);

            if (flagsAttribs.Length == 0)
            {
                return enumToDescription(value);
            }
            else
            {
                string[] descriptions = DescriptionFlags(value, enumToDescription);
                return string.Join(separator, descriptions);
            }
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Returns the defined description for an Enum where the Enum type does
        /// have the FlagsAttribute.
        /// </summary>
        /// <param name="value">The Enum value for which the description is required.</param>
        /// <param name="enumToDescription">The mapping function which returns the description for an enum's value.</param>
        /// <returns>
        /// The descriptions for an Enum.
        /// </returns>
        /// <remarks>
        /// If value is a combination of multiple Enum values, each value is
        /// returned.
        /// </remarks>
        private static string[] DescriptionFlags(Enum value, Func<Enum, string> enumToDescription)
        {
            Type enumType = value.GetType();

            long lngValue = Convert.ToInt64(value, CultureInfo.CurrentCulture);

            Array flagValues = Enum.GetValues(enumType);
            List<string> rtnList = new List<string>(flagValues.Length);
            foreach (Enum flagValue in flagValues)
            {
                long lngFlagValue = Convert.ToInt64(flagValue, CultureInfo.CurrentCulture);

                //Elements with exactly match will not be broken down to individual flags
                if (lngValue == lngFlagValue)
                {
                    string description = enumToDescription(flagValue);
                    return new string[] { description };
                }
                //Each individual flag which is set returns, unless an exact is found
                else if ((lngFlagValue != 0) && ((lngValue & lngFlagValue) == lngFlagValue))
                {
                    string description = enumToDescription(flagValue);
                    rtnList.Add(description);
                }
            }

            if (rtnList.Count == 0)
                return new string[] { value.ToString() };
            else
                return rtnList.ToArray();
        }

        private static Func<Enum, string> CreateEnumToDescrptionFunc(Type attributeType, string attributeProperty)
        {
            PropertyInfo propertyInfo = attributeType.GetProperty(attributeProperty, BindingFlags.Instance | BindingFlags.Public);
            Func<Enum, string> enumToDescrptionFunc = delegate(Enum value)
                {
                    Type enumType = value.GetType();

                    string enumString = value.ToString();

                    FieldInfo field = enumType.GetField(enumString);
                    if (field == null)
                        return enumString;

                    object[] attribs = field.GetCustomAttributes(attributeType, false);
                    if (attribs.Length == 0)
                        return enumString;

                    var description = propertyInfo.GetValue(attribs[0], null);
                    return description.ToString();
                };

            return enumToDescrptionFunc;
        }
        #endregion
    }
}
