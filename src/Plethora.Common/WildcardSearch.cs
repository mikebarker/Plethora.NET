﻿using System;

namespace Plethora
{
    /// <summary>
    /// Helper class to match strings to a pattern based on wildcard search criteria.
    /// </summary>
    public static class WildcardSearch
    {
        private const char WILDCARD = '*';

        public static char WildCard
        {
            get { return WILDCARD; }
        }

        /// <summary>
        /// Gets a flag indicating whether an input string matches a string pattern
        /// </summary>
        /// <param name="input">The input string to be tested.</param>
        /// <param name="pattern">The pattern to be matched. May contain wildcards [*].</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>
        /// True if the input string matches the pattern provided; otherwise false.
        /// </returns>
        /// <remarks>
        /// This method is more efficient than using <see cref="System.Text.RegularExpressions.Regex.IsMatch(string, string)"/>.
        /// </remarks>
        public static bool IsMatch(string input, string pattern, StringComparison comparisonType = StringComparison.Ordinal)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(input);
            ArgumentNullException.ThrowIfNull(pattern);


            //Special case
            if (pattern == string.Empty)
            {
                return (input == string.Empty);
            }


            string[] patternElements = pattern.Split(new[] { WILDCARD }, StringSplitOptions.None);

            //Special case - no wildcards
            if (patternElements.Length == 1)
            {
                return string.Equals(input, pattern, comparisonType);
            }


            string element;
            int nextStartIndex = 0;

            //First pattern match
            element = patternElements[0];
            if (element != string.Empty)
            {
                if (!input.StartsWith(element, comparisonType))
                    return false;

                nextStartIndex = element.Length;
            }

            //Mid-string pattern matches
            for (int i = 1; i < patternElements.Length - 1; i++)
            {
                element = patternElements[i];
                int elementIndex = input.IndexOf(element, nextStartIndex, comparisonType);

                if (elementIndex == -1)
                    return false;

                nextStartIndex = elementIndex + element.Length;
            }


            //Last pattern match
            element = patternElements[patternElements.Length - 1];
            if (element != string.Empty)
            {
                int elementIndex = input.IndexOf(element, nextStartIndex, comparisonType);

                if (elementIndex == -1)
                    return false;

                if (input.Length != elementIndex + element.Length)
                    return false;
            }


            return true;
        }
    }
}
