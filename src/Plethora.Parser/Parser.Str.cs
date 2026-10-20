using System;

namespace Plethora.Parsing;

public static partial class Parser
{
    /// <summary>
    /// Creates a parser which matches the specified string.
    /// </summary>
    public static Parser<Range> Str(string s, StringComparison comparisonType = StringComparison.Ordinal) => new(parseState =>
        {
            if (!parseState.IsSuccess)
            {
                return parseState.FailAs<Range>();
            }

            var span = parseState.RemainingInput;
            if (span.Length == 0)
            {
                return ParseState.Failure<Range>(parseState, new($"Expected {s} but received end of string"));
            }

            if (span.StartsWith(s, comparisonType))
            {
                Range range = new(parseState.Index, parseState.Index + s.Length);
                return ParseState.Success(parseState, parseState.Index + s.Length, range);
            }

            return ParseState.Failure<Range>(parseState, new($"Expected {s} but received {span[..Math.Min(10, span.Length)]}"));
        });
}
