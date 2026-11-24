using System;

namespace Plethora.Finance.Bond;

public class BondInterestCalculator1 : IBondInterestCalculator, IBondInterestCalculatorEOM, IBondInterestCalculatorICMA, IBondInterestCalculatorFull
{
    private static readonly Rational dayCountFraction = new(1, 1);

    public Rational CalculateDayCountFraction(
        DateTime accrualStartDate,
        DateTime accrualEndDate)
    {
        return dayCountFraction;
    }

    Rational IBondInterestCalculatorEOM.CalculateDayCountFraction(
        DateTime accrualStartDate,
        DateTime accrualEndDate,
        bool isPaysEndOfMonth)
    {
        return CalculateDayCountFraction(accrualStartDate, accrualEndDate);
    }

    Rational IBondInterestCalculatorICMA.CalculateDayCountFraction(
        DateTime accrualStartDate,
        DateTime accrualEndDate,
        DateTime nextPaymentDate,
        int frequency)
    {
        return CalculateDayCountFraction(accrualStartDate, accrualEndDate);
    }

    Rational IBondInterestCalculatorFull.CalculateDayCountFraction(
        DateTime accrualStartDate,
        DateTime accrualEndDate,
        DateTime nextPaymentDate,
        int frequency,
        bool isPaysEndOfMonth)
    {
        return CalculateDayCountFraction(accrualStartDate, accrualEndDate);
    }
}
