using System;
using System.Numerics;
using System.Runtime.InteropServices;

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
    /// <remarks>
    /// A decimal is reduced by first reducing the scale of the numerator and denominator equally,
    /// then reducing the mantissa of the numerator and denominator by their GCD.
    /// 
    /// This can lead some potentially unexpected results, for example:
    /// 0.05m / 0.1m reduces to 0.5m / 1m (as opposed to the 1m /2m could may have been expected).
    /// </remarks>
    public static (decimal numerator, decimal denominator) Reduce(decimal a, decimal b)
    {
        if (b == 0m)
            throw new DivideByZeroException();

        // Special cases
        if (a == 0m)
            return (0m, 1m);

        if (a == b)
            return (1m, 1m);

        if (a == -b)
            return (-1m, 1m);

        DecimalEx aEx = new(a);
        DecimalEx bEx = new(b);
        
        var minScale = Math.Min(aEx.Scale, bEx.Scale);

        if ((aEx.Hi32 == 0u) && (bEx.Hi32 == 0))
        {
            // If we are able to use ulong to represent the full significand, then
            // it is more optimal to utilise just Lo64 for the GCD calculation.
            var (aLo64, bLo64) = Reduce(aEx.Lo64, bEx.Lo64);

            DecimalEx numerator = new(
                aEx.IsNegative ^ bEx.IsNegative,
                (byte)(aEx.Scale - minScale),
                0u,
                aLo64
            );

            DecimalEx denominator = new(
                false,
                (byte)(bEx.Scale - minScale),
                0u,
                bLo64
            );

            return (numerator.Decimal, denominator.Decimal);
        }
        else
        {
            var (aSignificand, bSignificand) = Reduce(aEx.Significand, bEx.Significand);

            DecimalEx numerator = new(
                aEx.IsNegative ^ bEx.IsNegative,
                (byte)(aEx.Scale - minScale),
                aSignificand
            );

            DecimalEx denominator = new(
                false,
                (byte)(bEx.Scale - minScale),
                bSignificand
            );

            return (numerator.Decimal, denominator.Decimal);
        }
    }

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
        a = T.Abs(a);
        b = T.Abs(b);

        if (a == b)
            return a;

        if ((a == T.One) || (b == T.One))
            return T.One;

        if (a == T.Zero)
            return b;

        if (b == T.Zero)
            return a;

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

    [StructLayout(LayoutKind.Explicit)]
    private readonly struct DecimalEx
    {
        // By overlapping the fields we're able to reinterpret the decimal as a struct with the same layout as the Win32 decimal type.

        [FieldOffset(0)]
        private readonly decimal d;
        [FieldOffset(0)]
        private readonly int _flags;
        [FieldOffset(2)]
        private readonly byte _scale;
        [FieldOffset(3)]
        private readonly byte _sign;
        [FieldOffset(4)]
        private readonly uint _hi32;
        [FieldOffset(8)]
        private readonly ulong _lo64;

        public DecimalEx(decimal value)
        {
            this.d = value;
        }

        public DecimalEx(bool isNegative, byte scale, UInt128 mantissa)
            : this(isNegative, scale, unchecked((uint)(mantissa >> 64)), unchecked((ulong)mantissa))
        {
        }

        public DecimalEx(bool isNegative, byte scale, uint hi32, ulong lo64)
        {
            if (scale > 28)
                throw new ArgumentOutOfRangeException(nameof(scale), "Scale must be between 0 and 28.");

            this._hi32 = hi32;
            this._lo64 = lo64;
            this._scale = scale;
            this._sign = isNegative ? (byte)128 : (byte)0;
        }

        public int Flags => this._flags;
        public bool IsNegative =>this._sign != 0;
        public byte Scale => this._scale;
        public uint Hi32 => this._hi32;
        public ulong Lo64 => this._lo64;
        public UInt128 Significand => new(this._hi32, this._lo64);

        public decimal Decimal => this.d;
    }
}
