using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Calendar
{
    /// <summary>
    /// Represents a calendar which specifies dates which occur every n days.
    /// </summary>
    public sealed class DailyCalendarProperties : ICalendarProperties
    {
        private readonly int nDaily;
        private readonly DailyType dailyType;

        /// <summary>
        /// Initialise a new instance of the <see cref="DailyCalendarProperties"/> class.
        /// </summary>
        /// <param name="nDaily">The number of days between occurances in this calendar.</param>
        /// <param name="dailyType">The type of days to be represented in the calendar.</param>
        public DailyCalendarProperties(int nDaily, DailyType dailyType)
        {
            if (nDaily <= 0)
                throw new ArgumentOutOfRangeException(nameof(nDaily), nDaily, ResourceProvider.ArgMustBeGreaterThanZero(nameof(nDaily)));

            DailyType[] validDailyTypes =
            {
                DailyType.Day,
                DailyType.WeekDay,
                DailyType.WeekendDay,
                DailyType.BusinessDay,
                DailyType.NonBusinessDay,
            };

            if (!validDailyTypes.Contains(dailyType))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(dailyType),
                    dailyType,
                    ResourceProvider.ArgMustBeOneOf(nameof(dailyType), validDailyTypes));
            }


            this.nDaily = nDaily;
            this.dailyType = dailyType;
        }

        /// <inheritdoc/>
        public IEnumerable<DateTime> GenerateCalendar(
            DateTime startDate,
            IEnumerable<DayOfWeek> weekendDays,
            IEnumerable<DateTime> holidays)
        {
            if (weekendDays == null)
                throw new ArgumentNullException(nameof(weekendDays));

            if (holidays == null)
                throw new ArgumentNullException(nameof(holidays));


            weekendDays = weekendDays.Distinct().ToArray();

            //Avoid constructing a new hash set if not required.
            if (!(holidays is HashSet<DateTime>))
                holidays = new HashSet<DateTime>(holidays);

            int count = 0;
            DateTime date = startDate;
            while (true)
            {
                bool includeDate = true;
                switch (this.dailyType)
                {
                    case DailyType.Day:
                        includeDate = true;
                        break;

                    case DailyType.WeekDay:
                        includeDate = !weekendDays.Contains(date.DayOfWeek);
                        break;

                    case DailyType.WeekendDay:
                        includeDate = weekendDays.Contains(date.DayOfWeek);
                        break;

                    case DailyType.BusinessDay:
                        includeDate =
                            !weekendDays.Contains(date.DayOfWeek) &&
                            !holidays.Contains(date);
                        break;

                    case DailyType.NonBusinessDay:
                        includeDate =
                            weekendDays.Contains(date.DayOfWeek) ||
                            holidays.Contains(date);
                        break;
                }

                if (includeDate)
                {
                    if (count == 0)
                        yield return date;

                    count++;
                    count = count % this.nDaily;
                }

                date = date.AddDays(1);
            }
        }
    }

    /// <summary>
    /// The type of days to be represented in the daily calendar.
    /// </summary>
    public enum DailyType
    {
        /// <summary>
        /// Every day is represented in the daily calendar.
        /// </summary>
        Day,

        /// <summary>
        /// Only week days are represented in the daily calendar.
        /// </summary>
        WeekDay,

        /// <summary>
        /// Only weekend days are represented in the daily calendar.
        /// </summary>
        WeekendDay,

        /// <summary>
        /// Only business days are represented in the daily calendar.
        /// </summary>
        BusinessDay,

        /// <summary>
        /// Only non-business days are represented in the daily calendar.
        /// </summary>
        NonBusinessDay,
    }
}
