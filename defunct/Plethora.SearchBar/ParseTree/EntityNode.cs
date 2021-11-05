using System.Collections.Generic;
using System.Linq;

using Plethora.SearchBar.Definitions;

namespace Plethora.SearchBar.ParseTree
{
    public class EntityNode : Node
    {
        internal EntityNode(EntityDefinition definition, string text, IEnumerable<FieldNode> fields)
            : base(definition, text)
        {
            this.Fields = fields.ToArray();
        }

        public new EntityDefinition Definition
        {
            get { return (EntityDefinition)base.Definition; }
        }

        public FieldNode[] Fields { get; }
    }
}
