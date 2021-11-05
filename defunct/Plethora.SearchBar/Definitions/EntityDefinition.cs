using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace Plethora.SearchBar.Definitions
{
    public class EntityDefinition : SynonymDefinition
    {
        private readonly FieldDefinition[] fields;

        public EntityDefinition([NotNull] string name, [NotNull] IEnumerable<string> synonyms, [NotNull] IEnumerable<FieldDefinition> fields)
            : base(name, synonyms)
        {
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));


            this.fields = fields.ToArray();
        }

        [NotNull]
        public FieldDefinition[] Fields
        {
            get { return this.fields; }
        }
    }
}
