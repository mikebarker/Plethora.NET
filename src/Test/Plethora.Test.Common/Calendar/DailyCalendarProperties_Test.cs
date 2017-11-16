using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Plethora.Calendar;
using Plethora.Test.UtilityClasses;

namespace Plethora.Test.Calendar
{
    [TestFixture]
    public class DailyCalendarProperties_Test
    {
        [Test]
        public void Generate_Each_Day()
        {
            //setup
            ICalendarProperties calendarProperties = new DailyCalendarProperties(1, DailyType.Day);

            //exec
            IEnumerable<DateTime> calendar = calendarProperties.GenerateCalendar(new DateTime(2000, 01, 01), Dates.Weekends, Dates.EnglishHolidays);
            DateTime[] calendarArray = calendar.Take(10).ToArray();

            //test
            Assert.AreEqual(new DateTime(2000, 01, 01), calendarArray[0]);
            Assert.AreEqual(new DateTime(2000, 01, 02), calendarArray[1]);
            Assert.AreEqual(new DateTime(2000, 01, 03), calendarArray[2]);
            Assert.AreEqual(new DateTime(2000, 01, 04), calendarArray[3]);
            Assert.AreEqual(new DateTime(2000, 01, 05), calendarArray[4]);
            Assert.AreEqual(new DateTime(2000, 01, 06), calendarArray[5]);
            Assert.AreEqual(new DateTime(2000, 01, 07), calendarArray[6]);
            Assert.AreEqual(new DateTime(2000, 01, 08), calendarArray[7]);
            Assert.AreEqual(new DateTime(2000, 01, 09), calendarArray[8]);
            Assert.AreEqual(new DateTime(2000, 01, 10), calendarArray[9]);
        }

        [Test]
        public void Generate_EverySecond_Day()
        {
            //setup
            ICalendarProperties calendarProperties = new DailyCalendarProperties(2, DailyType.Day);

            //exec
            IEnumerable<DateTime> calendar = calendarProperties.GenerateCalendar(new DateTime(2000, 01, 01), Dates.Weekends, Dates.EnglishHolidays);
            DateTime[] calendarArray = calendar.Take(10).ToArray();

            //test
            Assert.AreEqual(new DateTime(2000, 01, 01), calendarArray[0]);
            Assert.AreEqual(new DateTime(2000, 01, 03), calendarArray[1]);
            Assert.AreEqual(new DateTime(2000, 01, 05), calendarArray[2]);
            Assert.AreEqual(new DateTime(2000, 01, 07), calendarArray[3]);
            Assert.AreEqual(new DateTime(2000, 01, 09), calendarArray[4]);
            Assert.AreEqual(new DateTime(2000, 01, 11), calendarArray[5]);
            Assert.AreEqual(new DateTime(2000, 01, 13), calendarArray[6]);
            Assert.AreEqual(new DateTime(2000, 01, 15), calendarArray[7]);
            Assert.AreEqual(new DateTime(2000, 01, 17), calendarArray[8]);
            Assert.AreEqual(new DateTime(2000, 01, 19), calendarArray[9]);
        }

        [Test]
        public void Generate_Each_WeekDay()
        {
            //setup
            ICalendarProperties calendarProperties = new DailyCalendarProperties(1, DailyType.WeekDay);

            //exec
            IEnumerable<DateTime> calendar = calendarProperties.GenerateCalendar(new DateTime(2000, 01, 01), Dates.Weekends, Dates.EnglishHolidays);
            DateTime[] calendarArray = calendar.Take(10).ToArray();

            //test
            Assert.AreEqual(new DateTime(2000, 01, 03), calendarArray[0]);
            Assert.AreEqual(new DateTime(2000, 01, 04), calendarArray[1]);
            Assert.AreEqual(new DateTime(2000, 01, 05), calendarArray[2]);
            Assert.AreEqual(new DateTime(2000, 01, 06), calendarArray[3]);
            Assert.AreEqual(new DateTime(2000, 01, 07), calendarArray[4]);
            Assert.AreEqual(new DateTime(2000, 01, 10), calendarArray[5]);
            Assert.AreEqual(new DateTime(2000, 01, 11), calendarArray[6]);
            Assert.AreEqual(new DateTime(2000, 01, 12), calendarArray[7]);
            Assert.AreEqual(new DateTime(2000, 01, 13), calendarArray[8]);
            Assert.AreEqual(new DateTime(2000, 01, 14), calendarArray[9]);
        }

        [Test]
        public void Generate_EverySecond_WeekDay()
        {
            //setup
            ICalendarProperties calendarProperties = new DailyCalendarProperties(2, DailyType.WeekDay);

            //exec
            IEnumerable<DateTime> calendar = calendarProperties.GenerateCalendar(new DateTime(2000, 01, 01), Dates.Weekends, Dates.EnglishHolidays);
            DateTime[] calendarArray = calendar.Take(10).ToArray();

            //test
            Assert.AreEqual(new DateTime(2000, 01, 03), calendarArray[0]);
            Assert.AreEqual(new DateTime(2000, 01, 05), calendarArray[1]);
            Assert.AreEqual(new DateTime(2000, 01, 07), calendarArray[2]);
            Assert.AreEqual(new DateTime(2000, 01, 11), calendarArray[3]);
            Assert.AreEqual(new DateTime(2000, 01, 13), calendarArray[4]);
            Assert.AreEqual(new DateTime(2000, 01, 17), calendarArray[5]);
            Assert.AreEqual(new DateTime(2000, 01, 19), calendarArray[6]);
            Assert.AreEqual(new DateTime(2000, 01, 21), calendarArray[7]);
            Assert.AreEqual(new DateTime(2000, 01, 25), calendarArray[8]);
            Assert.AreEqual(new DateTime(2000, 01, 27), calendarArray[9]);
        }

        [Test]
        public void Generate_Each_WeekendDay()
        {
            //setup
            ICalendarProperties calendarProperties = new DailyCalendarProperties(1, DailyType.WeekendDay);

            //exec
            IEnumerable<DateTime> calendar = calendarProperties.GenerateCalendar(new DateTime(2000, 01, 01), Dates.Weekends, Dates.EnglishHolidays);
            DateTime[] calendarArray = calendar.Take(10).ToArray();

            //test
            Assert.AreEqual(new DateTime(2000, 01, 01), calendarArray[0]);
            Assert.AreEqual(new DateTime(2000, 01, 02), calendarArray[1]);
            Assert.AreEqual(new DateTime(2000, 01, 08), calendarArray[2]);
            Assert.AreEqual(new DateTime(2000, 01, 09), calendarArray[3]);
            Assert.AreEqual(new DateTime(2000, 01, 15), calendarArray[4]);
            Assert.AreEqual(new DateTime(2000, 01, 16), calendarArray[5]);
            Assert.AreEqual(new DateTime(2000, 01, 22), calendarArray[6]);
            Assert.AreEqual(new DateTime(2000, 01, 23), calendarArray[7]);
            Assert.AreEqual(new DateTime(2000, 01, 29), calendarArray[8]);
            Assert.AreEqual(new DateTime(2000, 01, 30), calendarArray[9]);
        }

        [Test]
        public void Generate_EverySecond_WeekendDay()
        {
            //setup
            ICalendarProperties calendarProperties = new DailyCalendarProperties(2, DailyType.WeekendDay);

            //exec
            IEnumerable<DateTime> calendar = calendarProperties.GenerateCalendar(new DateTime(2000, 01, 01), Dates.Weekends, Dates.EnglishHolidays);
            DateTime[] calendarArray = calendar.Take(10).ToArray();

            //test
            Assert.AreEqual(new DateTime(2000, 01, 01), calendarArray[0]);
            Assert.AreEqual(new DateTime(2000, 01, 08), calendarArray[1]);
            Assert.AreEqual(new DateTime(2000, 01, 15), calendarArray[2]);
            Assert.AreEqual(new DateTime(2000, 01, 22), calendarArray[3]);
            Assert.AreEqual(new DateTime(2000, 01, 29), calendarArray[4]);
            Assert.AreEqual(new DateTime(2000, 02, 05), calendarArray[5]);
            Assert.AreEqual(new DateTime(2000, 02, 12), calendarArray[6]);
            Assert.AreEqual(new DateTime(2000, 02, 19), calendarArray[7]);
            Assert.AreEqual(new DateTime(2000, 02, 26), calendarArray[8]);
            Assert.AreEqual(new DateTime(2000, 03, 04), calendarArray[9]);
        }

        [Test]
        public void Generate_Each_BusinessDay()
        {
            //setup
            ICalendarProperties calendarProperties = new DailyCalendarProperties(1, DailyType.BusinessDay);

            //exec
            IEnumerable<DateTime> calendar = calendarProperties.GenerateCalendar(new DateTime(2000, 04, 17), Dates.Weekends, Dates.EnglishHolidays);
            DateTime[] calendarArray = calendar.Take(10).ToArray();

            //test
            Assert.AreEqual(new DateTime(2000, 04, 17), calendarArray[0]);
            Assert.AreEqual(new DateTime(2000, 04, 18), calendarArray[1]);
            Assert.AreEqual(new DateTime(2000, 04, 19), calendarArray[2]);
            Assert.AreEqual(new DateTime(2000, 04, 20), calendarArray[3]);
            // 2000-04-21  Good Friday
            // 2000-04-24  Easter Monday
            Assert.AreEqual(new DateTime(2000, 04, 25), calendarArray[4]);
            Assert.AreEqual(new DateTime(2000, 04, 26), calendarArray[5]);
            Assert.AreEqual(new DateTime(2000, 04, 27), calendarArray[6]);
            Assert.AreEqual(new DateTime(2000, 04, 28), calendarArray[7]);
            // 2000-05-01  Early May Bank Holiday
            Assert.AreEqual(new DateTime(2000, 05, 02), calendarArray[8]);
            Assert.AreEqual(new DateTime(2000, 05, 03), calendarArray[9]);
        }

        [Test]
        public void Generate_EverySecond_BusinessDay()
        {
            //setup
            ICalendarProperties calendarProperties = new DailyCalendarProperties(2, DailyType.BusinessDay);

            //exec
            IEnumerable<DateTime> calendar = calendarProperties.GenerateCalendar(new DateTime(2000, 04, 17), Dates.Weekends, Dates.EnglishHolidays);
            DateTime[] calendarArray = calendar.Take(10).ToArray();

            //test
            Assert.AreEqual(new DateTime(2000, 04, 17), calendarArray[0]);
            Assert.AreEqual(new DateTime(2000, 04, 19), calendarArray[1]);
            // 2000-04-21  Good Friday
            // 2000-04-24  Easter Monday
            Assert.AreEqual(new DateTime(2000, 04, 25), calendarArray[2]);
            Assert.AreEqual(new DateTime(2000, 04, 27), calendarArray[3]);
            // 2000-05-01  Early May Bank Holiday
            Assert.AreEqual(new DateTime(2000, 05, 02), calendarArray[4]);
            Assert.AreEqual(new DateTime(2000, 05, 04), calendarArray[5]);
            Assert.AreEqual(new DateTime(2000, 05, 08), calendarArray[6]);
            Assert.AreEqual(new DateTime(2000, 05, 10), calendarArray[7]);
            Assert.AreEqual(new DateTime(2000, 05, 12), calendarArray[8]);
            Assert.AreEqual(new DateTime(2000, 05, 16), calendarArray[9]);
        }

        [Test]
        public void Generate_Each_NonBusinessDay()
        {
            //setup
            ICalendarProperties calendarProperties = new DailyCalendarProperties(1, DailyType.NonBusinessDay);

            //exec
            IEnumerable<DateTime> calendar = calendarProperties.GenerateCalendar(new DateTime(2000, 04, 17), Dates.Weekends, Dates.EnglishHolidays);
            DateTime[] calendarArray = calendar.Take(10).ToArray();

            //test
            Assert.AreEqual(new DateTime(2000, 04, 21), calendarArray[0]); // Good Friday
            Assert.AreEqual(new DateTime(2000, 04, 22), calendarArray[1]);
            Assert.AreEqual(new DateTime(2000, 04, 23), calendarArray[2]);
            Assert.AreEqual(new DateTime(2000, 04, 24), calendarArray[3]); // Easter Monday
            Assert.AreEqual(new DateTime(2000, 04, 29), calendarArray[4]);
            Assert.AreEqual(new DateTime(2000, 04, 30), calendarArray[5]);
            Assert.AreEqual(new DateTime(2000, 05, 01), calendarArray[6]); // Early May Bank Holiday
            Assert.AreEqual(new DateTime(2000, 05, 06), calendarArray[7]);
            Assert.AreEqual(new DateTime(2000, 05, 07), calendarArray[8]);
            Assert.AreEqual(new DateTime(2000, 05, 13), calendarArray[9]);
        }

        [Test]
        public void Generate_EverySecond_NonBusinessDay()
        {
            //setup
            ICalendarProperties calendarProperties = new DailyCalendarProperties(2, DailyType.NonBusinessDay);

            //exec
            IEnumerable<DateTime> calendar = calendarProperties.GenerateCalendar(new DateTime(2000, 04, 17), Dates.Weekends, Dates.EnglishHolidays);
            DateTime[] calendarArray = calendar.Take(10).ToArray();

            //test
            Assert.AreEqual(new DateTime(2000, 04, 21), calendarArray[0]); // Good Friday
            Assert.AreEqual(new DateTime(2000, 04, 23), calendarArray[1]);
            Assert.AreEqual(new DateTime(2000, 04, 29), calendarArray[2]);
            Assert.AreEqual(new DateTime(2000, 05, 01), calendarArray[3]); // Early May Bank Holiday
            Assert.AreEqual(new DateTime(2000, 05, 07), calendarArray[4]);
            Assert.AreEqual(new DateTime(2000, 05, 14), calendarArray[5]);
            Assert.AreEqual(new DateTime(2000, 05, 21), calendarArray[6]);
            Assert.AreEqual(new DateTime(2000, 05, 28), calendarArray[7]);
            Assert.AreEqual(new DateTime(2000, 06, 03), calendarArray[8]);
            Assert.AreEqual(new DateTime(2000, 06, 10), calendarArray[9]);
        }
    }
}
