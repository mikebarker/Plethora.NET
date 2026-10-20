using System;

namespace Plethora.Parsing;

public static partial class Parser
{
    /// <summary>
    /// Creates a parser which matches up to the point where the inner parser succeeds.
    /// </summary>
    public static Parser<Range> Upto(IParser innerParser) => new(parseState =>
        {
            if (!parseState.IsSuccess)
            {
                return parseState.FailAs<Range>();
            }

            for(int i = parseState.Index; i < parseState.TargetString.Length; i++)
            {
                var nextParseState = ParseState.Success(parseState, i, parseState.GetResult());
                var resultState = innerParser.Run(nextParseState);
                if (resultState.IsSuccess)
                {
                    Range range = parseState.Index..i;
                    return ParseState.Success(parseState, i, range);
                }
            }

            Range fullRange = parseState.Index..;
            return ParseState.Success(parseState, parseState.TargetString.Length, fullRange);
        });
}
