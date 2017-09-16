using System.Collections.Generic;
using System.Linq;

namespace Plethora.SearchBar.Definitions
{
    public class EntityDefinition : SynonymDefinition
    {
        private readonly FieldDefinition[] fields;

        public EntityDefinition(string name, IEnumerable<string> synonyms, IEnumerable<FieldDefinition> fields)
            : base(name, synonyms)
        {
            this.fields = fields.ToArray();
        }

        public FieldDefinition[] Fields => this.fields;
    }
}
