using System;

namespace Plethora.Finance.Bond
{
    public class BondInterestCalculatorActual_365F : IBondInterestCalculator
    {
        Rational IBondInterestCalculator.CalculateDayCountFraction(
            DateTime accrualStartDate,
            DateTime accrualEndDate,
            bool isPaysEndOfMonth)
        {
            if (accrualEndDate <= accrualStartDate)
                throw new ArgumentException(ResourceProvider.ArgMustBeLessThan(nameof(accrualStartDate), nameof(accrualEndDate)));


            return this.CalculateDayCountFraction(accrualStartDate, accrualEndDate);
        }

        public Rational CalculateDayCountFraction(
            DateTime accrualStartDate,
            DateTime accrualEndDate)
        {
            if (accrualEndDate <= accrualStartDate)
                throw new ArgumentException(ResourceProvider.ArgMustBeLessThan(nameof(accrualStartDate), nameof(accrualEndDate)));


            int daysInYear = (accrualEndDate - accrualStartDate).Days;

            Rational dayCountFraction = new Rational(
                daysInYear,
                365);

            return dayCountFraction;
        }
    }
}
