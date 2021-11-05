using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Plethora.Test
{
    [TestClass]
    public class Rational_Test
    {
        #region Constructor

        [TestMethod]
        public void Constructor()
        {
            // Action
            Rational rational = new Rational(1, 2);

            // Assert
            Assert.AreEqual(1, rational.Numerator);
            Assert.AreEqual(2, rational.Denominator);
        }

        [TestMethod]
        public void Constructor_CanonicalFormCommonDivisor()
        {
            // Action
            Rational rational = new Rational(2, 4);

            // Assert
            Assert.AreEqual(1, rational.Numerator);
            Assert.AreEqual(2, rational.Denominator);
        }

        [TestMethod]
        public void Constructor_CanonicalFormNumeratorNegative()
        {
            // Action
            Rational rational = new Rational(-1, 2);

            // Assert
            Assert.AreEqual(-1, rational.Numerator);
            Assert.AreEqual(2, rational.Denominator);
        }

        [TestMethod]
        public void Constructor_CanonicalFormDenominatorNegative()
        {
            // Action
            Rational rational = new Rational(1, -2);

            // Assert
            Assert.AreEqual(-1, rational.Numerator);
            Assert.AreEqual(2, rational.Denominator);
        }

        [TestMethod]
        public void Constructor_NumeratorZero()
        {
            // Action
            Rational rational = new Rational(0, 12);

            // Assert
            Assert.AreEqual(0, rational.Numerator);
            Assert.AreEqual(1, rational.Denominator);
        }

        [TestMethod]
        public void Constructor_Error_DenominatorZero()
        {
            // Action
            try
            {
                Rational rational = new Rational(1, 0);

                Assert.Fail();
            }
            catch (DivideByZeroException)
            {
            }
        }

        [TestMethod]
        public void Constructor_Error_DenominatorNegativeMaximum()
        {
            // Action
            try
            {
                Rational rational = new Rational(1, int.MinValue);

                Assert.Fail();
            }
            catch (OverflowException)
            {
            }
        }

        #endregion

        #region Equality

        [TestMethod]
        public void Equals_AreEqual()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(1, 2);

            // Action
            bool result = x.Equals(y);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Equals_CanonicalAreEqual()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(2, 4);

            // Action
            bool result = x.Equals(y);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Equals_CanonicalNegativeAreEqual()
        {
            // Arrange
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(1, -2);

            // Action
            bool result = x.Equals(y);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Equals_NumeratorAreNotEqual()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 2);

            // Action
            bool result = x.Equals(y);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Equals_DenominatorAreNotEqual()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(1, 3);

            // Action
            bool result = x.Equals(y);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Equals_Null()
        {
            // Arrange
            Rational x = new Rational(1, 2);

            // Action
            bool result = x.Equals(null);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Equals_String()
        {
            // Arrange
            Rational x = new Rational(1, 2);

            // Action
            bool result = x.Equals("test");

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void GetHashCode_AreEqual()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(1, 2);

            // Action
            int xHash = x.GetHashCode();
            int yHash = y.GetHashCode();

            // Assert
            Assert.AreEqual(true, xHash == yHash);
        }

        [TestMethod]
        public void GetHashCode_CanonicalAreEqual()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(2, 4);

            // Action
            int xHash = x.GetHashCode();
            int yHash = y.GetHashCode();

            // Assert
            Assert.AreEqual(true, xHash == yHash);
        }

        [TestMethod]
        public void GetHashCode_CanonicalNegativeAreEqual()
        {
            // Arrange
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(1, -2);

            // Action
            int xHash = x.GetHashCode();
            int yHash = y.GetHashCode();

            // Assert
            Assert.AreEqual(true, xHash == yHash);
        }

        [TestMethod]
        public void GetHashCode_NumeratorAreNotEqual()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 2);

            // Action
            int xHash = x.GetHashCode();
            int yHash = y.GetHashCode();

            // Assert
            Assert.AreEqual(false, xHash == yHash);
        }

        [TestMethod]
        public void GetHashCode_DenominatorAreNotEqual()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(1, 3);

            // Action
            int xHash = x.GetHashCode();
            int yHash = y.GetHashCode();

            // Assert
            Assert.AreEqual(false, xHash == yHash);
        }

        [TestMethod]
        public void GetHashCode_Inverse()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(2, 1);

            // Action
            int xHash = x.GetHashCode();
            int yHash = y.GetHashCode();

            // Assert
            Assert.AreEqual(false, xHash == yHash);
        }

        #endregion

        #region CompareTo

        [TestMethod]
        public void CompareTo_AreEqual()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(1, 2);

            // Action
            int result = x.CompareTo(y);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CompareTo_AreEqual_Negative()
        {
            // Arrange
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(-1, 2);

            // Action
            int result = x.CompareTo(y);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CompareTo_LessThan()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 2);

            // Action
            int result = x.CompareTo(y);

            // Assert
            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void CompareTo_GreaterThan()
        {
            // Arrange
            Rational x = new Rational(3, 2);
            Rational y = new Rational(1, 2);

            // Action
            int result = x.CompareTo(y);

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void CompareTo_Negative_LessThan()
        {
            // Arrange
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(1, 2);

            // Action
            int result = x.CompareTo(y);

            // Assert
            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void CompareTo_Negative_GreaterThan()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(-1, 2);

            // Action
            int result = x.CompareTo(y);

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void CompareTo_Null()
        {
            // Arrange
            Rational x = new Rational(1, 2);

            // Action
            int result = ((IComparable)x).CompareTo(null);

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void CompareTo_String()
        {
            // Arrange
            Rational x = new Rational(1, 2);

            // Action
            try
            {
                int result = ((IComparable)x).CompareTo("test");

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        #endregion

        #region Convertion

        [TestMethod]
        public void ToDouble()
        {
            // Arrange
            Rational x = new Rational(1, 2);

            // Action
            double result = x.ToDouble();

            // Assert
            Assert.AreEqual(0.5, result);
        }

        [TestMethod]
        public void ToDecimal()
        {
            // Arrange
            Rational x = new Rational(1, 2);

            // Action
            decimal result = x.ToDecimal();

            // Assert
            Assert.AreEqual(0.5m, result);
        }

        [TestMethod]
        public void Cast_Double()
        {
            // Arrange
            Rational x = new Rational(1, 2);

            // Action
            double result = (double)x;

            // Assert
            Assert.AreEqual(0.5, result);
        }

        [TestMethod]
        public void Cast_Decimal()
        {
            // Arrange
            Rational x = new Rational(1, 2);

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
            Rational x = new Rational(1, 2);
            Rational y = new Rational(1, 2);

            // Action
            bool result = (x == y);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Op_Equality_CanonicalAreEqual()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(2, 4);

            // Action
            bool result = (x == y);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Op_Equality_CanonicalNegativeAreEqual()
        {
            // Arrange
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(1, -2);

            // Action
            bool result = (x == y);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Op_Equality_NumeratorAreNotEqual()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 2);

            // Action
            bool result = (x == y);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Op_Equality_DenominatorAreNotEqual()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(1, 3);

            // Action
            bool result = (x == y);

            // Assert
            Assert.AreEqual(false, result);
        }

        #endregion

        #region != operator

        [TestMethod]
        public void Op_Inequality_AreEqual()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(1, 2);

            // Action
            bool result = (x != y);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Op_Inequality_CanonicalAreEqual()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(2, 4);

            // Action
            bool result = (x != y);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Op_Inequality_CanonicalNegativeAreEqual()
        {
            // Arrange
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(1, -2);

            // Action
            bool result = (x != y);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Op_Inequality_NumeratorAreNotEqual()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 2);

            // Action
            bool result = (x != y);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Op_Inequality_DenominatorAreNotEqual()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(1, 3);

            // Action
            bool result = (x != y);

            // Assert
            Assert.AreEqual(true, result);
        }

        #endregion

        #region < operator

        [TestMethod]
        public void Op_LessThan_AreEqual()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(1, 2);

            // Action
            bool result = (x < y);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Op_LessThan_AreEqual_Negative()
        {
            // Arrange
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(-1, 2);

            // Action
            bool result = (x < y);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Op_LessThan_LessThan()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 2);

            // Action
            bool result = (x < y);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Op_LessThan_GreaterThan()
        {
            // Arrange
            Rational x = new Rational(3, 2);
            Rational y = new Rational(1, 2);

            // Action
            bool result = (x < y);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Op_LessThan_Negative_LessThan()
        {
            // Arrange
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(1, 2);

            // Action
            bool result = (x < y);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Op_LessThan_Negative_GreaterThan()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(-1, 2);

            // Action
            bool result = (x < y);

            // Assert
            Assert.AreEqual(false, result);
        }

        #endregion

        #region > operator

        [TestMethod]
        public void Op_GreaterThan_AreEqual()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(1, 2);

            // Action
            bool result = (x > y);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Op_GreaterThan_AreEqual_Negative()
        {
            // Arrange
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(-1, 2);

            // Action
            bool result = (x > y);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Op_GreaterThan_LessThan()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 2);

            // Action
            bool result = (x > y);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Op_GreaterThan_GreaterThan()
        {
            // Arrange
            Rational x = new Rational(3, 2);
            Rational y = new Rational(1, 2);

            // Action
            bool result = (x > y);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Op_GreaterThan_Negative_LessThan()
        {
            // Arrange
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(1, 2);

            // Action
            bool result = (x > y);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Op_GreaterThan_Negative_GreaterThan()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(-1, 2);

            // Action
            bool result = (x > y);

            // Assert
            Assert.AreEqual(true, result);
        }

        #endregion

        #region <= operator

        [TestMethod]
        public void Op_LessThanEqual_AreEqual()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(1, 2);

            // Action
            bool result = (x <= y);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Op_LessThanEqual_AreEqual_Negative()
        {
            // Arrange
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(-1, 2);

            // Action
            bool result = (x <= y);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Op_LessThanEqual_LessThan()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 2);

            // Action
            bool result = (x <= y);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Op_LessThanEqual_GreaterThan()
        {
            // Arrange
            Rational x = new Rational(3, 2);
            Rational y = new Rational(1, 2);

            // Action
            bool result = (x <= y);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Op_LessThanEqual_Negative_LessThan()
        {
            // Arrange
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(1, 2);

            // Action
            bool result = (x <= y);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Op_LessThanEqual_Negative_GreaterThan()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(-1, 2);

            // Action
            bool result = (x <= y);

            // Assert
            Assert.AreEqual(false, result);
        }

        #endregion

        #region >= operator

        [TestMethod]
        public void Op_GreaterThanEqual_AreEqual()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(1, 2);

            // Action
            bool result = (x >= y);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Op_GreaterThanEqual_AreEqual_Negative()
        {
            // Arrange
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(-1, 2);

            // Action
            bool result = (x >= y);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Op_GreaterThanEqual_LessThan()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 2);

            // Action
            bool result = (x >= y);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Op_GreaterThanEqual_GreaterThan()
        {
            // Arrange
            Rational x = new Rational(3, 2);
            Rational y = new Rational(1, 2);

            // Action
            bool result = (x >= y);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Op_GreaterThanEqual_Negative_LessThan()
        {
            // Arrange
            Rational x = new Rational(-1, 2);
            Rational y = new Rational(1, 2);

            // Action
            bool result = (x >= y);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Op_GreaterThanEqual_Negative_GreaterThan()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(-1, 2);

            // Action
            bool result = (x >= y);

            // Assert
            Assert.AreEqual(true, result);
        }

        #endregion

        #endregion

        #region Algebraic operators

        #region Additive operators

        [TestMethod]
        public void Op_Addition_Rationals()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 5);

            // Action
            Rational result = (x + y);

            // Assert
            Assert.AreEqual(11, result.Numerator);
            Assert.AreEqual(10, result.Denominator);
        }

        [TestMethod]
        public void Op_Addition_Rational_Int32()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            int y = 5;

            // Action
            Rational result = (x + y);

            // Assert
            Assert.AreEqual(11, result.Numerator);
            Assert.AreEqual(2, result.Denominator);
        }

        [TestMethod]
        public void Op_Addition_Int32_Rational()
        {
            // Arrange
            int x = 5;
            Rational y = new Rational(1, 2);

            // Action
            Rational result = (x + y);

            // Assert
            Assert.AreEqual(11, result.Numerator);
            Assert.AreEqual(2, result.Denominator);
        }


        [TestMethod]
        public void Op_Subtraction_Rationals()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 5);

            // Action
            Rational result = (x - y);

            // Assert
            Assert.AreEqual(-1, result.Numerator);
            Assert.AreEqual(10, result.Denominator);
        }

        [TestMethod]
        public void Op_Subtraction_Rational_Int32()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            int y = 5;

            // Action
            Rational result = (x - y);

            // Assert
            Assert.AreEqual(-9, result.Numerator);
            Assert.AreEqual(2, result.Denominator);
        }

        [TestMethod]
        public void Op_Subtraction_Int32_Rational()
        {
            // Arrange
            int x = 5;
            Rational y = new Rational(1, 2);

            // Action
            Rational result = (x - y);

            // Assert
            Assert.AreEqual(9, result.Numerator);
            Assert.AreEqual(2, result.Denominator);
        }

        #endregion

        #region Multiplicative operators

        [TestMethod]
        public void Op_Multiply_Rationals()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 5);

            // Action
            Rational result = (x * y);

            // Assert
            Assert.AreEqual(3, result.Numerator);
            Assert.AreEqual(10, result.Denominator);
        }

        [TestMethod]
        public void Op_Multiply_Rational_Int32()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            int y = 5;

            // Action
            Rational result = (x * y);

            // Assert
            Assert.AreEqual(5, result.Numerator);
            Assert.AreEqual(2, result.Denominator);
        }

        [TestMethod]
        public void Op_Multiply_Int32_Rational()
        {
            // Arrange
            int x = 5;
            Rational y = new Rational(1, 2);

            // Action
            Rational result = (x * y);

            // Assert
            Assert.AreEqual(5, result.Numerator);
            Assert.AreEqual(2, result.Denominator);
        }


        [TestMethod]
        public void Op_Division_Rationals()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 5);

            // Action
            Rational result = (x / y);

            // Assert
            Assert.AreEqual(5, result.Numerator);
            Assert.AreEqual(6, result.Denominator);
        }

        [TestMethod]
        public void Op_Division_Rational_Int32()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            int y = 5;

            // Action
            Rational result = (x / y);

            // Assert
            Assert.AreEqual(1, result.Numerator);
            Assert.AreEqual(10, result.Denominator);
        }

        [TestMethod]
        public void Op_Division_Int32_Rational()
        {
            // Arrange
            int x = 5;
            Rational y = new Rational(1, 2);

            // Action
            Rational result = (x / y);

            // Assert
            Assert.AreEqual(10, result.Numerator);
            Assert.AreEqual(1, result.Denominator);
        }


        [TestMethod]
        public void Op_Modulus_Rationals()
        {
            // Arrange
            Rational x = new Rational(1, 2);
            Rational y = new Rational(3, 5);

            // Action
            int result = (x % y);

            // Assert
            // 5 % 6 = 5
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void Op_Modulus_Rational_Int32()
        {
            // Arrange
            Rational x = new Rational(43, 4);
            int y = 5;

            // Action
            int result = (x % y);

            // Assert
            // 43 % 20 = 3
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void Op_Modulus_Int32_Rational()
        {
            // Arrange
            int x = 5;
            Rational y = new Rational(3, 4);

            // Action
            int result = (x % y);

            // Assert
            // 15 % 4 = 2
            Assert.AreEqual(2, result);
        }

        #endregion

        #endregion

        #endregion

        #region Invert

        [TestMethod]
        public void Invert()
        {
            // Arrange
            Rational x = new Rational(1, 2);

            // Action
            Rational y = x.Invert();

            // Assert
            Assert.AreEqual(2, y.Numerator);
            Assert.AreEqual(1, y.Denominator);
        }

        [TestMethod]
        public void Invert_Error_NumeratorZero()
        {
            // Arrange
            Rational x = new Rational(0, 2);

            // Action
            try
            {
                Rational y = x.Invert();

                Assert.Fail();
            }
            catch (DivideByZeroException)
            {
            }
        }

        #endregion
    }
}
