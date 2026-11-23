using Plethora.Linq;

namespace Plethora.Maths;

public static partial class Prime
{
    /// <summary>
    /// Determines whether a value is prime.
    /// </summary>
    /// <param name="value">The unsigned integer to test.</param>
    /// <returns><see langword="true"/> if the value is prime; otherwise, <see langword="false"/>.</returns>
    public static bool IsPrime(uint value)
    {
        if (IsKnownPrime(value))
        {
            return true;
        }

        if (value < KnownPrimes[^1])
        {
            return false;
        }

        // Mersenne primes
        if (value == 131071 ||
            value == 524287 ||
            value == 2147483647)
        {
            return true;
        }

        // Quick exclusion tests for even numbers and multiples of 3.
        // Is value of the form 6n ± 1?
        const uint Mask = 0b111;
        uint maskedBits = value & Mask;
        bool couldBePrime =
            maskedBits == 0b001 ||
            maskedBits == 0b011 ||
            maskedBits == 0b101 ||
            maskedBits == 0b111;

        if (!couldBePrime)
        {
            return false;
        }

        // Because the largest known prime (=104,729) is greater than SQRT(Max(value)) (=65,535), we can check for primality
        // by testing divisibility against all known primes.
        var sqrtValue = (uint)Math.Sqrt(value);
        foreach (var prime in KnownPrimes)
        {
            if (prime > sqrtValue)
            {
                break;
            }

            if (value % prime == 0)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Returns the prime numbers less than or equal to the specified maximum value.
    /// </summary>
    /// <param name="maxValue">The inclusive upper bound for the prime numbers to return.</param>
    /// <returns>An enumerable sequence of prime numbers less than or equal to <paramref name="maxValue"/>.</returns>
    public static IEnumerable<uint> GetPrimes(uint maxValue)
    {
        if (maxValue < 2)
            return Enumerable.Empty<uint>();

        if (maxValue <= KnownPrimes[^1])
        {
            var index = KnownPrimes.BinarySearch(maxValue);
            var count = index >= 0 ? index + 1 : ~index;
            return KnownPrimes.Take(count);
        }

        return Enumerable.Union(
            KnownPrimes,
            GetPrimesExtended(maxValue));
    }

    /// <summary>
    /// Generates primes greater than the largest known prime and no greater than the specified maximum.
    /// </summary>
    /// <param name="maxValue">The inclusive upper bound for the generated primes.</param>
    /// <returns>The generated prime numbers in ascending order.</returns>
    private static IEnumerable<uint> GetPrimesExtended(uint maxValue)
    {
        const int SegmentSize = 32 * 1024;
        var firstIndex = (ulong)KnownPrimes[^1] / 3 + 1;
        var endIndex = ((ulong)maxValue + 1) / 6 + ((ulong)maxValue + 5) / 6;
        var isComposit = new bool[Math.Min((ulong)SegmentSize, endIndex - firstIndex)];

        // Global index k represents 3k + 1 + (k & 1), the candidates 1, 5, 7, 11, ...
        for (var segmentStart = firstIndex; segmentStart < endIndex; segmentStart += SegmentSize)
        {
            Array.Clear(isComposit);
            var segmentEnd = Math.Min(segmentStart + SegmentSize, endIndex);
            var lower = 3 * segmentStart + 1 + (segmentStart & 1);
            var upper = 3 * (segmentEnd - 1) + 1 + ((segmentEnd - 1) & 1);

            // The known table contains all primes needed up to sqrt(uint.MaxValue).
            for (var primeIndex = 2; primeIndex < KnownPrimes.Length; primeIndex++)
            {
                var prime = (ulong)KnownPrimes[primeIndex];
                if (prime * prime > upper)
                    break;

                var multiplier = Math.Max(prime, (lower + prime - 1) / prime);
                if ((multiplier & 1) == 0)
                    multiplier++;
                if (multiplier % 3 == 0)
                    multiplier += 2;

                var step = multiplier % 6 == 5 ? 2UL : 4UL;
                for (var multiple = prime * multiplier; multiple <= upper; multiple += step * prime, step = 6 - step)
                {
                    isComposit[multiple / 3 - segmentStart] = true;
                }
            }

            for (var index = segmentStart; index < segmentEnd; index++)
            {
                if (!isComposit[index - segmentStart])
                    yield return ((uint)(3 * index + 1 + (index & 1)));
            }
        }
    }
}
