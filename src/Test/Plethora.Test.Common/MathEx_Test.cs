using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Plethora.Test;

[TestClass]
public class MathEx_Test
{
    #region GreatestCommonDivisor

    [TestMethod]
    public void GreatestCommonDivisor_Zero()
    {
        // Action
        int gcd = MathEx.GreatestCommonDivisor(0, 0);

        // Assert
        Assert.AreEqual(0, gcd);
    }

    [TestMethod]
    public void GreatestCommonDivisor_CommonElement()
    {
        // Action
        int gcd = MathEx.GreatestCommonDivisor(4, 2);

        // Assert
        Assert.AreEqual(2, gcd);
    }

    [TestMethod]
    public void GreatestCommonDivisor_PrimeMultiple()
    {
        // Action
        int gcd = MathEx.GreatestCommonDivisor(45, 20);

        // Assert
        Assert.AreEqual(5, gcd);
    }

    [TestMethod]
    public void GreatestCommonDivisor_DoublePrimeMultiple()
    {
        // Action
        int gcd = MathEx.GreatestCommonDivisor(50, 20);

        // Assert
        Assert.AreEqual(10, gcd);
    }

    [TestMethod]
    public void GreatestCommonDivisor_NoCommonDivisor()
    {
        // Action
        int gcd = MathEx.GreatestCommonDivisor(5, 2);

        // Assert
        Assert.AreEqual(1, gcd);
    }

    [TestMethod]
    public void GreatestCommonDivisor_With1()
    {
        // Action
        int gcd = MathEx.GreatestCommonDivisor(1024, 1);

        // Assert
        Assert.AreEqual(1, gcd);
    }

    [TestMethod]
    public void GreatestCommonDivisor_With0()
    {
        // Action
        int gcd = MathEx.GreatestCommonDivisor(1, 0);

        // Assert
        Assert.AreEqual(1, gcd);
    }

    [TestMethod]
    public void GreatestCommonDivisor_With0_v2()
    {
        // Action
        int gcd = MathEx.GreatestCommonDivisor(0, 1);

        // Assert
        Assert.AreEqual(1, gcd);
    }

    [TestMethod]
    public void GreatestCommonDivisor_LargestFirst()
    {
        // Action
        int gcd = MathEx.GreatestCommonDivisor(4, 2);

        // Assert
        Assert.AreEqual(2, gcd);
    }

    [TestMethod]
    public void GreatestCommonDivisor_LargestSecond()
    {
        // Action
        int gcd = MathEx.GreatestCommonDivisor(2, 4);

        // Assert
        Assert.AreEqual(2, gcd);
    }

    [TestMethod]
    public void GreatestCommonDivisor_NegativeFirst()
    {
        // Action
        int gcd = MathEx.GreatestCommonDivisor(-4, 2);

        // Assert
        Assert.AreEqual(2, gcd);
    }

    [TestMethod]
    public void GreatestCommonDivisor_NegativeSecond()
    {
        // Action
        int gcd = MathEx.GreatestCommonDivisor(4, -2);

        // Assert
        Assert.AreEqual(2, gcd);
    }

    [TestMethod]
    public void GreatestCommonDivisor_NegativeSecond_v2()
    {
        // Action
        int gcd = MathEx.GreatestCommonDivisor(4, -8);

        // Assert
        Assert.AreEqual(4, gcd);
    }

    [TestMethod]
    public void GreatestCommonDivisor_EqualNegativeInputs()
    {
        // Action
        int gcd = MathEx.GreatestCommonDivisor(-4, -4);

        // Assert
        Assert.AreEqual(4, gcd);
    }

    [TestMethod]
    public void GreatestCommonDivisor_ZeroAndNegative()
    {
        // Action
        int gcd = MathEx.GreatestCommonDivisor(0, -4);

        // Assert
        Assert.AreEqual(4, gcd);
    }

    [TestMethod]
    public void GreatestCommonDivisor_NegativeAndZero()
    {
        // Action
        int gcd = MathEx.GreatestCommonDivisor(-4, 0);

        // Assert
        Assert.AreEqual(4, gcd);
    }

    [TestMethod]
    public void GreatestCommonDivisor_NegativeOneAndOne()
    {
        // Action
        int gcd = MathEx.GreatestCommonDivisor(-1, 1);

        // Assert
        Assert.AreEqual(1, gcd);
    }

    [TestMethod]
    public void GreatestCommonDivisor_OneAndNegativeOne()
    {
        // Action
        int gcd = MathEx.GreatestCommonDivisor(1, -1);

        // Assert
        Assert.AreEqual(1, gcd);
    }

    [TestMethod]
    public void GreatestCommonDivisor_Long()
    {
        // Action
        long gcd = MathEx.GreatestCommonDivisor(45L, 20L);

        // Assert
        Assert.AreEqual(5L, gcd);
    }

    [TestMethod]
    public void GreatestCommonDivisor_Int128()
    {
        // Action
        Int128 gcd = MathEx.GreatestCommonDivisor((Int128)45, (Int128)20);

        // Assert
        Assert.AreEqual((Int128)5, gcd);
    }

    [TestMethod]
    public void GreatestCommonDivisor_Ulong()
    {
        // Action
        ulong gcd = MathEx.GreatestCommonDivisor(12_000_000_000UL, 18_000_000_000UL);

        // Assert
        Assert.AreEqual(6_000_000_000UL, gcd);
    }

    [TestMethod]
    public void GreatestCommonDivisor_UInt128()
    {
        UInt128 largeValue = (UInt128)1 << 100;

        // Action
        UInt128 gcd = MathEx.GreatestCommonDivisor(largeValue * 3, largeValue * 5);

        // Assert
        Assert.AreEqual(largeValue, gcd);
    }

    [TestMethod]
    public void GreatestCommonDivisor_Generic()
    {
        // Action
        decimal gcd = MathEx.GreatestCommonDivisor<decimal>(45m, 20m);

        // Assert
        Assert.AreEqual(5m, gcd);
    }

    #endregion

    #region LeastCommonMultiple

    [TestMethod]
    public void LeastCommonMultiple_CommonFactors()
    {
        // Action
        int lcm = MathEx.LeastCommonMultiple(4, 6);

        // Assert
        Assert.AreEqual(12, lcm);
    }

    [TestMethod]
    public void LeastCommonMultiple_Zero()
    {
        // Action
        int lcm = MathEx.LeastCommonMultiple(0, 6);

        // Assert
        Assert.AreEqual(0, lcm);
    }

    [TestMethod]
    public void LeastCommonMultiple_NegativeInputs()
    {
        // Action
        int lcm = MathEx.LeastCommonMultiple(-4, 6);

        // Assert
        Assert.AreEqual(12, lcm);
    }

    [TestMethod]
    public void LeastCommonMultiple_LargeUnsignedInputs()
    {
        // Action
        ulong lcm = MathEx.LeastCommonMultiple(6_000_000_000UL, 10_000_000_000UL);

        // Assert
        Assert.AreEqual(30_000_000_000UL, lcm);
    }

    [TestMethod]
    public void LeastCommonMultiple_DecimalInputs()
    {
        // Action
        decimal lcm = MathEx.LeastCommonMultiple(45m, 20m);

        // Assert
        Assert.AreEqual(180m, lcm);
    }

    #endregion

    #region Reduce

    [TestMethod]
    public void Reduce_CommonElement()
    {
        // Action
        var (numerator, denominator) = MathEx.Reduce(4, 2);

        // Assert
        Assert.AreEqual(2, numerator);
        Assert.AreEqual(1, denominator);
    }

    [TestMethod]
    public void Reduce_PrimeMultiple()
    {
        // Action
        var (numerator, denominator) = MathEx.Reduce(45, 20);

        // Assert
        Assert.AreEqual(9, numerator);
        Assert.AreEqual(4, denominator);
    }

    [TestMethod]
    public void Reduce_DoublePrimeMultiple()
    {
        // Action
        var (numerator, denominator) = MathEx.Reduce(50, 20);

        // Assert
        Assert.AreEqual(5, numerator);
        Assert.AreEqual(2, denominator);
    }

    [TestMethod]
    public void Reduce_NoCommonDivisor()
    {
        // Action
        var (numerator, denominator) = MathEx.Reduce(5, 2);

        // Assert
        Assert.AreEqual(5, numerator);
        Assert.AreEqual(2, denominator);
    }

    [TestMethod]
    public void Reduce_PowersOfTwo()
    {
        // Action
        var (numerator, denominator) = MathEx.Reduce(1024, 512);

        // Assert
        Assert.AreEqual(2, numerator);
        Assert.AreEqual(1, denominator);
    }

    [TestMethod]
    public void Reduce_CommonFactorOfTwo()
    {
        // Action
        var (numerator, denominator) = MathEx.Reduce(8, 12);

        // Assert
        Assert.AreEqual(2, numerator);
        Assert.AreEqual(3, denominator);
    }

    [TestMethod]
    public void Reduce_DenominatorIsOne()
    {
        // Action
        var (numerator, denominator) = MathEx.Reduce(7, 1);

        // Assert
        Assert.AreEqual(7, numerator);
        Assert.AreEqual(1, denominator);
    }

    [TestMethod]
    public void Reduce_NumeratorIsOne()
    {
        // Action
        var (numerator, denominator) = MathEx.Reduce(1, 7);

        // Assert
        Assert.AreEqual(1, numerator);
        Assert.AreEqual(7, denominator);
    }

    [TestMethod]
    public void Reduce_DenominatorDividesNumerator()
    {
        // Action
        var (numerator, denominator) = MathEx.Reduce(12, 4);

        // Assert
        Assert.AreEqual(3, numerator);
        Assert.AreEqual(1, denominator);
    }

    [TestMethod]
    public void Reduce_NumeratorIsZero()
    {
        // Action
        var (numerator, denominator) = MathEx.Reduce(0, 5);

        // Assert
        Assert.AreEqual(0, numerator);
        Assert.AreEqual(1, denominator);
    }

    [TestMethod]
    public void Reduce_AlreadyReduced()
    {
        // Action
        var (numerator, denominator) = MathEx.Reduce(9, 4);

        // Assert
        Assert.AreEqual(9, numerator);
        Assert.AreEqual(4, denominator);
    }

    [TestMethod]
    public void Reduce_NegativeNumerator()
    {
        // Action
        var (numerator, denominator) = MathEx.Reduce(-4, 6);

        // Assert
        Assert.AreEqual(-2, numerator);
        Assert.AreEqual(3, denominator);
    }

    [TestMethod]
    public void Reduce_NegativeDenominator()
    {
        // Action
        var (numerator, denominator) = MathEx.Reduce(4, -6);

        // Assert
        Assert.AreEqual(2, numerator);
        Assert.AreEqual(-3, denominator);
    }

    [TestMethod]
    public void Reduce_NegativeNumeratorAndDenominator()
    {
        // Action
        var (numerator, denominator) = MathEx.Reduce(-4, -6);

        // Assert
        Assert.AreEqual(-2, numerator);
        Assert.AreEqual(-3, denominator);
    }

    [TestMethod]
    public void Reduce_Long()
    {
        // Action
        var (numerator, denominator) = MathEx.Reduce(1_000_000L, 750_000L);

        // Assert
        Assert.AreEqual(4L, numerator);
        Assert.AreEqual(3L, denominator);
    }

    [TestMethod]
    public void Reduce_UnsignedInt()
    {
        // Action
        var (numerator, denominator) = MathEx.Reduce(12u, 18u);

        // Assert
        Assert.AreEqual(2u, numerator);
        Assert.AreEqual(3u, denominator);
    }

    [TestMethod]
    public void Reduce_LargeValues()
    {
        // Action
        var (numerator, denominator) = MathEx.Reduce(2147483646, 2);

        // Assert
        Assert.AreEqual(1073741823, numerator);
        Assert.AreEqual(1, denominator);
    }

    [TestMethod]
    public void Reduce_Decimal_CommonFactor()
    {
        // Action
        var (numerator, denominator) = MathEx.Reduce(1.5m, 2.5m);

        // Assert
        Assert.AreEqual(3m, numerator);
        Assert.AreEqual(5m, denominator);
    }

    [TestMethod]
    public void Reduce_Decimal_DifferentScales()
    {
        // Action
        var (numerator, denominator) = MathEx.Reduce(0.15m, 0.25m);

        // Assert
        Assert.AreEqual(3m, numerator);
        Assert.AreEqual(5m, denominator);
    }

    [TestMethod]
    public void Reduce_Decimal_NegativeNumerator()
    {
        // Action
        var (numerator, denominator) = MathEx.Reduce(-1.5m, 2.5m);

        // Assert
        Assert.AreEqual(-3m, numerator);
        Assert.AreEqual(5m, denominator);
    }

    [TestMethod]
    public void Reduce_Decimal_NegativeDenominator()
    {
        // Action
        var (numerator, denominator) = MathEx.Reduce(1.5m, -2.5m);

        // Assert
        Assert.AreEqual(-3m, numerator);
        Assert.AreEqual(5m, denominator);
    }

    [TestMethod]
    public void Reduce_Decimal_NumeratorZero()
    {
        // Action
        var (numerator, denominator) = MathEx.Reduce(0m, 0.5m);

        // Assert
        Assert.AreEqual(0m, numerator);
        Assert.AreEqual(1m, denominator);
    }

    [TestMethod]
    public void Reduce_Decimal_ZeroDenominatorThrows()
    {
        // Assert
        Assert.Throws<DivideByZeroException>(() => MathEx.Reduce(1.25m, 0m));
    }

    #endregion
}
