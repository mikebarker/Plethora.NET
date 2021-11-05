using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Finance.Bond;
using Plethora.Test.Finance.UtilityClasses;
using System;

namespace Plethora.Test.Finance.Bond
{
    [TestClass]
    public class BondInterestCalculatorActual_360_Test
    {
        [TestMethod]
        public void Error_EndBeforeStart()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            try
            {
                // Action
                Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan10, Dates.Jan01);

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
                Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan01);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }


        [TestMethod]
        public void Jan01_Jan02()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan02);

            // Assert
            Assert.AreEqual(new Rational(1, 360), dayCountFraction);
        }

        [TestMethod]
        public void Jan01_Jan29()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan29);

            // Assert
            Assert.AreEqual(new Rational(28, 360), dayCountFraction);
        }

        [TestMethod]
        public void Jan01_Jan30()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan30);

            // Assert
            Assert.AreEqual(new Rational(29, 360), dayCountFraction);
        }

        [TestMethod]
        public void Jan01_Jan31()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan31);

            // Assert
            Assert.AreEqual(new Rational(30, 360), dayCountFraction);
        }

        [TestMethod]
        public void Jan01_Feb01()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Feb01);

            // Assert
            Assert.AreEqual(new Rational(31, 360), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Feb02_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Feb01, Dates.Feb02);

            // Assert
            Assert.AreEqual(new Rational(1, 360), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Feb28_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Feb01, Dates.Feb28);

            // Assert
            Assert.AreEqual(new Rational(27, 360), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Feb29_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Feb01, Dates.Feb29);

            // Assert
            Assert.AreEqual(new Rational(28, 360), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Mar01_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Feb01, Dates.Mar01);

            // Assert
            Assert.AreEqual(new Rational(29, 360), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Feb02_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2001, 02, 01), new DateTime(2001, 02, 02));

            // Assert
            Assert.AreEqual(new Rational(1, 360), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Feb28_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2001, 02, 01), new DateTime(2001, 02, 28));

            // Assert
            Assert.AreEqual(new Rational(27, 360), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Mar01_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2001, 02, 01), new DateTime(2001, 03, 01));

            // Assert
            Assert.AreEqual(new Rational(28, 360), dayCountFraction);
        }


        [TestMethod]
        public void Apr01_Apr02()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Apr01, Dates.Apr02);

            // Assert
            Assert.AreEqual(new Rational(1, 360), dayCountFraction);
        }

        [TestMethod]
        public void Apr01_Apr29()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Apr01, Dates.Apr29);

            // Assert
            Assert.AreEqual(new Rational(28, 360), dayCountFraction);
        }

        [TestMethod]
        public void Apr01_Apr30()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Apr01, Dates.Apr30);

            // Assert
            Assert.AreEqual(new Rational(29, 360), dayCountFraction);
        }

        [TestMethod]
        public void Apr01_May01()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Apr01, Dates.May01);

            // Assert
            Assert.AreEqual(new Rational(30, 360), dayCountFraction);
        }


        [TestMethod]
        public void Jul31_Aug29()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jul31, Dates.Aug29);

            // Assert
            Assert.AreEqual(new Rational(29, 360), dayCountFraction);
        }

        [TestMethod]
        public void Jul31_Aug30()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jul31, Dates.Aug30);

            // Assert
            Assert.AreEqual(new Rational(30, 360), dayCountFraction);
        }

        [TestMethod]
        public void Jul31_Aug31()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jul31, Dates.Aug31);

            // Assert
            Assert.AreEqual(new Rational(31, 360), dayCountFraction);
        }

        [TestMethod]
        public void Jul31_Sep01()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jul31, Dates.Sep01);

            // Assert
            Assert.AreEqual(new Rational(32, 360), dayCountFraction);
        }


        private BondInterestCalculatorActual_360 CreateBondInterestCalculator()
        {
            return new BondInterestCalculatorActual_360();
        }
    }
}
