using System;

namespace Plethora
{
    /// <summary>
    /// Stores numbers in their rational representation.
    /// </summary>
    /// <remarks>
    /// The rational number is stored in its canonical form.
    /// <example>
    /// Thus 2/(-4) will be reduced and stored as (-1)/2.
    /// </example>
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("Rational [{" + nameof(numerator) + "} / {" + nameof(denominator) + "}]")]
    public struct Rational : IComparable, IComparable<Rational>, IEquatable<Rational>
    {
        private readonly int numerator;
        private readonly int denominator;

        /// <summary>
        /// Initializes a new <seealso cref="Rational"/>, providing the numerator and denominator.
        /// </summary>
        /// <param name="numerator">The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        public Rational(int numerator, int denominator)
            : this(numerator, denominator, true)
        {
        }

        /// <summary>
        /// Initializes a new <seealso cref="Rational"/>, providing the numerator, denominator, and a flag indicating whether the rational must be redued to its canonical form.
        /// </summary>
        /// <param name="numerator">The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        /// <param name="reduce">true if the numerator and denominator must be reduced to the canonical form; otherwise false.</param>
        private Rational(int numerator, int denominator, bool reduce)
        {
            if (denominator == 0m)
                throw new DivideByZeroException(ResourceProvider.ArgMustNotBeZero(nameof(denominator)));

            if (reduce)
            {
                int gcd = MathEx.GreatestCommonDivisor(numerator, denominator);

                numerator = numerator / gcd;
                denominator = denominator / gcd;
            }

            if (denominator < 0)
            {
                numerator = -numerator;
                denominator = -denominator;
            }

            this.numerator = numerator;
            this.denominator = denominator;
        }

        /// <summary>
        /// Gets the numerator.
        /// </summary>
        public int Numerator
        {
            get { return this.numerator; }
        }

        /// <summary>
        /// Gets the denominator.
        /// </summary>
        public int Denominator
        {
            get { return this.denominator; }
        }

        #region Equality

        public bool Equals(Rational other)
        {
            return
                (this.numerator == other.numerator) &&
                (this.denominator == other.denominator);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (!(obj is Rational))
                return false;

            return this.Equals((Rational)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return
                    (this.numerator.GetHashCode() * 397) ^
                    this.denominator.GetHashCode();
            }
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            return this.numerator.ToString() + " / " + this.denominator.ToString();
        }

        public string ToString(IFormatProvider formatProvider)
        {
            return this.numerator.ToString(formatProvider) + " / " + this.denominator.ToString(formatProvider);
        }

        public string ToString(string format)
        {
            return this.numerator.ToString(format) + " / " + this.denominator.ToString(format);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return this.numerator.ToString(format, formatProvider) + " / " + this.denominator.ToString(format, formatProvider);
        }

        #endregion

        #region Implementation of IComparable<Rational>

        int IComparable.CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj))
                return 1;

            if (!(obj is Rational))
                throw new ArgumentException(ResourceProvider.ArgMustBeOfType(nameof(obj), typeof(Rational)), nameof(obj));

            return this.CompareTo((Rational)obj);
        }

        public int CompareTo(Rational other)
        {
            double thisDouble = this.ToDouble();
            double otherDouble = other.ToDouble();

            return thisDouble.CompareTo(otherDouble);
        }

        #endregion

        #region Convertion

        public static explicit operator double(Rational rational)
        {
            return rational.ToDouble();
        }

        public static explicit operator decimal(Rational rational)
        {
            return rational.ToDecimal();
        }

        public double ToDouble()
        {
            return
                (double)this.numerator /
                (double)this.denominator;
        }

        public decimal ToDecimal()
        {
            return
                (decimal)this.numerator /
                (decimal)this.denominator;
        }

        #endregion

        #region Operators

        #region Logical operators

        public static bool operator ==(Rational x, Rational y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Rational x, Rational y)
        {
            return (!(x == y));
        }

        public static bool operator <(Rational x, Rational y)
        {
            return
                x.CompareTo(y) < 0;
        }

        public static bool operator >(Rational x, Rational y)
        {
            return
                x.CompareTo(y) > 0;
        }

        public static bool operator <=(Rational x, Rational y)
        {
            return
                x.CompareTo(y) <= 0;
        }

        public static bool operator >=(Rational x, Rational y)
        {
            return
                x.CompareTo(y) >= 0;
        }

        #endregion

        #region Algebraic operators

        #region Additive operators

        public static Rational operator +(Rational x, Rational y)
        {
            int numerator =
                (x.numerator * y.denominator) +
                (y.numerator * x.denominator);

            int denominator =
                (x.denominator * y.denominator);

            return new Rational(numerator, denominator, true);
        }

        public static Rational operator +(int x, Rational y)
        {
            return new Rational(x, 1, false) + y;
        }

        public static Rational operator +(Rational x, int y)
        {
            return x + new Rational(y, 1, false);
        }

        public static Rational operator -(Rational x, Rational y)
        {
            int numerator =
                (x.numerator * y.denominator) -
                (y.numerator * x.denominator);

            int denominator =
                (x.denominator * y.denominator);

            return new Rational(numerator, denominator, true);
        }

        public static Rational operator -(int x, Rational y)
        {
            return new Rational(x, 1, false) - y;
        }

        public static Rational operator -(Rational x, int y)
        {
            return x - new Rational(y, 1, false);
        }

        #endregion

        #region Multiplicative operators

        public static Rational operator *(Rational x, Rational y)
        {
            // By applying the reduction before multiplying we reduce the likely-hood of
            // arithmetic overflows, and ensure the numbers are stored in their canonical form.
            int gcd1 = MathEx.GreatestCommonDivisor(x.numerator, y.denominator);
            int numerator1 = x.numerator / gcd1;
            int denominator2 = y.denominator / gcd1;

            int gcd2 = MathEx.GreatestCommonDivisor(y.numerator, x.denominator);
            int numerator2 = y.numerator / gcd2;
            int denominator1 = x.denominator / gcd2;

            int numerator =
                (numerator1 * numerator2);

            int denominator =
                (denominator1 * denominator2);

            return new Rational(numerator, denominator, false);
        }

        public static Rational operator *(Rational x, int y)
        {
            // By applying the reduction before multiplying we reduce the likely-hood of
            // arithmetic overflows, and ensure the numbers are stored in their canonical form.
            int gcd = MathEx.GreatestCommonDivisor(y, x.denominator);
            y /= gcd;
            int denominator = x.denominator / gcd;

            int numerator = (x.numerator * y);

            return new Rational(numerator, denominator, false);
        }

        public static Rational operator *(int x, Rational y)
        {
            return y * x;
        }

        public static Rational operator /(Rational x, Rational y)
        {
            Rational result = x * y.Invert();
            return result;
        }

        public static Rational operator /(Rational x, int y)
        {
            return x * new Rational(1, y, false);
        }

        public static Rational operator /(int x, Rational y)
        {
            return new Rational(x, 1, false) / y;
        }

        public static int operator %(Rational x, Rational y)
        {
            Rational f = x / y;

            int remainder;
            Math.DivRem(
                f.numerator,
                f.denominator,
                out remainder);

            return remainder;
        }

        public static int operator %(int x, Rational y)
        {
            return new Rational(x, 1, false) % y;
        }

        public static int operator %(Rational x, int y)
        {
            return x % new Rational(y, 1, false);
        }

        #endregion

        #endregion

        #endregion

        /// <summary>
        /// Inverts a rational number.
        /// </summary>
        /// <returns>
        /// The inverted rational number.
        /// </returns>
        public Rational Invert()
        {
            return new Rational(this.denominator, this.numerator, false);
        }
    }
}
