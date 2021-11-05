using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plethora.Calendar;
using Plethora.Test.UtilityClasses;

namespace Plethora.Test.Calendar
{
    [TestClass]
    public class CalendarGenerator_Test
    {
        #region RollDate

        #region Actual

        [TestMethod]
        public void RollDate_Actual_Weekday()
        {
            // Arrange
            DateTime date = new DateTime(2000, 04, 12);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Actual,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2000, 04, 12), rolledDate);
        }

        [TestMethod]
        public void RollDate_Actual_Weekend()
        {
            // Arrange
            DateTime date = new DateTime(2000, 04, 15);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Actual,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2000, 04, 15), rolledDate);
        }

        [TestMethod]
        public void RollDate_Actual_Holiday()
        {
            // Arrange
            DateTime date = new DateTime(2000, 04, 24); //Easter Monday

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Actual,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2000, 04, 24), rolledDate);
        }

        #endregion

        #region Following

        [TestMethod]
        public void RollDate_Following_Friday01()
        {
            // Arrange
            DateTime date = new DateTime(2003, 08, 01);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Following,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2003, 08, 01), rolledDate);
        }

        [TestMethod]
        public void RollDate_Following_Tuesday()
        {
            // Arrange
            DateTime date = new DateTime(2003, 08, 12);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Following,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2003, 08, 12), rolledDate);
        }

        [TestMethod]
        public void RollDate_Following_Friday29()
        {
            // Arrange
            DateTime date = new DateTime(2003, 08, 29);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Following,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2003, 08, 29), rolledDate);
        }

        [TestMethod]
        public void RollDate_Following_Saturday30()
        {
            // Arrange
            DateTime date = new DateTime(2003, 08, 31);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Following,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2003, 09, 01), rolledDate);
        }

        [TestMethod]
        public void RollDate_Following_Sunday31()
        {
            // Arrange
            DateTime date = new DateTime(2003, 08, 31);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Following,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2003, 09, 01), rolledDate);
        }


        [TestMethod]
        public void RollDate_Following_Holiday()
        {
            // Arrange
            DateTime date = new DateTime(2000, 05, 01);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Following,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2000, 05, 02), rolledDate);
        }

        [TestMethod]
        public void RollDate_Following_HolidayChristmasOnWeekend()
        {
            // Arrange
            DateTime date = new DateTime(1999, 12, 26);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Following,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(1999, 12, 29), rolledDate);
        }

        [TestMethod]
        public void RollDate_Following_HolidayEaster()
        {
            // Arrange
            DateTime date = new DateTime(2000, 04, 23); // Easter Sunday

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Following,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2000, 04, 25), rolledDate);
        }

        [TestMethod]
        public void RollDate_Following_HolidayNewYear()
        {
            // Arrange
            DateTime date = new DateTime(2000, 01, 02);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Following,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2000, 01, 04), rolledDate);
        }

        #endregion

        #region ModifiedFollowing

        [TestMethod]
        public void RollDate_ModifiedFollowing_Friday01()
        {
            // Arrange
            DateTime date = new DateTime(2003, 08, 01);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedFollowing,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2003, 08, 01), rolledDate);
        }

        [TestMethod]
        public void RollDate_ModifiedFollowing_Tuesday()
        {
            // Arrange
            DateTime date = new DateTime(2003, 08, 12);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedFollowing,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2003, 08, 12), rolledDate);
        }

        [TestMethod]
        public void RollDate_ModifiedFollowing_Friday29()
        {
            // Arrange
            DateTime date = new DateTime(2003, 08, 29);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedFollowing,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2003, 08, 29), rolledDate);
        }

        [TestMethod]
        public void RollDate_ModifiedFollowing_Saturday30()
        {
            // Arrange
            DateTime date = new DateTime(2003, 08, 31);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedFollowing,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2003, 08, 29), rolledDate);
        }

        [TestMethod]
        public void RollDate_ModifiedFollowing_Sunday31()
        {
            // Arrange
            DateTime date = new DateTime(2003, 08, 31);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedFollowing,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2003, 08, 29), rolledDate);
        }


        [TestMethod]
        public void RollDate_ModifiedFollowing_Holiday()
        {
            // Arrange
            DateTime date = new DateTime(2000, 05, 01);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedFollowing,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2000, 05, 02), rolledDate);
        }

        [TestMethod]
        public void RollDate_ModifiedFollowing_HolidayChristmasOnWeekend()
        {
            // Arrange
            DateTime date = new DateTime(1999, 12, 26);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedFollowing,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(1999, 12, 29), rolledDate);
        }

        [TestMethod]
        public void RollDate_ModifiedFollowing_HolidayEaster()
        {
            // Arrange
            DateTime date = new DateTime(2000, 04, 23); // Easter Sunday

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedFollowing,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2000, 04, 25), rolledDate);
        }

        [TestMethod]
        public void RollDate_ModifiedFollowing_HolidayNewYear()
        {
            // Arrange
            DateTime date = new DateTime(2000, 01, 02);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedFollowing,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2000, 01, 04), rolledDate);
        }

        #endregion

        #region Preceding

        [TestMethod]
        public void RollDate_Preceding_Saturday01()
        {
            // Arrange
            DateTime date = new DateTime(2003, 11, 01);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Preceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2003, 10, 31), rolledDate);
        }

        [TestMethod]
        public void RollDate_Preceding_Sunday02()
        {
            // Arrange
            DateTime date = new DateTime(2003, 11, 02);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Preceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2003, 10, 31), rolledDate);
        }

        [TestMethod]
        public void RollDate_Preceding_Monday03()
        {
            // Arrange
            DateTime date = new DateTime(2003, 11, 03);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Preceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2003, 11, 03), rolledDate);
        }

        [TestMethod]
        public void RollDate_Preceding_Tuesday()
        {
            // Arrange
            DateTime date = new DateTime(2003, 11, 12);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Preceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2003, 11, 12), rolledDate);
        }

        [TestMethod]
        public void RollDate_Preceding_Monday31()
        {
            // Arrange
            DateTime date = new DateTime(2003, 03, 31);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Preceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2003, 03, 31), rolledDate);
        }


        [TestMethod]
        public void RollDate_Preceding_Holiday()
        {
            // Arrange
            DateTime date = new DateTime(2000, 05, 01);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Preceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2000, 04, 28), rolledDate);
        }

        [TestMethod]
        public void RollDate_Preceding_HolidayChristmasOnWeekend()
        {
            // Arrange
            DateTime date = new DateTime(1999, 12, 26);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Preceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(1999, 12, 24), rolledDate);
        }

        [TestMethod]
        public void RollDate_Preceding_HolidayEaster()
        {
            // Arrange
            DateTime date = new DateTime(2000, 04, 23); // Easter Sunday

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Preceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2000, 04, 20), rolledDate);
        }

        [TestMethod]
        public void RollDate_Preceding_HolidayNewYear()
        {
            // Arrange
            DateTime date = new DateTime(2000, 01, 02);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.Preceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(1999, 12, 30), rolledDate);
        }

        #endregion

        #region ModifiedPreceding

        [TestMethod]
        public void RollDate_ModifiedPreceding_Saturday01()
        {
            // Arrange
            DateTime date = new DateTime(2003, 11, 01);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedPreceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2003, 11, 03), rolledDate);
        }

        [TestMethod]
        public void RollDate_ModifiedPreceding_Sunday02()
        {
            // Arrange
            DateTime date = new DateTime(2003, 11, 02);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedPreceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2003, 11, 03), rolledDate);
        }

        [TestMethod]
        public void RollDate_ModifiedPreceding_Monday03()
        {
            // Arrange
            DateTime date = new DateTime(2003, 11, 03);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedPreceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2003, 11, 03), rolledDate);
        }

        [TestMethod]
        public void RollDate_ModifiedPreceding_Tuesday()
        {
            // Arrange
            DateTime date = new DateTime(2003, 11, 12);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedPreceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2003, 11, 12), rolledDate);
        }

        [TestMethod]
        public void RollDate_ModifiedPreceding_Monday31()
        {
            // Arrange
            DateTime date = new DateTime(2003, 03, 31);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedPreceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2003, 03, 31), rolledDate);
        }


        [TestMethod]
        public void RollDate_ModifiedPreceding_Holiday()
        {
            // Arrange
            DateTime date = new DateTime(2000, 05, 01);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedPreceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2000, 05, 02), rolledDate);
        }

        [TestMethod]
        public void RollDate_ModifiedPreceding_HolidayChristmasOnWeekend()
        {
            // Arrange
            DateTime date = new DateTime(1999, 12, 26);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedPreceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(1999, 12, 24), rolledDate);
        }

        [TestMethod]
        public void RollDate_ModifiedPreceding_HolidayEaster()
        {
            // Arrange
            DateTime date = new DateTime(2000, 04, 23); // Easter Sunday

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedPreceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2000, 04, 20), rolledDate);
        }

        [TestMethod]
        public void RollDate_ModifiedPreceding_HolidayNewYear()
        {
            // Arrange
            DateTime date = new DateTime(2000, 01, 02);

            // Action
            DateTime rolledDate = CalendarGenerator.RollDate(
                date,
                DateRollType.ModifiedPreceding,
                Dates.Weekends,
                Dates.EnglishHolidays);

            // Assert
            Assert.AreEqual(new DateTime(2000, 01, 04), rolledDate);
        }

        #endregion

        #endregion
    }
}
