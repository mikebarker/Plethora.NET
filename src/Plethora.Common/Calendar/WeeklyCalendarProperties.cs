using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Calendar
{
    /// <summary>
    /// Represents a calendar which specifies dates which occur every n months.
    /// </summary>
    public sealed class WeeklyCalendarProperties : ICalendarProperties
    {
        private readonly int nWeekly;
        private readonly DayOfWeek[] daysOfWeek;


        /// <summary>
        /// Initialise a new instance of the <see cref="WeeklyCalendarProperties"/> class.
        /// </summary>
        /// <param name="nWeekly">The number of weeks between occurances in this calendar.</param>
        public WeeklyCalendarProperties(int nWeekly, IEnumerable<DayOfWeek> daysOfWeek)
        {
            if (nWeekly <= 0)
                throw new ArgumentOutOfRangeException(nameof(nWeekly), nWeekly, ResourceProvider.ArgMustBeGreaterThanZero(nameof(nWeekly)));

            if (daysOfWeek == null)
                throw new ArgumentNullException(nameof(daysOfWeek));


            this.nWeekly = nWeekly;
            this.daysOfWeek = daysOfWeek.Distinct().OrderBy(dow => dow).ToArray();

            if (this.daysOfWeek.Length == 0)
                throw new ArgumentException(ResourceProvider.AtLeastOneDayOfWeek(), nameof(daysOfWeek));
        }

        /// <inheritdoc/>
        public IEnumerable<DateTime> GenerateCalendar(
            DateTime startDate,
            IEnumerable<DayOfWeek> weekendDays,
            IEnumerable<DateTime> holidays)
        {
            while (!((IList<DayOfWeek>)this.daysOfWeek).Contains(startDate.DayOfWeek))
            {
                startDate = startDate.AddDays(1);
            }

            DateTime firstDateOfWeek = startDate;
            firstDateOfWeek = firstDateOfWeek.AddDays(-(int)firstDateOfWeek.DayOfWeek);

            while (true)
            {
                foreach (DayOfWeek dayOfWeek in this.daysOfWeek)
                {
                    DateTime date = firstDateOfWeek.AddDays((int)dayOfWeek);

                    if (startDate <= date)
                    {
                        yield return date;
                    }
                }

                firstDateOfWeek = firstDateOfWeek.AddDays(7 * this.nWeekly);
            }
        }
    }
}
