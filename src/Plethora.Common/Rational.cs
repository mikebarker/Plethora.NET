using System;
using System.Numerics;

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
    private enum CtorFlag
    {
        Internal,
    }

    public static readonly Rational Zero = new(0, 1u, CtorFlag.Internal);
    public static readonly Rational One = new(1, 1u, CtorFlag.Internal);

    private readonly int numerator;
    private readonly uint denominator;

    /// <summary>
    /// Initializes a new <seealso cref="Rational"/>, providing the numerator and denominator.
    /// </summary>
    /// <param name="numerator">The numerator.</param>
    /// <param name="denominator">The denominator.</param>
    public Rational(int numerator, int denominator)
    {
        if (denominator == 0)
            throw new ArgumentOutOfRangeException(ResourceProvider.ArgMustNotBeZero(nameof(denominator)));


        var r = MathEx.Reduce(numerator, denominator);
        r = NormalizeSign(r);
        var p = GuardRange(r);

        this.numerator = p.numerator;
        this.denominator = p.denominator;
    }

    /// <summary>
    /// Initializes a new <seealso cref="Rational"/>, providing the numerator and denominator.
    /// </summary>
    /// <param name="numerator">The numerator.</param>
    /// <param name="denominator">The denominator.</param>
    public Rational(int numerator, uint denominator)
    {
        if (denominator == 0)
            throw new ArgumentOutOfRangeException(ResourceProvider.ArgMustNotBeZero(nameof(denominator)));


        var r = MathEx.Reduce((long)numerator, denominator);

        this.numerator = (int)r.numerator;
        this.denominator = (uint)r.denominator;
    }

    /// <summary>
    /// Initializes a new <seealso cref="Rational"/>, providing the numerator, denominator.
    /// </summary>
    /// <param name="numerator">The numerator.</param>
    /// <param name="denominator">The denominator.</param>
    private Rational(int numerator, uint denominator, CtorFlag _)
    {
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
    public uint Denominator
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

    public static Rational operator +(Rational x)
    {
        if (x.IsDefault)
            throw new InvalidOperationException();

        return x;
    }

    public static Rational operator -(Rational x)
    {
        if (x.IsDefault)
            throw new InvalidOperationException();

        return new Rational(-x.numerator, x.denominator, CtorFlag.Internal);
    }

    #region Additive operators

    public static Rational operator +(Rational x, Rational y)
    {
        if (x.IsDefault)
            throw new InvalidOperationException();

        if (y.IsDefault)
            throw new InvalidOperationException();

        long numerator =
            ((long)x.numerator * y.denominator) +
            ((long)y.numerator * x.denominator);

        ulong denominator =
            (ulong)x.denominator * y.denominator;

        var r = MathEx.Reduce((Int128)numerator, (Int128)denominator);
        r = NormalizeSign(r);
        var p = GuardRange(r);

        return new Rational(p.numerator, p.denominator, CtorFlag.Internal);
    }

    public static Rational operator +(int x, Rational y)
    {
        return new Rational(x, 1u, CtorFlag.Internal) + y;
    }

    public static Rational operator +(Rational x, int y)
    {
        return x + new Rational(y, 1u, CtorFlag.Internal);
    }

    public static Rational operator -(Rational x, Rational y)
    {
        return x + (-y);
    }

    public static Rational operator -(int x, Rational y)
    {
        return new Rational(x, 1u, CtorFlag.Internal) - y;
    }

    public static Rational operator -(Rational x, int y)
    {
        return x - new Rational(y, 1u, CtorFlag.Internal);
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

        if (x == Rational.One)
            return y;

        if (y == Rational.One)
            return x;

        var numerator = (long)x.numerator * y.numerator;
        var denominator = (ulong)x.denominator * y.denominator;

        var r = MathEx.Reduce((Int128)numerator, (Int128)denominator);
        r = NormalizeSign(r);
        var p = GuardRange(r);

        return new Rational(p.numerator, p.denominator, CtorFlag.Internal);
    }

    public static Rational operator *(Rational x, int y)
    {
        return x * new Rational(y, 1u, CtorFlag.Internal);
    }

    public static Rational operator *(int x, Rational y)
    {
        return new Rational(x, 1u, CtorFlag.Internal) * y;
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

        if (y == Rational.One)
            return x;

        if (x == Rational.One)
            return y.Invert();

        var numerator = x.numerator * y.denominator;
        var denominator = x.denominator * y.numerator;

        var r = MathEx.Reduce(numerator, denominator);
        r = NormalizeSign(r);
        var p = GuardRange(r);

        return new Rational(p.numerator, p.denominator, CtorFlag.Internal);
    }

    public static Rational operator /(Rational x, int y)
    {
        return x / new Rational(y, 1u, CtorFlag.Internal);
    }

    public static Rational operator /(int x, Rational y)
    {
        return new Rational(x, 1u, CtorFlag.Internal) / y;
    }

    public static Rational operator %(Rational x, Rational y)
    {
        var(_, remainder) = DivRem(x, y);
        return remainder;
    }

    public static Rational operator %(int x, Rational y)
    {
        return new Rational(x, 1u, CtorFlag.Internal) % y;
    }

    public static Rational operator %(Rational x, int y)
    {
        return x % new Rational(y, 1u, CtorFlag.Internal);
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

        var r = MathEx.Reduce(remainder, b);
        r = NormalizeSign(r);
        var p = GuardRange(r);

        return (quotient, new Rational(p.numerator, p.denominator, CtorFlag.Internal));
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

        if (this.numerator < 0)
        {
            if (this.denominator == unchecked((uint)int.MinValue))
                return new Rational(int.MinValue, (uint)(-this.numerator), CtorFlag.Internal);

            if (this.denominator > int.MaxValue)
                throw new OverflowException();

            return new Rational(-(int)this.denominator, (uint)(-this.numerator), CtorFlag.Internal);
        }
        else
        {
            if (this.denominator > int.MaxValue)
                throw new OverflowException();

            return new Rational((int)this.denominator, (uint)this.numerator, CtorFlag.Internal);
        }
    }


    private static (TNumerator numerator, TDenominator denominator) NormalizeSign<TNumerator, TDenominator>((TNumerator numerator, TDenominator denominator) arg)
        where TNumerator : INumberBase<TNumerator>
        where TDenominator : INumberBase<TDenominator>
    {
        if (TDenominator.IsNegative(arg.denominator))
        {
            return (-arg.numerator, -arg.denominator);
        }
        return arg;
    }

    private static (int numerator, uint denominator) GuardRange<TNumerator, TDenominator>((TNumerator numerator, TDenominator denominator) arg)
        where TNumerator : INumberBase<TNumerator>
        where TDenominator : INumberBase<TDenominator>
    {
        return (int.CreateChecked(arg.numerator), uint.CreateChecked(arg.denominator));
    }
}
