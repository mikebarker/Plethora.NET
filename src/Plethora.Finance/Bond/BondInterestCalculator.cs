using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Plethora.Finance.Bond
{
    public class BondInterestCalculator
    {
        private static readonly IDictionary<BondAccrualType, IBondInterestCalculator> bondInterestCalculatorRegister =
            new ConcurrentDictionary<BondAccrualType, IBondInterestCalculator>();

        public static void RegisterCalculator(BondAccrualType bondAccrualType, IBondInterestCalculator bondInterestCalculator)
        {
            lock (bondInterestCalculatorRegister)
            {
                bondInterestCalculatorRegister[bondAccrualType] = bondInterestCalculator;
            }
        }

        private static IBondInterestCalculator GetBondInterestCalculator(BondAccrualType bondAccrualType)
        {
            lock (bondInterestCalculatorRegister)
            {
                return bondInterestCalculatorRegister[bondAccrualType];
            }
        }

        static BondInterestCalculator()
        {
            RegisterCalculator(BondAccrualType._1, new BondInterestCalculator1());
            RegisterCalculator(BondAccrualType._30_360, new BondInterestCalculator30_360());
            RegisterCalculator(BondAccrualType._30U_360, new BondInterestCalculator30U_360());
            RegisterCalculator(BondAccrualType._30E_360, new BondInterestCalculator30E_360());
            RegisterCalculator(BondAccrualType._30E_360_ISDA, new BondInterestCalculator30E_360_ISDA());
            RegisterCalculator(BondAccrualType.Actual_Actual, new BondInterestCalculatorActual_Actual());
            //RegisterCalculator(BondAccrualType.Actual_Actual_ICMA, new BondInterestCalculatorActual_Actual_ICMA());
            RegisterCalculator(BondAccrualType.Actual_365F, new BondInterestCalculatorActual_365F());
            RegisterCalculator(BondAccrualType.Actual_360, new BondInterestCalculatorActual_360());
        }

        public Rational CalculateDayCountFraction(
            BondAccrualType bondAccrualType,
            DateTime accrualStartDate,
            DateTime accrualEndDate,
            bool isPaysEndOfMonth)
        {
            IBondInterestCalculator bondInterestCalculator = GetBondInterestCalculator(bondAccrualType);

            Rational dayCountFraction = bondInterestCalculator.CalculateDayCountFraction(
                accrualStartDate,
                accrualEndDate,
                isPaysEndOfMonth);

            return dayCountFraction;
        }
    }
}
