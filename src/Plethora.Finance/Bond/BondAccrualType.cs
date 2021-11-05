namespace Plethora.Finance.Bond
{
    /// <summary>
    /// The ISDA accrual types.
    /// </summary>
    /// <remarks>
    /// <seealso cref="https://www.rbs.com/content/dam/natwestmarkets_com/pdf/Disclosures/2006-isda-defs.pdf"/>
    /// </remarks>
    public enum BondAccrualType
    {
        _1,
        _30_360,
        _30U_360,
        _30E_360,
        _30E_360_ISDA,
        Actual_Actual,
        // Actual_Actual_ICMA,
        Actual_365F,
        Actual_360,
    }
}
