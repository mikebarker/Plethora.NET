using System;

using NUnit.Framework;

namespace Plethora.Test
{
    [TestFixture]
    public class Rational_Test
    {
        #region Constructor

        [Test]
        public void Constructor()
        {
            //exec
            Rational rational = new Rational(1, 2);

            //test
            Assert.AreEqual(1, rational.Numerator);
            Assert.AreEqual(2, rational.Denominator);
        }

        [Test]
        public void Constructor_CanonicalFormCommonDivisor()
        {
            //exec
            Rational rational = new Rational(2, 4);

            //test
            Assert.AreEqual(1, rational.Numerator);
            Assert.AreEqual(2, rational.Denominator);
        }

        [Test]
        public void Constructor_CanonicalFormNumeratorNegative()
        {
            //exec
            Rational rational = new Rational(-1, 2);

            //test
            Assert.AreEqual(-1, rational.Numerator);
            Assert.AreEqual(2, rational.Denominator);
        }

        [Test]
        public void Constructor_CanonicalFormDenominatorNegative()
        {
            //exec
            Rational rational = new Rational(1, -2);

            //test
            Assert.AreEqual(-1, rational.Numerator);
            Assert.AreEqual(2, rational.Denominator);
        }

        [Test]
        public void Constructor_NumeratorZero()
        {
            //exec
            Rational rational = new Rational(0, 12);

            //test
            Assert.AreEqual(0, rational.Numerator);
            Assert.AreEqual(1, rational.Denominator);
        }

        [Test]
        public void Constructor_Error_DenominatorZero()
        {
            //exec
            try
            {
                Rational rational = new Rational(1, 0);

                Assert.Fail();
            }
            catch (DivideByZeroException ex)
            {
            }
        }

        [Test]
        public void Constructor_Error_DenominatorNegativeMaximum()
        {
            //exec
            try
            {
                Rational rational = new Rational(1, int.MinValue);

                Assert.Fail();
            }
            catch (OverflowException ex)
            {
            }
        }

        #endregion

        #region Equality

        [Test]
        public void Equals_AreEqual()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(1, 2);

            //exec
            bool result = x.Equals(y);

            //test
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Equals_CanonicalAreEqual()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(2, 4);

            //exec
            bool result = x.Equals(y);

            //test
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Equals_CanonicalNegativeAreEqual()
        {
            //setup
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(1, -2);

            //exec
            bool result = x.Equals(y);

            //test
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Equals_NumeratorAreNotEqual()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 2);

            //exec
            bool result = x.Equals(y);

            //test
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Equals_DenominatorAreNotEqual()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(1, 3);

            //exec
            bool result = x.Equals(y);

            //test
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Equals_Null()
        {
            //setup
            Rational x = new Rational(1, 2);

            //exec
            bool result = x.Equals(null);

            //test
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Equals_String()
        {
            //setup
            Rational x = new Rational(1, 2);

            //exec
            bool result = x.Equals("test");

            //test
            Assert.AreEqual(false, result);
        }

        [Test]
        public void GetHashCode_AreEqual()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(1, 2);

            //exec
            int xHash = x.GetHashCode();
            int yHash = y.GetHashCode();

            //test
            Assert.AreEqual(true, xHash == yHash);
        }

        [Test]
        public void GetHashCode_CanonicalAreEqual()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(2, 4);

            //exec
            int xHash = x.GetHashCode();
            int yHash = y.GetHashCode();

            //test
            Assert.AreEqual(true, xHash == yHash);
        }

        [Test]
        public void GetHashCode_CanonicalNegativeAreEqual()
        {
            //setup
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(1, -2);

            //exec
            int xHash = x.GetHashCode();
            int yHash = y.GetHashCode();

            //test
            Assert.AreEqual(true, xHash == yHash);
        }

        [Test]
        public void GetHashCode_NumeratorAreNotEqual()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 2);

            //exec
            int xHash = x.GetHashCode();
            int yHash = y.GetHashCode();

            //test
            Assert.AreEqual(false, xHash == yHash);
        }

        [Test]
        public void GetHashCode_DenominatorAreNotEqual()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(1, 3);

            //exec
            int xHash = x.GetHashCode();
            int yHash = y.GetHashCode();

            //test
            Assert.AreEqual(false, xHash == yHash);
        }

        [Test]
        public void GetHashCode_Inverse()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(2, 1);

            //exec
            int xHash = x.GetHashCode();
            int yHash = y.GetHashCode();

            //test
            Assert.AreEqual(false, xHash == yHash);
        }

        #endregion

        #region CompareTo

        [Test]
        public void CompareTo_AreEqual()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(1, 2);

            //exec
            int result = x.CompareTo(y);

            //test
            Assert.AreEqual(0, result);
        }

        [Test]
        public void CompareTo_AreEqual_Negative()
        {
            //setup
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(-1, 2);

            //exec
            int result = x.CompareTo(y);

            //test
            Assert.AreEqual(0, result);
        }

        [Test]
        public void CompareTo_LessThan()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 2);

            //exec
            int result = x.CompareTo(y);

            //test
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void CompareTo_GreaterThan()
        {
            //setup
            Rational x = new Rational(3, 2);
            Rational y = new Rational(1, 2);

            //exec
            int result = x.CompareTo(y);

            //test
            Assert.AreEqual(1, result);
        }

        [Test]
        public void CompareTo_Negative_LessThan()
        {
            //setup
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(1, 2);

            //exec
            int result = x.CompareTo(y);

            //test
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void CompareTo_Negative_GreaterThan()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(-1, 2);

            //exec
            int result = x.CompareTo(y);

            //test
            Assert.AreEqual(1, result);
        }

        [Test]
        public void CompareTo_Null()
        {
            //setup
            Rational x = new Rational(1, 2);

            //exec
            int result = ((IComparable)x).CompareTo(null);

            //test
            Assert.AreEqual(1, result);
        }

        [Test]
        public void CompareTo_String()
        {
            //setup
            Rational x = new Rational(1, 2);

            //exec
            try
            {
                int result = ((IComparable)x).CompareTo("test");

                Assert.Fail();
            }
            catch (ArgumentException e)
            {
            }
        }

        #endregion

        #region Convertion

        [Test]
        public void ToDouble()
        {
            //setup
            Rational x = new Rational(1, 2);

            //exec
            double result = x.ToDouble();

            //test
            Assert.AreEqual(0.5, result);
        }

        [Test]
        public void ToDecimal()
        {
            //setup
            Rational x = new Rational(1, 2);

            //exec
            decimal result = x.ToDecimal();

            //test
            Assert.AreEqual(0.5m, result);
        }

        [Test]
        public void Cast_Double()
        {
            //setup
            Rational x = new Rational(1, 2);

            //exec
            double result = (double)x;

            //test
            Assert.AreEqual(0.5, result);
        }

        [Test]
        public void Cast_Decimal()
        {
            //setup
            Rational x = new Rational(1, 2);

            //exec
            decimal result = (decimal)x;

            //test
            Assert.AreEqual(0.5m, result);
        }

        #endregion

        #region Operators

        #region Logical operators

        #region == operator

        [Test]
        public void Op_Equality_AreEqual()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(1, 2);

            //exec
            bool result = (x == y);

            //test
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Op_Equality_CanonicalAreEqual()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(2, 4);

            //exec
            bool result = (x == y);

            //test
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Op_Equality_CanonicalNegativeAreEqual()
        {
            //setup
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(1, -2);

            //exec
            bool result = (x == y);

            //test
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Op_Equality_NumeratorAreNotEqual()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 2);

            //exec
            bool result = (x == y);

            //test
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Op_Equality_DenominatorAreNotEqual()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(1, 3);

            //exec
            bool result = (x == y);

            //test
            Assert.AreEqual(false, result);
        }

        #endregion

        #region != operator

        [Test]
        public void Op_Inequality_AreEqual()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(1, 2);

            //exec
            bool result = (x != y);

            //test
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Op_Inequality_CanonicalAreEqual()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(2, 4);

            //exec
            bool result = (x != y);

            //test
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Op_Inequality_CanonicalNegativeAreEqual()
        {
            //setup
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(1, -2);

            //exec
            bool result = (x != y);

            //test
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Op_Inequality_NumeratorAreNotEqual()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 2);

            //exec
            bool result = (x != y);

            //test
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Op_Inequality_DenominatorAreNotEqual()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(1, 3);

            //exec
            bool result = (x != y);

            //test
            Assert.AreEqual(true, result);
        }

        #endregion

        #region < operator

        [Test]
        public void Op_LessThan_AreEqual()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(1, 2);

            //exec
            bool result = (x < y);

            //test
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Op_LessThan_AreEqual_Negative()
        {
            //setup
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(-1, 2);

            //exec
            bool result = (x < y);

            //test
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Op_LessThan_LessThan()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 2);

            //exec
            bool result = (x < y);

            //test
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Op_LessThan_GreaterThan()
        {
            //setup
            Rational x = new Rational(3, 2);
            Rational y = new Rational(1, 2);

            //exec
            bool result = (x < y);

            //test
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Op_LessThan_Negative_LessThan()
        {
            //setup
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(1, 2);

            //exec
            bool result = (x < y);

            //test
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Op_LessThan_Negative_GreaterThan()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(-1, 2);

            //exec
            bool result = (x < y);

            //test
            Assert.AreEqual(false, result);
        }

        #endregion

        #region > operator

        [Test]
        public void Op_GreaterThan_AreEqual()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(1, 2);

            //exec
            bool result = (x > y);

            //test
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Op_GreaterThan_AreEqual_Negative()
        {
            //setup
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(-1, 2);

            //exec
            bool result = (x > y);

            //test
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Op_GreaterThan_LessThan()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 2);

            //exec
            bool result = (x > y);

            //test
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Op_GreaterThan_GreaterThan()
        {
            //setup
            Rational x = new Rational(3, 2);
            Rational y = new Rational(1, 2);

            //exec
            bool result = (x > y);

            //test
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Op_GreaterThan_Negative_LessThan()
        {
            //setup
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(1, 2);

            //exec
            bool result = (x > y);

            //test
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Op_GreaterThan_Negative_GreaterThan()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(-1, 2);

            //exec
            bool result = (x > y);

            //test
            Assert.AreEqual(true, result);
        }

        #endregion

        #region <= operator

        [Test]
        public void Op_LessThanEqual_AreEqual()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(1, 2);

            //exec
            bool result = (x <= y);

            //test
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Op_LessThanEqual_AreEqual_Negative()
        {
            //setup
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(-1, 2);

            //exec
            bool result = (x <= y);

            //test
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Op_LessThanEqual_LessThan()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 2);

            //exec
            bool result = (x <= y);

            //test
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Op_LessThanEqual_GreaterThan()
        {
            //setup
            Rational x = new Rational(3, 2);
            Rational y = new Rational(1, 2);

            //exec
            bool result = (x <= y);

            //test
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Op_LessThanEqual_Negative_LessThan()
        {
            //setup
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(1, 2);

            //exec
            bool result = (x <= y);

            //test
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Op_LessThanEqual_Negative_GreaterThan()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(-1, 2);

            //exec
            bool result = (x <= y);

            //test
            Assert.AreEqual(false, result);
        }

        #endregion

        #region >= operator

        [Test]
        public void Op_GreaterThanEqual_AreEqual()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(1, 2);

            //exec
            bool result = (x >= y);

            //test
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Op_GreaterThanEqual_AreEqual_Negative()
        {
            //setup
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(-1, 2);

            //exec
            bool result = (x >= y);

            //test
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Op_GreaterThanEqual_LessThan()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 2);

            //exec
            bool result = (x >= y);

            //test
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Op_GreaterThanEqual_GreaterThan()
        {
            //setup
            Rational x = new Rational(3, 2);
            Rational y = new Rational(1, 2);

            //exec
            bool result = (x >= y);

            //test
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Op_GreaterThanEqual_Negative_LessThan()
        {
            //setup
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(1, 2);

            //exec
            bool result = (x >= y);

            //test
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Op_GreaterThanEqual_Negative_GreaterThan()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(-1, 2);

            //exec
            bool result = (x >= y);

            //test
            Assert.AreEqual(true, result);
        }

        #endregion

        #endregion

        #region Algebraic operators

        #region Additive operators

        [Test]
        public void Op_Addition_Rationals()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 5);

            //exec
            Rational result = (x + y);

            //test
            Assert.AreEqual(11, result.Numerator);
            Assert.AreEqual(10, result.Denominator);
        }

        [Test]
        public void Op_Addition_Rational_Int32()
        {
            //setup
            Rational x = new Rational(1, 2);
            int y = 5;

            //exec
            Rational result = (x + y);

            //test
            Assert.AreEqual(11, result.Numerator);
            Assert.AreEqual(2, result.Denominator);
        }

        [Test]
        public void Op_Addition_Int32_Rational()
        {
            //setup
            int x = 5;
            Rational y = new Rational(1, 2);

            //exec
            Rational result = (x + y);

            //test
            Assert.AreEqual(11, result.Numerator);
            Assert.AreEqual(2, result.Denominator);
        }


        [Test]
        public void Op_Subtraction_Rationals()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 5);

            //exec
            Rational result = (x - y);

            //test
            Assert.AreEqual(-1, result.Numerator);
            Assert.AreEqual(10, result.Denominator);
        }

        [Test]
        public void Op_Subtraction_Rational_Int32()
        {
            //setup
            Rational x = new Rational(1, 2);
            int y = 5;

            //exec
            Rational result = (x - y);

            //test
            Assert.AreEqual(-9, result.Numerator);
            Assert.AreEqual(2, result.Denominator);
        }

        [Test]
        public void Op_Subtraction_Int32_Rational()
        {
            //setup
            int x = 5;
            Rational y = new Rational(1, 2);

            //exec
            Rational result = (x - y);

            //test
            Assert.AreEqual(9, result.Numerator);
            Assert.AreEqual(2, result.Denominator);
        }

        #endregion

        #region Multiplicative operators

        [Test]
        public void Op_Multiply_Rationals()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 5);

            //exec
            Rational result = (x * y);

            //test
            Assert.AreEqual(3, result.Numerator);
            Assert.AreEqual(10, result.Denominator);
        }

        [Test]
        public void Op_Multiply_Rational_Int32()
        {
            //setup
            Rational x = new Rational(1, 2);
            int y = 5;

            //exec
            Rational result = (x * y);

            //test
            Assert.AreEqual(5, result.Numerator);
            Assert.AreEqual(2, result.Denominator);
        }

        [Test]
        public void Op_Multiply_Int32_Rational()
        {
            //setup
            int x = 5;
            Rational y = new Rational(1, 2);

            //exec
            Rational result = (x * y);

            //test
            Assert.AreEqual(5, result.Numerator);
            Assert.AreEqual(2, result.Denominator);
        }


        [Test]
        public void Op_Division_Rationals()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 5);

            //exec
            Rational result = (x / y);

            //test
            Assert.AreEqual(5, result.Numerator);
            Assert.AreEqual(6, result.Denominator);
        }

        [Test]
        public void Op_Division_Rational_Int32()
        {
            //setup
            Rational x = new Rational(1, 2);
            int y = 5;

            //exec
            Rational result = (x / y);

            //test
            Assert.AreEqual(1, result.Numerator);
            Assert.AreEqual(10, result.Denominator);
        }

        [Test]
        public void Op_Division_Int32_Rational()
        {
            //setup
            int x = 5;
            Rational y = new Rational(1, 2);

            //exec
            Rational result = (x / y);

            //test
            Assert.AreEqual(10, result.Numerator);
            Assert.AreEqual(1, result.Denominator);
        }


        [Test]
        public void Op_Modulus_Rationals()
        {
            //setup
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 5);

            //exec
            int result = (x % y);

            //test
            // 5 % 6 = 5
            Assert.AreEqual(5, result);
        }

        [Test]
        public void Op_Modulus_Rational_Int32()
        {
            //setup
            Rational x = new Rational(43, 4);
            int y = 5;

            //exec
            int result = (x % y);

            //test
            // 43 % 20 = 3
            Assert.AreEqual(3, result);
        }

        [Test]
        public void Op_Modulus_Int32_Rational()
        {
            //setup
            int x = 5;
            Rational y = new Rational(3, 4);

            //exec
            int result = (x % y);

            //test
            // 15 % 4 = 2
            Assert.AreEqual(2, result);
        }

        #endregion

        #endregion

        #endregion

        #region Invert

        [Test]
        public void Invert()
        {
            //setup
            Rational x = new Rational(1, 2);

            //exec
            Rational y = x.Invert();

            //test
            Assert.AreEqual(2, y.Numerator);
            Assert.AreEqual(1, y.Denominator);
        }

        [Test]
        public void Invert_Error_NumeratorZero()
        {
            //setup
            Rational x = new Rational(0, 2);

            //exec
            try
            {
                Rational y = x.Invert();

                Assert.Fail();
            }
            catch (DivideByZeroException ex)
            {
            }
        }

        #endregion
    }
}
