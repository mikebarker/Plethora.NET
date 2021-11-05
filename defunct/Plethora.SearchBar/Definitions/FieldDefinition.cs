using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace Plethora.SearchBar.Definitions
{
    public class FieldDefinition : SynonymDefinition 
    {
        private readonly ComparisonDefinition[] comparisons;

        public FieldDefinition([NotNull] string name, [NotNull] IEnumerable<string> synonyms, [NotNull] IEnumerable<ComparisonDefinition> comparisons)
            : base(name, synonyms)
        {
            if (comparisons == null)
                throw new ArgumentNullException(nameof(comparisons));


            this.comparisons = comparisons.ToArray();
        }

        [NotNull]
        public ComparisonDefinition[] Comparisons
        {
            get { return this.comparisons; }
        }
    }
}
