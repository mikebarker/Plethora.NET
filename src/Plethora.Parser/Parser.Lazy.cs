using System;

namespace Plethora.Parsing;

public static partial class Parser
{
    /// <summary>
    /// Creates a parser which lazily evaluates the specified parser factory function.
    /// </summary>
    /// <remarks>
    /// This allows interdependent parsers to be defined.
    /// </remarks>
    public static Parser<T> Lazy<T>(Func<Parser<T>> parserThunk) => new(parserState =>
        {
            var parser = parserThunk();
            return parser.Run(parserState);
        });
}
