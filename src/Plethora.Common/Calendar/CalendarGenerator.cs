using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Calendar
{
    public static class CalendarGenerator
    {
        public static DateTime[] Generate(
            DateTime startDate,
            DateTime endDate,
            ICalendarProperties calendarProperties,
            DateRollType dateRollType,
            IEnumerable<DayOfWeek> weekendDays,
            IEnumerable<DateTime> holidays)
        {
            IEnumerable<DateTime> calendar = GenerateInternal(
                startDate,
                calendarProperties,
                dateRollType,
                weekendDays,
                holidays);

            //Date rolling can cause dates to occur outside the requested range. Therefore re-filter.
            DateTime[] result = calendar
                .SkipWhile(date => date < startDate)
                .TakeWhile(date => date <= endDate)
                .ToArray();

            return result;
        }

        public static DateTime[] Generate(
            DateTime startDate,
            int numberOfEntries,
            ICalendarProperties calendarProperties,
            DateRollType dateRollType,
            IEnumerable<DayOfWeek> weekendDays,
            IEnumerable<DateTime> holidays)
        {
            IEnumerable<DateTime> calendar = GenerateInternal(
                startDate,
                calendarProperties,
                dateRollType,
                weekendDays,
                holidays);

            //Date rolling can cause dates to occur outside the requested range. Therefore re-filter.
            DateTime[] result = calendar
                .SkipWhile(date => date < startDate)
                .Take(numberOfEntries)
                .ToArray();

            return result;
        }

        private static IEnumerable<DateTime> GenerateInternal(
            DateTime startDate,
            ICalendarProperties calendarProperties,
            DateRollType dateRollType,
            IEnumerable<DayOfWeek> weekendDays,
            IEnumerable<DateTime> holidays)
        {
            ValidateDateRollType(dateRollType);

            // weekdays is unlikely to contain more than 2 days, and highly unlikely to contain more than 7. Therefore a linear collection search will be faster than hash lookup.
            ICollection<DayOfWeek> weekendDaysHashSet = weekendDays.Distinct().OrderBy(dow => dow).ToArray();
            ICollection<DateTime> holidaysHashSet = new HashSet<DateTime>(holidays);

            IEnumerable<DateTime> calendar = calendarProperties.GenerateCalendar(startDate, weekendDays, holidays);

            IEnumerable<DateTime> rolledCalendar = calendar.Select(date =>
                RollDateInternal(
                    date,
                    dateRollType,
                    weekendDaysHashSet,
                    holidaysHashSet));

            return rolledCalendar;
        }

        public static DateTime RollDate(
            DateTime date,
            DateRollType dateRollType,
            IEnumerable<DayOfWeek> weekendDays,
            IEnumerable<DateTime> holidays)
        {
            ValidateDateRollType(dateRollType);

            // weekdays is unlikely to contain more than 2 days, and highly unlikely to contain more than 7. Therefore a linear collection search will be faster than hash lookup.
            ICollection<DayOfWeek> weekendDaysHashSet = weekendDays.Distinct().OrderBy(dow => dow).ToArray();
            ICollection<DateTime> holidaysHashSet = new HashSet<DateTime>(holidays);

            return RollDateInternal(
                date,
                dateRollType,
                weekendDaysHashSet,
                holidaysHashSet);
        }

        private static void ValidateDateRollType(DateRollType dateRollType)
        {
            if ((dateRollType != DateRollType.Actual) &&
                (dateRollType != DateRollType.Following) &&
                (dateRollType != DateRollType.ModifiedFollowing) &&
                (dateRollType != DateRollType.Preceding) &&
                (dateRollType != DateRollType.ModifiedPreceding))
            {
                throw new ArgumentException("Unknown DateRollType.", nameof(dateRollType));
            }
        }

        private static DateTime RollDateInternal(
            DateTime date,
            DateRollType dateRollType,
            ICollection<DayOfWeek> weekendDays,
            ICollection<DateTime> holidays)
        {
            if (dateRollType == DateRollType.Actual)
                return date;

            DateTime initialDate = date;
            bool isModified = false;

            while (weekendDays.Contains(date.DayOfWeek) || holidays.Contains(date))
            {
                switch (dateRollType)
                {
                    case DateRollType.Following:
                        date = date.AddDays(1);
                        break;

                    case DateRollType.ModifiedFollowing:
                        if (!isModified)
                        {
                            date = date.AddDays(1);

                            if (date.Month != initialDate.Month)
                            {
                                isModified = true;
                                date = initialDate;
                            }
                        }

                        if (isModified)
                        {
                            date = date.AddDays(-1);
                        }

                        break;

                    case DateRollType.Preceding:
                        date = date.AddDays(-1);
                        break;

                    case DateRollType.ModifiedPreceding:
                        if (!isModified)
                        {
                            date = date.AddDays(-1);

                            if (date.Month != initialDate.Month)
                            {
                                isModified = true;
                                date = initialDate;
                            }
                        }

                        if (isModified)
                        {
                            date = date.AddDays(1);
                        }

                        break;
                }
            }

            return date;
        }
    }
}
