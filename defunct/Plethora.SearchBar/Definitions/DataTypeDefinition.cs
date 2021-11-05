using System;

using JetBrains.Annotations;

namespace Plethora.SearchBar.Definitions
{
    public delegate bool TryParseFunction([NotNull] string text, [CanBeNull] out object value);


    public class DataTypeDefinition : Definition
    {
        private readonly string regexPattern;
        private readonly TryParseFunction tryParseFunction;

        public DataTypeDefinition([NotNull] string name, [NotNull] string regexPattern, [NotNull] TryParseFunction tryParseFunction)
            : base(name)
        {
            if (regexPattern == null)
                throw new ArgumentNullException(nameof(regexPattern));

            if (tryParseFunction == null)
                throw new ArgumentNullException(nameof(tryParseFunction));


            this.regexPattern = regexPattern;
            this.tryParseFunction = tryParseFunction;
        }

        [NotNull]
        public string RegexPattern
        {
            get { return this.regexPattern; }
        }

        public bool TryParse([NotNull] string text, [CanBeNull] out object value)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));


            bool result = this.tryParseFunction(text, out value);
            return result;
        }
    }
}
