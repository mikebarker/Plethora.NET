using NUnit.Framework;

namespace Plethora.Test
{
    [TestFixture]
    public class MathEx_Test
    {
        #region GreatestCommonDivisor

        [Test]
        public void GreatestCommonDivisor_CommonElement()
        {
            //exec
            int gcd = MathEx.GreatestCommonDivisor(4, 2);

            //test
            Assert.AreEqual(2, gcd);
        }

        [Test]
        public void GreatestCommonDivisor_PrimeMultiple()
        {
            //exec
            int gcd = MathEx.GreatestCommonDivisor(45, 20);

            //test
            Assert.AreEqual(5, gcd);
        }

        [Test]
        public void GreatestCommonDivisor_DoublePrimeMultiple()
        {
            //exec
            int gcd = MathEx.GreatestCommonDivisor(50, 20);

            //test
            Assert.AreEqual(10, gcd);
        }

        [Test]
        public void GreatestCommonDivisor_NoCommonDivisor()
        {
            //exec
            int gcd = MathEx.GreatestCommonDivisor(5, 2);

            //test
            Assert.AreEqual(1, gcd);
        }

        [Test]
        public void GreatestCommonDivisor_With1()
        {
            //exec
            int gcd = MathEx.GreatestCommonDivisor(1024, 1);

            //test
            Assert.AreEqual(1, gcd);
        }

        [Test]
        public void GreatestCommonDivisor_With0()
        {
            //exec
            int gcd = MathEx.GreatestCommonDivisor(1, 0);

            //test
            Assert.AreEqual(1, gcd);
        }

        [Test]
        public void GreatestCommonDivisor_With0_v2()
        {
            //exec
            int gcd = MathEx.GreatestCommonDivisor(0, 1);

            //test
            Assert.AreEqual(1, gcd);
        }

        [Test]
        public void GreatestCommonDivisor_LargestFirst()
        {
            //exec
            int gcd = MathEx.GreatestCommonDivisor(4, 2);

            //test
            Assert.AreEqual(2, gcd);
        }

        [Test]
        public void GreatestCommonDivisor_LargestSecond()
        {
            //exec
            int gcd = MathEx.GreatestCommonDivisor(2, 4);

            //test
            Assert.AreEqual(2, gcd);
        }

        [Test]
        public void GreatestCommonDivisor_NegativeFirst()
        {
            //exec
            int gcd = MathEx.GreatestCommonDivisor(-4, 2);

            //test
            Assert.AreEqual(2, gcd);
        }

        [Test]
        public void GreatestCommonDivisor_NegativeSecond()
        {
            //exec
            int gcd = MathEx.GreatestCommonDivisor(4, -2);

            //test
            Assert.AreEqual(2, gcd);
        }

        [Test]
        public void GreatestCommonDivisor_NegativeSecond_v2()
        {
            //exec
            int gcd = MathEx.GreatestCommonDivisor(4, -8);

            //test
            Assert.AreEqual(4, gcd);
        }

        #endregion
    }
}
