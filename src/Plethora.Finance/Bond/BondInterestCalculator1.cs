using System;

namespace Plethora.Finance.Bond;

public class BondInterestCalculator1 : IBondInterestCalculator
{
    Rational IBondInterestCalculator.CalculateDayCountFraction(
        DateTime accrualStartDate,
        DateTime accrualEndDate,
        bool isPaysEndOfMonth)
    {
        return CalculateDayCountFraction();
    }

    public static Rational CalculateDayCountFraction()
    {
        Rational dayCountFraction = new(1, 1);
        return dayCountFraction;
    }
}
