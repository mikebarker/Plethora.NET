namespace Plethora.Parsing;

public static partial class Parser
{
    /// <summary>
    /// Creates a parser factory which matches a value between two specified parsers.
    /// </summary>
    /// <param name="leftParser">The parser used to match the left value.</param>
    /// <param name="rightParser">The parser used to match the right value.</param>
    public static BetweenParser Between(IParser leftParser, IParser rightParser) => new(leftParser, rightParser);

    public class BetweenParser
    {
        private readonly IParser leftParser;
        private readonly IParser rightParser;

        internal BetweenParser(IParser leftParser, IParser rightParser)
        {
            this.leftParser = leftParser;
            this.rightParser = rightParser;
        }

        /// <summary>
        /// Creates a new parser which matches a value between the left and right parsers.
        /// </summary>
        public Parser<T> Content<T>(Parser<T> contentParser) => SequenceOf(leftParser, contentParser, rightParser).Map(results => (T)results[1]!);
    }
}
