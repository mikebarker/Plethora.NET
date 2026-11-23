using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Maths;

namespace Plethora.Test.Maths;

/// <summary>
/// Test class for the <see cref="Prime"/> class.
/// </summary>
[TestClass]
public class PrimeTests
{
    #region IsPrime

    [TestMethod]
    [DataRow(2u)]
    [DataRow(3u)]
    [DataRow(97u)]
    [DataRow(104729u)]
    [DataRow(104743u)]
    [DataRow(131071u)]
    [DataRow(524287u)]
    [DataRow((uint)int.MaxValue)]
    [DataRow(4294967291u)]
    public void IsPrime_PrimeNumber(uint value)
    {
        // Action
        var isPrime = Prime.IsPrime(value);

        // Assert
        Assert.IsTrue(isPrime);
    }

    [TestMethod]
    [DataRow(0u)]
    [DataRow(1u)]
    [DataRow(4u)]
    [DataRow(9u)]
    [DataRow(104728u)]
    [DataRow(104730u)]
    [DataRow(104733u)]
    [DataRow(109561u)] // 331 squared.
    [DataRow(4293001441u)] // 65521 squared.
    [DataRow(uint.MaxValue)]
    public void IsPrime_NonPrimeNumber(uint value)
    {
        // Action
        var isPrime = Prime.IsPrime(value);

        // Assert
        Assert.IsFalse(isPrime);
    }

    #endregion

    #region GetPrimes

    [TestMethod]
    [DataRow(0u, new uint[] { })]
    [DataRow(1u, new uint[] { })]
    [DataRow(2u, new uint[] { 2 })]
    [DataRow(3u, new uint[] { 2, 3 })]
    [DataRow(4u, new uint[] { 2, 3 })]
    [DataRow(5u, new uint[] { 2, 3, 5 })]
    [DataRow(6u, new uint[] { 2, 3, 5 })]
    [DataRow(29u, new uint[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29 })]
    [DataRow(30u, new uint[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29 })]
    public void GetPrimes_SmallMaximum(uint maxValue, uint[] expected)
    {
        // Action
        var primes = Prime.GetPrimes(maxValue);

        // Assert
        Assert.AreSequenceEqual(expected, primes);
    }

    [TestMethod]
    [DataRow(104728u)]
    [DataRow(104729u)]
    [DataRow(104730u)]
    [DataRow(104731u)]
    [DataRow(104743u)]
    [DataRow(109561u)] // 331 squared.
    [DataRow(203034u)]
    [DataRow(203035u)] // First candidate in the second sieve segment.
    [DataRow(203036u)]
    [DataRow(301339u)] // First candidate in the third sieve segment.
    [DataRow(1000000u)]
    public void GetPrimes_MatchesIndependentSieve(uint maxValue)
    {
        // Arrange
        var isComposite = new bool[maxValue + 1];
        for (uint factor = 2; factor * factor <= maxValue; factor++)
        {
            if (!isComposite[factor])
            {
                for (uint multiple = factor * factor; multiple <= maxValue; multiple += factor)
                    isComposite[multiple] = true;
            }
        }

        var expected = Enumerable.Range(2, (int)maxValue - 1)
            .Select(value => (uint)value)
            .Where(value => !isComposite[value])
            .ToArray();

        // Action
        var primes = Prime.GetPrimes(maxValue);

        // Assert
        Assert.AreSequenceEqual(expected, primes);
    }

    #endregion

    #region GetFactors

    [TestMethod]
    public void GetFactors_PrimeNumber()
    {
        // Action
        var factors = Prime.GetFactors(7);

        // Assert
        Assert.AreSequenceEqual([ 7u ], factors);
    }

    [TestMethod]
    public void GetFactors_CompositeNumber()
    {
        // Action
        var factors = Prime.GetFactors(28);

        // Assert
        Assert.AreSequenceEqual([ 2u, 2u, 7u ], factors);
    }

    [TestMethod]
    public void GetFactors_One()
    {
        // Action
        var factors = Prime.GetFactors(1);

        // Assert
        Assert.AreSequenceEqual([], factors);
    }

    [TestMethod]
    public void GetFactors_Zero()
    {
        // Action
        Assert.Throws<ArgumentOutOfRangeException>(() => Prime.GetFactors(0));
    }

    #endregion
}
