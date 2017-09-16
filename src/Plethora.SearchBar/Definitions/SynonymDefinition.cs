using System.Collections.Generic;
using System.Linq;

namespace Plethora.SearchBar.Definitions
{
    public abstract class SynonymDefinition : Definition
    {
        private readonly string[] synonyms;

        protected SynonymDefinition(string name, IEnumerable<string> synonyms)
            : base(name)
        {
            this.synonyms = synonyms.ToArray();
        }

        public string[] Synonyms
        {
            get { return this.synonyms; }
        }
    }
}
