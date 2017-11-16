using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Plethora.Calendar;

namespace Plethora.Test.Calendar
{
    [TestFixture]
    public class WeeklyCalendarProperties_Test
    {
        [Test]
        public void Generate_Weekly_Monday()
        {
            //setup
            ICalendarProperties calendarProperties = new WeeklyCalendarProperties(1, new[] { DayOfWeek.Monday });

            //exec
            IEnumerable<DateTime> calendar = calendarProperties.GenerateCalendar(new DateTime(2000, 01, 01), new DayOfWeek[0], new DateTime[0]);
            DateTime[] calendarArray = calendar.Take(10).ToArray();

            //test
            Assert.AreEqual(new DateTime(2000, 01, 03), calendarArray[0]);
            Assert.AreEqual(new DateTime(2000, 01, 10), calendarArray[1]);
            Assert.AreEqual(new DateTime(2000, 01, 17), calendarArray[2]);
            Assert.AreEqual(new DateTime(2000, 01, 24), calendarArray[3]);
            Assert.AreEqual(new DateTime(2000, 01, 31), calendarArray[4]);
            Assert.AreEqual(new DateTime(2000, 02, 07), calendarArray[5]);
            Assert.AreEqual(new DateTime(2000, 02, 14), calendarArray[6]);
            Assert.AreEqual(new DateTime(2000, 02, 21), calendarArray[7]);
            Assert.AreEqual(new DateTime(2000, 02, 28), calendarArray[8]);
            Assert.AreEqual(new DateTime(2000, 03, 06), calendarArray[9]);
        }

        [Test]
        public void Generate_Weekly_Monday_Thursday()
        {
            //setup
            ICalendarProperties calendarProperties = new WeeklyCalendarProperties(1, new[] { DayOfWeek.Monday, DayOfWeek.Thursday });

            //exec
            IEnumerable<DateTime> calendar = calendarProperties.GenerateCalendar(new DateTime(2000, 01, 01), new DayOfWeek[0], new DateTime[0]);
            DateTime[] calendarArray = calendar.Take(10).ToArray();

            //test
            Assert.AreEqual(new DateTime(2000, 01, 03), calendarArray[0]);
            Assert.AreEqual(new DateTime(2000, 01, 06), calendarArray[1]);
            Assert.AreEqual(new DateTime(2000, 01, 10), calendarArray[2]);
            Assert.AreEqual(new DateTime(2000, 01, 13), calendarArray[3]);
            Assert.AreEqual(new DateTime(2000, 01, 17), calendarArray[4]);
            Assert.AreEqual(new DateTime(2000, 01, 20), calendarArray[5]);
            Assert.AreEqual(new DateTime(2000, 01, 24), calendarArray[6]);
            Assert.AreEqual(new DateTime(2000, 01, 27), calendarArray[7]);
            Assert.AreEqual(new DateTime(2000, 01, 31), calendarArray[8]);
            Assert.AreEqual(new DateTime(2000, 02, 03), calendarArray[9]);
        }

        [Test]
        public void Generate_BiWeekly_Monday()
        {
            //setup
            ICalendarProperties calendarProperties = new WeeklyCalendarProperties(2, new[] { DayOfWeek.Monday });

            //exec
            IEnumerable<DateTime> calendar = calendarProperties.GenerateCalendar(new DateTime(2000, 01, 01), new DayOfWeek[0], new DateTime[0]);
            DateTime[] calendarArray = calendar.Take(10).ToArray();

            //test
            Assert.AreEqual(new DateTime(2000, 01, 03), calendarArray[0]);
            Assert.AreEqual(new DateTime(2000, 01, 17), calendarArray[1]);
            Assert.AreEqual(new DateTime(2000, 01, 31), calendarArray[2]);
            Assert.AreEqual(new DateTime(2000, 02, 14), calendarArray[3]);
            Assert.AreEqual(new DateTime(2000, 02, 28), calendarArray[4]);
            Assert.AreEqual(new DateTime(2000, 03, 13), calendarArray[5]);
            Assert.AreEqual(new DateTime(2000, 03, 27), calendarArray[6]);
            Assert.AreEqual(new DateTime(2000, 04, 10), calendarArray[7]);
            Assert.AreEqual(new DateTime(2000, 04, 24), calendarArray[8]);
            Assert.AreEqual(new DateTime(2000, 05, 08), calendarArray[9]);
        }

        [Test]
        public void Generate_BiWeekly_Monday_StartWednesday()
        {
            //setup
            ICalendarProperties calendarProperties = new WeeklyCalendarProperties(2, new[] { DayOfWeek.Monday, });

            //exec
            IEnumerable<DateTime> calendar = calendarProperties.GenerateCalendar(new DateTime(2000, 01, 05), new DayOfWeek[0], new DateTime[0]);
            DateTime[] calendarArray = calendar.Take(10).ToArray();

            //test
            Assert.AreEqual(new DateTime(2000, 01, 10), calendarArray[0]);
            Assert.AreEqual(new DateTime(2000, 01, 24), calendarArray[1]);
            Assert.AreEqual(new DateTime(2000, 02, 07), calendarArray[2]);
            Assert.AreEqual(new DateTime(2000, 02, 21), calendarArray[3]);
            Assert.AreEqual(new DateTime(2000, 03, 06), calendarArray[4]);
            Assert.AreEqual(new DateTime(2000, 03, 20), calendarArray[5]);
            Assert.AreEqual(new DateTime(2000, 04, 03), calendarArray[6]);
            Assert.AreEqual(new DateTime(2000, 04, 17), calendarArray[7]);
            Assert.AreEqual(new DateTime(2000, 05, 01), calendarArray[8]);
            Assert.AreEqual(new DateTime(2000, 05, 15), calendarArray[9]);
        }

        [Test]
        public void Generate_BiWeekly_Monday_Thursday_StartWednesday()
        {
            //setup
            ICalendarProperties calendarProperties = new WeeklyCalendarProperties(2, new[] { DayOfWeek.Monday, DayOfWeek.Thursday, });

            //exec
            IEnumerable<DateTime> calendar = calendarProperties.GenerateCalendar(new DateTime(2000, 01, 05), new DayOfWeek[0], new DateTime[0]);
            DateTime[] calendarArray = calendar.Take(10).ToArray();

            //test
            Assert.AreEqual(new DateTime(2000, 01, 06), calendarArray[0]);
            Assert.AreEqual(new DateTime(2000, 01, 17), calendarArray[1]);
            Assert.AreEqual(new DateTime(2000, 01, 20), calendarArray[2]);
            Assert.AreEqual(new DateTime(2000, 01, 31), calendarArray[3]);
            Assert.AreEqual(new DateTime(2000, 02, 03), calendarArray[4]);
            Assert.AreEqual(new DateTime(2000, 02, 14), calendarArray[5]);
            Assert.AreEqual(new DateTime(2000, 02, 17), calendarArray[6]);
            Assert.AreEqual(new DateTime(2000, 02, 28), calendarArray[7]);
            Assert.AreEqual(new DateTime(2000, 03, 02), calendarArray[8]);
            Assert.AreEqual(new DateTime(2000, 03, 13), calendarArray[9]);
        }
    }
}
