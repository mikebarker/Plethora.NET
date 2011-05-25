using System.Text;

namespace Plethora.Globalization
{
    /// <summary>
    /// Culture extension class for English.
    /// </summary>
    /// <remarks>
    /// Culture code 'en'.
    /// </remarks>
    /// <seealso cref="NumericHelper"/>
    public sealed class CultureExtension_en : CultureExtensionBase
    {
        #region Constants

        private const string CULTURE_NAME = "en";
        #endregion

        #region Singleton Implementation

        public static readonly CultureExtension_en Instance = new CultureExtension_en();

        /// <summary>
        /// Initialise a new instance of the <see cref="CultureExtension_en"/> class.
        /// </summary>
        private CultureExtension_en()
            : base(CULTURE_NAME)
        {
        }

        #endregion

        #region CultureExtensionBase Overrides

        /// <summary>
        /// Gets the suffix of an ordinal, which can be appended to a numeric
        /// value, to provide a human readable form.
        /// </summary>
        /// <param name="number">
        /// The number for which a ordinal suffix is required.
        /// </param>
        /// <returns>
        /// The suffix for the numer provided.
        /// </returns>
        /// <example>
        /// The number 21 should take the suffix 'st' (i.e. 21st).
        /// </example>
        public override string GetOrdinalSuffix(int number)
        {
            int unitDigit = number % 10;
            int tensDigit = (number / 10) % 10;

            //For most numbers the ordinal ends in "th",
            // except for those where the unit is 1, 2 or 3,
            //The exception to this rule is 11, 12 and 13.
            string ordinal = "th";
            if (tensDigit != 1)
            {
                switch (unitDigit)
                {
                    case 1:
                        ordinal = "st";
                        break;

                    case 2:
                        ordinal = "nd";
                        break;

                    case 3:
                        ordinal = "rd";
                        break;
                }
            }

            return ordinal;
        }

        #region NumericWords

        private static string[] NumericWords =
            {
                "zero",
                "one",
                "two",
                "three",
                "four",
                "five",
                "six",
                "seven",
                "eight",
                "nine",
                "ten",
                "eleven",
                "tweleve",
                "thirteen",
                "forteen",
                "fifteen",
                "sixteen",
                "seventeen",
                "eighteen",
                "nineteen",
                "twenty",
                "twenty-one",
                "twenty-two",
                "twenty-three",
                "twenty-four",
                "twenty-five",
                "twenty-six",
                "twenty-seven",
                "twenty-eight",
                "twenty-nine",
                "thirty",
                "thirty-one",
                "thirty-two",
                "thirty-three",
                "thirty-four",
                "thirty-five",
                "thirty-six",
                "thirty-seven",
                "thirty-eight",
                "thirty-nine",
                "fourty",
                "fourty-one",
                "fourty-two",
                "fourty-three",
                "fourty-four",
                "fourty-five",
                "fourty-six",
                "fourty-seven",
                "fourty-eight",
                "fourty-nine",
                "fifty",
                "fifty-one",
                "fifty-two",
                "fifty-three",
                "fifty-four",
                "fifty-five",
                "fifty-six",
                "fifty-seven",
                "fifty-eight",
                "fifty-nine",
                "sixty",
                "sixty-one",
                "sixty-two",
                "sixty-three",
                "sixty-four",
                "sixty-five",
                "sixty-six",
                "sixty-seven",
                "sixty-eight",
                "sixty-nine",
                "seven",
                "seven-one",
                "seven-two",
                "seven-three",
                "seven-four",
                "seven-five",
                "seven-six",
                "seven-seven",
                "seven-eight",
                "seven-nine",
                "eight",
                "eight-one",
                "eight-two",
                "eight-three",
                "eight-four",
                "eight-five",
                "eight-six",
                "eight-seven",
                "eight-eight",
                "eight-nine",
                "ninety",
                "ninety-one",
                "ninety-two",
                "ninety-three",
                "ninety-four",
                "ninety-five",
                "ninety-six",
                "ninety-seven",
                "ninety-eight",
                "ninety-nine",
            };
        #endregion

        /// <summary>
        /// Gets the number in a written, human readable form.
        /// </summary>
        public override string GetWordForm(int number)
        {
            if (number == 0)
                return NumericWords[0];

            StringBuilder sb = new StringBuilder();
            if (number < 0)
            {
                sb.Append("minus ");
                number = -number;
            }

            int divisor = 1000000000;
            int remaining = number;
            bool precedingDigits = false;
            while (divisor != 0)
            {
                int triDigits = remaining/divisor;
                remaining = remaining%divisor;

                if (triDigits != 0)
                {
                    int tensAndUnits = triDigits % 100;
                    int hundreds = triDigits/100;

                    if (precedingDigits)
                    {
                        sb.Append(", ");
                    }
                    if (hundreds != 0)
                    {
                        sb.Append(NumericWords[hundreds]);
                        sb.Append(" hundred ");
                        if (tensAndUnits != 0)
                            sb.Append("and ");
                    }
                    if (tensAndUnits != 0)
                    {
                        sb.Append(NumericWords[tensAndUnits]);
                    }

                    switch (divisor)
                    {
                        case 1000000000:
                            sb.Append(" billion");
                            break;
                        case 1000000:
                            sb.Append(" million");
                            break;
                        case 1000:
                            sb.Append(" thousand");
                            break;
                    }

                    precedingDigits = true;
                }

                divisor = divisor / 1000;
            }

            return sb.ToString().Trim();
        }
        #endregion
    }
}
