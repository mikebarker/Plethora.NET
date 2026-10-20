using System.Collections.Generic;

namespace Plethora.Parsing;

public static partial class Parser
{
    /// <summary>
    /// Creates a parser which matches zero or more occurrences of a specified parser.
    /// </summary>
    /// <remarks>
    /// <see cref="ParserExtensions.AtLeast{T}"/> and <see cref="ParserExtensions.AtMost{T}"/>
    /// can be used to match a specified minimum or maximum number of occurrences.
    /// </remarks>
    public static Parser<T[]> Many<T>(Parser<T> parser) => new(parseState =>
        {
            if (!parseState.IsSuccess)
            {
                return parseState.FailAs<T[]>();
            }

            List<T> results = new();
            var currentState = parseState;
            while (true)
            {
                var nextState = parser.Run(currentState);
                if (nextState.IsSuccess)
                {
                    currentState = nextState;
                    results.Add(nextState.Result);
                }
                else
                {
                    break;
                }
            }

            return ParseState.Success(currentState, currentState.Index, results.ToArray());
        });
}
