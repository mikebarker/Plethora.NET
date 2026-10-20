namespace Plethora.Parsing;

public static partial class Parser
{
    /// <summary>
    /// Creates a parser which matches the end of input.
    /// </summary>
    public static Parser<object?> EndOfInput => new(parseState =>
        {
            if (!parseState.IsSuccess)
            {
                return parseState.FailAs<object?>();
            }

            if (parseState.Index >= parseState.TargetString.Length)
            {
                return ParseState.Success(parseState, parseState.Index, (object?)null);
            }
        
            return ParseState.Failure<object?>(parseState, new($"Expected end of input."));
        });
}
