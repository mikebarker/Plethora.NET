using System;

namespace Plethora.Finance.Bond;


public interface IBondInterestCalculator
{
    Rational CalculateDayCountFraction(
        DateTime accrualStartDate,
        DateTime accrualEndDate);
}

public interface IBondInterestCalculatorEOM
{
    Rational CalculateDayCountFraction(
        DateTime accrualStartDate,
        DateTime accrualEndDate,
        bool isPaysEndOfMonth);
}

public interface IBondInterestCalculatorICMA
{
    Rational CalculateDayCountFraction(
        DateTime accrualStartDate,
        DateTime accrualEndDate,
        DateTime nextPaymentDate,
        int frequency);
}

public interface IBondInterestCalculatorFull
{
    Rational CalculateDayCountFraction(
        DateTime accrualStartDate,
        DateTime accrualEndDate,
        DateTime nextPaymentDate,
        int frequency,
        bool isPaysEndOfMonth);
}
