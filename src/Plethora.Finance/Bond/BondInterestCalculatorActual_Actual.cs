using System;

namespace Plethora.Finance.Bond
{
    public class BondInterestCalculatorActual_Actual : IBondInterestCalculator
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


            if (accrualStartDate.Year == accrualEndDate.Year)
            {
                int daysInYear = (accrualEndDate - accrualStartDate).Days;

                Rational dayCountFraction = new Rational(
                    daysInYear,
                    DateTime.IsLeapYear(accrualStartDate.Year) ? 366 : 365);

                return dayCountFraction;
            }
            else
            {
                int daysInStartYear = (new DateTime(accrualStartDate.Year + 1, 01, 01) - accrualStartDate).Days;
                int daysInEndYear = (accrualEndDate - new DateTime(accrualEndDate.Year, 01, 01)).Days;

                Rational startYearFraction = new Rational(
                    daysInStartYear,
                    DateTime.IsLeapYear(accrualStartDate.Year) ? 366 : 365);

                Rational endYearFraction = new Rational(
                    daysInEndYear,
                    DateTime.IsLeapYear(accrualEndDate.Year) ? 366 : 365);

                Rational midYearFractions = new Rational(
                    (accrualEndDate.Year - accrualStartDate.Year - 1),
                    1);

                Rational dayCountFraction = startYearFraction + midYearFractions + endYearFraction;
                return dayCountFraction;
            }
        }
    }
}
