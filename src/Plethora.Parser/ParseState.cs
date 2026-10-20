using Plethora.Convert;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Plethora.Parsing;

/// <summary>
/// Represents the immutable state of a parse operation, including the input text, current position, and success or
/// error outcome.
/// </summary>
public abstract record ParseState
{
    protected ParseState(string targetString, int index, ParseError? error)
    {
        this.TargetString = targetString;
        this.Index = index;
        this.Error = error;
    }

    /// <summary>
    /// Gets the target string.
    /// </summary>
    public string TargetString { get; }

    /// <summary>
    /// Gets the current index in the target string.
    /// </summary>
    public int Index { get; private init; }

    /// <summary>
    /// Gets the parse error, if any.
    /// </summary>
    public ParseError? Error { get; private init; }

    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess => Error is null;

    /// <summary>
    /// Gets the remaining input as a <see cref="ReadOnlySpan{Char}"/>.
    /// </summary>
    public ReadOnlySpan<char> RemainingInput => this.TargetString.AsSpan().Slice(this.Index);

    /// <summary>
    /// Gets the result of the parse state.
    /// </summary>
    /// <returns>The result of the parse state.</returns>
    public abstract object? GetResult();

    /// <summary>
    /// Creates the initial parse state for the specified input string.
    /// </summary>
    /// <param name="targetString">Input string to parse.</param>
    /// <returns>
    /// A <see cref="ParseState"/> instance initialized with the input string, index 0, and no result or error.
    /// </returns>
    public static ParseState Initial(string targetString) => new ParseState<object>(targetString, 0, null, null);

    /// <summary>
    /// Creates a successful parse state at the specified index with the specified result.
    /// </summary>
    /// <remarks>Copies the target string from the provided parse state.</remarks>
    /// <typeparam name="T">The type of the parsed result.</typeparam>
    /// <param name="state">The source parse state that provides the target string.</param>
    /// <param name="index">The position in the target string after the successful parse.</param>
    /// <param name="result">The parsed value.</param>
    /// <returns>A parse state containing the original target string, the specified index, the specified result, and no error.</returns>
    public static ParseState<T> Success<T>(ParseState state, int index, T result) =>
        new(state.TargetString, index, result, null);

    /// <summary>
    /// Creates a failed parse state at the current input position with the specified parse error.
    /// </summary>
    /// <typeparam name="T">The type of the parsed value.</typeparam>
    /// <param name="state">The current parse state containing the target input and index.</param>
    /// <param name="error">The parse error to associate with the failed state.</param>
    /// <returns>A <see cref="ParseState{T}"/> that preserves the input and index from <paramref name="state"/>, sets the value
    /// to the default for <typeparamref name="T"/>, and stores <paramref name="error"/>.</returns>
    public static ParseState<T> Failure<T>(ParseState state, ParseError error) =>
        new(state.TargetString, state.Index, default, error);

    /// <inheritdoc/>
    public override string ToString()
    {
        if (IsSuccess)
        {
            return $"{{ TargetString = {this.TargetString}, Index = {this.Index}, IsSuccess = {this.IsSuccess}, Result = {this.GetResult()} }}";
        }
        else
        {
            return $"{{ TargetString = {this.TargetString}, Index = {this.Index}, IsSuccess = {this.IsSuccess}, Error = {this.Error} }}";
        }
    }
}

/// <summary>
/// Represents the state of a parse operation with a strongly typed result.
/// </summary>
/// <typeparam name="T">The type of the parsed result.</typeparam>
public record ParseState<T> : ParseState
{
    private readonly T? result;

    internal ParseState(string targetString, int index, T? result, ParseError? error)
        : base(targetString, index, error)
    {
        this.result = result;
    }
    
    /// <summary>
    /// Gets the parsed value when the parse operation succeeds.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the instance indicates a failed parse operation.</exception>
    public T Result => this.IsSuccess
        ? this.result!
        : throw new InvalidOperationException("Attempted to access Result of a failed ParseState.");

    /// <inheritdoc/>
    public override object? GetResult() => this.Result;

    /// <inheritdoc/>
    public override string ToString() => base.ToString();
}

public static class ParseStateExtensions
{
    /// <summary>
    /// Converts a parse state to a strongly typed failed parse state.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the parse state is successful.</exception>
    public static ParseState<T> FailAs<T>(this ParseState parseState)
    {
        if (parseState.IsSuccess)
        {
            throw new InvalidOperationException("Cannot convert a successful parse state to a failed parse state.");
        }

        if (parseState is ParseState<T> typedParseState)
        {
            return typedParseState;
        }

        return ParseState.Failure<T>(parseState, parseState.Error!);
    }
}