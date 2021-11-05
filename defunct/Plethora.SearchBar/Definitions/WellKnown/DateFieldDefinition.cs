using System.Collections.Generic;
using System.Globalization;

namespace Plethora.SearchBar.Definitions.WellKnown
{
    public class DateFieldDefinition : ComparableFieldDefinition
    {
        public DateFieldDefinition(string name)
            : base(name, new string[0], new DataTypeDefinition[] { new DateDataTypeDefinition(), new DateRangeDataTypeDefinition()} )
        {
        }

        public DateFieldDefinition(string name, IEnumerable<string> synonyms)
            : base(name, synonyms, new DataTypeDefinition[] { new DateDataTypeDefinition(), new DateRangeDataTypeDefinition() })
        {
        }

        public DateFieldDefinition(string name, IEnumerable<string> synonyms, CultureInfo cultureInfo)
            : base(name, synonyms, new DataTypeDefinition[] { new DateDataTypeDefinition(cultureInfo), new DateRangeDataTypeDefinition(cultureInfo) })
        {
        }
    }
}
