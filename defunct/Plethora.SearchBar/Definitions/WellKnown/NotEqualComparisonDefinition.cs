using System.Collections.Generic;

namespace Plethora.SearchBar.Definitions.WellKnown
{
    public class NotEqualComparisonDefinition : ComparisonDefinition
    {
        public NotEqualComparisonDefinition(IEnumerable<DataTypeDefinition> dataTypes)
            : base("notEqual", new[] { "!=", "<>" }, dataTypes)
        {
        }
    }
}
