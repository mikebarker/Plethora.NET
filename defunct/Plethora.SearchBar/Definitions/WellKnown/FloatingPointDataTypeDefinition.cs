using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Plethora.SearchBar.Definitions.WellKnown
{
    public class FloatingPointDataTypeDefinition : DataTypeDefinition
    {
        public FloatingPointDataTypeDefinition()
            : this(CultureInfo.CurrentCulture)
        {
        }

        public FloatingPointDataTypeDefinition(CultureInfo cultureInfo)
            : base("floatingPoint", ConstructRegexPattern(cultureInfo), GetParseDouble(cultureInfo))
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

        private static TryParseFunction GetParseDouble(IFormatProvider formatProvider)
        {
            return delegate(string text, out object value)
            {
                double doubleValue;
                bool result = double.TryParse(text, NumberStyles.Float | NumberStyles.AllowThousands, formatProvider, out doubleValue);
                value = doubleValue;
                return result;
            };
        }
    }
}
