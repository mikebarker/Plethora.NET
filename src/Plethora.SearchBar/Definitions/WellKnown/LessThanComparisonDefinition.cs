using System.Collections.Generic;

namespace Plethora.SearchBar.Definitions.WellKnown
{
    public class LessThanComparisonDefinition : ComparisonDefinition
    {
        public LessThanComparisonDefinition(IEnumerable<DataTypeDefinition> dataTypes)
            : base("lessThan", new[] { "<" }, dataTypes)
        {
        }
    }
}
