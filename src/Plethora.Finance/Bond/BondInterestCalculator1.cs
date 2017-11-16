using System;

namespace Plethora.Finance.Bond
{
    public class BondInterestCalculator1 : IBondInterestCalculator
    {
        Rational IBondInterestCalculator.CalculateDayCountFraction(
            DateTime accrualStartDate,
            DateTime accrualEndDate,
            bool isPaysEndOfMonth)
        {
            return this.CalculateDayCountFraction();
        }

        public Rational CalculateDayCountFraction()
        {
            Rational dayCountFraction = new Rational(1, 1);
            return dayCountFraction;
        }
    }
}
