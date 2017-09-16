using System.Collections.Generic;

namespace Plethora.SearchBar.Definitions.WellKnown
{
    public class LessThanEqualComparisonDefinition : ComparisonDefinition
    {
        public LessThanEqualComparisonDefinition(IEnumerable<DataTypeDefinition> dataTypes)
            : base("lessThanEqual", new[] { "<=" }, dataTypes)
        {
        }
    }
}
