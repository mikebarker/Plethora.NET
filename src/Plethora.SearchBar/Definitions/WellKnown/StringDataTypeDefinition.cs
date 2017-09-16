namespace Plethora.SearchBar.Definitions.WellKnown
{
    public class StringDataTypeDefinition : DataTypeDefinition
    {
        public StringDataTypeDefinition()
            : base("string", ConstructRegexPattern(), ParseStringFunction)
        {
        }

        private static string ConstructRegexPattern()
        {
            string pattern = @"(""([^""]|"""")*""|\S+)";

            return pattern;
        }

        private static TryParseFunction ParseStringFunction
        {
            get
            {
                return delegate (string text, out object value)
                {
                    value = text;
                    return true;
                };
            }
        }
    }
}
