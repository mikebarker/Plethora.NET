using System;
using System.Collections.Generic;

namespace Plethora.Calendar
{
    public sealed class YearlyCalendarProperties : ICalendarProperties
    {
        private readonly int nYearly;

        public YearlyCalendarProperties(int nYearly)
        {
            if (nYearly <= 0)
                throw new ArgumentOutOfRangeException(nameof(nYearly), nYearly, ResourceProvider.ArgMustBeGreaterThanZero(nameof(nYearly)));


            this.nYearly = nYearly;
        }

        public IEnumerable<DateTime> GenerateCalendar(
            DateTime startDate,
            IEnumerable<DayOfWeek> weekendDays,
            IEnumerable<DateTime> holidays)
        {
            int year = startDate.Year;
            int month = startDate.Month;
            int day = startDate.Day;

            while (true)
            {
                DateTime date = new DateTime(year, month, day);
                if (date >= startDate)
                {
                    yield return date;
                }

                year += this.nYearly;
            }
        }


    }
}
