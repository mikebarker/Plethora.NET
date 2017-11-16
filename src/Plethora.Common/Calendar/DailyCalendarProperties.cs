using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Calendar
{
    public sealed class DailyCalendarProperties : ICalendarProperties
    {
        private readonly int nDaily;
        private readonly DailyType dailyType;

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

    public enum DailyType
    {
        Day,
        WeekDay,
        WeekendDay,
        BusinessDay,
        NonBusinessDay,
    }
}
