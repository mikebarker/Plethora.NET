using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Plethora.SearchBar.Definitions.WellKnown
{
    public class IntegerDataTypeDefinition : DataTypeDefinition
    {
        public IntegerDataTypeDefinition()
            : this(CultureInfo.CurrentCulture)
        {
        }

        public IntegerDataTypeDefinition(CultureInfo cultureInfo)
            : base("integer", ConstructRegexPattern(cultureInfo), GetParseLong(cultureInfo))
        {
        }

        private static string ConstructRegexPattern(CultureInfo cultureInfo)
        {
            string groupSeparator = cultureInfo.NumberFormat.NumberGroupSeparator;

            string escapedGroupSeparator = Regex.Escape(groupSeparator);

            string pattern = string.Format(@"[-+]?(\d|{0})+",
                escapedGroupSeparator);

            return pattern;
        }

        private static TryParseFunction GetParseLong(IFormatProvider formatProvider)
        {
            return delegate (string text, out object value)
            {
                long longValue;
                bool result = long.TryParse(text, NumberStyles.Integer, formatProvider, out longValue);
                value = longValue;
                return result;
            };
        }
    }
}
