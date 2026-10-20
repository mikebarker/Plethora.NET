using System;

namespace Plethora.Parsing;

public static partial class Parser
{
    /// <summary>
    /// Creates a parser which matches a sequence of characters from the specified set.
    /// </summary>
    public static Parser<Range> Chars(char[] chars) => new(parseState =>
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

            var length = span.StartsWithAnyCount(chars);
            if (length == 0)
            {
                return ParseState.Failure<Range>(parseState, new($"Couldn't match chars at {parseState.Index}"));
            }

            Range range = new(parseState.Index, parseState.Index + length);
            return ParseState.Success(parseState, parseState.Index + length, range);
        });
}
