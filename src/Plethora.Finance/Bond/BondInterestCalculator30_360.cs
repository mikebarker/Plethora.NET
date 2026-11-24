using System;

namespace Plethora.Finance.Bond;

public class BondInterestCalculator30_360 : IBondInterestCalculator, IBondInterestCalculatorEOM, IBondInterestCalculatorICMA, IBondInterestCalculatorFull
{
    public Rational CalculateDayCountFraction(
        DateTime accrualStartDate,
        DateTime accrualEndDate)
    {
        if (accrualEndDate <= accrualStartDate)
            throw new ArgumentException(ResourceProvider.ArgMustBeLessThan(nameof(accrualStartDate), nameof(accrualEndDate)));


        int Y1 = accrualStartDate.Year;
        int Y2 = accrualEndDate.Year;
        int M1 = accrualStartDate.Month;
        int M2 = accrualEndDate.Month;

        int D1 = (accrualStartDate.Day == 31)
            ? 30
            : accrualStartDate.Day;

        int D2 = ((accrualEndDate.Day == 31) && (D1 > 29))
            ? 30
            : accrualEndDate.Day;

        int numerator = 360 * (Y2 - Y1) +
                        30 * (M2 - M1) +
                        (D2 - D1);

        int denominator = 360;

        Rational dayCountFraction = new(numerator, denominator);
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
