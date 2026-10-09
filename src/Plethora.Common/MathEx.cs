using System;
using System.Numerics;

namespace Plethora;

public static class MathEx
{
    /// <summary>
    /// Returns the greatest common divisor of two numbers.
    /// </summary>
    /// <param name="a">The first number.</param>
    /// <param name="b">The second number.</param>
    /// <returns>The greatest common divisor of <paramref name="a"/> and <paramref name="b"/>.</returns>
    public static int GreatestCommonDivisor(int a, int b) => (int)GreatestCommonDivisor((long)a, (long)b);

    /// <summary>
    /// Returns the greatest common divisor of two numbers.
    /// </summary>
    /// <param name="a">The first number.</param>
    /// <param name="b">The second number.</param>
    /// <returns>The greatest common divisor of <paramref name="a"/> and <paramref name="b"/>.</returns>
    /// <remarks>
    /// This implementation uses the binary variant of the Euclidean algorithm.
    /// <seealso href="https://en.wikipedia.org/wiki/Binary_GCD_algorithm"/>
    /// </remarks>
    public static long GreatestCommonDivisor(long a, long b)
    {
        if (a == 0)
            return b;

        if (b == 0)
            return a;

        a = Math.Abs(a);
        b = Math.Abs(b);

        (a, b) = DivBy2UntilEitherOdd(a, b, out int d);

        while (a != b)
        {
            if (a > b)
            {
                a = DivBy2UntilOdd(a - b, out _);
            }
            else if (a < b)
            {
                b = DivBy2UntilOdd(b - a, out _);
            }
        }

        return a << d;
    }

    private static (long a, long b) DivBy2UntilEitherOdd(long a, long b, out int count)
    {
        count = BitOperations.TrailingZeroCount(a | b);
        a >>= count;
        b >>= count;
        return (a, b);
    }

    private static long DivBy2UntilOdd(long value, out int count)
    {
        count = BitOperations.TrailingZeroCount(value);
        value >>= count;
        return value;
    }
}
