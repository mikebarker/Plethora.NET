using System.Diagnostics;

namespace Plethora.Maths;

public static partial class Prime
{
    /// <summary>
    /// Gets the prime factors of a value.
    /// </summary>
    /// <param name="value">The positive value to factor.</param>
    /// <returns>
    /// The prime factors in ascending order, including repeated factors; returns an empty collection for 1.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is zero.</exception>
    public static IReadOnlyCollection<uint> GetFactors(uint value)
    {
        if (value == 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Value must be greater than zero.");

        if (value == 1)
            return [];

        var sqrt = Math.Sqrt(value);
        Debug.Assert(sqrt <= KnownPrimes[^1], $"The square root of the value exceeds the largest known prime, {KnownPrimes[^1]}.");

        List<uint> factors = new();
        foreach (var prime in KnownPrimes)
        {
            if (prime > sqrt)
                break;

            while (value % prime == 0)
            {
                factors.Add(prime);
                value /= prime;
            }
        }

        if (value > 1)
            factors.Add(value);

        return factors;
    }
}
