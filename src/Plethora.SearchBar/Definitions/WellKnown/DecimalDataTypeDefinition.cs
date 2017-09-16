using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Plethora.SearchBar.Definitions.WellKnown
{
    public class DecimalDataTypeDefinition : DataTypeDefinition
    {
        public DecimalDataTypeDefinition()
            : this(CultureInfo.CurrentCulture)
        {
        }

        public DecimalDataTypeDefinition(CultureInfo cultureInfo)
            : base("decimal", ConstructRegexPattern(cultureInfo), GetParseDecimal(cultureInfo))
        {
        }

        private static string ConstructRegexPattern(CultureInfo cultureInfo)
        {
            string decimalSeparator = cultureInfo.NumberFormat.NumberDecimalSeparator;
            string groupSeparator = cultureInfo.NumberFormat.NumberGroupSeparator;

            string escapedDecimalSeparator = Regex.Escape(decimalSeparator);
            string escapedGroupSeparator = Regex.Escape(groupSeparator);

            string pattern = string.Format(@"[-+]?(\d|{0})*({1})?\d+",
                escapedGroupSeparator,
                escapedDecimalSeparator);

            return pattern;
        }

        private static TryParseFunction GetParseDecimal(IFormatProvider formatProvider)
        {
            return delegate(string text, out object value)
            {
                decimal decimalValue;
                bool result = decimal.TryParse(text, NumberStyles.Number, formatProvider, out decimalValue);
                value = decimalValue;
                return result;
            };
        }
    }
}
