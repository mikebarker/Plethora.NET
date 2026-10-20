namespace Plethora.Parsing;

public static partial class Parser
{
    /// <summary>
    /// Creates a parser which matches a specified sequence.
    /// </summary>
    public static Parser<object?[]> SequenceOf(params IParser[] parsers) => new(parseState =>
        {
            if (!parseState.IsSuccess)
            {
                return parseState.FailAs<object?[]>();
            }

            var results = new object?[parsers.Length];
            var currentState = parseState;

            int i = 0;
            foreach(var parser in parsers)
            {
                currentState = parser.Run(currentState);

                if (!currentState.IsSuccess)
                {
                    return currentState.FailAs<object?[]>();
                }

                results[i] = currentState.GetResult();
                i++;
            }

            return ParseState.Success(currentState, currentState.Index, results);
        });
}
