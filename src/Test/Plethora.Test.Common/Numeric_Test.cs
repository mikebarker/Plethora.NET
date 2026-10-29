using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Plethora.Test;

/// <summary>
/// Test class for the <see cref="Numeric"/> class.
/// </summary>
[TestClass]
public class NumericTest
{
    #region TryCreate

    [TestMethod]
    public void TryCreate_RepresentableValue()
    {
        // Action
        bool created = Numeric.TryCreate<int, long>(42, out long result);

        // Assert
        Assert.IsTrue(created);
        Assert.AreEqual(42L, result);
    }

    [TestMethod]
    public void TryCreate_ValueOutsideRange()
    {
        // Action
        bool created = Numeric.TryCreate<long, int>((long)int.MaxValue + 1, out int result);

        // Assert
        Assert.IsFalse(created);
    }

    [TestMethod]
    public void TryCreate_NegativeValueToUnsignedType()
    {
        // Action
        bool created = Numeric.TryCreate<int, uint>(-1, out uint result);

        // Assert
        Assert.IsFalse(created);
    }

    [TestMethod]
    public void TryCreate_FractionalValueToIntegerType()
    {
        // Action
        bool created = Numeric.TryCreate<double, int>(1.5, out int result);

        // Assert
        Assert.IsFalse(created);
    }

    #endregion

    #region Compare

    [TestMethod]
    public void Compare_EqualValuesOfDifferentTypes()
    {
        // Action
        int result = Numeric.Compare(42, 42L);

        // Assert
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Compare_LeftValueIsSmaller()
    {
        // Action
        int result = Numeric.Compare(41, 42L);

        // Assert
        Assert.IsTrue(result < 0);
    }

    [TestMethod]
    public void Compare_RightValueAboveLeftTypeRange()
    {
        // Action
        int result = Numeric.Compare(int.MaxValue, (long)int.MaxValue + 1);

        // Assert
        Assert.IsTrue(result < 0);
    }

    [TestMethod]
    public void Compare_RightValueBelowLeftTypeRange()
    {
        // Action
        int result = Numeric.Compare(int.MinValue, (long)int.MinValue - 1);

        // Assert
        Assert.IsTrue(result > 0);
    }

    #endregion

    #region CompareInRange

    [TestMethod]
    public void CompareInRange_EqualValuesOfDifferentTypes()
    {
        // Action
        int result = Numeric.CompareInRange(42, 42L);

        // Assert
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void CompareInRange_LeftValueIsGreater()
    {
        // Action
        int result = Numeric.CompareInRange(43, 42L);

        // Assert
        Assert.IsTrue(result > 0);
    }

    #endregion

    #region Translate

    [TestMethod]
    public void Translate_Max()
    {
        // Arrange
        double value = 0.6;
        double max = 10;

        // Action
        double valueOut = Numeric.Translate(value, max);

        // Assert
        Assert.AreEqual(6, valueOut);
    }

    [TestMethod]
    public void Translate_MaxMin()
    {
        // Arrange
        double value = 0.6;
        double max = 11;
        double min = 1;

        // Action
        double valueOut = Numeric.Translate(value, max, min);

        // Assert
        Assert.AreEqual(7, valueOut);
    }

    [TestMethod]
    public void Translate_OldRange()
    {
        // Arrange
        double value = 7;
        double max = 1;
        double min = 0;
        double maxOld = 11;
        double minOld = 1;

        // Action
        double valueOut = Numeric.Translate(value, max, min, maxOld, minOld);

        // Assert
        Assert.AreEqual(0.6, valueOut);
    }


    [TestMethod]
    public void Translate_Fail_Max_MaxLessThanZero()
    {
        // Arrange
        double value = 0.6;
        double max = -1;

        // Action
        Assert.Throws<ArgumentOutOfRangeException>(() => Numeric.Translate(value, max));
    }

    [TestMethod]
    public void Translate_Fail_Max_ValueNotInRange()
    {
        // Arrange
        double value = 1.6;
        double max = 10;

        // Action
        Assert.Throws<ArgumentOutOfRangeException>(() => Numeric.Translate(value, max));
    }

    [TestMethod]
    public void Translate_Fail_MaxMin_MaxLessThanMin()
    {
        // Arrange
        double value = 0.6;
        double max = 1;
        double min = 2;

        // Action
        Assert.Throws<ArgumentException>(() => Numeric.Translate(value, max, min));
    }

    [TestMethod]
    public void Translate_Fail_MaxMin_ValueNotInRange()
    {
        // Arrange
        double value = 1.6;
        double max = 1;
        double min = 0;

        // Action
        Assert.Throws<ArgumentOutOfRangeException>(() => Numeric.Translate(value, max, min));
    }


    [TestMethod]
    public void Translate_Fail_OldRange_OldMaxLessThanOldMax()
    {
        // Arrange
        double value = 7;
        double max = 1;
        double min = 0;
        double maxOld = 1;
        double minOld = 11;

        // Action
        Assert.Throws<ArgumentException>(() => Numeric.Translate(value, max, min, maxOld, minOld));
    }

    [TestMethod]
    public void Translate_Fail_OldRange_NewMaxLessThanNewMax()
    {
        // Arrange
        double value = 7;
        double max = 0;
        double min = 1;
        double maxOld = 11;
        double minOld = 1;

        // Action
        Assert.Throws<ArgumentException>(() => Numeric.Translate(value, max, min, maxOld, minOld));
    }

    [TestMethod]
    public void Translate_Fail_OldRange_ValueOutOfRange()
    {
        // Arrange
        double value = 21;
        double max = 1;
        double min = 0;
        double maxOld = 11;
        double minOld = 1;

        // Action
        Assert.Throws<ArgumentException>(() => Numeric.Translate(value, max, min, maxOld, minOld));
    }
    #endregion

    #region Saturate

    [TestMethod]
    public void Saturate_Low()
    {
        // Arrange
        double value = -1.0;
        double min = 0.0;
        double max = 10.0;

        // Action
        double valueOut = Numeric.Saturate(value, min, max);

        // Assert
        Assert.AreEqual(min, valueOut);
    }

    [TestMethod]
    public void Saturate_InRange()
    {
        // Arrange
        double value = 5.0;
        double min = 0.0;
        double max = 10.0;

        // Action
        double valueOut = Numeric.Saturate(value, min, max);

        // Assert
        Assert.AreEqual(value, valueOut);
    }

    [TestMethod]
    public void Saturate_High()
    {
        // Arrange
        double value = 21.0;
        double min = 0.0;
        double max = 10.0;

        // Action
        double valueOut = Numeric.Saturate(value, min, max);

        // Assert
        Assert.AreEqual(max, valueOut);
    }

    [TestMethod]
    public void Saturate_Int()
    {
        // Arrange
        int value = 5;
        int min = 0;
        int max = 10;

        // Action
        int valueOut = Numeric.Saturate(value, min, max);

        // Assert
        Assert.AreEqual(value, valueOut);
    }

    [TestMethod]
    public void Saturate_Decimal()
    {
        // Arrange
        decimal value = 5m;
        decimal min = 0m;
        decimal max = 10m;

        // Action
        decimal valueOut = Numeric.Saturate(value, min, max);

        // Assert
        Assert.AreEqual(value, valueOut);
    }

    [TestMethod]
    public void Saturate_Float()
    {
        // Arrange
        float value = 5f;
        float min = 0f;
        float max = 10f;

        // Action
        float valueOut = Numeric.Saturate(value, min, max);

        // Assert
        Assert.AreEqual(value, valueOut);
    }

    [TestMethod]
    public void Saturate_Fail_MaxLessThanMin()
    {
        // Arrange
        double value = 14;
        double min = 10;
        double max = 0;

        // Action
        Assert.Throws<ArgumentException>(() => Numeric.Saturate(value, min, max));
    }
    #endregion

    #region Roman Numerals

    [TestMethod]
    public void ToRomanNumerals()
    {
        // Action
        string result = Numeric.ToRomanNumerals(1987);

        // Assert
        Assert.AreEqual("MCMLXXXVII", result);
    }

    [TestMethod]
    public void ToRomanNumerals_Fail_Zero()
    {
        // Action
        Assert.Throws<ArgumentOutOfRangeException>(() => Numeric.ToRomanNumerals(0));
    }

    [TestMethod]
    public void ToRomanNumerals_Fail_TooSmall()
    {
        // Action
        Assert.Throws<ArgumentOutOfRangeException>(() => Numeric.ToRomanNumerals(-1));
    }

    [TestMethod]
    public void ToRomanNumerals_Fail_TooBig()
    {
        // Action
        Assert.Throws<ArgumentOutOfRangeException>(() => Numeric.ToRomanNumerals(4000));
    }

    [TestMethod]
    public void FromRomanNumerals()
    {
        // Arrange
        string numeral = "MCMLXXXVII";

        // Action
        int result = Numeric.FromRomanNumerals(numeral);

        // Assert
        Assert.AreEqual(1987, result);
    }

    [TestMethod]
    public void FromRomanNumerals_Fail_Null()
    {
        // Action
        Assert.Throws<ArgumentNullException>(() => Numeric.FromRomanNumerals(null));
    }

    [TestMethod]
    public void FromRomanNumerals_Fail_Empty()
    {
        // Action
        Assert.Throws<ArgumentException>(() => Numeric.FromRomanNumerals(""));
    }

    [TestMethod]
    public void FromRomanNumerals_Fail_InvalidNumeral()
    {
        // Action
        Assert.Throws<ArgumentException>(() => Numeric.FromRomanNumerals("XVK"));
    }

    [TestMethod]
    public void FromRomanNumerals_Fail_InvalidSequence()
    {
        // Action
        Assert.Throws<ArgumentException>(() => Numeric.FromRomanNumerals("MXM"));
    }
    #endregion
}
