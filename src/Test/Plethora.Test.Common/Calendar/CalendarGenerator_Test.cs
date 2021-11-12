using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plethora.Calendar;
using Plethora.Test.UtilityClasses;

namespace Plethora.Test.Calendar
{
    [TestClass]
    public class CalendarGenerator_Test
    {
        #region Generate

        [TestMethod]
        public void Generate_Daily_SpecifyEndDate()
        {
            // Arrange
            ICalendarProperties calendarProperties = new DailyCalendarProperties(1, DailyType.Day);
            IEnumerable<DayOfWeek> weekDays = new HashSet<DayOfWeek> { DayOfWeek.Sunday, DayOfWeek.Saturday };
            IEnumerable<DateTime> holidays = Dates.EnglishHolidays;


            // Action
            DateTime[] calendar = CalendarGenerator.Generate(Dates.Jan01, Dates.Jan31, calendarProperties, DateRollType.Actual, weekDays, holidays);

            // Assert
            Assert.AreEqual(31, calendar.Length);
            Assert.AreEqual(Dates.Jan01, calendar[0]);
            Assert.AreEqual(Dates.Jan31, calendar[30]);
        }

        [TestMethod]
        public void Generate_Daily_SpecifyEndDate_Weekdays()
        {
            // Arrange
            ICalendarProperties calendarProperties = new DailyCalendarProperties(1, DailyType.WeekDay);
            IEnumerable<DayOfWeek> weekDays = new HashSet<DayOfWeek> { DayOfWeek.Sunday, DayOfWeek.Saturday };
            IEnumerable<DateTime> holidays = Dates.EnglishHolidays;


            // Action
            DateTime[] calendar = CalendarGenerator.Generate(Dates.Jan01, Dates.Jan31, calendarProperties, DateRollType.Actual, weekDays, holidays);

            // Assert
            Assert.AreEqual(21, calendar.Length);
            Assert.AreEqual(Dates.Jan03, calendar[0]); // Jan 1= Sat, Jan 2= Sun
            Assert.AreEqual(Dates.Jan31, calendar[20]);
        }

        [TestMethod]
        public void Generate_Daily_SpecifyEndDate_BusinessDay()
        {
            // Arrange
            ICalendarProperties calendarProperties = new DailyCalendarProperties(1, DailyType.BusinessDay);
            IEnumerable<DayOfWeek> weekDays = new HashSet<DayOfWeek> { DayOfWeek.Sunday, DayOfWeek.Saturday };
            IEnumerable<DateTime> holidays = Dates.EnglishHolidays;


            // Action
            DateTime[] calendar = CalendarGenerator.Generate(Dates.Jan01, Dates.Jan31, calendarProperties, DateRollType.Actual, weekDays, holidays);

            // Assert
            Assert.AreEqual(20, calendar.Length);
            Assert.AreEqual(Dates.Jan04, calendar[0]); // Jan 1= Sat, Jan 2= Sun, Jan 3= New Year's Day
            Assert.AreEqual(Dates.Jan31, calendar[19]);
        }

        [TestMethod]
        public void Generate_Daily_SpecifyOccurences()
        {
            // Arrange
            ICalendarProperties calendarProperties = new DailyCalendarProperties(1, DailyType.Day);
            IEnumerable<DayOfWeek> weekDays = new HashSet<DayOfWeek> { DayOfWeek.Sunday, DayOfWeek.Saturday };
            IEnumerable<DateTime> holidays = Dates.EnglishHolidays;


            // Action
            DateTime[] calendar = CalendarGenerator.Generate(Dates.Jan01, 12, calendarProperties, DateRollType.Actual, weekDays, holidays);

            // Assert
            Assert.AreEqual(12, calendar.Length);
            Assert.AreEqual(Dates.Jan01, calendar[0]);
            Assert.AreEqual(Dates.Jan12, calendar[11]);
        }

        [TestMethod]
        public void Generate_Daily_SpecifyOccurences_WeekDay()
        {
            // Arrange
            ICalendarProperties calendarProperties = new DailyCalendarProperties(1, DailyType.WeekDay);
            IEnumerable<DayOfWeek> weekDays = new HashSet<DayOfWeek> { DayOfWeek.Sunday, DayOfWeek.Saturday };
            IEnumerable<DateTime> holidays = Dates.EnglishHolidays;


            // Action
            DateTime[] calendar = CalendarGenerator.Generate(Dates.Jan01, 12, calendarProperties, DateRollType.Actual, weekDays, holidays);

            // Assert
            Assert.AreEqual(12, calendar.Length);
            Assert.AreEqual(Dates.Jan03, calendar[0]); // Jan 1= Sat, Jan 2= Sun
            Assert.AreEqual(Dates.Jan18, calendar[11]);
        }

        [TestMethod]
        public void Generate_Daily_SpecifyOccurences_BusinessDay()
        {
            // Arrange
            ICalendarProperties calendarProperties = new DailyCalendarProperties(1, DailyType.BusinessDay);
            IEnumerable<DayOfWeek> weekDays = new HashSet<DayOfWeek> { DayOfWeek.Sunday, DayOfWeek.Saturday };
            IEnumerable<DateTime> holidays = Dates.EnglishHolidays;


            // Action
            DateTime[] calendar = CalendarGenerator.Generate(Dates.Jan01, 12, calendarProperties, DateRollType.Actual, weekDays, holidays);

            // Assert
            Assert.AreEqual(12, calendar.Length);
            Assert.AreEqual(Dates.Jan04, calendar[0]); // Jan 1= Sat, Jan 2= Sun, Jan 3= New Year's Day
            Assert.AreEqual(Dates.Jan19, calendar[11]);
        }

        #endregion

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
