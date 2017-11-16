namespace Plethora.Calendar
{
    /// <summary>
    /// The nature of the date rolling which is to be conducted to ensure a date falls on a business day.
    /// </summary>
    public enum DateRollType
    {
        /// <summary>
        /// No rolling occurs, even if it is a non-business day
        /// </summary>
        Actual,

        /// <summary>
        /// If the date is a non-business day, the date is rolled to the next business day.
        /// </summary>
        Following,

        /// <summary>
        /// If the date is a non-business day, the date is rolled to the next business day, unless doing so would cause
        /// the payment to be in the next calendar month, in which case the date is rolled to the previous business day.
        /// </summary>
        ModifiedFollowing,

        /// <summary>
        /// If the date is a non-business day, the date is rolled to the next business day.
        /// </summary>
        Preceding,

        /// <summary>
        /// If the date is a non-business day, the date is rolled to the previous business day, unless doing so would cause
        /// the payment to be in the previous calendar month, in which case the date is rolled to the next business day.
        /// </summary>
        ModifiedPreceding,
    }
}
