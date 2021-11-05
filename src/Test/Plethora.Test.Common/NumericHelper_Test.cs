using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Plethora.Test
{
    /// <summary>
    /// Test class for the <see cref="NumericHelper"/> class.
    /// </summary>
    [TestClass]
    public class NumericHelperTest
    {
        #region Translate

        [TestMethod]
        public void Translate_Max()
        {
            // Arrange
            double value = 0.6;
            double max = 10;

            // Action
            double valueOut = NumericHelper.Translate(value, max);

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
            double valueOut = NumericHelper.Translate(value, max, min);

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
            double valueOut = NumericHelper.Translate(value, max, min, maxOld, minOld);

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
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => NumericHelper.Translate(value, max));
        }

        [TestMethod]
        public void Translate_Fail_Max_ValueNotInRange()
        {
            // Arrange
            double value = 1.6;
            double max = 10;

            // Action
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => NumericHelper.Translate(value, max));
        }

        [TestMethod]
        public void Translate_Fail_MaxMin_MaxLessThanMin()
        {
            // Arrange
            double value = 0.6;
            double max = 1;
            double min = 2;

            // Action
            Assert.ThrowsException<ArgumentException>(() => NumericHelper.Translate(value, max, min));
        }

        [TestMethod]
        public void Translate_Fail_MaxMin_ValueNotInRange()
        {
            // Arrange
            double value = 1.6;
            double max = 1;
            double min = 0;

            // Action
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => NumericHelper.Translate(value, max, min));
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
            Assert.ThrowsException<ArgumentException>(() => NumericHelper.Translate(value, max, min, maxOld, minOld));
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
            Assert.ThrowsException<ArgumentException>(() => NumericHelper.Translate(value, max, min, maxOld, minOld));
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
            Assert.ThrowsException<ArgumentException>(() => NumericHelper.Translate(value, max, min, maxOld, minOld));
        }
        #endregion

        #region Constrain

        [TestMethod]
        public void Constrain_Low()
        {
            // Arrange
            double value = -1.0;
            double min = 0.0;
            double max = 10.0;

            // Action
            double valueOut = NumericHelper.Constrain(value, min, max);

            // Assert
            Assert.AreEqual(min, valueOut);
        }

        [TestMethod]
        public void Constrain_InRange()
        {
            // Arrange
            double value = 5.0;
            double min = 0.0;
            double max = 10.0;

            // Action
            double valueOut = NumericHelper.Constrain(value, min, max);

            // Assert
            Assert.AreEqual(value, valueOut);
        }

        [TestMethod]
        public void Constrain_High()
        {
            // Arrange
            double value = 21.0;
            double min = 0.0;
            double max = 10.0;

            // Action
            double valueOut = NumericHelper.Constrain(value, min, max);

            // Assert
            Assert.AreEqual(max, valueOut);
        }

        [TestMethod]
        public void Constrain_Int()
        {
            // Arrange
            int value = 5;
            int min = 0;
            int max = 10;

            // Action
            int valueOut = NumericHelper.Constrain(value, min, max);

            // Assert
            Assert.AreEqual(value, valueOut);
        }

        [TestMethod]
        public void Constrain_Decimal()
        {
            // Arrange
            decimal value = 5m;
            decimal min = 0m;
            decimal max = 10m;

            // Action
            decimal valueOut = NumericHelper.Constrain(value, min, max);

            // Assert
            Assert.AreEqual(value, valueOut);
        }

        [TestMethod]
        public void Constrain_Float()
        {
            // Arrange
            float value = 5f;
            float min = 0f;
            float max = 10f;

            // Action
            float valueOut = NumericHelper.Constrain(value, min, max);

            // Assert
            Assert.AreEqual(value, valueOut);
        }

        [TestMethod]
        public void Constrain_Fail_MaxLessThanMin()
        {
            // Arrange
            double value = 14;
            double min = 10;
            double max = 0;

            // Action
            Assert.ThrowsException<ArgumentException>(() => NumericHelper.Constrain(value, min, max));
        }
        #endregion

        #region Roman Numerals

        [TestMethod]
        public void ToRomanNumerals()
        {
            // Action
            string result = NumericHelper.ToRomanNumerals(1987);

            // Assert
            Assert.AreEqual("MCMLXXXVII", result);
        }

        [TestMethod]
        public void ToRomanNumerals_Fail_Zero()
        {
            // Action
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => NumericHelper.ToRomanNumerals(0));
        }

        [TestMethod]
        public void ToRomanNumerals_Fail_TooSmall()
        {
            // Action
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => NumericHelper.ToRomanNumerals(-1));
        }

        [TestMethod]
        public void ToRomanNumerals_Fail_TooBig()
        {
            // Action
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => NumericHelper.ToRomanNumerals(4000));
        }

        [TestMethod]
        public void FromRomanNumerals()
        {
            // Arrange
            string numeral = "MCMLXXXVII";

            // Action
            int result = NumericHelper.FromRomanNumerals(numeral);

            // Assert
            Assert.AreEqual(1987, result);
        }

        [TestMethod]
        public void FromRomanNumerals_Fail_Null()
        {
            // Action
            Assert.ThrowsException<ArgumentNullException>(() => NumericHelper.FromRomanNumerals(null));
        }

        [TestMethod]
        public void FromRomanNumerals_Fail_Empty()
        {
            // Action
            Assert.ThrowsException<ArgumentException>(() => NumericHelper.FromRomanNumerals(""));
        }

        [TestMethod]
        public void FromRomanNumerals_Fail_InvalidNumeral()
        {
            // Action
            Assert.ThrowsException<ArgumentException>(() => NumericHelper.FromRomanNumerals("XVK"));
        }

        [TestMethod]
        public void FromRomanNumerals_Fail_InvalidSequence()
        {
            // Action
            Assert.ThrowsException<ArgumentException>(() => NumericHelper.FromRomanNumerals("IVII"));
        }
        #endregion
    }
}
