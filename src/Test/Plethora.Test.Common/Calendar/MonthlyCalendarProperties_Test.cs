using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plethora.Calendar;
using Plethora.Test.UtilityClasses;

namespace Plethora.Test.Calendar
{
    [TestClass]
    public class MonthlyCalendarProperties_Test
    {
        [TestMethod]
        public void Generate_Monthly_1st()
        {
            // Arrange
            ICalendarProperties calendarProperties = new MonthlyCalendarProperties(1, new[] {1});

            // Action
            IEnumerable<DateTime> calendar = calendarProperties.GenerateCalendar(Dates.Jan01, new DayOfWeek[0], new DateTime[0]);
            DateTime[] calendarArray = calendar.Take(13).ToArray();

            // Assert
            Assert.AreEqual(Dates.Jan01, calendarArray[0]);
            Assert.AreEqual(Dates.Feb01, calendarArray[1]);
            Assert.AreEqual(Dates.Mar01, calendarArray[2]);
            Assert.AreEqual(Dates.Apr01, calendarArray[3]);
            Assert.AreEqual(Dates.May01, calendarArray[4]);
            Assert.AreEqual(Dates.Jun01, calendarArray[5]);
            Assert.AreEqual(Dates.Jul01, calendarArray[6]);
            Assert.AreEqual(Dates.Aug01, calendarArray[7]);
            Assert.AreEqual(Dates.Sep01, calendarArray[8]);
            Assert.AreEqual(Dates.Oct01, calendarArray[9]);
            Assert.AreEqual(Dates.Nov01, calendarArray[10]);
            Assert.AreEqual(Dates.Dec01, calendarArray[11]);
            Assert.AreEqual(Dates.Jan01.AddYears(1), calendarArray[12]);
        }

        [TestMethod]
        public void Generate_Monthly_28th()
        {
            // Arrange
            ICalendarProperties calendarProperties = new MonthlyCalendarProperties(1, new[] {28});

            // Action
            IEnumerable<DateTime> calendar = calendarProperties.GenerateCalendar(Dates.Jan01, new DayOfWeek[0], new DateTime[0]);
            DateTime[] calendarArray = calendar.Take(12).ToArray();

            // Assert
            Assert.AreEqual(Dates.Jan28, calendarArray[0]);
            Assert.AreEqual(Dates.Feb28, calendarArray[1]);
            Assert.AreEqual(Dates.Mar28, calendarArray[2]);
            Assert.AreEqual(Dates.Apr28, calendarArray[3]);
            Assert.AreEqual(Dates.May28, calendarArray[4]);
            Assert.AreEqual(Dates.Jun28, calendarArray[5]);
            Assert.AreEqual(Dates.Jul28, calendarArray[6]);
            Assert.AreEqual(Dates.Aug28, calendarArray[7]);
            Assert.AreEqual(Dates.Sep28, calendarArray[8]);
            Assert.AreEqual(Dates.Oct28, calendarArray[9]);
            Assert.AreEqual(Dates.Nov28, calendarArray[10]);
            Assert.AreEqual(Dates.Dec28, calendarArray[11]);
        }

        [TestMethod]
        public void Generate_Monthly_15th_28th()
        {
            // Arrange
            ICalendarProperties calendarProperties = new MonthlyCalendarProperties(1, new[] {15, 28});

            // Action
            IEnumerable<DateTime> calendar = calendarProperties.GenerateCalendar(Dates.Jan01, new DayOfWeek[0], new DateTime[0]);
            DateTime[] calendarArray = calendar.Take(24).ToArray();

            // Assert
            Assert.AreEqual(Dates.Jan15, calendarArray[0]);
            Assert.AreEqual(Dates.Jan28, calendarArray[1]);
            Assert.AreEqual(Dates.Feb15, calendarArray[2]);
            Assert.AreEqual(Dates.Feb28, calendarArray[3]);
            Assert.AreEqual(Dates.Mar15, calendarArray[4]);
            Assert.AreEqual(Dates.Mar28, calendarArray[5]);
            Assert.AreEqual(Dates.Apr15, calendarArray[6]);
            Assert.AreEqual(Dates.Apr28, calendarArray[7]);
            Assert.AreEqual(Dates.May15, calendarArray[8]);
            Assert.AreEqual(Dates.May28, calendarArray[9]);
            Assert.AreEqual(Dates.Jun15, calendarArray[10]);
            Assert.AreEqual(Dates.Jun28, calendarArray[11]);
            Assert.AreEqual(Dates.Jul15, calendarArray[12]);
            Assert.AreEqual(Dates.Jul28, calendarArray[13]);
            Assert.AreEqual(Dates.Aug15, calendarArray[14]);
            Assert.AreEqual(Dates.Aug28, calendarArray[15]);
            Assert.AreEqual(Dates.Sep15, calendarArray[16]);
            Assert.AreEqual(Dates.Sep28, calendarArray[17]);
            Assert.AreEqual(Dates.Oct15, calendarArray[18]);
            Assert.AreEqual(Dates.Oct28, calendarArray[19]);
            Assert.AreEqual(Dates.Nov15, calendarArray[20]);
            Assert.AreEqual(Dates.Nov28, calendarArray[21]);
            Assert.AreEqual(Dates.Dec15, calendarArray[22]);
            Assert.AreEqual(Dates.Dec28, calendarArray[23]);
        }

        [TestMethod]
        public void Error_Generate_Monthly_31st()
        {
            // Arrange
            ICalendarProperties calendarProperties = new MonthlyCalendarProperties(1, new[] {31});

            // Action
            try
            {
                // Attempting to generate "31 February" should fail
                IEnumerable<DateTime> calendar = calendarProperties.GenerateCalendar(Dates.Jan01, new DayOfWeek[0], new DateTime[0]);
                DateTime[] calendarArray = calendar.Take(2).ToArray();

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void Generate_BiMonthly_28th()
        {
            // Arrange
            ICalendarProperties calendarProperties = new MonthlyCalendarProperties(2, new[] {28});

            // Action
            IEnumerable<DateTime> calendar = calendarProperties.GenerateCalendar(Dates.Jan01, new DayOfWeek[0], new DateTime[0]);
            DateTime[] calendarArray = calendar.Take(6).ToArray();

            // Assert
            Assert.AreEqual(Dates.Jan28, calendarArray[0]);
            Assert.AreEqual(Dates.Mar28, calendarArray[1]);
            Assert.AreEqual(Dates.May28, calendarArray[2]);
            Assert.AreEqual(Dates.Jul28, calendarArray[3]);
            Assert.AreEqual(Dates.Sep28, calendarArray[4]);
            Assert.AreEqual(Dates.Nov28, calendarArray[5]);
        }

        [TestMethod]
        public void Generate_Monthly_EndOfMonth()
        {
            // Arrange
            ICalendarProperties calendarProperties = new MonthlyCalendarProperties(1, new int[0], true);

            // Action
            IEnumerable<DateTime> calendar = calendarProperties.GenerateCalendar(Dates.Jan01, new DayOfWeek[0], new DateTime[0]);
            DateTime[] calendarArray = calendar.Take(12).ToArray();

            // Assert
            Assert.AreEqual(Dates.Jan31, calendarArray[0]);
            Assert.AreEqual(Dates.Feb29, calendarArray[1]);
            Assert.AreEqual(Dates.Mar31, calendarArray[2]);
            Assert.AreEqual(Dates.Apr30, calendarArray[3]);
            Assert.AreEqual(Dates.May31, calendarArray[4]);
            Assert.AreEqual(Dates.Jun30, calendarArray[5]);
            Assert.AreEqual(Dates.Jul31, calendarArray[6]);
            Assert.AreEqual(Dates.Aug31, calendarArray[7]);
            Assert.AreEqual(Dates.Sep30, calendarArray[8]);
            Assert.AreEqual(Dates.Oct31, calendarArray[9]);
            Assert.AreEqual(Dates.Nov30, calendarArray[10]);
            Assert.AreEqual(Dates.Dec31, calendarArray[11]);
        }

        [TestMethod]
        public void Generate_Monthly_15th_EndOfMonth()
        {
            // Arrange
            ICalendarProperties calendarProperties = new MonthlyCalendarProperties(1, new[] {15}, true);

            // Action
            IEnumerable<DateTime> calendar = calendarProperties.GenerateCalendar(Dates.Jan01, new DayOfWeek[0], new DateTime[0]);
            DateTime[] calendarArray = calendar.Take(24).ToArray();

            // Assert
            Assert.AreEqual(Dates.Jan15, calendarArray[0]);
            Assert.AreEqual(Dates.Jan31, calendarArray[1]);
            Assert.AreEqual(Dates.Feb15, calendarArray[2]);
            Assert.AreEqual(Dates.Feb29, calendarArray[3]);
            Assert.AreEqual(Dates.Mar15, calendarArray[4]);
            Assert.AreEqual(Dates.Mar31, calendarArray[5]);
            Assert.AreEqual(Dates.Apr15, calendarArray[6]);
            Assert.AreEqual(Dates.Apr30, calendarArray[7]);
            Assert.AreEqual(Dates.May15, calendarArray[8]);
            Assert.AreEqual(Dates.May31, calendarArray[9]);
            Assert.AreEqual(Dates.Jun15, calendarArray[10]);
            Assert.AreEqual(Dates.Jun30, calendarArray[11]);
            Assert.AreEqual(Dates.Jul15, calendarArray[12]);
            Assert.AreEqual(Dates.Jul31, calendarArray[13]);
            Assert.AreEqual(Dates.Aug15, calendarArray[14]);
            Assert.AreEqual(Dates.Aug31, calendarArray[15]);
            Assert.AreEqual(Dates.Sep15, calendarArray[16]);
            Assert.AreEqual(Dates.Sep30, calendarArray[17]);
            Assert.AreEqual(Dates.Oct15, calendarArray[18]);
            Assert.AreEqual(Dates.Oct31, calendarArray[19]);
            Assert.AreEqual(Dates.Nov15, calendarArray[20]);
            Assert.AreEqual(Dates.Nov30, calendarArray[21]);
            Assert.AreEqual(Dates.Dec15, calendarArray[22]);
            Assert.AreEqual(Dates.Dec31, calendarArray[23]);
        }

        [TestMethod]
        public void Generate_Monthly_28th_EndOfMonth()
        {
            // Arrange
            ICalendarProperties calendarProperties = new MonthlyCalendarProperties(1, new[] {28}, true);

            // Action
            IEnumerable<DateTime> calendar = calendarProperties.GenerateCalendar(new DateTime(2001, 01, 01), new DayOfWeek[0], new DateTime[0]);
            DateTime[] calendarArray = calendar.Take(24).ToArray();

            // Assert
            Assert.AreEqual(new DateTime(2001, 01, 28), calendarArray[0]);
            Assert.AreEqual(new DateTime(2001, 01, 31), calendarArray[1]);
            Assert.AreEqual(new DateTime(2001, 02, 28), calendarArray[2]);
            Assert.AreEqual(new DateTime(2001, 03, 28), calendarArray[3]);
            Assert.AreEqual(new DateTime(2001, 03, 31), calendarArray[4]);
            Assert.AreEqual(new DateTime(2001, 04, 28), calendarArray[5]);
            Assert.AreEqual(new DateTime(2001, 04, 30), calendarArray[6]);
            Assert.AreEqual(new DateTime(2001, 05, 28), calendarArray[7]);
            Assert.AreEqual(new DateTime(2001, 05, 31), calendarArray[8]);
            Assert.AreEqual(new DateTime(2001, 06, 28), calendarArray[9]);
            Assert.AreEqual(new DateTime(2001, 06, 30), calendarArray[10]);
            Assert.AreEqual(new DateTime(2001, 07, 28), calendarArray[11]);
            Assert.AreEqual(new DateTime(2001, 07, 31), calendarArray[12]);
            Assert.AreEqual(new DateTime(2001, 08, 28), calendarArray[13]);
            Assert.AreEqual(new DateTime(2001, 08, 31), calendarArray[14]);
            Assert.AreEqual(new DateTime(2001, 09, 28), calendarArray[15]);
            Assert.AreEqual(new DateTime(2001, 09, 30), calendarArray[16]);
            Assert.AreEqual(new DateTime(2001, 10, 28), calendarArray[17]);
            Assert.AreEqual(new DateTime(2001, 10, 31), calendarArray[18]);
            Assert.AreEqual(new DateTime(2001, 11, 28), calendarArray[19]);
            Assert.AreEqual(new DateTime(2001, 11, 30), calendarArray[20]);
            Assert.AreEqual(new DateTime(2001, 12, 28), calendarArray[21]);
            Assert.AreEqual(new DateTime(2001, 12, 31), calendarArray[22]);
            Assert.AreEqual(new DateTime(2002, 01, 28), calendarArray[23]);
        }
    }
}
