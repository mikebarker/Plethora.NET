using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Calendar
{
    /// <summary>
    /// Represents a calendar which specifies dates which occur every n months.
    /// </summary>
    public sealed class MonthlyCalendarProperties : ICalendarProperties
    {
        private readonly int nMonthly;
        private readonly bool lastDayOfMonth;
        private readonly int[] daysOfMonth;

        /// <summary>
        /// Initialise a new instance of the <see cref="MonthlyCalendarProperties"/> class.
        /// </summary>
        /// <param name="nMonthly">The number of months between occurrences in this calendar.</param>
        /// <param name="daysOfMonth">The days of the month which are included in this calendar.</param>
        public MonthlyCalendarProperties(int nMonthly, IEnumerable<int> daysOfMonth)
            : this(nMonthly, daysOfMonth, false)
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="MonthlyCalendarProperties"/> class.
        /// </summary>
        /// <param name="nMonthly">The number of months between occurrences in this calendar.</param>
        /// <param name="daysOfMonth">The days of the month which are included in this calendar.</param>
        /// <param name="lastDayOfMonth">True if the last day of the month is included in the calendar; otherwise false.</param>
        /// <remarks>
        /// <paramref name="lastDayOfMonth"/> is considered in addition to <see cref="daysOfMonth"/>.
        /// </remarks>
        public MonthlyCalendarProperties(int nMonthly, IEnumerable<int> daysOfMonth, bool lastDayOfMonth)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(nMonthly, 0);
            ArgumentNullException.ThrowIfNull(daysOfMonth);


            this.nMonthly = nMonthly;
            this.lastDayOfMonth = lastDayOfMonth;
            this.daysOfMonth = daysOfMonth.Distinct().OrderBy(dt => dt).ToArray();

            if ((!lastDayOfMonth) && (this.daysOfMonth.Length == 0))
                throw new ArgumentException(ResourceProvider.AtLeastOneDateOrEom());
        }

        /// <inheritdoc/>
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
                    date = new(year, month, day);

                    if (date >= startDate)
                    {
                        yield return date;
                    }
                }

                if (this.lastDayOfMonth)
                {
                    int day = DateTime.DaysInMonth(year, month);
                    DateTime lastDateOfMonth = new(year, month, day);

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
                    int quotient = Math.DivRem(month, 12, out var remainder);

                    month = remainder;
                    year += quotient;
                }
            }
        }
    }
}
