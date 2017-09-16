using System.Collections.Generic;

namespace Plethora.SearchBar.Definitions.WellKnown
{
    public class GreaterThanComparisonDefinition : ComparisonDefinition
    {
        public GreaterThanComparisonDefinition(IEnumerable<DataTypeDefinition> dataTypes)
            : base("greaterThan", new[] { ">" }, dataTypes)
        {
        }
    }
}
