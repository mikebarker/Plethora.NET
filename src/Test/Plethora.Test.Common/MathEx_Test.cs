using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Plethora.Test
{
    [TestClass]
    public class MathEx_Test
    {
        #region GreatestCommonDivisor

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

        #endregion
    }
}
