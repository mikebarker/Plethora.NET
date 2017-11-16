using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Calendar
{
    public sealed class MonthlyCalendarProperties : ICalendarProperties
    {
        private readonly int nMonthly;
        private readonly bool lastDayOfMonth;
        private readonly int[] daysOfMonth;

        public MonthlyCalendarProperties(int nMonthly, IEnumerable<int> daysOfMonth)
            : this(nMonthly, daysOfMonth, false)
        {
        }

        public MonthlyCalendarProperties(int nMonthly, IEnumerable<int> daysOfMonth, bool lastDayOfMonth)
        {
            if (nMonthly <= 0)
                throw new ArgumentOutOfRangeException(nameof(nMonthly), nMonthly, ResourceProvider.ArgMustBeGreaterThanZero(nameof(nMonthly)));

            if (daysOfMonth == null)
                throw new ArgumentNullException(nameof(daysOfMonth));


            this.nMonthly = nMonthly;
            this.lastDayOfMonth = lastDayOfMonth;
            this.daysOfMonth = daysOfMonth.Distinct().OrderBy(dt => dt).ToArray();

            if ((!lastDayOfMonth) && (this.daysOfMonth.Length == 0))
                throw new ArgumentException(ResourceProvider.AtLeastOneDateOrEom());
        }

        public IEnumerable<DateTime> GenerateCalendar(
            DateTime startDate,
            IEnumerable<DayOfWeek> weekendDays,
            IEnumerable<DateTime> holidays)
        {
            int year = startDate.Year;
            int month = startDate.Month;

            while (true)
            {
                DateTime date = DateTime.MinValue;
                foreach (int day in this.daysOfMonth)
                {
                    date = new DateTime(year, month, day);

                    if (date >= startDate)
                    {
                        yield return date;
                    }
                }

                if (this.lastDayOfMonth)
                {
                    int day = DateTime.DaysInMonth(year, month);
                    DateTime lastDateOfMonth = new DateTime(year, month, day);

                    if (date != lastDateOfMonth)
                    {
                        if (lastDateOfMonth >= startDate)
                        {
                            yield return lastDateOfMonth;
                        }
                    }
                }

                month += this.nMonthly;
                if (month >= 13)
                {
                    int remainder;
                    int quotient = Math.DivRem(month, 12, out remainder);

                    month = remainder;
                    year += quotient;
                }
            }
        }
    }
}
