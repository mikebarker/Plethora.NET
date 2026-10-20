using System.Runtime.CompilerServices;

namespace Plethora.Parsing;

/// <summary>
/// Represents an error produced while parsing input, including the error message and the originating member name.
/// </summary>
/// <param name="message">The error description.</param>
/// <param name="origin">The name of the member where the error originated.</param>
public class ParseError(string message, [CallerMemberName] string origin = "")
{
    /// <summary>
    /// Gets the error description.
    /// </summary>
    public string Message { get; } = message;

    /// <summary>
    /// Gets the name of the member where the error originated.
    /// </summary>
    public string Origin { get; } = origin;

    /// <inheritdoc/>
    public override string ToString() => $"{Origin}: {Message}";
}
