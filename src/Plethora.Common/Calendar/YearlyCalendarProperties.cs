using System;
using System.Collections.Generic;

namespace Plethora.Calendar
{
    /// <summary>
    /// Represents a calendar which specifies dates which occur every n years.
    /// </summary>
    public sealed class YearlyCalendarProperties : ICalendarProperties
    {
        private readonly int nYearly;

        /// <summary>
        /// Initialises a new instance of the <see cref="YearlyCalendarProperties"/> class.
        /// </summary>
        /// <param name="nYearly">The number of years between occurances in this calendar.</param>
        public YearlyCalendarProperties(int nYearly)
        {
            if (nYearly <= 0)
                throw new ArgumentOutOfRangeException(nameof(nYearly), nYearly, ResourceProvider.ArgMustBeGreaterThanZero(nameof(nYearly)));


            this.nYearly = nYearly;
        }

        /// <inheritdoc/>
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
