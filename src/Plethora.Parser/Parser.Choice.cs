using System.Collections.Generic;

namespace Plethora.Parsing;

public static partial class Parser
{
    /// <summary>
    /// Creates a parser which matches the first parser in the specified collection which succeeds.
    /// </summary>
    public static Parser<object?> Choice(params IParser[] parsers) => Choice((IEnumerable<IParser>)parsers);

    /// <summary>
    /// Creates a parser which matches the first parser in the specified collection which succeeds.
    /// </summary>
    public static Parser<object?> Choice(IEnumerable<IParser> parsers) => new(parseState =>
        {
            if (!parseState.IsSuccess)
            {
                return parseState.FailAs<object?>();
            }

            foreach (var parser in parsers)
            {
                var currentState = parser.Run(parseState);
                if (currentState.IsSuccess)
                {
                    return ParseState.Success(currentState, currentState.Index, currentState.GetResult());
                }
            }

            return ParseState.Failure<object?>(parseState, new($"Couldn't match any parser at index {parseState.Index}"));
        });

    /// <summary>
    /// Creates a parser which matches the first parser in the specified collection which succeeds.
    /// </summary>
    public static Parser<T> Choice<T>(params Parser<T>[] parsers) => Choice((IEnumerable<Parser<T>>)parsers);

    /// <summary>
    /// Creates a parser which matches the first parser in the specified collection which succeeds.
    /// </summary>
    public static Parser<T> Choice<T>(IEnumerable<Parser<T>> parsers) => new(parseState =>
        {
            if (!parseState.IsSuccess)
            {
                return parseState.FailAs<T>();
            }

            foreach (var parser in parsers)
            {
                var currentState = parser.Run(parseState);
                if (currentState.IsSuccess)
                {
                    return currentState;
                }
            }

            return ParseState.Failure<T>(parseState, new($"Couldn't match any parser at index {parseState.Index}"));
        });
}
