using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Plethora.SearchBar.Definitions;
using Plethora.SearchBar.ParseTree;

namespace Plethora.SearchBar
{
    public class SearchBarParser
    {
        private const string ENTITY_GROUP_NAME_PREFIX = "entity_";
        private const string FIELD_GROUP_NAME_PREFIX = "field_";
        private const string COMPARISON_GROUP_NAME_PREFIX = "comparison_";
        private const string DATA_TYPE_GROUP_NAME_PREFIX = "dataType_";

        private readonly IEnumerable<EntityDefinition> entities;
        private Regex parseRegex;

        public SearchBarParser(IEnumerable<EntityDefinition> entities)
        {
            List<EntityDefinition> list = entities.ToList();
            list.TrimExcess();
            this.entities = list;
        }

        public IEnumerable<EntityNode> Parse(string text)
        {
            text = PreFilter.Prepare(text);

            Match match = this.ParseRegex.Match(text);

            if (!match.Success)
            {
                return new List<EntityNode>();
            }

            Group entityGroup;
            Dictionary<Capture, string> entityCaptureToNameMap;

            Group fieldGroup;
            Dictionary<Capture, string> fieldCaptureToNameMap;

            Group comparisonGroup;
            Dictionary<Capture, string> comparisonCaptureToNameMap;

            Group dataTypeGroup;
            Dictionary<Capture, string> dataTypeCaptureToNameMap;

            IsolateGroups(
                parseRegex, match,
                out entityGroup,
                out entityCaptureToNameMap,
                out fieldGroup,
                out fieldCaptureToNameMap,
                out comparisonGroup,
                out comparisonCaptureToNameMap,
                out dataTypeGroup,
                out dataTypeCaptureToNameMap);

            List<EntityNode> entityNodes = new List<EntityNode>();
            foreach (Capture entityCapture in entityGroup.Captures)
            {
                Capture entityIdCapture = entityCaptureToNameMap.Keys
                    .AllIn(entityCapture)
                    .First();

                string entityGroupName = entityCaptureToNameMap[entityIdCapture];
                string entityId = entityGroupName.Substring(ENTITY_GROUP_NAME_PREFIX.Length);

                EntityDefinition entityDefinition = entities.First(e => e.Name == entityId);

                IEnumerable<Capture> fieldCaptures = fieldGroup.Captures
                    .AllIn(entityCapture);

                List<FieldNode> fieldNodes = new List<FieldNode>();
                foreach (Capture fieldCapture in fieldCaptures)
                {
                    Capture fieldIdCapture = fieldCaptureToNameMap.Keys
                        .AllIn(fieldCapture)
                        .First();

                    string fieldGroupName = fieldCaptureToNameMap[fieldIdCapture];
                    string fieldId = fieldGroupName.Substring(FIELD_GROUP_NAME_PREFIX.Length);

                    FieldDefinition fieldDefinition = entityDefinition.Fields.First(f => f.Name == fieldId);

                    IEnumerable<Capture> comparisonCaptures = comparisonGroup.Captures
                        .AllIn(fieldCapture);

                    List<ComparisonNode> comparisonNodes = new List<ComparisonNode>();
                    foreach (Capture comparisonCapture in comparisonCaptures)
                    {
                        Capture comparisonIdCapture = comparisonCaptureToNameMap.Keys
                            .AllIn(comparisonCapture)
                            .First();

                        string comparisonGroupName = comparisonCaptureToNameMap[comparisonIdCapture];
                        string comparisonId = comparisonGroupName.Substring(COMPARISON_GROUP_NAME_PREFIX.Length);

                        ComparisonDefinition comparisonDefinition = fieldDefinition.Comparisons.First(c => c.Name == comparisonId);

                        IEnumerable<Capture> dataTypeCaptures = dataTypeGroup.Captures
                            .AllIn(comparisonCapture);

                        List<ValueNode> valueNodes = new List<ValueNode>();
                        foreach (Capture dataTypeCapture in dataTypeCaptures)
                        {
                            Capture dataTypeIdCapture = dataTypeCaptureToNameMap.Keys
                                .AllIn(dataTypeCapture)
                                .First();

                            string dataTypeGroupName = dataTypeCaptureToNameMap[dataTypeIdCapture];
                            string dataTypeId = dataTypeGroupName.Substring(DATA_TYPE_GROUP_NAME_PREFIX.Length);

                            DataTypeDefinition dataTypeDefinition = comparisonDefinition.DataTypes.First(d => d.Name == dataTypeId);

                            ValueNode valueNode = new ValueNode(
                                dataTypeDefinition,
                                dataTypeCapture.Value);

                            valueNodes.Add(valueNode);
                        }

                        ComparisonNode comparisonNode = new ComparisonNode(
                            comparisonDefinition,
                            comparisonIdCapture.Value,
                            valueNodes);

                        comparisonNodes.Add(comparisonNode);
                    }

                    FieldNode fieldNode = new FieldNode(
                        fieldDefinition,
                        fieldIdCapture.Value,
                        comparisonNodes);

                    fieldNodes.Add(fieldNode);
                }

                EntityNode entityNode = new EntityNode(
                    entityDefinition,
                    entityIdCapture.Value,
                    fieldNodes);

                entityNodes.Add(entityNode);
            }

            return entityNodes;
        }

        private Regex ParseRegex
        {
            get
            {
                if (this.parseRegex == null)
                {
                    this.parseRegex = RegexBuilder.BuildRegex(entities);
                }
                return this.parseRegex;
            }
        }

        private static void IsolateGroups(
            Regex regex,
            Match match,
            out Group entityGroup,
            out Dictionary<Capture, string> entityCaptureToNameMap,
            out Group fieldGroup,
            out Dictionary<Capture, string> fieldCaptureToNameMap,
            out Group comparisonGroup,
            out Dictionary<Capture, string> comparisonCaptureToNameMap,
            out Group dataTypeGroup,
            out Dictionary<Capture, string> dataTypeCaptureToNameMap)
        {
            GroupCollection groups = match.Groups;

            entityGroup = null;
            entityCaptureToNameMap = new Dictionary<Capture, string>();
            fieldGroup = null;
            fieldCaptureToNameMap = new Dictionary<Capture, string>();
            comparisonGroup = null;
            comparisonCaptureToNameMap = new Dictionary<Capture, string>();
            dataTypeGroup = null;
            dataTypeCaptureToNameMap = new Dictionary<Capture, string>();

            for (var i = 0; i < groups.Count; i++)
            {
                string groupName = regex.GroupNameFromNumber(i);

                if (groupName == "entity")
                {
                    entityGroup = groups[i];
                }
                else if (groupName == "field")
                {
                    fieldGroup = groups[i];
                }
                else if (groupName == "comparison")
                {
                    comparisonGroup = groups[i];
                }
                else if (groupName == "dataType")
                {
                    dataTypeGroup = groups[i];
                }
                else if (groupName.StartsWith(ENTITY_GROUP_NAME_PREFIX))
                {
                    foreach (Capture capture in groups[i].Captures)
                    {
                        entityCaptureToNameMap.Add(capture, groupName);
                    }
                }
                else if (groupName.StartsWith(FIELD_GROUP_NAME_PREFIX))
                {
                    foreach (Capture capture in groups[i].Captures)
                    {
                        fieldCaptureToNameMap.Add(capture, groupName);
                    }
                }
                else if (groupName.StartsWith(COMPARISON_GROUP_NAME_PREFIX))
                {
                    foreach (Capture capture in groups[i].Captures)
                    {
                        comparisonCaptureToNameMap.Add(capture, groupName);
                    }
                }
                else if (groupName.StartsWith(DATA_TYPE_GROUP_NAME_PREFIX))
                {
                    foreach (Capture capture in groups[i].Captures)
                    {
                        dataTypeCaptureToNameMap.Add(capture, groupName);
                    }
                }
            }
        }
    }

    internal static class CaptureHelper
    {
        public static IEnumerable<Capture> AllIn(this CaptureCollection collection, Capture outerCapture)
        {
            return collection.OfType<Capture>()
                .AllIn(outerCapture);
        }

        public static IEnumerable<Capture> AllIn(this IEnumerable<Capture> captures, Capture outerCapture)
        {
            return captures
                .Where(capture => capture.IsIn(outerCapture));
        }

        public static bool IsIn(this Capture inner, Capture outer)
        {
            return
                (inner.Index >= outer.Index) &&
                (inner.Index + inner.Length) <= (outer.Index + outer.Length);
        }
    }
}
