namespace Plethora.SearchBar.Definitions
{
    public delegate bool TryParseFunction(string text, out object value);


    public class DataTypeDefinition : Definition
    {
        private readonly string regexPattern;
        private readonly TryParseFunction tryParseFunction;

        public DataTypeDefinition(string name, string regexPattern, TryParseFunction tryParseFunction)
            : base(name)
        {
            this.regexPattern = regexPattern;
            this.tryParseFunction = tryParseFunction;
        }

        public string RegexPattern => this.regexPattern;

        public bool TryParse(string text, out object value)
        {
            bool result = this.tryParseFunction(text, out value);
            return result;
        }
    }
}
