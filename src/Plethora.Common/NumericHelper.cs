using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace Plethora
{
    /// <summary>
    /// Helper class to assist in working with numerics.
    /// </summary>
    public static class NumericHelper
    {
        #region Public Methods

        #region IsInteger

        public static bool IsInteger(this decimal d)
        {
            return ((d % 1m) == 0m);
        }

        public static bool IsInteger(this double d)
        {
            return ((d % 1.0) == 0.0);
        }

        public static bool IsInteger(this float d)
        {
            return ((d % 1.0f) == 0.0f);
        }

        #endregion

        #region Translate

        /// <summary>
        /// Translates a value within one range to within another.
        /// </summary>
        /// <param name="value">The value to be translated.</param>
        /// <param name="oldMin">The minimum value of the old range of 'value'.</param>
        /// <param name="oldMax">The maximum value of the old range of 'value'.</param>
        /// <param name="newMin">The minimum value of the new range.</param>
        /// <param name="newMax">The maximum value of the new range.</param>
        /// <returns>
        /// The proportional value of 'value' in the range 'min' to 'max'.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///  <para>
        ///  Thrown if 'value' is not within the range 'oldMin' to 'oldMax'.
        ///  </para>
        ///  <para>
        ///  Thrown if 'oldMax' is not greater than 'oldMin'.
        ///  </para>
        ///  <para>
        ///  Thrown if 'newMax' is not greater than 'newMin'.
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
        /// The proportional value of 'value' in the range 'min' to 'max'.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if 'value' is not within the range 0 to 1.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if 'max' is not greater than 'min'.
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
        /// The proportional value of 'value' in the range 0 to 'max'.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///  <para>
        ///  Thrown if 'value' is not within the range 0 to 1.
        ///  </para>
        ///  <para>
        ///  Thrown if 'max' is less than 0.
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

        #region Constrain

        /// <summary>
        /// Returns the value constrained by a minimum and maximum.
        /// </summary>
        /// <param name="value">The value to be constrained.</param>
        /// <param name="min">The minimum value of the constraint.</param>
        /// <param name="max">The maximum value of the constraint.</param>
        /// <returns>
        /// If: <br/>
        /// <![CDATA[   value < min         : min ]]><br/>
        /// <![CDATA[   min <= value <= max : value ]]><br/>
        /// <![CDATA[   max < value         : max ]]><br/>
        /// </returns>
        public static int Constrain(int value, int min, int max)
        {
            //Validation
            if (max < min)
                throw new ArgumentException(ResourceProvider.ArgMustBeGreaterThan(nameof(max), "min"));

            return Math.Min(max, Math.Max(min, value));
        }

        /// <summary>
        /// Returns the value constrained by a minimum and maximum.
        /// </summary>
        /// <param name="value">The value to be constrained.</param>
        /// <param name="min">The minimum value of the constraint.</param>
        /// <param name="max">The maximum value of the constraint.</param>
        /// <returns>
        /// If: <br/>
        /// <![CDATA[   value < min         : min ]]><br/>
        /// <![CDATA[   min <= value <= max : value ]]><br/>
        /// <![CDATA[   max < value         : max ]]><br/>
        /// </returns>
        public static decimal Constrain(decimal value, decimal min, decimal max)
        {
            //Validation
            if (max < min)
                throw new ArgumentException(ResourceProvider.ArgMustBeGreaterThan(nameof(max), "min"));

            return Math.Min(max, Math.Max(min, value));
        }

        /// <summary>
        /// Returns the value constrained by a minimum and maximum.
        /// </summary>
        /// <param name="value">The value to be constrained.</param>
        /// <param name="min">The minimum value of the constraint.</param>
        /// <param name="max">The maximum value of the constraint.</param>
        /// <returns>
        /// If: <br/>
        /// <![CDATA[   value < min         : min ]]><br/>
        /// <![CDATA[   min <= value <= max : value ]]><br/>
        /// <![CDATA[   max < value         : max ]]><br/>
        /// </returns>
        public static float Constrain(float value, float min, float max)
        {
            //Validation
            if (max < min)
                throw new ArgumentException(ResourceProvider.ArgMustBeGreaterThan(nameof(max), "min"));

            return Math.Min(max, Math.Max(min, value));
        }

        /// <summary>
        /// Returns the value constrained by a minimum and maximum.
        /// </summary>
        /// <param name="value">The value to be constrained.</param>
        /// <param name="min">The minimum value of the constraint.</param>
        /// <param name="max">The maximum value of the constraint.</param>
        /// <returns>
        /// If: <br/>
        /// <![CDATA[   value < min         : min ]]><br/>
        /// <![CDATA[   min <= value <= max : value ]]><br/>
        /// <![CDATA[   max < value         : max ]]><br/>
        /// </returns>
        public static double Constrain(double value, double min, double max)
        {
            //Validation
            if (max < min)
                throw new ArgumentException(ResourceProvider.ArgMustBeGreaterThan(nameof(max), "min"));

            return Math.Min(max, Math.Max(min, value));
        }
        #endregion

        #region Roman Numerals

        /// <summary>
        /// Struct used to store a roman numeral and its value.
        /// </summary>
        private readonly struct Numeral
        {
            #region Fields

            private readonly int value;
            private readonly string symbol;
            #endregion

            #region Constructors

            public Numeral(string symbol, int value)
            {
                this.symbol = symbol;
                this.value = value;
            }
            #endregion

            #region Properties

            public int Value
            {
                get { return this.value; }
            }

            public string Symbol
            {
                get { return this.symbol; }
            }
            #endregion
        }

        private static Numeral[]? ROMAN_NUMERALS;
        private static Numeral[] RomanNumerals
        {
            get
            {
                ROMAN_NUMERALS ??=
                        [
                            new Numeral( "M", 1000),
                            new Numeral("CM",  900),
                            new Numeral( "D",  500),
                            new Numeral("CD",  400),
                            new Numeral( "C",  100),
                            new Numeral("XC",   90),
                            new Numeral( "L",   50),
                            new Numeral("XL",   40),
                            new Numeral( "X",   10),
                            new Numeral("IX",    9),
                            new Numeral( "V",    5),
                            new Numeral("IV",    4),
                            new Numeral( "I",    1),
                        ];

                return ROMAN_NUMERALS;
            }
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
        /// This conversion follows the strict definition where a value of 10E+x
        /// can not proceed a value of 10E+(x+1). Thus, 1990 is represented by
        /// 'MCMXC' and not by 'MXM'.
        /// </remarks>
        public static int FromRomanNumerals(string romanNumerals)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(romanNumerals);

            if (romanNumerals.Length == 0)
                throw new ArgumentException(ResourceProvider.ArgStringEmpty(nameof(romanNumerals)), nameof(romanNumerals));

            Regex romanNumeralRegex =
              new("^M{0,3}(D?C{0,3}|C[DM])(L?X{0,3}|X[LC])(V?I{0,3}|I[VX])$");

            if (!romanNumeralRegex.IsMatch(romanNumerals))
                throw new ArgumentException(ResourceProvider.ArgInvalid(nameof(romanNumerals)),
                  nameof(romanNumerals));


            string temp = romanNumerals;
            int value = 0;
            int i = 0;

            while ((temp.Length > 0) && (i < RomanNumerals.Length))
            {
                Numeral numeral = RomanNumerals[i];

                if (temp.StartsWith(numeral.Symbol))
                {
                    value += numeral.Value;
                    temp = temp.Substring(numeral.Symbol.Length);
                }
                else
                {
                    i++;
                }
            }

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
                Numeral numeral = RomanNumerals[i];

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
        #endregion

        /// <summary>
        /// Compares an IComparable value to zero.
        /// </summary>
        /// <param name="value">The value to be compared to zero.</param>
        /// <param name="result">The result of a CompareTo operation with zero.</param>
        /// <returns>
        /// true if the comparison could be conducted safely; else false.
        /// </returns>
        public static bool TypeSafeComparisonToZero(object value, out int result)
        {
            try
            {
                //Create an instance of the same type as value, which represents zero (if possible)
                object zero = Convert.ChangeType(0, value.GetType());
                result = Comparer.Default.Compare(value, zero);
                return true;
            }
            catch (InvalidCastException)
            {
                result = 0;
                return false;
            }
        }
        #endregion
    }
}
