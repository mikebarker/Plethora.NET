﻿using System;

using NUnit.Framework;

using Plethora.Finance.Bond;
using Plethora.Test.Finance.UtilityClasses;

namespace Plethora.Test.Finance.Bond
{
    [TestFixture]
    public class BondInterestCalculator30U_360_Test
    {
        [Test]
        public void Error_EndBeforeStart()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            try
            {
                //Exec
                Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan10, Dates.Jan01, false);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [Test]
        public void Error_EndEqualsStart()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            try
            {
                //Exec
                Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan01, false);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }


        [Test]
        public void Jan01_Jan02()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan02, false);

            //Test
            Assert.AreEqual(new Rational(1, 360), dayCountFraction);
        }

        [Test]
        public void Jan01_Jan29()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan29, false);

            //Test
            Assert.AreEqual(new Rational(28, 360), dayCountFraction);
        }

        [Test]
        public void Jan01_Jan30()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan30, false);

            //Test
            Assert.AreEqual(new Rational(29, 360), dayCountFraction);
        }

        [Test]
        public void Jan01_Jan31()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan31, false);

            //Test
            Assert.AreEqual(new Rational(30, 360), dayCountFraction);
        }

        [Test]
        public void Jan01_Feb01()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Feb01, false);

            //Test
            Assert.AreEqual(new Rational(30, 360), dayCountFraction);
        }

        [Test]
        public void Feb01_Feb02_LeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Feb01, Dates.Feb02, false);

            //Test
            Assert.AreEqual(new Rational(1, 360), dayCountFraction);
        }

        [Test]
        public void Feb01_Feb28_LeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Feb01, Dates.Feb28, false);

            //Test
            Assert.AreEqual(new Rational(27, 360), dayCountFraction);
        }

        [Test]
        public void Feb01_Feb29_LeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Feb01, Dates.Feb29, false);

            //Test
            Assert.AreEqual(new Rational(28, 360), dayCountFraction);
        }

        [Test]
        public void Feb01_Mar01_LeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Feb01, Dates.Mar01, false);

            //Test
            Assert.AreEqual(new Rational(30, 360), dayCountFraction);
        }

        [Test]
        public void Feb01_Feb02_NotLeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2001, 02, 01), new DateTime(2001, 02, 02), false);

            //Test
            Assert.AreEqual(new Rational(1, 360), dayCountFraction);
        }

        [Test]
        public void Feb01_Feb28_NotLeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2001, 02, 01), new DateTime(2001, 02, 28), false);

            //Test
            Assert.AreEqual(new Rational(27, 360), dayCountFraction);
        }

        [Test]
        public void Feb01_Mar01_NotLeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2001, 02, 01), new DateTime(2001, 03, 01), false);

            //Test
            Assert.AreEqual(new Rational(30, 360), dayCountFraction);
        }


        [Test]
        public void Feb29_Feb28_IsEOM()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2000, 02, 29), new DateTime(2001, 02, 28), true);

            //Test
            Assert.AreEqual(new Rational(1, 1), dayCountFraction);
        }

        [Test]
        public void Feb29_Feb28_IsNotEOM()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2000, 02, 29), new DateTime(2001, 02, 28), false);

            //Test
            Assert.AreEqual(new Rational(359, 360), dayCountFraction);
        }

        [Test]
        public void Feb29_Mar31_IsEOM()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Feb29, Dates.Mar31, true);

            //Test
            Assert.AreEqual(new Rational(30, 360), dayCountFraction);
        }

        [Test]
        public void Feb29_Mar31_IsNotEOM()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Feb29, Dates.Mar31, false);

            //Test
            Assert.AreEqual(new Rational(32, 360), dayCountFraction);
        }


        [Test]
        public void Apr01_Apr02()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Apr01, Dates.Apr02, false);

            //Test
            Assert.AreEqual(new Rational(1, 360), dayCountFraction);
        }

        [Test]
        public void Apr01_Apr29()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Apr01, Dates.Apr29, false);

            //Test
            Assert.AreEqual(new Rational(28, 360), dayCountFraction);
        }

        [Test]
        public void Apr01_Apr30()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Apr01, Dates.Apr30, false);

            //Test
            Assert.AreEqual(new Rational(29, 360), dayCountFraction);
        }

        [Test]
        public void Apr01_May01()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Apr01, Dates.May01, false);

            //Test
            Assert.AreEqual(new Rational(30, 360), dayCountFraction);
        }


        [Test]
        public void Jul31_Aug29()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jul31, Dates.Aug29, false);

            //Test
            Assert.AreEqual(new Rational(29, 360), dayCountFraction);
        }

        [Test]
        public void Jul31_Aug30()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jul31, Dates.Aug30, false);

            //Test
            Assert.AreEqual(new Rational(30, 360), dayCountFraction);
        }

        [Test]
        public void Jul31_Aug31()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jul31, Dates.Aug31, false);

            //Test
            Assert.AreEqual(new Rational(30, 360), dayCountFraction);
        }

        [Test]
        public void Jul31_Sep01()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jul31, Dates.Sep01, false);

            //Test
            Assert.AreEqual(new Rational(31, 360), dayCountFraction);
        }


        private BondInterestCalculator30U_360 CreateBondInterestCalculator()
        {
            return new BondInterestCalculator30U_360();
        }
    }
}
