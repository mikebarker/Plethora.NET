using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Finance.Bond;
using Plethora.Test.Finance.UtilityClasses;
using System;

namespace Plethora.Test.Finance.Bond
{
    [TestClass]
    public class BondInterestCalculatorActual_Actual_Test
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
        public void Jan01_Jan02_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan02);

            // Assert
            Assert.AreEqual(new Rational(1, 366), dayCountFraction);
        }

        [TestMethod]
        public void Jan01_Jan02_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2001, 01, 01), new DateTime(2001, 01, 02));

            // Assert
            Assert.AreEqual(new Rational(1, 365), dayCountFraction);
        }

        [TestMethod]
        public void Jan01_Jan29_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan29);

            // Assert
            Assert.AreEqual(new Rational(28, 366), dayCountFraction);
        }

        [TestMethod]
        public void Jan01_Jan29_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2001, 01, 01), new DateTime(2001, 01, 29));

            // Assert
            Assert.AreEqual(new Rational(28, 365), dayCountFraction);
        }

        [TestMethod]
        public void Jan01_Jan30_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan30);

            // Assert
            Assert.AreEqual(new Rational(29, 366), dayCountFraction);
        }

        [TestMethod]
        public void Jan01_Jan30_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2001, 01, 01), new DateTime(2001, 01, 30));

            // Assert
            Assert.AreEqual(new Rational(29, 365), dayCountFraction);
        }

        [TestMethod]
        public void Jan01_Jan31_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan31);

            // Assert
            Assert.AreEqual(new Rational(30, 366), dayCountFraction);
        }

        [TestMethod]
        public void Jan01_Jan31_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2001, 01, 01), new DateTime(2001, 01, 31));

            // Assert
            Assert.AreEqual(new Rational(30, 365), dayCountFraction);
        }

        [TestMethod]
        public void Jan01_Feb01_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Feb01);

            // Assert
            Assert.AreEqual(new Rational(31, 366), dayCountFraction);
        }

        [TestMethod]
        public void Jan01_Feb01_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2001, 01, 01), new DateTime(2001, 02, 01));

            // Assert
            Assert.AreEqual(new Rational(31, 365), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Feb02_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Feb01, Dates.Feb02);

            // Assert
            Assert.AreEqual(new Rational(1, 366), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Feb28_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Feb01, Dates.Feb28);

            // Assert
            Assert.AreEqual(new Rational(27, 366), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Feb29_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Feb01, Dates.Feb29);

            // Assert
            Assert.AreEqual(new Rational(28, 366), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Mar01_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Feb01, Dates.Mar01);

            // Assert
            Assert.AreEqual(new Rational(29, 366), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Feb02_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2001, 02, 01), new DateTime(2001, 02, 02));

            // Assert
            Assert.AreEqual(new Rational(1, 365), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Feb28_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2001, 02, 01), new DateTime(2001, 02, 28));

            // Assert
            Assert.AreEqual(new Rational(27, 365), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Mar01_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2001, 02, 01), new DateTime(2001, 03, 01));

            // Assert
            Assert.AreEqual(new Rational(28, 365), dayCountFraction);
        }


        [TestMethod]
        public void Apr01_Apr02_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Apr01, Dates.Apr02);

            // Assert
            Assert.AreEqual(new Rational(1, 366), dayCountFraction);
        }

        [TestMethod]
        public void Apr01_Apr02_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2001, 04, 01), new DateTime(2001, 04, 02));

            // Assert
            Assert.AreEqual(new Rational(1, 365), dayCountFraction);
        }

        [TestMethod]
        public void Apr01_Apr29_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Apr01, Dates.Apr29);

            // Assert
            Assert.AreEqual(new Rational(28, 366), dayCountFraction);
        }

        [TestMethod]
        public void Apr01_Apr29_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2001, 04, 01), new DateTime(2001, 04, 29));

            // Assert
            Assert.AreEqual(new Rational(28, 365), dayCountFraction);
        }

        [TestMethod]
        public void Apr01_Apr30_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Apr01, Dates.Apr30);

            // Assert
            Assert.AreEqual(new Rational(29, 366), dayCountFraction);
        }

        [TestMethod]
        public void Apr01_Apr30_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2001, 04, 01), new DateTime(2001, 04, 30));

            // Assert
            Assert.AreEqual(new Rational(29, 365), dayCountFraction);
        }

        [TestMethod]
        public void Apr01_May01_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Apr01, Dates.May01);

            // Assert
            Assert.AreEqual(new Rational(30, 366), dayCountFraction);
        }

        [TestMethod]
        public void Apr01_May01_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2001, 04, 01), new DateTime(2001, 05, 01));

            // Assert
            Assert.AreEqual(new Rational(30, 365), dayCountFraction);
        }


        [TestMethod]
        public void Jul31_Aug29_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jul31, Dates.Aug29);

            // Assert
            Assert.AreEqual(new Rational(29, 366), dayCountFraction);
        }

        [TestMethod]
        public void Jul31_Aug29_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2001, 07, 31), new DateTime(2001, 08, 29));

            // Assert
            Assert.AreEqual(new Rational(29, 365), dayCountFraction);
        }

        [TestMethod]
        public void Jul31_Aug30_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jul31, Dates.Aug30);

            // Assert
            Assert.AreEqual(new Rational(30, 366), dayCountFraction);
        }

        [TestMethod]
        public void Jul31_Aug30_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2001, 07, 31), new DateTime(2001, 08, 30));

            // Assert
            Assert.AreEqual(new Rational(30, 365), dayCountFraction);
        }

        [TestMethod]
        public void Jul31_Aug31_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jul31, Dates.Aug31);

            // Assert
            Assert.AreEqual(new Rational(31, 366), dayCountFraction);
        }

        [TestMethod]
        public void Jul31_Aug31_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2001, 07, 31), new DateTime(2001, 08, 31));

            // Assert
            Assert.AreEqual(new Rational(31, 365), dayCountFraction);
        }

        [TestMethod]
        public void Jul31_Sep01_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jul31, Dates.Sep01);

            // Assert
            Assert.AreEqual(new Rational(32, 366), dayCountFraction);
        }

        [TestMethod]
        public void Jul31_Sep01_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2001, 07, 31), new DateTime(2001, 09, 01));

            // Assert
            Assert.AreEqual(new Rational(32, 365), dayCountFraction);
        }


        [TestMethod]
        public void Dec20_LeapYear_Jan04_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2000, 12, 20), new DateTime(2001, 01, 05));

            // Assert
            // 12 days in the leap year, and 4 days in the non-leap year
            Assert.AreEqual(new Rational(12, 366) + new Rational(4, 365), dayCountFraction);
        }

        [TestMethod]
        public void AcrossYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2000, 07, 01), new DateTime(2001, 07, 01));

            // Assert
            // 184 days in 2000 and 181 in 2001
            // Note: this does not equal 1, even though it is a full year
            Assert.AreEqual(new Rational(184, 366) + new Rational(181, 365), dayCountFraction);
        }

        [TestMethod]
        public void MultiYear_5Years()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2000, 01, 01), new DateTime(2005, 01, 01));

            // Assert
            Assert.AreEqual(new Rational(5, 1), dayCountFraction);
        }


        private BondInterestCalculatorActual_Actual CreateBondInterestCalculator()
        {
            return new BondInterestCalculatorActual_Actual();
        }
    }
}
