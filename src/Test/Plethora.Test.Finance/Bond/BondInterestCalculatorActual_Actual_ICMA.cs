using System;

using NUnit.Framework;

using Plethora.Finance.Bond;
using Plethora.Test.Finance.UtilityClasses;

namespace Plethora.Test.Finance.Bond
{
    [TestFixture]
    public class BondInterestCalculatorActual_Actual_ICMA_Test
    {
        [Test]
        public void Error_EndBeforeStart()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            try
            {
                //Exec
                Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan10, Dates.Jan01, Dates.Feb01, 12);

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
                Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan01, Dates.Feb01, 12);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [Test]
        public void Error_NextPayBeforeStart()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            try
            {
                //Exec
                Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan10, Dates.Jan30, Dates.Jan01, 12);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [Test]
        public void Error_NextPayEqualsStart()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            try
            {
                //Exec
                Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan30, Dates.Jan01, 12);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [Test]
        public void Error_NextPayBeforeEnd()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            try
            {
                //Exec
                Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan30, Dates.Jan20, 12);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [Test]
        public void Error_FrequencyZero()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            try
            {
                //Exec
                Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan30, Dates.Feb01, 0);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [Test]
        public void Error_FrequencyNegative()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            try
            {
                //Exec
                Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(Dates.Jan01, Dates.Jan30, Dates.Feb01, -12);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }


        [Test]
        public void Jan01_Jan02_LeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Jan01,
                Dates.Jan02,
                Dates.Feb01,
                12);

            //Test
            Assert.AreEqual(new Rational(1, 372), dayCountFraction);
        }

        [Test]
        public void Jan01_Jan02_NotLeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 01, 01),
                new DateTime(2001, 01, 02),
                new DateTime(2001, 02, 01),
                12);

            //Test
            Assert.AreEqual(new Rational(1, 372), dayCountFraction);
        }

        [Test]
        public void Jan01_Jan29_LeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Jan01,
                Dates.Jan29,
                Dates.Feb01,
                12);

            //Test
            Assert.AreEqual(new Rational(28, 372), dayCountFraction);
        }

        [Test]
        public void Jan01_Jan29_NotLeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 01, 01),
                new DateTime(2001, 01, 29),
                new DateTime(2001, 02, 01),
                12);

            //Test
            Assert.AreEqual(new Rational(28, 372), dayCountFraction);
        }

        [Test]
        public void Jan01_Jan30_LeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Jan01,
                Dates.Jan30,
                Dates.Feb01,
                12);

            //Test
            Assert.AreEqual(new Rational(29, 372), dayCountFraction);
        }

        [Test]
        public void Jan01_Jan30_NotLeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 01, 01),
                new DateTime(2001, 01, 30),
                new DateTime(2001, 02, 01),
                12);

            //Test
            Assert.AreEqual(new Rational(29, 372), dayCountFraction);
        }

        [Test]
        public void Jan01_Jan31_LeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Jan01,
                Dates.Jan31,
                Dates.Feb01,
                12);

            //Test
            Assert.AreEqual(new Rational(30, 372), dayCountFraction);
        }

        [Test]
        public void Jan01_Jan31_NotLeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 01, 01),
                new DateTime(2001, 01, 31),
                new DateTime(2001, 02, 01),
                12);

            //Test
            Assert.AreEqual(new Rational(30, 372), dayCountFraction);
        }

        [Test]
        public void Jan01_Feb01_LeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Jan01,
                Dates.Feb01,
                Dates.Feb01,
                12);

            //Test
            Assert.AreEqual(new Rational(1, 12), dayCountFraction);
        }

        [Test]
        public void Jan01_Feb01_NotLeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 01, 01),
                new DateTime(2001, 02, 01),
                new DateTime(2001, 02, 01),
                12);

            //Test
            Assert.AreEqual(new Rational(1, 12), dayCountFraction);
        }

        [Test]
        public void Feb01_Feb02_LeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Feb01,
                Dates.Feb02,
                Dates.Mar01,
                12);

            //Test
            Assert.AreEqual(new Rational(1, 348), dayCountFraction);
        }

        [Test]
        public void Feb01_Feb28_LeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Feb01,
                Dates.Feb28,
                Dates.Mar01,
                12);

            //Test
            Assert.AreEqual(new Rational(27, 348), dayCountFraction);
        }

        [Test]
        public void Feb01_Feb29_LeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Feb01,
                Dates.Feb29,
                Dates.Mar01,
                12);

            //Test
            Assert.AreEqual(new Rational(28, 348), dayCountFraction);
        }

        [Test]
        public void Feb01_Mar01_LeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Feb01,
                Dates.Mar01,
                Dates.Mar01,
                12);

            //Test
            Assert.AreEqual(new Rational(29, 348), dayCountFraction);
        }

        [Test]
        public void Feb01_Feb02_NotLeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 02, 01),
                new DateTime(2001, 02, 02),
                new DateTime(2001, 03, 01),
                12);

            //Test
            Assert.AreEqual(new Rational(1, 336), dayCountFraction);
        }

        [Test]
        public void Feb01_Feb28_NotLeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 02, 01),
                new DateTime(2001, 02, 28),
                new DateTime(2001, 03, 01),
                12);

            //Test
            Assert.AreEqual(new Rational(27, 336), dayCountFraction);
        }

        [Test]
        public void Feb01_Mar01_NotLeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 02, 01),
                new DateTime(2001, 03, 01),
                new DateTime(2001, 03, 01),
                12);

            //Test
            Assert.AreEqual(new Rational(28, 336), dayCountFraction);
        }


        [Test]
        public void Apr01_Apr02_LeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Apr01,
                Dates.Apr02,
                Dates.May01,
                12);

            //Test
            Assert.AreEqual(new Rational(1, 360), dayCountFraction);
        }

        [Test]
        public void Apr01_Apr02_NotLeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 04, 01),
                new DateTime(2001, 04, 02),
                new DateTime(2001, 05, 01),
                12);

            //Test
            Assert.AreEqual(new Rational(1, 360), dayCountFraction);
        }

        [Test]
        public void Apr01_Apr29_LeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Apr01,
                Dates.Apr29,
                Dates.May01,
                12);

            //Test
            Assert.AreEqual(new Rational(28, 360), dayCountFraction);
        }

        [Test]
        public void Apr01_Apr29_NotLeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 04, 01),
                new DateTime(2001, 04, 29),
                new DateTime(2001, 05, 01),
                12);

            //Test
            Assert.AreEqual(new Rational(28, 360), dayCountFraction);
        }

        [Test]
        public void Apr01_Apr30_LeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Apr01,
                Dates.Apr30,
                Dates.May01,
                12);

            //Test
            Assert.AreEqual(new Rational(29, 360), dayCountFraction);
        }

        [Test]
        public void Apr01_Apr30_NotLeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 04, 01),
                new DateTime(2001, 04, 30),
                new DateTime(2001, 05, 01),
                12);

            //Test
            Assert.AreEqual(new Rational(29, 360), dayCountFraction);
        }

        [Test]
        public void Apr01_May01_LeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Apr01,
                Dates.May01,
                Dates.May01,
                12);

            //Test
            Assert.AreEqual(new Rational(30, 360), dayCountFraction);
        }

        [Test]
        public void Apr01_May01_NotLeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 04, 01),
                new DateTime(2001, 05, 01),
                new DateTime(2001, 05, 01),
                12);

            //Test
            Assert.AreEqual(new Rational(30, 360), dayCountFraction);
        }


        [Test]
        public void Jul31_Aug29_LeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Jul31,
                Dates.Aug29,
                Dates.Aug31,
                12);

            //Test
            Assert.AreEqual(new Rational(29, 372), dayCountFraction);
        }

        [Test]
        public void Jul31_Aug29_NotLeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 07, 31),
                new DateTime(2001, 08, 29),
                new DateTime(2001, 08, 31),
                12);

            //Test
            Assert.AreEqual(new Rational(29, 372), dayCountFraction);
        }

        [Test]
        public void Jul31_Aug30_LeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Jul31,
                Dates.Aug30,
                Dates.Aug31,
                12);

            //Test
            Assert.AreEqual(new Rational(30, 372), dayCountFraction);
        }

        [Test]
        public void Jul31_Aug30_NotLeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 07, 31),
                new DateTime(2001, 08, 30),
                new DateTime(2001, 08, 31),
                12);

            //Test
            Assert.AreEqual(new Rational(30, 372), dayCountFraction);
        }

        [Test]
        public void Jul31_Aug31_LeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Jul31,
                Dates.Aug31,
                Dates.Aug31,
                12);

            //Test
            Assert.AreEqual(new Rational(31, 372), dayCountFraction);
        }

        [Test]
        public void Jul31_Aug31_NotLeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                new DateTime(2001, 07, 31),
                new DateTime(2001, 08, 31),
                new DateTime(2001, 08, 31),
                12);

            //Test
            Assert.AreEqual(new Rational(31, 372), dayCountFraction);
        }

        [Test]
        public void Jul31_Sep01_LeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                Dates.Jul31,
                Dates.Sep01,
                Dates.Oct01,
                12);

            //Test
            Assert.AreEqual(new Rational(32, 372), dayCountFraction);
        }

        [Test]
        public void Jul31_Sep01_NotLeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2001, 07, 31), new DateTime(2001, 09, 01), Dates.Feb01, 12);

            //Test
            Assert.AreEqual(new Rational(32, 372), dayCountFraction);
        }


        [Test]
        public void Dec20_LeapYear_Jan04_NotLeapYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2000, 12, 20), new DateTime(2001, 01, 05), Dates.Feb01, 12);

            //Test
            // 12 days in the leap year, and 4 days in the non-leap year
            Assert.AreEqual(new Rational(12, 366) + new Rational(4, 365), dayCountFraction);
        }

        [Test]
        public void AcrossYear()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2000, 07, 01), new DateTime(2001, 07, 01), Dates.Feb01, 12);

            //Test
            // 184 days in 2000 and 181 in 2001
            // Note: this does not equal 1, even though it is a full year
            Assert.AreEqual(new Rational(184, 366) + new Rational(181, 365), dayCountFraction);
        }

        [Test]
        public void MultiYear_5Years()
        {
            //Setup
            var bondInterestCalculator = this.CreateBondInterestCalculator();

            //Exec
            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(new DateTime(2000, 01, 01), new DateTime(2005, 01, 01), Dates.Feb01, 12);

            //Test
            Assert.AreEqual(new Rational(5, 1), dayCountFraction);
        }


        private BondInterestCalculatorActual_Actual_ICMA CreateBondInterestCalculator()
        {
            return new BondInterestCalculatorActual_Actual_ICMA();
        }
    }
}
