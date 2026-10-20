using System;

namespace Plethora.Parsing;

public static partial class Parser
{
    /// <summary>
    /// Creates a parser which matches any input that does not contain the specified string.
    /// </summary>
    public static Parser<Range> NotStr(string s, StringComparison comparisonType = StringComparison.Ordinal) => new(parseState =>
        {
            if (!parseState.IsSuccess)
            {
                return parseState.FailAs<Range>();
            }

            var span = parseState.RemainingInput;
            var index = span.IndexOf(s, comparisonType);
            if (index == -1)
            {
                Range range = parseState.Index..;
                return ParseState.Success(parseState, parseState.TargetString.Length, range);
            }
            else
            {
                Range range = parseState.Index..(parseState.Index + index);
                return ParseState.Success(parseState, parseState.Index + index, range);
            }
        });
}
