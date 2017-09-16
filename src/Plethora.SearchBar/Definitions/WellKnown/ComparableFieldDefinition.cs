using System.Collections.Generic;

namespace Plethora.SearchBar.Definitions.WellKnown
{
    public abstract class ComparableFieldDefinition : FieldDefinition
    {
        protected ComparableFieldDefinition(string name, IEnumerable<string> synonyms, DataTypeDefinition comparableDataTypeDefinition)
            : base(name, synonyms, GetNumericComparisons(comparableDataTypeDefinition))
        {
        }

        private static IEnumerable<ComparisonDefinition> GetNumericComparisons(DataTypeDefinition comparableDataTypeDefinition)
        {
            ComparisonDefinition[] numericComparisons = new ComparisonDefinition[]
            {
                new EqualComparisonDefinition(new[] {comparableDataTypeDefinition}),
                new NotEqualComparisonDefinition(new[] {comparableDataTypeDefinition}),
                new GreaterThanComparisonDefinition(new[] {comparableDataTypeDefinition}),
                new GreaterThanEqualComparisonDefinition(new[] {comparableDataTypeDefinition}),
                new LessThanComparisonDefinition(new[] {comparableDataTypeDefinition}),
                new LessThanEqualComparisonDefinition(new[] {comparableDataTypeDefinition}),
            };

            return numericComparisons;
        }
    }
}
