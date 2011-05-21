using System;
using NUnit.Framework;

namespace Plethora.Test
{
    /// <summary>
    /// Test class for the <see cref="NumericHelper"/> class.
    /// </summary>
    [TestFixture]
    public class NumericHelperTest
    {
        #region Translate

        [Test]
        public void Translate_Max()
        {
            //init
            double value = 0.6;
            double max = 10;

            //exec
            double valueOut = NumericHelper.Translate(value, max);

            //test
            Assert.AreEqual(6, valueOut);
        }

        [Test]
        public void Translate_MaxMin()
        {
            //init
            double value = 0.6;
            double max = 11;
            double min = 1;

            //exec
            double valueOut = NumericHelper.Translate(value, max, min);

            //test
            Assert.AreEqual(7, valueOut);
        }

        [Test]
        public void Translate_OldRange()
        {
            //init
            double value = 7;
            double max = 1;
            double min = 0;
            double maxOld = 11;
            double minOld = 1;

            //exec
            double valueOut = NumericHelper.Translate(value, max, min, maxOld, minOld);

            //test
            Assert.AreEqual(0.6, valueOut);
        }


        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Translate_Fail_Max_MaxLessThanZero()
        {
            //init
            double value = 0.6;
            double max = -1;

            //exec
            double valueOut = NumericHelper.Translate(value, max);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Translate_Fail_Max_ValueNotInRange()
        {
            //init
            double value = 1.6;
            double max = 10;

            //exec
            double valueOut = NumericHelper.Translate(value, max);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Translate_Fail_MaxMin_MaxLessThanMin()
        {
            //init
            double value = 0.6;
            double max = 1;
            double min = 2;

            //exec
            double valueOut = NumericHelper.Translate(value, max, min);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Translate_Fail_MaxMin_ValueNotInRange()
        {
            //init
            double value = 1.6;
            double max = 1;
            double min = 0;

            //exec
            double valueOut = NumericHelper.Translate(value, max, min);
        }


        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Translate_Fail_OldRange_OldMaxLessThanOldMax()
        {
            //init
            double value = 7;
            double max = 1;
            double min = 0;
            double maxOld = 1;
            double minOld = 11;

            //exec
            double valueOut = NumericHelper.Translate(value, max, min, maxOld, minOld);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Translate_Fail_OldRange_NewMaxLessThanNewMax()
        {
            //init
            double value = 7;
            double max = 0;
            double min = 1;
            double maxOld = 11;
            double minOld = 1;

            //exec
            double valueOut = NumericHelper.Translate(value, max, min, maxOld, minOld);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Translate_Fail_OldRange_ValueOutOfRange()
        {
            //init
            double value = 21;
            double max = 1;
            double min = 0;
            double maxOld = 11;
            double minOld = 1;

            //exec
            double valueOut = NumericHelper.Translate(value, max, min, maxOld, minOld);
        }
        #endregion

        #region Wrap

        [Test]
        public void Wrap_Low()
        {
            //init
            double value = -2;
            double min = 0;
            double max = 10;

            //exec
            double valueOut = NumericHelper.Wrap(value, min, max);

            Assert.AreEqual(8, valueOut);
        }

        [Test]
        public void Wrap_InRange()
        {
            //init
            double value = 5;
            double min = 0;
            double max = 10;

            //exec
            double valueOut = NumericHelper.Wrap(value, min, max);

            //test
            Assert.AreEqual(5, valueOut);
        }

        [Test]
        public void Wrap_High()
        {
            //init
            double value = 14;
            double min = 0;
            double max = 10;

            //exec
            double valueOut = NumericHelper.Wrap(value, min, max);

            //test
            Assert.AreEqual(4, valueOut);
        }

        [Test]
        public void Wrap_Int()
        {
            //init
            int value = 14;
            int min = 0;
            int max = 10;

            //exec
            int valueOut = NumericHelper.Wrap(value, min, max);

            //test
            Assert.AreEqual(4, valueOut);
        }

        [Test]
        public void Wrap_Decimal()
        {
            //init
            decimal value = 14m;
            decimal min = 0m;
            decimal max = 10m;

            //exec
            decimal valueOut = NumericHelper.Wrap(value, min, max);

            //test
            Assert.AreEqual(4m, valueOut);
        }

        [Test]
        public void Wrap_Float()
        {
            //init
            float value = 14f;
            float min = 0f;
            float max = 10f;

            //exec
            float valueOut = NumericHelper.Wrap(value, min, max);

            //test
            Assert.AreEqual(4f, valueOut);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Wrap_Fail_MaxLessThanMin()
        {
            //init
            double value = 14;
            double min = 10;
            double max = 0;

            //exec
            double valueOut = NumericHelper.Wrap(value, min, max);
        }
        #endregion

        #region Constrain

        [Test]
        public void Constrain_Low()
        {
            //init
            double value = -1.0;
            double min = 0.0;
            double max = 10.0;

            //exec
            double valueOut = NumericHelper.Constrain(value, min, max);

            //test
            Assert.AreEqual(min, valueOut);
        }

        [Test]
        public void Constrain_InRange()
        {
            //init
            double value = 5.0;
            double min = 0.0;
            double max = 10.0;

            //exec
            double valueOut = NumericHelper.Constrain(value, min, max);

            //test
            Assert.AreEqual(value, valueOut);
        }

        [Test]
        public void Constrain_High()
        {
            //init
            double value = 21.0;
            double min = 0.0;
            double max = 10.0;

            //exec
            double valueOut = NumericHelper.Constrain(value, min, max);

            //test
            Assert.AreEqual(max, valueOut);
        }

        [Test]
        public void Constrain_Int()
        {
            //init
            int value = 5;
            int min = 0;
            int max = 10;

            //exec
            int valueOut = NumericHelper.Constrain(value, min, max);

            //test
            Assert.AreEqual(value, valueOut);
        }

        [Test]
        public void Constrain_Decimal()
        {
            //init
            decimal value = 5m;
            decimal min = 0m;
            decimal max = 10m;

            //exec
            decimal valueOut = NumericHelper.Constrain(value, min, max);

            //test
            Assert.AreEqual(value, valueOut);
        }

        [Test]
        public void Constrain_Float()
        {
            //init
            float value = 5f;
            float min = 0f;
            float max = 10f;

            //exec
            float valueOut = NumericHelper.Constrain(value, min, max);

            //test
            Assert.AreEqual(value, valueOut);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Constrain_Fail_MaxLessThanMin()
        {
            //init
            double value = 14;
            double min = 10;
            double max = 0;

            //exec
            double valueOut = NumericHelper.Constrain(value, min, max);
        }
        #endregion

        #region Ordinals

        [Test]
        public void GetOrdinalSuffix()
        {
            //exec
            var suffix1 = NumericHelper.GetOrdinalSuffix(1);
            var suffix2 = NumericHelper.GetOrdinalSuffix(2);
            var suffix3 = NumericHelper.GetOrdinalSuffix(3);
            var suffix4 = NumericHelper.GetOrdinalSuffix(4);
            var suffix10 = NumericHelper.GetOrdinalSuffix(10);
            var suffix11 = NumericHelper.GetOrdinalSuffix(11);
            var suffix12 = NumericHelper.GetOrdinalSuffix(12);
            var suffix13 = NumericHelper.GetOrdinalSuffix(13);
            var suffix14 = NumericHelper.GetOrdinalSuffix(14);
            var suffix20 = NumericHelper.GetOrdinalSuffix(20);
            var suffix21 = NumericHelper.GetOrdinalSuffix(21);
            var suffix22 = NumericHelper.GetOrdinalSuffix(22);
            var suffix23 = NumericHelper.GetOrdinalSuffix(23);
            var suffix24 = NumericHelper.GetOrdinalSuffix(24);

            //test
            Assert.AreEqual("st", suffix1);
            Assert.AreEqual("nd", suffix2);
            Assert.AreEqual("rd", suffix3);
            Assert.AreEqual("th", suffix4);
            Assert.AreEqual("th", suffix10);
            Assert.AreEqual("th", suffix11);
            Assert.AreEqual("th", suffix12);
            Assert.AreEqual("th", suffix13);
            Assert.AreEqual("th", suffix14);
            Assert.AreEqual("th", suffix20);
            Assert.AreEqual("st", suffix21);
            Assert.AreEqual("nd", suffix22);
            Assert.AreEqual("rd", suffix23);
            Assert.AreEqual("th", suffix24);
        }
        #endregion

        #region Roman Numerals

        [Test]
        public void ToRomanNumerals()
        {
            //exec
            string result = NumericHelper.ToRomanNumerals(1987);

            //test
            Assert.AreEqual("MCMLXXXVII", result);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ToRomanNumerals_Fail_Zero()
        {
            //exec
            NumericHelper.ToRomanNumerals(0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ToRomanNumerals_Fail_TooSmall()
        {
            //exec
            NumericHelper.ToRomanNumerals(-1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ToRomanNumerals_Fail_TooBig()
        {
            //exec
            NumericHelper.ToRomanNumerals(4000);
        }

        [Test]
        public void FromRomanNumerals()
        {
            //init
            string numeral = "MCMLXXXVII";

            //exec
            int result = NumericHelper.FromRomanNumerals(numeral);

            //test
            Assert.AreEqual(1987, result);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FromRomanNumerals_Fail_Null()
        {
            //exec
            NumericHelper.FromRomanNumerals(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void FromRomanNumerals_Fail_Empty()
        {
            //exec
            NumericHelper.FromRomanNumerals("");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void FromRomanNumerals_Fail_InvalidNumeral()
        {
            //exec
            NumericHelper.FromRomanNumerals("XVK");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void FromRomanNumerals_Fail_InvalidSequence()
        {
            //exec
            NumericHelper.FromRomanNumerals("IVII");
        }
        #endregion
    }
}
