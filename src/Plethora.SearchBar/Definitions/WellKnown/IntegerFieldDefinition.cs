using System.Collections.Generic;
using System.Globalization;

namespace Plethora.SearchBar.Definitions.WellKnown
{
    public class IntegerFieldDefinition : ComparableFieldDefinition
    {
        public IntegerFieldDefinition(string name, IEnumerable<string> synonyms)
            : base(name, synonyms, new IntegerDataTypeDefinition())
        {
        }

        public IntegerFieldDefinition(string name, IEnumerable<string> synonyms, CultureInfo cultureInfo)
            : base(name, synonyms, new IntegerDataTypeDefinition(cultureInfo))
        {
        }
    }
}
