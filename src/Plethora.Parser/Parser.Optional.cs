namespace Plethora.Parsing;

public static partial class Parser
{
    /// <summary>
    /// Creates a parser which optionally matches a value.
    /// </summary>
    public static Parser<T?> Optional<T>(Parser<T> parser) => new(parseState =>
        {
            if (!parseState.IsSuccess)
            {
                return parseState.FailAs<T?>();
            }

            var resultState = parser.Run(parseState);
            if (resultState.IsSuccess)
            {
                return ParseState.Success(parseState, resultState.Index, (T?)resultState.Result);
            }

            return ParseState.Success(parseState, parseState.Index, (T?)default);
        });
}
