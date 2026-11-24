using System;

namespace Plethora.Finance.Bond;

public class BondInterestCalculatorActual_360 : IBondInterestCalculator, IBondInterestCalculatorEOM, IBondInterestCalculatorICMA, IBondInterestCalculatorFull
{
    public Rational CalculateDayCountFraction(
        DateTime accrualStartDate,
        DateTime accrualEndDate)
    {
        if (accrualEndDate <= accrualStartDate)
            throw new ArgumentException(ResourceProvider.ArgMustBeLessThan(nameof(accrualStartDate), nameof(accrualEndDate)));


        int daysInYear = (accrualEndDate - accrualStartDate).Days;

        Rational dayCountFraction = new(
            daysInYear,
            360);

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
