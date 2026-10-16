using System;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Plethora.Test;

[TestClass]
public class Rational_Test
{
    #region Constructor

    [TestMethod]
    public void Constructor()
    {
        // Action
        Rational rational = new(1, 2);

        // Assert
        Assert.AreEqual(1, rational.Numerator);
        Assert.AreEqual(2u, rational.Denominator);
    }

    [TestMethod]
    public void Constructor_CanonicalFormCommonDivisor()
    {
        // Action
        Rational rational = new(2, 4);

        // Assert
        Assert.AreEqual(1, rational.Numerator);
        Assert.AreEqual(2u, rational.Denominator);
    }

    [TestMethod]
    public void Constructor_CanonicalFormNumeratorNegative()
    {
        // Action
        Rational rational = new(-1, 2);

        // Assert
        Assert.AreEqual(-1, rational.Numerator);
        Assert.AreEqual(2u, rational.Denominator);
    }

    [TestMethod]
    public void Constructor_CanonicalFormDenominatorNegative()
    {
        // Action
        Rational rational = new(1, -2);

        // Assert
        Assert.AreEqual(-1, rational.Numerator);
        Assert.AreEqual(2u, rational.Denominator);
    }

    [TestMethod]
    public void Constructor_NumeratorZero()
    {
        // Action
        Rational rational = new(0, 12);

        // Assert
        Assert.AreEqual(0, rational.Numerator);
        Assert.AreEqual(1u, rational.Denominator);
    }

    [TestMethod]
    public void Constructor_Error_DenominatorZero()
    {
        // Action & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new Rational(1, 0));
    }

    [TestMethod]
    public void Constructor_Error_DenominatorNegativeMaximum()
    {
        // Action & Assert
        Assert.Throws<OverflowException>(() => new Rational(1, int.MinValue));
    }

    #endregion

    #region Equality

    [TestMethod]
    public void Equals_AreEqual()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(1, 2);

        // Action
        bool result = x.Equals(y);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Equals_CanonicalAreEqual()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(2, 4);

        // Action
        bool result = x.Equals(y);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Equals_CanonicalNegativeAreEqual()
    {
        // Arrange
        Rational x = new(-1, 2);
        Rational y = new(1, -2);

        // Action
        bool result = x.Equals(y);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Equals_NumeratorAreNotEqual()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(3, 2);

        // Action
        bool result = x.Equals(y);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Equals_DenominatorAreNotEqual()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(1, 3);

        // Action
        bool result = x.Equals(y);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Equals_Null()
    {
        // Arrange
        Rational x = new(1, 2);

        // Action
        bool result = x.Equals(null);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Equals_String()
    {
        // Arrange
        Rational x = new(1, 2);

        // Action
        bool result = x.Equals("test");

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void GetHashCode_AreEqual()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(1, 2);

        // Action
        int xHash = x.GetHashCode();
        int yHash = y.GetHashCode();

        // Assert
        Assert.AreEqual(yHash, xHash);
    }

    [TestMethod]
    public void GetHashCode_CanonicalAreEqual()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(2, 4);

        // Action
        int xHash = x.GetHashCode();
        int yHash = y.GetHashCode();

        // Assert
        Assert.AreEqual(yHash, xHash);
    }

    [TestMethod]
    public void GetHashCode_CanonicalNegativeAreEqual()
    {
        // Arrange
        Rational x = new(-1, 2);
        Rational y = new(1, -2);

        // Action
        int xHash = x.GetHashCode();
        int yHash = y.GetHashCode();

        // Assert
        Assert.AreEqual(yHash, xHash);
    }

    [TestMethod]
    public void GetHashCode_NumeratorAreNotEqual()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(3, 2);

        // Action
        int xHash = x.GetHashCode();
        int yHash = y.GetHashCode();

        // Assert
        Assert.AreNotEqual(yHash, xHash);
    }

    [TestMethod]
    public void GetHashCode_DenominatorAreNotEqual()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(1, 3);

        // Action
        int xHash = x.GetHashCode();
        int yHash = y.GetHashCode();

        // Assert
        Assert.AreNotEqual(yHash, xHash);
    }

    [TestMethod]
    public void GetHashCode_Inverse()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(2, 1);

        // Action
        int xHash = x.GetHashCode();
        int yHash = y.GetHashCode();

        // Assert
        Assert.AreNotEqual(yHash, xHash);
    }

    #endregion

    #region CompareTo

    [TestMethod]
    public void CompareTo_AreEqual()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(1, 2);

        // Action
        int result = x.CompareTo(y);

        // Assert
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void CompareTo_AreEqual_Negative()
    {
        // Arrange
        Rational x = new(-1, 2);
        Rational y = new(-1, 2);

        // Action
        int result = x.CompareTo(y);

        // Assert
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void CompareTo_LessThan()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(3, 2);

        // Action
        int result = x.CompareTo(y);

        // Assert
        Assert.AreEqual(-1, result);
    }

    [TestMethod]
    public void CompareTo_GreaterThan()
    {
        // Arrange
        Rational x = new(3, 2);
        Rational y = new(1, 2);

        // Action
        int result = x.CompareTo(y);

        // Assert
        Assert.AreEqual(1, result);
    }

    [TestMethod]
    public void CompareTo_Negative_LessThan()
    {
        // Arrange
        Rational x = new(-1, 2);
        Rational y = new(1, 2);

        // Action
        int result = x.CompareTo(y);

        // Assert
        Assert.AreEqual(-1, result);
    }

    [TestMethod]
    public void CompareTo_Negative_GreaterThan()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(-1, 2);

        // Action
        int result = x.CompareTo(y);

        // Assert
        Assert.AreEqual(1, result);
    }

    [TestMethod]
    public void CompareTo_Null()
    {
        // Arrange
        Rational x = new(1, 2);

        // Action
        int result = ((IComparable)x).CompareTo(null);

        // Assert
        Assert.AreEqual(1, result);
    }

    [TestMethod]
    public void CompareTo_String()
    {
        // Arrange
        Rational x = new(1, 2);

        // Action
        Assert.Throws<ArgumentException>(() => ((IComparable)x).CompareTo("test"));
    }

    #endregion

    #region Convertion

    [TestMethod]
    public void ToDouble()
    {
        // Arrange
        Rational x = new(1, 2);

        // Action
        double result = x.ToDouble();

        // Assert
        Assert.AreEqual(0.5, result);
    }

    [TestMethod]
    public void ToDecimal()
    {
        // Arrange
        Rational x = new(1, 2);

        // Action
        decimal result = x.ToDecimal();

        // Assert
        Assert.AreEqual(0.5m, result);
    }

    [TestMethod]
    public void Cast_Double()
    {
        // Arrange
        Rational x = new(1, 2);

        // Action
        double result = (double)x;

        // Assert
        Assert.AreEqual(0.5, result);
    }

    [TestMethod]
    public void Cast_Decimal()
    {
        // Arrange
        Rational x = new(1, 2);

        // Action
        decimal result = (decimal)x;

        // Assert
        Assert.AreEqual(0.5m, result);
    }

    #endregion

    #region Operators

    #region Logical operators

    #region == operator

    [TestMethod]
    public void Op_Equality_AreEqual()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(1, 2);

        // Action
        bool result = (x == y);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Op_Equality_CanonicalAreEqual()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(2, 4);

        // Action
        bool result = (x == y);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Op_Equality_CanonicalNegativeAreEqual()
    {
        // Arrange
        Rational x = new(-1, 2);
        Rational y = new(1, -2);

        // Action
        bool result = (x == y);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Op_Equality_NumeratorAreNotEqual()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(3, 2);

        // Action
        bool result = (x == y);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Op_Equality_DenominatorAreNotEqual()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(1, 3);

        // Action
        bool result = (x == y);

        // Assert
        Assert.IsFalse(result);
    }

    #endregion

    #region != operator

    [TestMethod]
    public void Op_Inequality_AreEqual()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(1, 2);

        // Action
        bool result = (x != y);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Op_Inequality_CanonicalAreEqual()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(2, 4);

        // Action
        bool result = (x != y);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Op_Inequality_CanonicalNegativeAreEqual()
    {
        // Arrange
        Rational x = new(-1, 2);
        Rational y = new(1, -2);

        // Action
        bool result = (x != y);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Op_Inequality_NumeratorAreNotEqual()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(3, 2);

        // Action
        bool result = (x != y);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Op_Inequality_DenominatorAreNotEqual()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(1, 3);

        // Action
        bool result = (x != y);

        // Assert
        Assert.IsTrue(result);
    }

    #endregion

    #region < operator

    [TestMethod]
    public void Op_LessThan_AreEqual()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(1, 2);

        // Action
        bool result = (x < y);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Op_LessThan_AreEqual_Negative()
    {
        // Arrange
        Rational x = new(-1, 2);
        Rational y = new(-1, 2);

        // Action
        bool result = (x < y);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Op_LessThan_LessThan()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(3, 2);

        // Action
        bool result = (x < y);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Op_LessThan_GreaterThan()
    {
        // Arrange
        Rational x = new(3, 2);
        Rational y = new(1, 2);

        // Action
        bool result = (x < y);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Op_LessThan_Negative_LessThan()
    {
        // Arrange
        Rational x = new(-1, 2);
        Rational y = new(1, 2);

        // Action
        bool result = (x < y);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Op_LessThan_Negative_GreaterThan()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(-1, 2);

        // Action
        bool result = (x < y);

        // Assert
        Assert.IsFalse(result);
    }

    #endregion

    #region > operator

    [TestMethod]
    public void Op_GreaterThan_AreEqual()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(1, 2);

        // Action
        bool result = (x > y);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Op_GreaterThan_AreEqual_Negative()
    {
        // Arrange
        Rational x = new(-1, 2);
        Rational y = new(-1, 2);

        // Action
        bool result = (x > y);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Op_GreaterThan_LessThan()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(3, 2);

        // Action
        bool result = (x > y);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Op_GreaterThan_GreaterThan()
    {
        // Arrange
        Rational x = new(3, 2);
        Rational y = new(1, 2);

        // Action
        bool result = (x > y);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Op_GreaterThan_Negative_LessThan()
    {
        // Arrange
        Rational x = new(-1, 2);
        Rational y = new(1, 2);

        // Action
        bool result = (x > y);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Op_GreaterThan_Negative_GreaterThan()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(-1, 2);

        // Action
        bool result = (x > y);

        // Assert
        Assert.IsTrue(result);
    }

    #endregion

    #region <= operator

    [TestMethod]
    public void Op_LessThanEqual_AreEqual()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(1, 2);

        // Action
        bool result = (x <= y);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Op_LessThanEqual_AreEqual_Negative()
    {
        // Arrange
        Rational x = new(-1, 2);
        Rational y = new(-1, 2);

        // Action
        bool result = (x <= y);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Op_LessThanEqual_LessThan()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(3, 2);

        // Action
        bool result = (x <= y);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Op_LessThanEqual_GreaterThan()
    {
        // Arrange
        Rational x = new(3, 2);
        Rational y = new(1, 2);

        // Action
        bool result = (x <= y);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Op_LessThanEqual_Negative_LessThan()
    {
        // Arrange
        Rational x = new(-1, 2);
        Rational y = new(1, 2);

        // Action
        bool result = (x <= y);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Op_LessThanEqual_Negative_GreaterThan()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(-1, 2);

        // Action
        bool result = (x <= y);

        // Assert
        Assert.IsFalse(result);
    }

    #endregion

    #region >= operator

    [TestMethod]
    public void Op_GreaterThanEqual_AreEqual()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(1, 2);

        // Action
        bool result = (x >= y);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Op_GreaterThanEqual_AreEqual_Negative()
    {
        // Arrange
        Rational x = new(-1, 2);
        Rational y = new(-1, 2);

        // Action
        bool result = (x >= y);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Op_GreaterThanEqual_LessThan()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(3, 2);

        // Action
        bool result = (x >= y);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Op_GreaterThanEqual_GreaterThan()
    {
        // Arrange
        Rational x = new(3, 2);
        Rational y = new(1, 2);

        // Action
        bool result = (x >= y);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Op_GreaterThanEqual_Negative_LessThan()
    {
        // Arrange
        Rational x = new(-1, 2);
        Rational y = new(1, 2);

        // Action
        bool result = (x >= y);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Op_GreaterThanEqual_Negative_GreaterThan()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(-1, 2);

        // Action
        bool result = (x >= y);

        // Assert
        Assert.IsTrue(result);
    }

    #endregion

    #endregion

    #region Algebraic operators

    #region Additive operators

    [TestMethod]
    public void Op_Addition_Rationals()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(3, 5);

        // Action
        Rational result = (x + y);

        // Assert
        Assert.AreEqual(11, result.Numerator);
        Assert.AreEqual(10u, result.Denominator);
    }

    [TestMethod]
    public void Op_Addition_Rationals_ReducesBeforeNarrowing()
    {
        // Arrange
        Rational x = new Rational(1, int.MaxValue);

        // Action
        Rational result = x + x;

        // Assert
        Rational expected = new Rational(2, int.MaxValue);
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Op_Addition_Rationals_ThrowsWhenResultCannotBeRepresented()
    {
        Rational x = new Rational(int.MaxValue, 1);

        Assert.Throws<OverflowException>(() => x + 1);
    }

    [TestMethod]
    public void Op_Addition_Rational_Int32()
    {
        // Arrange
        Rational x = new(1, 2);
        int y = 5;

        // Action
        Rational result = (x + y);

        // Assert
        Assert.AreEqual(11, result.Numerator);
        Assert.AreEqual(2u, result.Denominator);
    }

    [TestMethod]
    public void Op_Addition_Int32_Rational()
    {
        // Arrange
        int x = 5;
        Rational y = new(1, 2);

        // Action
        Rational result = (x + y);

        // Assert
        Assert.AreEqual(11, result.Numerator);
        Assert.AreEqual(2u, result.Denominator);
    }


    [TestMethod]
    public void Op_Subtraction_Rationals()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(3, 5);

        // Action
        Rational result = (x - y);

        // Assert
        Assert.AreEqual(-1, result.Numerator);
        Assert.AreEqual(10u, result.Denominator);
    }

    [TestMethod]
    public void Op_Subtraction_Rationals_ReducesBeforeNarrowing()
    {
        // Arrange
        Rational x = new(2, int.MaxValue);
        Rational y = new(1, int.MaxValue);

        // Action
        Rational result = x - y;

        // Assert
        Assert.AreEqual(new Rational(1, int.MaxValue), result);
    }

    [TestMethod]
    public void Op_Subtraction_Rationals_ThrowsWhenResultCannotBeRepresented()
    {
        Rational x = new(int.MinValue, 1);

        Assert.Throws<OverflowException>(() => x - 1);
    }

    [TestMethod]
    public void Op_Subtraction_Rational_Int32()
    {
        // Arrange
        Rational x = new(1, 2);
        int y = 5;

        // Action
        Rational result = (x - y);

        // Assert
        Assert.AreEqual(-9, result.Numerator);
        Assert.AreEqual(2u, result.Denominator);
    }

    [TestMethod]
    public void Op_Subtraction_Int32_Rational()
    {
        // Arrange
        int x = 5;
        Rational y = new(1, 2);

        // Action
        Rational result = (x - y);

        // Assert
        Assert.AreEqual(9, result.Numerator);
        Assert.AreEqual(2u, result.Denominator);
    }

    #endregion

    #region Multiplicative operators

    [TestMethod]
    public void Op_Multiply_Rationals()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(3, 5);

        // Action
        Rational result = (x * y);

        // Assert
        Assert.AreEqual(3, result.Numerator);
        Assert.AreEqual(10u, result.Denominator);
    }

    [TestMethod]
    public void Op_Multiply_Rationals_ThrowsWhenResultCannotBeRepresented()
    {
        Rational x = new(50_000, 1);

        Assert.Throws<OverflowException>(() => x * x);
    }

    [TestMethod]
    public void Op_Multiply_Integer_ThrowsWhenResultCannotBeRepresented()
    {
        Rational x = new(50_000, 1);

        Assert.Throws<OverflowException>(() => x * 50_000);
    }

    [TestMethod]
    public void Op_Multiply_Rational_Int32()
    {
        // Arrange
        Rational x = new(1, 2);
        int y = 5;

        // Action
        Rational result = (x * y);

        // Assert
        Assert.AreEqual(5, result.Numerator);
        Assert.AreEqual(2u, result.Denominator);
    }

    [TestMethod]
    public void Op_Multiply_Int32_Rational()
    {
        // Arrange
        int x = 5;
        Rational y = new(1, 2);

        // Action
        Rational result = (x * y);

        // Assert
        Assert.AreEqual(5, result.Numerator);
        Assert.AreEqual(2u, result.Denominator);
    }


    [TestMethod]
    public void Op_Division_Rationals()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(3, 5);

        // Action
        Rational result = (x / y);

        // Assert
        Assert.AreEqual(5, result.Numerator);
        Assert.AreEqual(6u, result.Denominator);
    }

    [TestMethod]
    public void Op_Division_Rationals_ReducesBeforeNarrowing()
    {
        // Arrange
        Rational x = new(2, 1);
        Rational y = new(int.MinValue, 1);

        // Action
        Rational result = x / y;

        // Assert
        Assert.AreEqual(new Rational(-1, 1_073_741_824), result);
    }

    [TestMethod]
    public void Op_Division_Rational_Integer_ReducesBeforeNarrowing()
    {
        Rational x = new(2, 1);

        Rational result = x / int.MinValue;

        Assert.AreEqual(new Rational(-1, 1_073_741_824), result);
    }

    [TestMethod]
    public void Op_Division_Integer_Rational_ReducesBeforeNarrowing()
    {
        Rational result = int.MinValue / new Rational(-2, 1);

        Assert.AreEqual(new Rational(1_073_741_824, 1), result);
    }

    [TestMethod]
    public void Op_Division_Rationals_ThrowsWhenResultCannotBeRepresented()
    {
        Rational x = new(int.MinValue, 1);

        Assert.Throws<OverflowException>(() => x / new Rational(-1, 1));
    }

    [TestMethod]
    public void Op_Division_Rational_Int32()
    {
        // Arrange
        Rational x = new(1, 2);
        int y = 5;

        // Action
        Rational result = (x / y);

        // Assert
        Assert.AreEqual(1, result.Numerator);
        Assert.AreEqual(10u, result.Denominator);
    }

    [TestMethod]
    public void Op_Division_Int32_Rational()
    {
        // Arrange
        int x = 5;
        Rational y = new(1, 2);

        // Action
        Rational result = (x / y);

        // Assert
        Assert.AreEqual(10, result.Numerator);
        Assert.AreEqual(1u, result.Denominator);
    }


    [TestMethod]
    public void DivRem()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(3, 5);

        // Action
        var result = Rational.DivRem(x, y);

        // Assert
        // 1/2 / 3/5 = 0 r 5/6
        Assert.AreEqual(0, result.Quotient);
        Assert.AreEqual(new(5, 6), result.Remainder);
    }

    [TestMethod]
    public void Op_Modulus_Rationals()
    {
        // Arrange
        Rational x = new(1, 2);
        Rational y = new(3, 5);

        // Action
        var result = (x % y);

        // Assert
        // 1/2 % 3/5 = 5/6
        Assert.AreEqual(new(5, 6), result);
    }

    [TestMethod]
    public void Op_Modulus_Rational_Int32()
    {
        // Arrange
        Rational x = new(43, 4);
        int y = 5;

        // Action
        var result = (x % y);

        // Assert
        // 43/4 % 5/1 = 3/20 (quotient = 4)
        Assert.AreEqual(new(3, 20), result);
    }

    [TestMethod]
    public void Op_Modulus_Int32_Rational()
    {
        // Arrange
        int x = 5;
        Rational y = new(3, 4);

        // Action
        var result = (x % y);

        // Assert
        // 5/1 % 3/4 = 2/3 (quotient = 6)
        Assert.AreEqual(new(2, 3), result);
    }

    #endregion

    #endregion

    #endregion

    #region Invert

    [TestMethod]
    public void Invert()
    {
        // Arrange
        Rational x = new(1, 2);

        // Action
        Rational result = x.Invert();

        // Assert
        Assert.AreEqual(2, result.Numerator);
        Assert.AreEqual(1u, result.Denominator);
    }

    [TestMethod]
    public void Invert_Overflow()
    {
        // Arrange
        Rational x = new(1, uint.MaxValue);

        // Action & Assert
        Assert.Throws<OverflowException>(() => x.Invert());
    }

    [TestMethod]
    public void Invert_Overflow2()
    {
        // Arrange
        Rational x = new(-1, int.MaxValue + 2u);

        // Action & Assert
        Assert.Throws<OverflowException>(() => x.Invert());
    }

    [TestMethod]
    public void Invert_Denominator_IntMinValue_Negative()
    {
        // Arrange
        Rational x = new(-1, int.MaxValue + 1u);

        // Action
        Rational result = x.Invert();

        // Assert
        Assert.AreEqual(int.MinValue, (long)result.Numerator);
        Assert.AreEqual(1u, result.Denominator);
    }

    [TestMethod]
    public void Invert_Denominator_IntMinValue_Positive()
    {
        // Arrange
        Rational x = new(1, int.MaxValue + 1u);

        // Action & Assert
        Assert.Throws<OverflowException>(() => x.Invert());
    }

    [TestMethod]
    public void Invert_Negative()
    {
        // Arrange
        Rational x = new(-1, 2);

        // Action
        Rational result = x.Invert();

        // Assert
        Assert.AreEqual(-2, result.Numerator);
        Assert.AreEqual(1u, result.Denominator);
    }

    [TestMethod]
    public void Invert_Negative_DenominatorIntMinValue()
    {
        // Arrange
        Rational x = new(-1, unchecked((uint)int.MinValue));

        // Action
        Rational result = x.Invert();

        // Assert
        Assert.AreEqual(-2147483648, result.Numerator);
        Assert.AreEqual(1u, result.Denominator);
    }

    [TestMethod]
    public void Invert_Error_NumeratorZero()
    {
        // Arrange
        Rational x = new(0, 2);

        // Action & Assert
        Assert.Throws<DivideByZeroException>(() => x.Invert());
    }

    #endregion
}
