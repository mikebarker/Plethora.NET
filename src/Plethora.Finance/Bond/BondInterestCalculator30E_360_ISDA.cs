using System;

namespace Plethora.Finance.Bond
{
    public class BondInterestCalculator30E_360_ISDA : IBondInterestCalculator
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


            int Y1 = accrualStartDate.Year;
            int Y2 = accrualEndDate.Year;
            int M1 = accrualStartDate.Month;
            int M2 = accrualEndDate.Month;

            int D1 = accrualStartDate.Day;
            DateTime lastDayOfFebStartYear = new DateTime(accrualStartDate.Year, 3, 1).AddDays(-1);
            if ((accrualStartDate == lastDayOfFebStartYear) || (D1 == 31))
            {
                D1 = 30;
            }

            int D2 = accrualEndDate.Day;
            DateTime lastDayOfFebEndYear = new DateTime(accrualEndDate.Year, 3, 1).AddDays(-1);
            if ((accrualEndDate == lastDayOfFebEndYear) || (D2 == 31))
            {
                D2 = 30;
            }
            

            int numerator = 360 * (Y2 - Y1) +
                            30 * (M2 - M1) +
                            (D2 - D1);

            int denominator = 360;

            Rational dayCountFraction = new Rational(numerator, denominator);
            return dayCountFraction;
        }
    }
}
