using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace Plethora.SearchBar.Definitions
{
    public abstract class SynonymDefinition : Definition
    {
        private readonly string[] synonyms;

        protected SynonymDefinition([NotNull] string name, [NotNull] IEnumerable<string> synonyms)
            : base(name)
        {
            if (synonyms == null)
                throw new ArgumentNullException(nameof(synonyms));


            this.synonyms = synonyms.ToArray();
        }

        [NotNull]
        public string[] Synonyms
        {
            get { return this.synonyms; }
        }
    }
}
