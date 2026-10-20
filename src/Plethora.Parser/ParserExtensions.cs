using System;

namespace Plethora.Parsing;

public static class ParserExtensions
{
    /// <summary>
    /// Chains parsers by using the first parser result to choose the next parser.
    /// </summary>
    /// <typeparam name="TSource">The type of the result produced by the first parser.</typeparam>
    /// <typeparam name="TTarget">The type of the result produced by the chained parser.</typeparam>
    /// <param name="parser">The initial parser.</param>
    /// <param name="parserFn">A function that maps the first parser result to the next parser.</param>
    /// <returns>A parser that runs the initial parser and then runs the parser returned by <paramref name="parserFn"/>.</returns>
    /// <remarks>
    /// Enables dependent parser composition where the second parser is selected from the result of the first.
    /// </remarks>
    public static Parser<TTarget> Chain<TSource, TTarget>(this Parser<TSource> parser, Func<TSource, Parser<TTarget>> parserFn)
    {
        return ChainState(parser, state => parserFn(state.Result));
    }

    /// <summary>
    /// Chains parsers by using the first parser result to choose the next parser.
    /// </summary>
    /// <typeparam name="TSource">The type of the result produced by the first parser.</typeparam>
    /// <typeparam name="TTarget">The type of the result produced by the chained parser.</typeparam>
    /// <param name="parser">The initial parser.</param>
    /// <param name="parserFn">A function that maps the first parser result to the next parser.</param>
    /// <returns>
    /// A parser that returns the first failure if the initial parser fails; otherwise, returns the result of the parser
    /// produced by <paramref name="parserFn"/>.
    /// </returns>
    /// <remarks>
    /// Enables dependent parser composition where the second parser is selected from the result of the first.
    /// </remarks>
    public static Parser<TTarget> ChainState<TSource, TTarget>(this Parser<TSource> parser, Func<ParseState<TSource>, Parser<TTarget>> parserFn)
    {
        return new(state =>
        {
            var nextState = parser.Run(state);
            if (!nextState.IsSuccess)
            {
                return nextState.FailAs<TTarget>();
            }

            var nextParser = parserFn(nextState);
            return nextParser.Run(nextState);
        });
    }

    /// <summary>
    /// Projects the successful result produced by a parser into a new value by applying a mapping function.
    /// </summary>
    /// <typeparam name="TSource">The type of the value produced by the source parser.</typeparam>
    /// <typeparam name="TTarget">The type of the value produced after mapping.</typeparam>
    /// <param name="parser">The parser that produces the source value.</param>
    /// <param name="mapFunction">The function that converts the source value into the target value.</param>
    /// <returns>
    /// A parser that runs the source parser and returns the mapped result on success.
    /// </returns>
    /// <remarks>The mapping function is invoked only when the source parser succeeds.</remarks>
    public static Parser<TTarget> Map<TSource, TTarget>(this Parser<TSource> parser, Func<TSource, TTarget> mapFunction)
    {
        return MapState(parser, state =>
        {
            var mappedResult = mapFunction(state.Result);
            return ParseState.Success(state, state.Index, mappedResult);
        });
    }

    /// <summary>
    /// Projects the successful <see cref="ParseState{TSource}"/> produced by a parser into a new <see cref="ParseState{TTarget}"/> by applying a mapping function.
    /// </summary>
    /// <typeparam name="TSource">The type of the source parser result.</typeparam>
    /// <typeparam name="TTarget">The type of the mapped parser result.</typeparam>
    /// <param name="parser">The parser that produces the source value.</param>
    /// <param name="mapFunction">A function that converts a successful source parse state to a target parse state.</param>
    /// <returns>
    /// A parser that returns the mapped target parse state when parsing succeeds; otherwise returns a failed target
    /// parse state.
    /// </returns>
    /// <remarks>The mapping function is invoked only when the source parser succeeds.</remarks>
    public static Parser<TTarget> MapState<TSource, TTarget>(this Parser<TSource> parser, Func<ParseState<TSource>, ParseState<TTarget>> mapFunction)
    {
        return new(parseState =>
        {
            var state = parser.Run(parseState);
            if (!state.IsSuccess)
            {
                return state.FailAs<TTarget>();
            }

            var mappedState = mapFunction(state);
            return mappedState;
        });
    }

    /// <summary>
    /// Transforms the parse error produced by a parser when parsing fails.
    /// </summary>
    /// <typeparam name="T">The type of the parsed result.</typeparam>
    /// <param name="parser">The parser whose failure error is transformed.</param>
    /// <param name="errorMapFunction">A function that converts the original parse error into a new parse error.</param>
    /// <returns>A parser that preserves successful results and replaces failure errors with the mapped error.</returns>
    /// <remarks>The mapping function is invoked only when parsing fails.</remarks>
    public static Parser<T> MapError<T>(this Parser<T> parser, Func<ParseError, ParseError> errorMapFunction)
    {
        return new(parseState =>
        {
            var state = parser.Run(parseState);
            if (state.IsSuccess)
            {
                return state;
            }

            var mappedError = errorMapFunction(state.Error!);
            return ParseState.Failure<T>(state, mappedError);
        });
    }

    /// <summary>
    /// Creates a parser that matches one or more occurrences using the specified parser.
    /// </summary>
    /// <typeparam name="T">The type of the parsed elements.</typeparam>
    /// <param name="parser">The parser to apply one or more times.</param>
    /// <returns>
    /// A parser that succeeds only when at least one occurrence is matched and returns the matched results.
    /// </returns>
    public static Parser<T[]> AtLeastOne<T>(this Parser<T[]> parser) => AtLeast(parser, 1);

    /// <summary>
    /// Ensures that the parsed result contains at least the specified number of matches.
    /// </summary>
    /// <typeparam name="T">The type of elements produced by the parser.</typeparam>
    /// <param name="parser">The parser whose array result is validated against a minimum match count.</param>
    /// <param name="n">The minimum number of matches required.</param>
    /// <returns>
    /// A parser that preserves the current parse state when the result length is at least <paramref name="n"/>;
    /// otherwise, a failed parse state with an error at the current index.
    /// </returns>
    public static Parser<T[]> AtLeast<T>(this Parser<T[]> parser, int n) => parser.MapState(state =>
    {
        if (state.Result.Length < n)
        {
            return ParseState.Failure<T[]>(state, new(n == 1
                ? $"Expected at least one match at index {state.Index}"
                : $"Expected at least {n} matches at index {state.Index}"));
        }

        return state;
    });

    /// <summary>
    /// Ensures that the parsed result contains at most the specified number of matches.
    /// </summary>
    /// <typeparam name="T">The type of elements produced by the parser.</typeparam>
    /// <param name="parser">The parser whose array result is validated against a maximum match count.</param>
    /// <param name="n">The maximum number of matches allowed.</param>
    /// <returns>
    /// A parser that preserves the current parse state when the result length is at most <paramref name="n"/>;
    /// otherwise, a failed parse state with an error at the current index.
    /// </returns>
    public static Parser<T[]> AtMost<T>(this Parser<T[]> parser, int n) => parser.MapState(state =>
    {
        if (state.Result.Length > n)
        {
            return ParseState.Failure<T[]>(state, new(n == 1
                ? $"Expected at most one match at index {state.Index}"
                : $"Expected at most {n} matches at index {state.Index}"));
        }
        return state;
    });

    /// <summary>
    /// Ensures that the parsed result contains exactly the specified number of matches.
    /// </summary>
    /// <typeparam name="T">The type of elements produced by the parser.</typeparam>
    /// <param name="parser">The parser whose array result is validated against an exact match count.</param>
    /// <param name="n">The number of matches required.</param>
    /// <returns>
    /// A parser that preserves the current parse state when the result length is exactly <paramref name="n"/>;
    /// otherwise, a failed parse state with an error at the current index.
    /// </returns>
    public static Parser<T[]> Exactly<T>(this Parser<T[]> parser, int n) => parser.MapState(state =>
    {
        if (state.Result.Length != n)
        {
            return ParseState.Failure<T[]>(state, new(n == 1
                ? $"Expected exactly one match at index {state.Index}"
                : $"Expected exactly {n} matches at index {state.Index}"));
        }
        return state;
    });
}