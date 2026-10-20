using System;
using System.Collections.Generic;
using System.Text;

namespace Plethora;

/// <summary>
/// Provides extension methods for <see cref="ReadOnlySpan{T}"/>.
/// </summary>
public static class SpanExtensions
{
    /// <summary>
    /// Determines whether a span starts with consecutive characters that belong to a specified set.
    /// </summary>
    /// <param name="span">The character span to examine.</param>
    /// <param name="chars">The characters that define the matching set.</param>
    /// <returns>
    /// The number of consecutive leading characters that are in the specified set.
    /// </returns>
    public static int StartsWithAnyCount(this ReadOnlySpan<char> span, char[] chars)
    {
        HashSet<char> charSet = new(chars);
        return StartsWithCharCount(span, charSet.Contains);
    }

    /// <summary>
    /// Determines whether a span starts with consecutive characters that are letters.
    /// </summary>
    /// <param name="span">The character span to examine.</param>
    /// <returns>
    /// The number of consecutive leading characters that are letters.
    /// </returns>
    /// <remarks>
    /// Letters are matched using the <see cref="Rune.IsLetter(Rune)"/> method, which checks for Unicode letter characters.
    /// </remarks>
    public static int StartsWithLettersCount(this ReadOnlySpan<char> span)
    {
        return StartsWithRuneCount(span, Rune.IsLetter);
    }

    /// <summary>
    /// Determines whether a span starts with consecutive characters that are digits.
    /// </summary>
    /// <param name="span">The character span to examine.</param>
    /// <returns>
    /// The number of consecutive leading characters that are digits.
    /// </returns>
    /// <remarks>
    /// Digits are matched using the <see cref="Rune.IsDigit(Rune)"/> method, which checks for Unicode digit characters.
    /// </remarks>
    public static int StartsWithDigitsCount(this ReadOnlySpan<char> span)
    {
        return StartsWithRuneCount(span, Rune.IsDigit);
    }

    /// <summary>
    /// Determines whether a span starts with consecutive characters that are whitespace.
    /// </summary>
    /// <param name="span">The character span to examine.</param>
    /// <returns>
    /// The number of consecutive leading characters that are whitespace.
    /// </returns>
    /// <remarks>
    /// Whitespace characters are matched using the <see cref="char.IsWhiteSpace(char)"/> method.
    /// </remarks>
    public static int StartsWithWhitespaceCount(this ReadOnlySpan<char> span)
    {
        return StartsWithCharCount(span, char.IsWhiteSpace);
    }

    private static int StartsWithCharCount(ReadOnlySpan<char> span, Func<char, bool> predicate)
    {
        return StartsWithCount(
            span.GetEnumerator(),
            predicate,
            r => 1);
    }

    private static int StartsWithRuneCount(ReadOnlySpan<char> span, Func<Rune, bool> predicate)
    {
        return StartsWithCount(
            span.EnumerateRunes(),
            predicate,
            r => r.Utf16SequenceLength);
    }

    private static int StartsWithCount<TEnumerator, T>(
        TEnumerator enumerator,
        Func<T, bool> predicate,
        Func<T, int> getLength)
        where TEnumerator : IEnumerator<T>, allows ref struct
    {
        int length = 0;
        while (enumerator.MoveNext())
        {
            var current = enumerator.Current;
            if (!predicate(current))
                break;

            length += getLength(current);
        }

        return length;
    }
}
