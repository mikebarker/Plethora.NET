using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using Plethora.SearchBar.Definitions;

namespace Plethora.SearchBar
{
    public static class RegexBuilder
    {
        internal static RegexOptions Options
        {
            get
            {
                return RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase ;
            }
        }

        public static Regex BuildRegex(IEnumerable<EntityDefinition> entities)
        {
            StringBuilder patternBuilder = new StringBuilder();
            patternBuilder.Append("^");
            patternBuilder.AppendEntities(entities);
            patternBuilder.Append("$");

            string pattern = patternBuilder.ToString();

            Regex regex = new Regex(
                pattern,
                RegexOptions.Compiled | Options);

            return regex;
        }

        private static void AppendEntities(this StringBuilder builder, IEnumerable<EntityDefinition> entities)
        {
            builder.Append("(?<entities>(");
            bool isFirst = true;
            foreach (EntityDefinition entity in entities)
            {
                if (!isFirst)
                    builder.Append("|");
                else
                    isFirst = false;

                builder.AppendEntity(entity);
                builder.Append(@"\s?");
            }
            builder.Append(@"))");
        }

        private static void AppendEntity(this StringBuilder builder, EntityDefinition entity)
        {
            builder.Append(@"(?<entity>(?<entity_");
            builder.Append(entity.Name);
            builder.Append(@">(");

            bool isFirst = true;
            foreach (string synonym in entity.Synonyms)
            {
                if (!isFirst)
                    builder.Append("|");
                else
                    isFirst = false;

                builder.Append(synonym);
            }

            builder.Append(@"))\s?");
            builder.AppendFields(entity.Fields);
            builder.Append(@"\s?)");
        }

        private static void AppendFields(this StringBuilder builder, IEnumerable<FieldDefinition> fields)
        {
            builder.Append("(?<fields>(");
            bool isFirst = true;
            foreach (FieldDefinition field in fields)
            {
                if (!isFirst)
                    builder.Append("|");
                else
                    isFirst = false;

                builder.AppendField(field);
                builder.Append(@"\s?");
            }
            builder.Append(@")+)");
        }

        private static void AppendField(this StringBuilder builder, FieldDefinition field)
        {
            builder.Append(@"(?<field>(?<field_");
            builder.Append(field.Name);
            builder.Append(@">(");

            bool isFirst = true;
            foreach (string synonym in field.Synonyms)
            {
                if (!isFirst)
                    builder.Append("|");
                else
                    isFirst = false;

                builder.Append(synonym);
            }

            builder.Append(@"))");
            builder.AppendComparisons(field.Comparisons);
            builder.Append(@")");
        }

        private static void AppendComparisons(this StringBuilder builder, IEnumerable<ComparisonDefinition> comparisons)
        {
            builder.Append("(?<comparisons>(");
            bool isFirst = true;
            foreach (ComparisonDefinition comparison in comparisons)
            {
                if (!isFirst)
                    builder.Append("|");
                else
                    isFirst = false;

                builder.AppendComparison(comparison);
                builder.Append(@"\s?");
            }
            builder.Append(@")+)");
        }

        private static void AppendComparison(this StringBuilder builder, ComparisonDefinition comparison)
        {
            builder.Append(@"\s?(?<comparison>(?<comparison_");
            builder.Append(comparison.Name);
            builder.Append(@">(");

            bool isFirst = true;
            foreach (string synonym in comparison.Synonyms)
            {
                if (!isFirst)
                    builder.Append("|");
                else
                    isFirst = false;

                builder.Append(synonym);
            }

            builder.Append(@"))\s?");
            builder.AppendDataTypes(comparison.DataTypes);
            builder.Append(@")\s?");
        }

        private static void AppendDataTypes(this StringBuilder builder, IEnumerable<DataTypeDefinition> dataTypes)
        {
            builder.Append(@"(?<dataTypes>(");
            bool isFirst = true;
            foreach (DataTypeDefinition dataType in dataTypes)
            {
                if (!isFirst)
                    builder.Append("|");
                else
                    isFirst = false;

                builder.AppendDataType(dataType);
            }
            builder.Append(@"))");
        }

        private static void AppendDataType(this StringBuilder builder, DataTypeDefinition dataType)
        {
            builder.Append(@"(?<dataType>(?<dataType_");
            builder.Append(dataType.Name);
            builder.Append(@">");
            builder.Append(dataType.RegexPattern);
            builder.Append(@"))");
        }
    }
}
