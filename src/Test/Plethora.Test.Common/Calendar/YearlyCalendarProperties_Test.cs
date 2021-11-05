using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plethora.Calendar;

namespace Plethora.Test.Calendar
{
    [TestClass]
    public class YearlyCalendarProperties_Test
    {
        [TestMethod]
        public void Generate_Yearly_1st()
        {
            // Arrange
            ICalendarProperties calendarProperties = new YearlyCalendarProperties(1);

            // Action
            IEnumerable<DateTime> calendar = calendarProperties.GenerateCalendar(new DateTime(2000, 01, 01), new DayOfWeek[0], new DateTime[0]);
            DateTime[] calendarArray = calendar.Take(10).ToArray();

            // Assert
            Assert.AreEqual(new DateTime(2000, 01, 01), calendarArray[0]);
            Assert.AreEqual(new DateTime(2001, 01, 01), calendarArray[1]);
            Assert.AreEqual(new DateTime(2002, 01, 01), calendarArray[2]);
            Assert.AreEqual(new DateTime(2003, 01, 01), calendarArray[3]);
            Assert.AreEqual(new DateTime(2004, 01, 01), calendarArray[4]);
            Assert.AreEqual(new DateTime(2005, 01, 01), calendarArray[5]);
            Assert.AreEqual(new DateTime(2006, 01, 01), calendarArray[6]);
            Assert.AreEqual(new DateTime(2007, 01, 01), calendarArray[7]);
            Assert.AreEqual(new DateTime(2008, 01, 01), calendarArray[8]);
            Assert.AreEqual(new DateTime(2009, 01, 01), calendarArray[9]);
        }

        [TestMethod]
        public void Generate_Yearly_28th()
        {
            // Arrange
            ICalendarProperties calendarProperties = new YearlyCalendarProperties(1);

            // Action
            IEnumerable<DateTime> calendar = calendarProperties.GenerateCalendar(new DateTime(2000, 01, 28), new DayOfWeek[0], new DateTime[0]);
            DateTime[] calendarArray = calendar.Take(10).ToArray();

            // Assert
            Assert.AreEqual(new DateTime(2000, 01, 28), calendarArray[0]);
            Assert.AreEqual(new DateTime(2001, 01, 28), calendarArray[1]);
            Assert.AreEqual(new DateTime(2002, 01, 28), calendarArray[2]);
            Assert.AreEqual(new DateTime(2003, 01, 28), calendarArray[3]);
            Assert.AreEqual(new DateTime(2004, 01, 28), calendarArray[4]);
            Assert.AreEqual(new DateTime(2005, 01, 28), calendarArray[5]);
            Assert.AreEqual(new DateTime(2006, 01, 28), calendarArray[6]);
            Assert.AreEqual(new DateTime(2007, 01, 28), calendarArray[7]);
            Assert.AreEqual(new DateTime(2008, 01, 28), calendarArray[8]);
            Assert.AreEqual(new DateTime(2009, 01, 28), calendarArray[9]);
        }

        [TestMethod]
        public void Generate_BiYearly()
        {
            // Arrange
            ICalendarProperties calendarProperties = new YearlyCalendarProperties(2);

            // Action
            IEnumerable<DateTime> calendar = calendarProperties.GenerateCalendar(new DateTime(2000, 01, 01), new DayOfWeek[0], new DateTime[0]);
            DateTime[] calendarArray = calendar.Take(10).ToArray();

            // Assert
            Assert.AreEqual(new DateTime(2000, 01, 01), calendarArray[0]);
            Assert.AreEqual(new DateTime(2002, 01, 01), calendarArray[1]);
            Assert.AreEqual(new DateTime(2004, 01, 01), calendarArray[2]);
            Assert.AreEqual(new DateTime(2006, 01, 01), calendarArray[3]);
            Assert.AreEqual(new DateTime(2008, 01, 01), calendarArray[4]);
            Assert.AreEqual(new DateTime(2010, 01, 01), calendarArray[5]);
            Assert.AreEqual(new DateTime(2012, 01, 01), calendarArray[6]);
            Assert.AreEqual(new DateTime(2014, 01, 01), calendarArray[7]);
            Assert.AreEqual(new DateTime(2016, 01, 01), calendarArray[8]);
            Assert.AreEqual(new DateTime(2018, 01, 01), calendarArray[9]);
        }
    }
}
