using System;

namespace Plethora.Parsing;

public interface IParser
{
    ParseState Run(ParseState parseState);
}

public class Parser<T> : IParser
{
    private readonly Func<ParseState, ParseState<T>> parseFunction;

    public Parser(Func<ParseState, ParseState<T>> parseFunction)
    {
        this.parseFunction = parseFunction;
    }

    public ParseState<T> Run(string input)
    {
        return this.Run(ParseState.Initial(input));
    }

    ParseState IParser.Run(ParseState parseState)
    {
        return this.parseFunction(parseState);
    }

    public ParseState<T> Run(ParseState parseState)
    {
        return this.parseFunction(parseState);
    }
}