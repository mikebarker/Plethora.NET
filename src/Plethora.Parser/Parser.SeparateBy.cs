using System.Collections.Generic;

namespace Plethora.Parsing;

public static partial class Parser
{
    /// <summary>
    /// Creates a parser factory which matches a sequence of values separated by a specified separator.
    /// </summary>
    /// <param name="separatorParser">The parser used to match the separator.</param>
    public static SeparateByParser SeparateBy(IParser separatorParser) => new(separatorParser);

    public class SeparateByParser
    {
        private readonly IParser separatorParser;

        internal SeparateByParser(IParser separatorParser)
        {
            this.separatorParser = separatorParser;
        }

        /// <summary>
        /// Creates a parser which matches a sequence of values separated by a specified separator.
        /// </summary>
        /// <param name="valueParser">The parser used to match each element.</param>
        public Parser<T[]> Value<T>(Parser<T> valueParser) => new(parseState =>
            {
                if (!parseState.IsSuccess)
                {
                    return parseState.FailAs<T[]>();
                }

                List<T> results = new();
                var currentState = parseState;
                while (true)
                {
                    var valueState = valueParser.Run(currentState);
                    if (!valueState.IsSuccess)
                    {
                        break;
                    }

                    results.Add(valueState.Result);
                    currentState = valueState;

                    var separatorState = separatorParser.Run(currentState);
                    if (!separatorState.IsSuccess)
                    {
                        break;
                    }

                    currentState = separatorState;
                }

                return ParseState.Success(currentState, currentState.Index, results.ToArray());
            });
    }
}
