using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace Plethora.SearchBar.Definitions
{
    public class ComparisonDefinition : SynonymDefinition
    {
        private readonly DataTypeDefinition[] dataTypes;

        public ComparisonDefinition([NotNull] string name, [NotNull] IEnumerable<string> synonyms, [NotNull] IEnumerable<DataTypeDefinition> dataTypes)
            : base(name, synonyms)
        {
            if (dataTypes == null)
                throw new ArgumentNullException(nameof(dataTypes));


            this.dataTypes = dataTypes.ToArray();
        }

        [NotNull]
        public DataTypeDefinition[] DataTypes
        {
            get { return this.dataTypes; }
        }
    }
}
