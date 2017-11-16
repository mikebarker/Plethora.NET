using System;

using NUnit.Framework;

using Plethora.Calendar;
using Plethora.Test.UtilityClasses;

namespace Plethora.Test.Calendar
{
    [TestFixture]
    public class CalendarGenerator_Test
    {
        #region RollDate

        #region Actual

        [Test]
        public void RollDate_Actual_Weekday()
        {
            //setup
            DateTime date = new DateTime(2000, 04, 12);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Actual,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2000, 04, 12), rolledDate);
        }

        [Test]
        public void RollDate_Actual_Weekend()
        {
            //setup
            DateTime date = new DateTime(2000, 04, 15);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Actual,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2000, 04, 15), rolledDate);
        }

        [Test]
        public void RollDate_Actual_Holiday()
        {
            //setup
            DateTime date = new DateTime(2000, 04, 24); //Easter Monday

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Actual,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2000, 04, 24), rolledDate);
        }

        #endregion

        #region Following

        [Test]
        public void RollDate_Following_Friday01()
        {
            //setup
            DateTime date = new DateTime(2003, 08, 01);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Following,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2003, 08, 01), rolledDate);
        }

        [Test]
        public void RollDate_Following_Tuesday()
        {
            //setup
            DateTime date = new DateTime(2003, 08, 12);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Following,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2003, 08, 12), rolledDate);
        }

        [Test]
        public void RollDate_Following_Friday29()
        {
            //setup
            DateTime date = new DateTime(2003, 08, 29);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Following,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2003, 08, 29), rolledDate);
        }

        [Test]
        public void RollDate_Following_Saturday30()
        {
            //setup
            DateTime date = new DateTime(2003, 08, 31);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Following,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2003, 09, 01), rolledDate);
        }

        [Test]
        public void RollDate_Following_Sunday31()
        {
            //setup
            DateTime date = new DateTime(2003, 08, 31);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Following,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2003, 09, 01), rolledDate);
        }


        [Test]
        public void RollDate_Following_Holiday()
        {
            //setup
            DateTime date = new DateTime(2000, 05, 01);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Following,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2000, 05, 02), rolledDate);
        }

        [Test]
        public void RollDate_Following_HolidayChristmasOnWeekend()
        {
            //setup
            DateTime date = new DateTime(1999, 12, 26);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Following,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(1999, 12, 29), rolledDate);
        }

        [Test]
        public void RollDate_Following_HolidayEaster()
        {
            //setup
            DateTime date = new DateTime(2000, 04, 23); // Easter Sunday

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Following,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2000, 04, 25), rolledDate);
        }

        [Test]
        public void RollDate_Following_HolidayNewYear()
        {
            //setup
            DateTime date = new DateTime(2000, 01, 02);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Following,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2000, 01, 04), rolledDate);
        }

        #endregion

        #region ModifiedFollowing

        [Test]
        public void RollDate_ModifiedFollowing_Friday01()
        {
            //setup
            DateTime date = new DateTime(2003, 08, 01);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedFollowing,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2003, 08, 01), rolledDate);
        }

        [Test]
        public void RollDate_ModifiedFollowing_Tuesday()
        {
            //setup
            DateTime date = new DateTime(2003, 08, 12);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedFollowing,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2003, 08, 12), rolledDate);
        }

        [Test]
        public void RollDate_ModifiedFollowing_Friday29()
        {
            //setup
            DateTime date = new DateTime(2003, 08, 29);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedFollowing,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2003, 08, 29), rolledDate);
        }

        [Test]
        public void RollDate_ModifiedFollowing_Saturday30()
        {
            //setup
            DateTime date = new DateTime(2003, 08, 31);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedFollowing,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2003, 08, 29), rolledDate);
        }

        [Test]
        public void RollDate_ModifiedFollowing_Sunday31()
        {
            //setup
            DateTime date = new DateTime(2003, 08, 31);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedFollowing,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2003, 08, 29), rolledDate);
        }


        [Test]
        public void RollDate_ModifiedFollowing_Holiday()
        {
            //setup
            DateTime date = new DateTime(2000, 05, 01);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedFollowing,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2000, 05, 02), rolledDate);
        }

        [Test]
        public void RollDate_ModifiedFollowing_HolidayChristmasOnWeekend()
        {
            //setup
            DateTime date = new DateTime(1999, 12, 26);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedFollowing,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(1999, 12, 29), rolledDate);
        }

        [Test]
        public void RollDate_ModifiedFollowing_HolidayEaster()
        {
            //setup
            DateTime date = new DateTime(2000, 04, 23); // Easter Sunday

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedFollowing,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2000, 04, 25), rolledDate);
        }

        [Test]
        public void RollDate_ModifiedFollowing_HolidayNewYear()
        {
            //setup
            DateTime date = new DateTime(2000, 01, 02);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedFollowing,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2000, 01, 04), rolledDate);
        }

        #endregion

        #region Preceding

        [Test]
        public void RollDate_Preceding_Saturday01()
        {
            //setup
            DateTime date = new DateTime(2003, 11, 01);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Preceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2003, 10, 31), rolledDate);
        }

        [Test]
        public void RollDate_Preceding_Sunday02()
        {
            //setup
            DateTime date = new DateTime(2003, 11, 02);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Preceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2003, 10, 31), rolledDate);
        }

        [Test]
        public void RollDate_Preceding_Monday03()
        {
            //setup
            DateTime date = new DateTime(2003, 11, 03);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Preceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2003, 11, 03), rolledDate);
        }

        [Test]
        public void RollDate_Preceding_Tuesday()
        {
            //setup
            DateTime date = new DateTime(2003, 11, 12);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Preceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2003, 11, 12), rolledDate);
        }

        [Test]
        public void RollDate_Preceding_Monday31()
        {
            //setup
            DateTime date = new DateTime(2003, 03, 31);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Preceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2003, 03, 31), rolledDate);
        }


        [Test]
        public void RollDate_Preceding_Holiday()
        {
            //setup
            DateTime date = new DateTime(2000, 05, 01);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Preceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2000, 04, 28), rolledDate);
        }

        [Test]
        public void RollDate_Preceding_HolidayChristmasOnWeekend()
        {
            //setup
            DateTime date = new DateTime(1999, 12, 26);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Preceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(1999, 12, 24), rolledDate);
        }

        [Test]
        public void RollDate_Preceding_HolidayEaster()
        {
            //setup
            DateTime date = new DateTime(2000, 04, 23); // Easter Sunday

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Preceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2000, 04, 20), rolledDate);
        }

        [Test]
        public void RollDate_Preceding_HolidayNewYear()
        {
            //setup
            DateTime date = new DateTime(2000, 01, 02);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Preceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(1999, 12, 30), rolledDate);
        }

        #endregion

        #region ModifiedPreceding

        [Test]
        public void RollDate_ModifiedPreceding_Saturday01()
        {
            //setup
            DateTime date = new DateTime(2003, 11, 01);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedPreceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2003, 11, 03), rolledDate);
        }

        [Test]
        public void RollDate_ModifiedPreceding_Sunday02()
        {
            //setup
            DateTime date = new DateTime(2003, 11, 02);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedPreceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2003, 11, 03), rolledDate);
        }

        [Test]
        public void RollDate_ModifiedPreceding_Monday03()
        {
            //setup
            DateTime date = new DateTime(2003, 11, 03);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedPreceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2003, 11, 03), rolledDate);
        }

        [Test]
        public void RollDate_ModifiedPreceding_Tuesday()
        {
            //setup
            DateTime date = new DateTime(2003, 11, 12);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedPreceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2003, 11, 12), rolledDate);
        }

        [Test]
        public void RollDate_ModifiedPreceding_Monday31()
        {
            //setup
            DateTime date = new DateTime(2003, 03, 31);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedPreceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2003, 03, 31), rolledDate);
        }


        [Test]
        public void RollDate_ModifiedPreceding_Holiday()
        {
            //setup
            DateTime date = new DateTime(2000, 05, 01);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedPreceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2000, 05, 02), rolledDate);
        }

        [Test]
        public void RollDate_ModifiedPreceding_HolidayChristmasOnWeekend()
        {
            //setup
            DateTime date = new DateTime(1999, 12, 26);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedPreceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(1999, 12, 24), rolledDate);
        }

        [Test]
        public void RollDate_ModifiedPreceding_HolidayEaster()
        {
            //setup
            DateTime date = new DateTime(2000, 04, 23); // Easter Sunday

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedPreceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2000, 04, 20), rolledDate);
        }

        [Test]
        public void RollDate_ModifiedPreceding_HolidayNewYear()
        {
            //setup
            DateTime date = new DateTime(2000, 01, 02);

            // exec
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedPreceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // test
            Assert.AreEqual(new DateTime(2000, 01, 04), rolledDate);
        }

        #endregion

        #endregion
    }
}
