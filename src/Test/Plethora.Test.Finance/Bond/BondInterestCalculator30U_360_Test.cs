﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Finance.Bond;
using Plethora.Test.Finance.UtilityClasses;
using System;

namespace Plethora.Test.Finance.Bond
{
    [TestClass]
    public class BondInterestCalculator30U_360_Test
    {
        [TestMethod]
        public void Error_EndBeforeStart()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            try
            {
                // Action
                Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan10, Dates.Jan01, false);

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
                Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan01, false);

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
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan02, false);

            // Assert
            Assert.AreEqual(new Rational(1, 360), dayCountFraction);
        }

        [TestMethod]
        public void Jan01_Jan29()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan29, false);

            // Assert
            Assert.AreEqual(new Rational(28, 360), dayCountFraction);
        }

        [TestMethod]
        public void Jan01_Jan30()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan30, false);

            // Assert
            Assert.AreEqual(new Rational(29, 360), dayCountFraction);
        }

        [TestMethod]
        public void Jan01_Jan31()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan31, false);

            // Assert
            Assert.AreEqual(new Rational(30, 360), dayCountFraction);
        }

        [TestMethod]
        public void Jan01_Feb01()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Feb01, false);

            // Assert
            Assert.AreEqual(new Rational(30, 360), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Feb02_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Feb01, Dates.Feb02, false);

            // Assert
            Assert.AreEqual(new Rational(1, 360), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Feb28_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Feb01, Dates.Feb28, false);

            // Assert
            Assert.AreEqual(new Rational(27, 360), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Feb29_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Feb01, Dates.Feb29, false);

            // Assert
            Assert.AreEqual(new Rational(28, 360), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Mar01_LeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Feb01, Dates.Mar01, false);

            // Assert
            Assert.AreEqual(new Rational(30, 360), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Feb02_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2001, 02, 01), new DateTime(2001, 02, 02), false);

            // Assert
            Assert.AreEqual(new Rational(1, 360), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Feb28_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2001, 02, 01), new DateTime(2001, 02, 28), false);

            // Assert
            Assert.AreEqual(new Rational(27, 360), dayCountFraction);
        }

        [TestMethod]
        public void Feb01_Mar01_NotLeapYear()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2001, 02, 01), new DateTime(2001, 03, 01), false);

            // Assert
            Assert.AreEqual(new Rational(30, 360), dayCountFraction);
        }


        [TestMethod]
        public void Feb29_Feb28_IsEOM()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2000, 02, 29), new DateTime(2001, 02, 28), true);

            // Assert
            Assert.AreEqual(new Rational(1, 1), dayCountFraction);
        }

        [TestMethod]
        public void Feb29_Feb28_IsNotEOM()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2000, 02, 29), new DateTime(2001, 02, 28), false);

            // Assert
            Assert.AreEqual(new Rational(359, 360), dayCountFraction);
        }

        [TestMethod]
        public void Feb29_Mar31_IsEOM()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Feb29, Dates.Mar31, true);

            // Assert
            Assert.AreEqual(new Rational(30, 360), dayCountFraction);
        }

        [TestMethod]
        public void Feb29_Mar31_IsNotEOM()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Feb29, Dates.Mar31, false);

            // Assert
            Assert.AreEqual(new Rational(32, 360), dayCountFraction);
        }


        [TestMethod]
        public void Apr01_Apr02()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Apr01, Dates.Apr02, false);

            // Assert
            Assert.AreEqual(new Rational(1, 360), dayCountFraction);
        }

        [TestMethod]
        public void Apr01_Apr29()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Apr01, Dates.Apr29, false);

            // Assert
            Assert.AreEqual(new Rational(28, 360), dayCountFraction);
        }

        [TestMethod]
        public void Apr01_Apr30()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Apr01, Dates.Apr30, false);

            // Assert
            Assert.AreEqual(new Rational(29, 360), dayCountFraction);
        }

        [TestMethod]
        public void Apr01_May01()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Apr01, Dates.May01, false);

            // Assert
            Assert.AreEqual(new Rational(30, 360), dayCountFraction);
        }


        [TestMethod]
        public void Jul31_Aug29()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jul31, Dates.Aug29, false);

            // Assert
            Assert.AreEqual(new Rational(29, 360), dayCountFraction);
        }

        [TestMethod]
        public void Jul31_Aug30()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jul31, Dates.Aug30, false);

            // Assert
            Assert.AreEqual(new Rational(30, 360), dayCountFraction);
        }

        [TestMethod]
        public void Jul31_Aug31()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jul31, Dates.Aug31, false);

            // Assert
            Assert.AreEqual(new Rational(30, 360), dayCountFraction);
        }

        [TestMethod]
        public void Jul31_Sep01()
        {
            // Arrange
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            // Action
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jul31, Dates.Sep01, false);

            // Assert
            Assert.AreEqual(new Rational(31, 360), dayCountFraction);
        }


        private BondInterestCalculator30U_360 CreateBondInterestCalculator()
        {
            return new BondInterestCalculator30U_360();
        }
    }
}
