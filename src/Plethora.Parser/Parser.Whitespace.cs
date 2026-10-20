using System;

namespace Plethora.Parsing;

public static partial class Parser
{
    /// <summary>
    /// Creates a parser which matches one or more leading whitespace characters.
    /// </summary>
    public static Parser<Range> Whitespace => new(parseState =>
        {
            if (!parseState.IsSuccess)
            {
                return parseState.FailAs<Range>();
            }

            var span = parseState.RemainingInput;
            if (span.Length == 0)
            {
                return ParseState.Failure<Range>(parseState, new($"Received end of string"));
            }

            var length = span.StartsWithWhitespaceCount();
            if (length == 0)
            {
                return ParseState.Failure<Range>(parseState, new($"Couldn't match whitespace at {parseState.Index}"));
            }

            Range range = new(parseState.Index, parseState.Index + length);
            return ParseState.Success(parseState, parseState.Index + length, range);
        });
}
