using System.Text;

namespace Plethora.SearchBar
{
    internal static class PreFilter
    {
        private const char QUOTE = '"';

        public static string Prepare(string str)
        {
            str = str.Trim();
            str = RemoveRepeatedWhitespace(str);

            return str;
        }

        private static string RemoveRepeatedWhitespace(string str)
        {
            StringBuilder rtn = new StringBuilder(str.Length);

            bool isInQuote = false;
            char prevChar = default(char);
            foreach (char c in str)
            {
                if (c == QUOTE)
                {
                    isInQuote = !isInQuote;
                }

                if (isInQuote)
                {
                }
                else
                {
                    if (char.IsWhiteSpace(c) && char.IsWhiteSpace(prevChar))
                    {
                        continue;
                    }
                }

                rtn.Append(c);
                prevChar = c;
            }

            return rtn.ToString();
        }
    }
}
