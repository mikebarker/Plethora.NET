using System.Collections.Generic;

namespace Plethora.SearchBar.Definitions.WellKnown
{
    public class GreaterThanEqualComparisonDefinition : ComparisonDefinition
    {
        public GreaterThanEqualComparisonDefinition(IEnumerable<DataTypeDefinition> dataTypes)
            : base("greaterThanEqual", new[] { ">=" }, dataTypes)
        {
        }
    }
}
