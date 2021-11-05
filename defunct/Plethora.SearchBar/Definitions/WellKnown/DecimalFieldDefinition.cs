using System.Collections.Generic;
using System.Globalization;

namespace Plethora.SearchBar.Definitions.WellKnown
{
    public class DecimalFieldDefinition : ComparableFieldDefinition
    {
        public DecimalFieldDefinition(string name)
            : base(name, new string[0], new DecimalDataTypeDefinition())
        {
        }

        public DecimalFieldDefinition(string name, IEnumerable<string> synonyms)
            : base(name, synonyms, new DecimalDataTypeDefinition())
        {
        }

        public DecimalFieldDefinition(string name, IEnumerable<string> synonyms, CultureInfo cultureInfo)
            : base(name, synonyms, new DecimalDataTypeDefinition(cultureInfo))
        {
        }
    }
}
