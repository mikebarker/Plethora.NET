using System.Collections.Generic;
using System.Linq;

using Plethora.SearchBar.Definitions;

namespace Plethora.SearchBar.ParseTree
{
    public class ComparisonNode : Node
    {
        internal ComparisonNode(ComparisonDefinition definition, string text, IEnumerable<ValueNode> values)
            : base(definition, text)
        {
            this.Values = values.ToArray();
        }

        public new ComparisonDefinition Definition
        {
            get { return (ComparisonDefinition)base.Definition; }
        }

        public ValueNode[] Values { get; }
    }
}
