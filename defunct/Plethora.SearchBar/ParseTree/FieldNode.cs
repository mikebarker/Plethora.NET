using System.Collections.Generic;
using System.Linq;

using Plethora.SearchBar.Definitions;

namespace Plethora.SearchBar.ParseTree
{
    public class FieldNode : Node
    {
        internal FieldNode(FieldDefinition definition, string text, IEnumerable<ComparisonNode> comparisons)
            : base(definition, text)
        {
            this.Comparisons = comparisons.ToArray();
        }

        public new FieldDefinition Definition
        {
            get { return (FieldDefinition)base.Definition; }
        }

        public ComparisonNode[] Comparisons { get; }
    }
}
