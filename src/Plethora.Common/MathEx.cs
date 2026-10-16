using System;
using System.Numerics;

namespace Plethora;

public static class MathEx
{
    /// <summary>
    /// Reduces a fraction to its simplest form by dividing both the numerator and denominator by their greatest common divisor (GCD).
    /// </summary>
    /// <typeparam name="T">The type of the numerator and denominator.</typeparam>
    /// <param name="a">The numerator of the fraction.</param>
    /// <param name="b">The denominator of the fraction.</param>
    /// <returns>A tuple containing the reduced numerator and denominator.</returns>
    public static (T numerator, T denominator) Reduce<T>(T a, T b)
        where T : IBinaryInteger<T>
    {
        if (b == T.Zero)
            throw new DivideByZeroException();

        // Special cases
        if (a == T.Zero)
            return (T.Zero, T.One);

        if (a == b)
            return (T.One, T.One);

        if (a == -b)
            return (-T.One, T.One);

        // Special case reduce by common multiples of 2
        T countT = T.TrailingZeroCount(a | b);
        int count = int.CreateTruncating(countT);
        a >>= count;
        b >>= count;

        if ((a == T.One) || (a == -T.One) || (b == T.One) || (b == -T.One))
            return (a, b);

        T gcd = GreatestCommonDivisor(a, b);
        a /= gcd;
        b /= gcd;

        return (a, b);
    }

    /// <summary>
    /// Returns the greatest common divisor of two numbers.
    /// </summary>
    /// <param name="a">The first number.</param>
    /// <param name="b">The second number.</param>
    /// <returns>The greatest common divisor of <paramref name="a"/> and <paramref name="b"/>.</returns>
    public static T GreatestCommonDivisor<T>(T a, T b)
        where T : IBinaryInteger<T>
    {
        if (a == b)
            return a;

        if ((a == T.One) || (b == T.One))
            return T.One;

        if (a == T.Zero)
            return b;

        if (b == T.Zero)
            return a;

        if (T.IsNegative(a))
            a = -a;

        if (T.IsNegative(b))
            b = -b;

        // This implementation uses the binary variant of the Euclidean algorithm.
        //   https://en.wikipedia.org/wiki/Binary_GCD_algorithm

        // Calculate the common trailing zeroes of a and b
        T dT = T.TrailingZeroCount(a | b);
        int d = int.CreateTruncating(dT);

        a = DivBy2UntilOdd(a);

        while (b != T.Zero)
        {
            b = DivBy2UntilOdd(b);

            var min = (a < b) ? a : b;
            var max = (a > b) ? a : b;
            a = min;
            b = max - min;
        }

        return a << d;
    }

    private static T DivBy2UntilOdd<T>(T value)
        where T : IBinaryInteger<T>
    {
        T countT = T.TrailingZeroCount(value);
        int count = int.CreateTruncating(countT);
        return value >> count;
    }
}
