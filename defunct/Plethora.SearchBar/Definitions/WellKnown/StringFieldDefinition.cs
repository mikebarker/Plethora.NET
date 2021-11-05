using System.Collections.Generic;

namespace Plethora.SearchBar.Definitions.WellKnown
{
    public class StringFieldDefinition : ComparableFieldDefinition
    {
        public StringFieldDefinition(string name)
            : base(name, new string[0], new StringDataTypeDefinition())
        {
        }

        public StringFieldDefinition(string name, IEnumerable<string> synonyms)
            : base(name, synonyms, new StringDataTypeDefinition())
        {
        }
    }
}
