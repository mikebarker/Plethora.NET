using System;
using System.Collections.Generic;

namespace Plethora.Calendar
{
    public interface ICalendarProperties
    {
        /// <summary>
        /// Generate a non-terminating list of dates.
        /// </summary>
        /// <param name="startDate">The date from which the series must start.</param>
        /// <param name="weekendDays">A list of <see cref="DayOfWeek"/> which represents the weekend.</param>
        /// <param name="holidays">A list of <see cref="DateTime"/> which represents the holidays.</param>
        /// <returns>
        /// A non-ending list of dates.
        /// </returns>
        /// <remarks>
        /// Care must be taken when using the results of this function, as the enumerator will not terminate.
        /// </remarks>
        IEnumerable<DateTime> GenerateCalendar(
            DateTime startDate,
            IEnumerable<DayOfWeek> weekendDays,
            IEnumerable<DateTime> holidays);
    }
}
