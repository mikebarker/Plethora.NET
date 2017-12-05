using System;

namespace Plethora
{
    public static class MathEx
    {
        /// <summary>
        /// Returns the greatest common divisor of two numbers.
        /// </summary>
        /// <param name="a">The first number.</param>
        /// <param name="b">The second number.</param>
        /// <returns>The greatest common divisor of <paramref name="a"/> and <paramref name="b"/>.</returns>
        /// <remarks>
        /// This implementation uses the Euclidean algorithm.
        /// <seealso href="https://en.wikipedia.org/wiki/Euclidean_algorithm"/>
        /// </remarks>
        public static int GreatestCommonDivisor(int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);

            while (b != 0)
            {
                int rem = a % b;
                a = b;
                b = rem;
            }
            return a;
        }
    }
}
