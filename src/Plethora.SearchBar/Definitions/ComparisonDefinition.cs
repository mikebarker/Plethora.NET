using System.Collections.Generic;
using System.Linq;

namespace Plethora.SearchBar.Definitions
{
    public class ComparisonDefinition : SynonymDefinition   
    {
        private readonly DataTypeDefinition[] dataTypes;

        public ComparisonDefinition(string name, IEnumerable<string> synonyms, IEnumerable<DataTypeDefinition> dataTypes)
            : base(name, synonyms)
        {
            this.dataTypes = dataTypes.ToArray();
        }

        public DataTypeDefinition[] DataTypes => this.dataTypes;
    }
}
