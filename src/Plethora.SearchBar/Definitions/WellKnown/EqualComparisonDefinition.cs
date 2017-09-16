using System.Collections.Generic;

namespace Plethora.SearchBar.Definitions.WellKnown
{
    public class EqualComparisonDefinition : ComparisonDefinition
    {
        public EqualComparisonDefinition(IEnumerable<DataTypeDefinition> dataTypes)
            : base("equal", new[] { "", "=", "==" }, dataTypes)
        {
        }
    }
}
