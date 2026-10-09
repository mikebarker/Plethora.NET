using System;

namespace Plethora;

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
public readonly struct Rational : IComparable, IComparable<Rational>, IEquatable<Rational>
{
    public static readonly Rational Zero = new(0, 1, false);

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
    /// Initializes a new <seealso cref="Rational"/>, providing the numerator, denominator, and a flag indicating whether the rational must be reduced to its canonical form.
    /// </summary>
    /// <param name="numerator">The numerator.</param>
    /// <param name="denominator">The denominator.</param>
    /// <param name="reduce">true if the numerator and denominator must be reduced to the canonical form; otherwise false.</param>
    private Rational(long numerator, long denominator, bool reduce)
    {
        if (denominator == 0)
            throw new DivideByZeroException(ResourceProvider.ArgMustNotBeZero(nameof(denominator)));

        if (numerator == 0)
        {
            this.numerator = 0;
            this.denominator = 1;
            return;
        }

        if (reduce)
        {
            var gcd = MathEx.GreatestCommonDivisor(numerator, denominator);

            numerator /= gcd;
            denominator /= gcd;
        }

        if (denominator < 0)
        {
            numerator = -numerator;
            denominator = -denominator;
        }

        if (numerator < int.MinValue || numerator > int.MaxValue)
            throw new OverflowException();

        if (denominator < int.MinValue || denominator > int.MaxValue)
            throw new OverflowException();

        this.numerator = (int)numerator;
        this.denominator = (int)denominator;
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

    private bool IsDefault => ((this.numerator == 0) && (this.denominator == 0));

    #region Equality

    public readonly bool Equals(Rational other)
    {
        return
            (this.numerator == other.numerator) &&
            (this.denominator == other.denominator);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj is not Rational)
            return false;

        return this.Equals((Rational)obj);
    }

    public override readonly int GetHashCode()
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

    int IComparable.CompareTo(object? obj)
    {
        if (obj is null)
            return 1;

        if (obj is not Rational)
            throw new ArgumentException(ResourceProvider.ArgMustBeOfType(nameof(obj), typeof(Rational)), nameof(obj));

        return this.CompareTo((Rational)obj);
    }

    public int CompareTo(Rational other)
    {
        if (this.IsDefault && other.IsDefault)
            return 0;

        if (this.IsDefault || other.IsDefault)
            throw new InvalidOperationException();

        long left = (long)this.numerator * (long)other.denominator;
        long right = (long)other.numerator * (long)this.denominator;

        return left.CompareTo(right);
    }

    #endregion

    #region Conversion

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
        if (this.IsDefault)
            throw new InvalidOperationException();

        return
            (double)this.numerator /
            (double)this.denominator;
    }

    public decimal ToDecimal()
    {
        if (this.IsDefault)
            throw new InvalidOperationException();

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
        if (x.IsDefault)
            throw new InvalidOperationException();

        if (y.IsDefault)
            throw new InvalidOperationException();

        int numerator =
            (x.numerator * y.denominator) +
            (y.numerator * x.denominator);

        int denominator =
            (x.denominator * y.denominator);

        return new Rational(numerator, denominator, true);
    }

    public static Rational operator +(int x, Rational y)
    {
        if (y.IsDefault)
            throw new InvalidOperationException();

        return new Rational(x, 1, false) + y;
    }

    public static Rational operator +(Rational x, int y)
    {
        if (x.IsDefault)
            throw new InvalidOperationException();

        return x + new Rational(y, 1, false);
    }

    public static Rational operator -(Rational x, Rational y)
    {
        if (x.IsDefault)
            throw new InvalidOperationException();

        if (y.IsDefault)
            throw new InvalidOperationException();

        int numerator =
            (x.numerator * y.denominator) -
            (y.numerator * x.denominator);

        int denominator =
            (x.denominator * y.denominator);

        return new Rational(numerator, denominator, true);
    }

    public static Rational operator -(int x, Rational y)
    {
        if (y.IsDefault)
            throw new InvalidOperationException();

        return new Rational(x, 1, false) - y;
    }

    public static Rational operator -(Rational x, int y)
    {
        if (x.IsDefault)
            throw new InvalidOperationException();

        return x - new Rational(y, 1, false);
    }

    #endregion

    #region Multiplicative operators

    public static Rational operator *(Rational x, Rational y)
    {
        if (x.IsDefault)
            throw new InvalidOperationException();

        if (y.IsDefault)
            throw new InvalidOperationException();

        if (y.numerator == 0)
            return Rational.Zero;

        if (x.numerator == 0)
            return Rational.Zero;

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
        if (x.IsDefault)
            throw new InvalidOperationException();

        if (y == 0)
            return Rational.Zero;

        if (y == 1)
            return x;

        if (x.numerator == 0)
            return Rational.Zero;

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
        if (y.IsDefault)
            throw new InvalidOperationException();

        return y * x;
    }

    public static Rational operator /(Rational x, Rational y)
    {
        if (x.IsDefault)
            throw new InvalidOperationException();

        if (y.IsDefault)
            throw new InvalidOperationException();

        if (y.numerator == 0)
            throw new DivideByZeroException(ResourceProvider.ArgMustNotBeZero(nameof(y)));

        if (x.numerator == 0)
            return Rational.Zero;

        Rational result = x * y.Invert();
        return result;
    }

    public static Rational operator /(Rational x, int y)
    {
        if (x.IsDefault)
            throw new InvalidOperationException();

        if (y == 0)
            throw new DivideByZeroException(ResourceProvider.ArgMustNotBeZero(nameof(y)));

        if (y == 1)
            return x;

        if (x.numerator == 0)
            return Rational.Zero;

        return x * new Rational(1, y, false);
    }

    public static Rational operator /(int x, Rational y)
    {
        if (y.IsDefault)
            throw new InvalidOperationException();

        if (y.numerator == 0)
            throw new DivideByZeroException(ResourceProvider.ArgMustNotBeZero(nameof(y)));

        if (x == 0)
            return Rational.Zero;

        return new Rational(x, 1, false) / y;
    }

    public static Rational operator %(Rational x, Rational y)
    {
        if (x.IsDefault)
            throw new InvalidOperationException();

        if (y.IsDefault)
            throw new InvalidOperationException();

        if (y.numerator == 0)
            throw new DivideByZeroException(ResourceProvider.ArgMustNotBeZero(nameof(y)));

        var(_, remainder) = DivRem(x, y);
        return remainder;
    }

    public static Rational operator %(int x, Rational y)
    {
        if (y.IsDefault)
            throw new InvalidOperationException();

        if (y.numerator == 0)
            throw new DivideByZeroException(ResourceProvider.ArgMustNotBeZero(nameof(y)));

        return new Rational(x, 1, false) % y;
    }

    public static Rational operator %(Rational x, int y)
    {
        if (x.IsDefault)
            throw new InvalidOperationException();

        if (y == 0)
            throw new DivideByZeroException(ResourceProvider.ArgMustNotBeZero(nameof(y)));

        return x % new Rational(y, 1, false);
    }

    public static (long Quotient, Rational Remainder) DivRem(Rational x, Rational y)
    {
        if (x.IsDefault)
            throw new InvalidOperationException();

        if (y.IsDefault)
            throw new InvalidOperationException();

        if (y.numerator == 0)
            throw new DivideByZeroException(ResourceProvider.ArgMustNotBeZero(nameof(y)));

        var a = (long)x.numerator * (long)y.denominator;
        var b = (long)x.denominator * (long)y.numerator;

        var (quotient, remainder) = Math.DivRem(a, b);

        return (quotient, new Rational(remainder, b, true));
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
        if (this.IsDefault)
            throw new InvalidOperationException();

        if (this.numerator == 0)
            throw new DivideByZeroException(ResourceProvider.ArgMustNotBeZero(nameof(this.Numerator)));

        return new Rational(this.denominator, this.numerator, false);
    }
}
