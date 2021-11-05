using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plethora.Finance.Bond;
using Plethora.Test.Finance.UtilityClasses;

namespace Plethora.Test.Finance.Bond
{
    [TestClass]
    public class BondInterestCalculatorActual_Actual_ICMA_Test
    {
        [TestMethod]
        public void Error_EndBeforeStart()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            try
            {
                // Action
                Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan10, Dates.Jan01, Dates.Feb01, 12);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void Error_EndEqualsStart()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            try
            {
                // Action
                Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan01, Dates.Feb01, 12);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void Error_NextPayBeforeStart()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            try
            {
                // Action
                Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan10, Dates.Jan30, Dates.Jan01, 12);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void Error_NextPayEqualsStart()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            try
            {
                // Action
                Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan30, Dates.Jan01, 12);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void Error_NextPayBeforeEnd()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            try
            {
                // Action
                Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan30, Dates.Jan20, 12);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void Error_FrequencyZero()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            try
            {
                // Action
                Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan30, Dates.Feb01, 0);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void Error_FrequencyNegative()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            try
            {
                // Action
                Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan30, Dates.Feb01, -12);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }


        [TestMethod]
        public void Jan01_Jan02_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Jan01,
                Dates.Jan02,
                Dates.Feb01,
                12);

            // Assert
            Assert.AreEqual(new Rational(1, 372), dayCountFraction);
        }

        [TestMethod]
        public void Jan01_Jan02_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 01, 01),
                new DateTime(2001, 01, 02),
                new DateTime(2001, 02, 01),
                12);

            // Assert
            Assert.AreEqual(new Rational(1, 372), dayCountFraction);
        }

        [TestMethod]
        public void Jan01_Jan29_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Jan01,
                Dates.Jan29,
                Dates.Feb01,
                12);

            // Assert
            Assert.AreEqual(new Rational(28, 372), dayCountFraction);
        }

        [TestMethod]
        public void Jan01_Jan29_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 01, 01),
                new DateTime(2001, 01, 29),
                new DateTime(2001, 02, 01),
                12);

            // Assert
            Assert.AreEqual(new Rational(28, 372), dayCountFraction);
        }

        [TestMethod]
        public void Jan01_Jan30_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Jan01,
                Dates.Jan30,
                Dates.Feb01,
                12);

            // Assert
            Assert.AreEqual(new Rational(29, 372), dayCountFraction);
        }

        [TestMethod]
        public void Jan01_Jan30_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 01, 01),
                new DateTime(2001, 01, 30),
                new DateTime(2001, 02, 01),
                12);

            // Assert
            Assert.AreEqual(new Rational(29, 372), dayCountFraction);
        }

        [TestMethod]
        public void Jan01_Jan31_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Jan01,
                Dates.Jan31,
                Dates.Feb01,
                12);

            // Assert
            Assert.AreEqual(new Rational(30, 372), dayCountFraction);
        }

        [TestMethod]
        public void Jan01_Jan31_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 01, 01),
                new DateTime(2001, 01, 31),
                new DateTime(2001, 02, 01),
                12);

            // Assert
            Assert.AreEqual(new Rational(30, 372), dayCountFraction);
        }

        [TestMethod]
        public void Jan01_Feb01_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Jan01,
                Dates.Feb01,
                Dates.Feb01,
                12);

            // Assert
            Assert.AreEqual(new Rational(1, 12), dayCountFraction);
        }

        [TestMethod]
        public void Jan01_Feb01_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 01, 01),
                new DateTime(2001, 02, 01),
                new DateTime(2001, 02, 01),
                12);

            // Assert
            Assert.AreEqual(new Rational(1, 12), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Feb02_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Feb01,
                Dates.Feb02,
                Dates.Mar01,
                12);

            // Assert
            Assert.AreEqual(new Rational(1, 348), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Feb28_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Feb01,
                Dates.Feb28,
                Dates.Mar01,
                12);

            // Assert
            Assert.AreEqual(new Rational(27, 348), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Feb29_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Feb01,
                Dates.Feb29,
                Dates.Mar01,
                12);

            // Assert
            Assert.AreEqual(new Rational(28, 348), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Mar01_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Feb01,
                Dates.Mar01,
                Dates.Mar01,
                12);

            // Assert
            Assert.AreEqual(new Rational(29, 348), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Feb02_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 02, 01),
                new DateTime(2001, 02, 02),
                new DateTime(2001, 03, 01),
                12);

            // Assert
            Assert.AreEqual(new Rational(1, 336), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Feb28_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 02, 01),
                new DateTime(2001, 02, 28),
                new DateTime(2001, 03, 01),
                12);

            // Assert
            Assert.AreEqual(new Rational(27, 336), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Mar01_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 02, 01),
                new DateTime(2001, 03, 01),
                new DateTime(2001, 03, 01),
                12);

            // Assert
            Assert.AreEqual(new Rational(28, 336), dayCountFraction);
        }


        [TestMethod]
        public void Apr01_Apr02_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Apr01,
                Dates.Apr02,
                Dates.May01,
                12);

            // Assert
            Assert.AreEqual(new Rational(1, 360), dayCountFraction);
        }

        [TestMethod]
        public void Apr01_Apr02_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 04, 01),
                new DateTime(2001, 04, 02),
                new DateTime(2001, 05, 01),
                12);

            // Assert
            Assert.AreEqual(new Rational(1, 360), dayCountFraction);
        }

        [TestMethod]
        public void Apr01_Apr29_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Apr01,
                Dates.Apr29,
                Dates.May01,
                12);

            // Assert
            Assert.AreEqual(new Rational(28, 360), dayCountFraction);
        }

        [TestMethod]
        public void Apr01_Apr29_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 04, 01),
                new DateTime(2001, 04, 29),
                new DateTime(2001, 05, 01),
                12);

            // Assert
            Assert.AreEqual(new Rational(28, 360), dayCountFraction);
        }

        [TestMethod]
        public void Apr01_Apr30_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Apr01,
                Dates.Apr30,
                Dates.May01,
                12);

            // Assert
            Assert.AreEqual(new Rational(29, 360), dayCountFraction);
        }

        [TestMethod]
        public void Apr01_Apr30_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 04, 01),
                new DateTime(2001, 04, 30),
                new DateTime(2001, 05, 01),
                12);

            // Assert
            Assert.AreEqual(new Rational(29, 360), dayCountFraction);
        }

        [TestMethod]
        public void Apr01_May01_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Apr01,
                Dates.May01,
                Dates.May01,
                12);

            // Assert
            Assert.AreEqual(new Rational(30, 360), dayCountFraction);
        }

        [TestMethod]
        public void Apr01_May01_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 04, 01),
                new DateTime(2001, 05, 01),
                new DateTime(2001, 05, 01),
                12);

            // Assert
            Assert.AreEqual(new Rational(30, 360), dayCountFraction);
        }


        [TestMethod]
        public void Jul31_Aug29_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Jul31,
                Dates.Aug29,
                Dates.Aug31,
                12);

            // Assert
            Assert.AreEqual(new Rational(29, 372), dayCountFraction);
        }

        [TestMethod]
        public void Jul31_Aug29_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 07, 31),
                new DateTime(2001, 08, 29),
                new DateTime(2001, 08, 31),
                12);

            // Assert
            Assert.AreEqual(new Rational(29, 372), dayCountFraction);
        }

        [TestMethod]
        public void Jul31_Aug30_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Jul31,
                Dates.Aug30,
                Dates.Aug31,
                12);

            // Assert
            Assert.AreEqual(new Rational(30, 372), dayCountFraction);
        }

        [TestMethod]
        public void Jul31_Aug30_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 07, 31),
                new DateTime(2001, 08, 30),
                new DateTime(2001, 08, 31),
                12);

            // Assert
            Assert.AreEqual(new Rational(30, 372), dayCountFraction);
        }

        [TestMethod]
        public void Jul31_Aug31_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Jul31,
                Dates.Aug31,
                Dates.Aug31,
                12);

            // Assert
            Assert.AreEqual(new Rational(31, 372), dayCountFraction);
        }

        [TestMethod]
        public void Jul31_Aug31_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 07, 31),
                new DateTime(2001, 08, 31),
                new DateTime(2001, 08, 31),
                12);

            // Assert
            Assert.AreEqual(new Rational(31, 372), dayCountFraction);
        }

        [TestMethod]
        public void AcrossYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2000, 07, 01),
                new DateTime(2001, 07, 01),
                new DateTime(2001, 07, 01),
                12);

            // Assert
            Assert.AreEqual(new Rational(1, 12), dayCountFraction);
        }

        [TestMethod]
        public void MultiYear_5Years()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2000, 01, 01),
                new DateTime(2005, 01, 01),
                new DateTime(2005, 01, 01),
                12);

            // Assert
            Assert.AreEqual(new Rational(1, 12), dayCountFraction);
        }


        private BondInterestCalculatorActual_Actual_ICMA CreateBondInterestCalculator()
        {
            return new BondInterestCalculatorActual_Actual_ICMA();
        }
    }
}
