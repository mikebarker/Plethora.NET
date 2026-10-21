using System;

namespace Plethora.Finance.Bond;

public class BondInterestCalculatorActual_Actual_ICMA
{
    public static Rational CalculateDayCountFraction(
        DateTime accrualStartDate,
        DateTime accrualEndDate,
        DateTime nextPaymentDate,
        int frequency,
        bool isPaysEndOfMonth)
    {
        if (accrualEndDate <= accrualStartDate)
            throw new ArgumentException(ResourceProvider.ArgMustBeLessThan(nameof(accrualStartDate), nameof(accrualEndDate)));

        if (nextPaymentDate <= accrualStartDate)
            throw new ArgumentException(ResourceProvider.ArgMustBeLessThan(nameof(accrualStartDate), nameof(nextPaymentDate)));

        if (nextPaymentDate < accrualEndDate)
            throw new ArgumentException(ResourceProvider.ArgMustBeLessThanEqualTo(nameof(accrualEndDate), nameof(nextPaymentDate)));

        if (frequency <= 0)
            throw new ArgumentException(ResourceProvider.ArgMustBeGreaterThanZero(nameof(frequency)));


        return CalculateDayCountFraction(accrualStartDate, accrualEndDate, nextPaymentDate, frequency);
    }

    public static Rational CalculateDayCountFraction(
        DateTime accrualStartDate,
        DateTime accrualEndDate,
        DateTime nextPaymentDate,
        int frequency)
    {
        if (accrualEndDate <= accrualStartDate)
            throw new ArgumentException(ResourceProvider.ArgMustBeLessThan(nameof(accrualStartDate), nameof(accrualEndDate)));

        if (nextPaymentDate <= accrualStartDate)
            throw new ArgumentException(ResourceProvider.ArgMustBeLessThan(nameof(accrualStartDate), nameof(nextPaymentDate)));

        if (nextPaymentDate < accrualEndDate)
            throw new ArgumentException(ResourceProvider.ArgMustBeLessThanEqualTo(nameof(accrualEndDate), nameof(nextPaymentDate)));

        if (frequency <= 0)
            throw new ArgumentException(ResourceProvider.ArgMustBeGreaterThanZero(nameof(frequency)));


        int days12 = (accrualEndDate - accrualStartDate).Days;
        int days13 = (nextPaymentDate - accrualStartDate).Days;

        int numerator = days12;
        int denominator = frequency * days13;

        Rational dayCountFraction = new(
            numerator,
            denominator);

        return dayCountFraction;
    }
}
