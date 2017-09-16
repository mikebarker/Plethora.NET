using System.Collections.Generic;
using System.Globalization;

namespace Plethora.SearchBar.Definitions.WellKnown
{
    public class FloatingPointFieldDefinition : ComparableFieldDefinition
    {
        public FloatingPointFieldDefinition(string name, IEnumerable<string> synonyms)
            : base(name, synonyms, new FloatingPointDataTypeDefinition())
        {
        }

        public FloatingPointFieldDefinition(string name, IEnumerable<string> synonyms, CultureInfo cultureInfo)
            : base(name, synonyms, new FloatingPointDataTypeDefinition(cultureInfo))
        {
        }
    }
}
