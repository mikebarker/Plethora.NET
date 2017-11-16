using System.Collections.Generic;

namespace Plethora.SearchBar.Definitions.WellKnown
{
    public abstract class ComparableFieldDefinition : FieldDefinition
    {
        protected ComparableFieldDefinition(string name, IEnumerable<string> synonyms, DataTypeDefinition comparableDataTypeDefinition)
            : base(name, synonyms, GetComparisons(new [] { comparableDataTypeDefinition }))
        {
        }

        protected ComparableFieldDefinition(string name, IEnumerable<string> synonyms, IEnumerable<DataTypeDefinition> comparableDataTypeDefinitions)
            : base(name, synonyms, GetComparisons(comparableDataTypeDefinitions))
        {
        }

        private static IEnumerable<ComparisonDefinition> GetComparisons(IEnumerable<DataTypeDefinition> comparableDataTypeDefinitions)
        {
            ComparisonDefinition[] numericComparisons = new ComparisonDefinition[]
            {
                new EqualComparisonDefinition(comparableDataTypeDefinitions),
                new NotEqualComparisonDefinition(comparableDataTypeDefinitions),
                new GreaterThanComparisonDefinition(comparableDataTypeDefinitions),
                new GreaterThanEqualComparisonDefinition(comparableDataTypeDefinitions),
                new LessThanComparisonDefinition(comparableDataTypeDefinitions),
                new LessThanEqualComparisonDefinition(comparableDataTypeDefinitions),
            };

            return numericComparisons;
        }
    }
}
