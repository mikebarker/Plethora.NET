using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Plethora.Finance.Bond;

public class BondInterestCalculator : IBondInterestCalculator, IBondInterestCalculatorEOM, IBondInterestCalculatorICMA, IBondInterestCalculatorFull
{
    private static readonly Dictionary<BondAccrualType, IBondInterestCalculatorFull> register = new()
    {
        { BondAccrualType._1, new BondInterestCalculator1() },
        { BondAccrualType._30_360, new BondInterestCalculator30_360() },
        { BondAccrualType._30U_360, new BondInterestCalculator30U_360() },
        { BondAccrualType._30E_360, new BondInterestCalculator30E_360() },
        { BondAccrualType._30E_360_ISDA, new BondInterestCalculator30E_360_ISDA() },
        { BondAccrualType.Actual_Actual, new BondInterestCalculatorActual_Actual() },
        { BondAccrualType.Actual_Actual_ICMA, new BondInterestCalculatorActual_Actual_ICMA() },
        { BondAccrualType.Actual_365F, new BondInterestCalculatorActual_365F() },
        { BondAccrualType.Actual_360, new BondInterestCalculatorActual_360() },
    };

    public static void RegisterCalculator(BondAccrualType bondAccrualType, IBondInterestCalculatorFull bondInterestCalculator)
    {
        lock (register)
        {
            register[bondAccrualType] = bondInterestCalculator;
        }
    }

    public static IBondInterestCalculatorFull GetCalculator(BondAccrualType bondAccrualType)
    {
        lock (register)
        {
            return GetCalculator<IBondInterestCalculatorFull>(bondAccrualType);
        }
    }

    public static T GetCalculator<T>(BondAccrualType bondAccrualType)
    {
        lock (register)
        {
            if (!register.TryGetValue(bondAccrualType, out var calculator))
            {
                throw new NotSupportedException("There is no calculator registered for the specified BondAccrualType.");
            }

            if (calculator is not T tCalculator)
            {
                throw new NotSupportedException("The calculator registered for the specified BondAccrualType is not of the expected type.");
            }

            return tCalculator;
        }
    }

    private readonly BondAccrualType bondAccrualType;

    public BondInterestCalculator(BondAccrualType bondAccrualType)
    {
        this.bondAccrualType = bondAccrualType;
    }

    Rational IBondInterestCalculator.CalculateDayCountFraction(
        DateTime accrualStartDate,
        DateTime accrualEndDate)
    {
        return GetCalculator<IBondInterestCalculator>(this.bondAccrualType).CalculateDayCountFraction(
            accrualStartDate,
            accrualEndDate);
    }

    Rational IBondInterestCalculatorEOM.CalculateDayCountFraction(
        DateTime accrualStartDate,
        DateTime accrualEndDate,
        bool isPaysEndOfMonth)
    {
        return GetCalculator<IBondInterestCalculatorEOM>(this.bondAccrualType).CalculateDayCountFraction(
            accrualStartDate,
            accrualEndDate,
            isPaysEndOfMonth);
    }

    Rational IBondInterestCalculatorICMA.CalculateDayCountFraction(
        DateTime accrualStartDate,
        DateTime accrualEndDate,
        DateTime nextPaymentDate,
        int frequency)
    {
        return GetCalculator<IBondInterestCalculatorICMA>(this.bondAccrualType).CalculateDayCountFraction(
            accrualStartDate,
            accrualEndDate,
            nextPaymentDate,
            frequency);
    }

    Rational IBondInterestCalculatorFull.CalculateDayCountFraction(
        DateTime accrualStartDate,
        DateTime accrualEndDate,
        DateTime nextPaymentDate,
        int frequency,
        bool isPaysEndOfMonth)
    {
        return GetCalculator<IBondInterestCalculatorFull>(this.bondAccrualType).CalculateDayCountFraction(
            accrualStartDate,
            accrualEndDate,
            nextPaymentDate,
            frequency,
            isPaysEndOfMonth);
    }
}
