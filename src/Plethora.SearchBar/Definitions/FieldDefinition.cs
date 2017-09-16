using System.Collections.Generic;
using System.Linq;

namespace Plethora.SearchBar.Definitions
{
    public class FieldDefinition : SynonymDefinition 
    {
        private readonly ComparisonDefinition[] comparisons;

        public FieldDefinition(string name, IEnumerable<string> synonyms, IEnumerable<ComparisonDefinition> comparisons)
            : base(name, synonyms)
        {
            this.comparisons = comparisons.ToArray();
        }

        public ComparisonDefinition[] Comparisons => this.comparisons;
    }
}
