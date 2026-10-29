using System;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace Plethora;

/// <summary>
/// Helper class to assist in working with numerics.
/// </summary>
public static partial class Numeric
{
    /// <summary>
    /// Tries to creates an instance of the current type from a value.
    /// </summary>
    /// <typeparam name="T">The numeric type of the value.</typeparam>
    /// <typeparam name="TOther">The numeric type to create.</typeparam>
    /// <param name="value">The value to convert.</param>
    /// <param name="result">The result of the conversion.</param>
    /// <returns>
    /// <see langword="true"/> if the value is representable in <typeparamref name="TOther"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryCreate<T, TOther>(this T value, out TOther result)
        where T : INumberBase<T>
        where TOther : INumberBase<TOther>, IEquatable<TOther>
    {
        try
        {
            result = TOther.CreateSaturating(value);
        }
        catch (NotSupportedException)
        {
            result = TOther.Zero;
            return false;
        }

        // Use a round-trip conversion to determine if the value is representable in TOther.
        return T.CreateSaturating(result).Equals(value);
    }

    /// <summary>
    /// Compares two numeric values which may have different numeric types and returns
    /// an integer that indicates whether the left value precedes, follows, or
    /// occurs in the same position in the sort order as the right value.
    /// </summary>
    /// <typeparam name="TLeft">The numeric type of the left value.</typeparam>
    /// <typeparam name="TRight">The numeric type of the right value.</typeparam>
    /// <param name="left">The left value to compare.</param>
    /// <param name="right">The right value to compare.</param>
    /// <returns>
    /// A value that indicates the relative order of the objects being compared. The
    /// return value has these meanings:
    /// <list type="table">
    ///   <listheader>
    ///     <term>Value</term>
    ///     <description>Meaning</description>
    ///   </listheader>
    ///   <item>
    ///     <term>Less than zero</term>
    ///     <description><paramref name="left"/> precedes <paramref name="right"/> in the sort order.</description>
    ///   </item>
    ///   <item>
    ///     <term>Zero</term>
    ///     <description><paramref name="left"/> occurs in the same position in the sort order as <paramref name="right"/>.</description>
    ///   </item>
    ///   <item>
    ///     <term>Greater than zero</term>
    ///     <description><paramref name="left"/> follows <paramref name="right"/> in the sort order.</description>
    ///   </item>
    /// </list>
    /// </returns>
    /// <remarks>
    /// This method assumes that <c>TRight.IsNegative(a) != TRight.IsNegative(a)</c> for all
    /// non-zero values of a. Where this identity is not valid the method does not guarantee a correct result,
    /// should <paramref name="right"/> fall outside the representable range of <c>TLeft</c>.
    /// </remarks>
    public static int Compare<TLeft, TRight>(TLeft left, TRight right)
        where TLeft : INumberBase<TLeft>, IComparable<TLeft>
        where TRight : INumberBase<TRight>
    {
        // If 'right' is representable in TLeft, so compare directly
        if (TryCreate<TRight, TLeft>(right, out var r))
        {
            return left.CompareTo(r);
        }

        // 'right' is outside TLeft's range:
        //   right > TLeft.MaxValue (right > 0) -> any TLeft value is smaller
        //   right < TLeft.MinValue (right < 0) -> any TLeft value is larger
        return TRight.IsNegative(right) ? 1 : -1;
    }

    /// <summary>
    /// Compares two numeric values which may have different numeric types and returns
    /// an integer that indicates whether the left value precedes, follows, or
    /// occurs in the same position in the sort order as the right value.
    /// </summary>
    /// <typeparam name="TLeft">The numeric type of the left value.</typeparam>
    /// <typeparam name="TRight">The numeric type of the right value.</typeparam>
    /// <param name="left">The left value to compare.</param>
    /// <param name="right">The right value to compare.</param>
    /// <returns>
    /// A value that indicates the relative order of the objects being compared. The
    /// return value has these meanings:
    /// <list type="table">
    ///   <listheader>
    ///     <term>Value</term>
    ///     <description>Meaning</description>
    ///   </listheader>
    ///   <item>
    ///     <term>Less than zero</term>
    ///     <description><paramref name="left"/> precedes <paramref name="right"/> in the sort order.</description>
    ///   </item>
    ///   <item>
    ///     <term>Zero</term>
    ///     <description><paramref name="left"/> occurs in the same position in the sort order as <paramref name="right"/>.</description>
    ///   </item>
    ///   <item>
    ///     <term>Greater than zero</term>
    ///     <description><paramref name="left"/> follows <paramref name="right"/> in the sort order.</description>
    ///   </item>
    /// </list>
    /// </returns>
    /// <remarks>
    /// The method requires that <paramref name="right"/> is representable in <c>TLeft</c>.
    /// 
    /// This method is more efficient than <see cref="Compare{TLeft, TRight}(TLeft, TRight)"/> as it avoids range checking overhead.
    /// </remarks>
    public static int CompareInRange<TLeft, TRight>(TLeft left, TRight right)
        where TLeft : INumberBase<TLeft>, IComparable<TLeft>
        where TRight : INumberBase<TRight>
    {
        TLeft v = TLeft.CreateSaturating(right);
        return left.CompareTo(v);
    }


    #region Translate

    /// <summary>
    /// Translates a value within one range to within another.
    /// </summary>
    /// <param name="value">The value to be translated.</param>
    /// <param name="oldMin">The minimum value of the old range of <paramref name="value"/>.</param>
    /// <param name="oldMax">The maximum value of the old range of <paramref name="value"/>.</param>
    /// <param name="newMin">The minimum value of the new range.</param>
    /// <param name="newMax">The maximum value of the new range.</param>
    /// <returns>
    /// The proportional value of <paramref name="value"/> in the range <paramref name="newMin"/> to <paramref name="newMax"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///  <para>
    ///  Thrown if <paramref name="value"/> is not within the range <paramref name="oldMin"/> to <paramref name="oldMax"/>.
    ///  </para>
    ///  <para>
    ///  Thrown if <paramref name="oldMax"/> is not greater than <paramref name="oldMin"/>.
    ///  </para>
    ///  <para>
    ///  Thrown if <paramref name="newMax"/> is not greater than <paramref name="newMin"/>.
    ///  </para>
    /// </exception>
    public static double Translate(double value,
                                   double newMax, double newMin,
                                   double oldMax, double oldMin)
    {
        //Validation
        if (oldMax <= oldMin)
            throw new ArgumentException(ResourceProvider.ArgMustBeGreaterThan(nameof(oldMax), "oldMin"));

        if ((value < oldMin) || (value > oldMax))
            throw new ArgumentException(ResourceProvider.ArgMustBeBetween(nameof(value), "oldMin", "oldMax"));

        if (newMax <= newMin)
            throw new ArgumentException(ResourceProvider.ArgMustBeGreaterThan(nameof(newMax), "newMin"));


        double _value = (value - oldMin) / (oldMax - oldMin);
        return Translate(_value, newMax, newMin);
    }

    /// <summary>
    /// Translates a value between 0 and 1 to between a specified minimum and
    /// maximum.
    /// </summary>
    /// <param name="value">The value to be translated.</param>
    /// <param name="max">The maximum value of the new range.</param>
    /// <param name="min">The minimum value of the new range.</param>
    /// <returns>
    /// The proportional value of <paramref name="value"/> in the range <paramref name="min"/> to <paramref name="max"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="value"/> is not within the range 0 to 1.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="max"/> is not greater than <paramref name="min"/>.
    /// </exception>
    public static double Translate(double value, double max, double min)
    {
        //Validation
        if ((value < 0.0) || (value > 1.0))
            throw new ArgumentOutOfRangeException(nameof(value), value,
              ResourceProvider.ArgMustBeBetween(nameof(value), 0.0, 1.0));

        if (max <= min)
            throw new ArgumentException(ResourceProvider.ArgMustBeGreaterThan(nameof(max), "min"));

        double range = max - min;
        return (value * range) + min;
    }

    /// <summary>
    /// Translates a value between 0 and 1 to between 0 and a specified
    /// maximum.
    /// </summary>
    /// <param name="value">The value to be translated.</param>
    /// <param name="max">The maximum value of the new range.</param>
    /// <returns>
    /// The proportional value of <paramref name="value"/> in the range 0 to <paramref name="max"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///  <para>
    ///  Thrown if <paramref name="value"/> is not within the range 0 to 1.
    ///  </para>
    ///  <para>
    ///  Thrown if <paramref name="max"/> is less than 0.
    ///  </para>
    /// </exception>
    public static double Translate(double value, double max)
    {
        //Validation
        if ((value < 0.0) || (value > 1.0))
            throw new ArgumentOutOfRangeException(nameof(value), value,
              ResourceProvider.ArgMustBeBetween(nameof(value), 0.0, 1.0));

        if (max < 0.0)
            throw new ArgumentOutOfRangeException(nameof(max), max,
              ResourceProvider.ArgMustBeGreaterThanZero(nameof(max)));


        return Translate(value, max, 0.0);
    }
    #endregion

    #region Saturate

    /// <summary>
    /// Returns the value saturated by a minimum and maximum.
    /// </summary>
    /// <param name="value">The value to be saturated.</param>
    /// <param name="min">The minimum value of the saturation.</param>
    /// <param name="max">The maximum value of the saturation.</param>
    /// <returns>
    /// If: <br/>
    /// <![CDATA[   value < min         : min ]]><br/>
    /// <![CDATA[   min <= value <= max : value ]]><br/>
    /// <![CDATA[   max < value         : max ]]><br/>
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="max"/> is not greater than <paramref name="min"/>.
    /// </exception>
    public static T Saturate<T>(T value, T min, T max)
        where T : INumber<T>
    {
        //Validation
        if (max < min)
            throw new ArgumentException(ResourceProvider.ArgMustBeGreaterThan(nameof(max), "min"));

        return T.Min(max, T.Max(min, value));
    }

    #endregion

    #region Roman Numerals

    private static (string Symbol, int Value)[]? ROMAN_NUMERALS;
    private static (string Symbol, int Value)[] RomanNumerals
    {
        get
        {
            ROMAN_NUMERALS ??=
                    [
                        new( "M", 1000),
                        new("CM",  900),
                        new( "D",  500),
                        new("CD",  400),
                        new( "C",  100),
                        new("XC",   90),
                        new( "L",   50),
                        new("XL",   40),
                        new( "X",   10),
                        new("IX",    9),
                        new( "V",    5),
                        new("IV",    4),
                        new( "I",    1),
                    ];

            return ROMAN_NUMERALS;
        }
    }

    /// <summary>
    /// Determines whether a string is a valid Roman numeral representation.
    /// </summary>
    /// <param name="romanNumerals">The uppercase string containing the roman numeral representation to be validated.</param>
    /// <returns>
    /// <see langword="true"/> if the input matches the Roman numeral validation pattern; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsValidRomanNumerals(string romanNumerals)
    {
        Regex romanNumeralRegex = RomanNumeralValidation();

        return romanNumeralRegex.IsMatch(romanNumerals);
    }

    /// <summary>
    /// Converts a roman numeral representation into an integer.
    /// </summary>
    /// <param name="romanNumerals">
    /// The uppercase string containing the roman numeral representation.
    /// </param>
    /// <returns>
    /// The integer value of the roman numeral representation.
    /// </returns>
    /// <remarks>
    ///   <para>
    ///     This conversion follows the strict definition where a value of 10E+x
    ///     can not proceed a value of 10E+(x+1). Thus, 1990 is represented by
    ///     'MCMXC' and not by 'MXM'.
    ///   </para>
    ///   <para>
    ///     This method does not validate that a the provided roman numeral representation
    ///     meets the strict definition for representation. For example, 'IVII' is not a
    ///     valid numeral, but this method will return 6 for this input.
    ///   </para>
    /// </remarks>
    public static int FromRomanNumerals(string romanNumerals)
    {
        //Validation
        ArgumentNullException.ThrowIfNull(romanNumerals);

        if (romanNumerals.Length == 0)
            throw new ArgumentException(ResourceProvider.ArgStringEmpty(nameof(romanNumerals)), nameof(romanNumerals));


        var span = romanNumerals.AsSpan();
        int value = 0;
        int i = 0;

        while ((span.Length > 0) && (i < RomanNumerals.Length))
        {
            var numeral = RomanNumerals[i];

            if (span.StartsWith(numeral.Symbol))
            {
                value += numeral.Value;
                span = span.Slice(numeral.Symbol.Length);
            }
            else
            {
                i++;
            }
        }

        // Error test
        if (span.Length > 0)
            throw new ArgumentException(ResourceProvider.ArgInvalid(nameof(romanNumerals)),
              nameof(romanNumerals));

        return value;
    }

    /// <summary>
    /// Converts an integer into its roman numeral representation.
    /// </summary>
    /// <param name="number">
    /// The integer for which the roman numeral is required.
    ///  <para>
    ///  This integer must be between 1 and 3999 (inclusive).
    ///  </para>
    /// </param>
    /// <returns>
    /// The uppercase roman numeral representation of the number.
    /// </returns>
    /// <remarks>
    /// This conversion follows the strict definition where a value of 10E+x
    /// can not proceed a value of 10E+(x+1). Thus, 1990 is represented by
    /// 'MCMXC' and not by 'MXM'.
    /// </remarks>
    public static string ToRomanNumerals(int number)
    {
        //Validation
        if ((number < 1) || (number > 3999))
            throw new ArgumentOutOfRangeException(nameof(number),
              ResourceProvider.ArgMustBeBetween(nameof(number), 1, 3999));


        StringBuilder sb = new();
        int temp = number;
        int i = 0;

        while ((temp > 0) && (i < RomanNumerals.Length))
        {
            var numeral = RomanNumerals[i];

            if (temp >= numeral.Value)
            {
                temp -= numeral.Value;
                sb.Append(numeral.Symbol);
            }
            else
            {
                i++;
            }
        }

        return sb.ToString();
    }

    [GeneratedRegex("^M{0,3}(D?C{0,3}|C[DM])(L?X{0,3}|X[LC])(V?I{0,3}|I[VX])$")]
    private static partial Regex RomanNumeralValidation();

    #endregion
}
