using System;

namespace Plethora.Finance.Bond
{
    public interface IBondInterestCalculator
    {
        Rational CalculateDayCountFraction(
            DateTime accrualStartDate,
            DateTime accrualEndDate,
            bool isPaysEndOfMonth);
    }
}
