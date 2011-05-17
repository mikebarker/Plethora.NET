using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Plethora
{
    /// <summary>
    /// Helper class for strings.
    /// </summary>
    public static class StringHelper
    {
        #region Fields

        private static char[] whiteSpace;
        private static Regex beginWordRegex;
        private static Regex endWordRegex;
        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets an array representing the whitespace characters.
        /// </summary>
        public static char[] WhiteSpace
        {
            get
            {
                if (whiteSpace == null)
                {
                    List<char> whitespaceList = new List<char>(30);
                    for (ushort i = 0; i < ushort.MaxValue; i++)
                    {
                        char whitespaceChar = (char)i;
                        if (char.IsWhiteSpace(whitespaceChar))
                            whitespaceList.Add(whitespaceChar);
                    }
                    whiteSpace = whitespaceList.ToArray();
                }

                return (char[])whiteSpace.Clone();
            }
        }
        #endregion

        #region Public Static Methods

        #region IndexNotOfAny

        /// <summary>
        /// Reports the index of the first character which is not a character in a
        /// specified array of Unicode characters.
        /// </summary>
        /// <param name="str">
        /// The string in which to find the non specified character.
        /// </param>
        /// <param name="values">
        /// A Unicode character array containing one or more characters to seek.
        /// </param>
        /// <returns>
        /// The index position of the first character not in anyOf was found;
        /// otherwise, -1 if no character not in anyOf was found.
        /// </returns>
        public static int IndexNotOfAny(this string str, char[] values)
        {
            //Validation
            if (str == null)
                throw new ArgumentNullException("str");

            return IndexNotOfAny(str, values, 0, str.Length);
        }

        /// <summary>
        /// Reports the index of the first character which is not a character in a
        /// specified array of Unicode characters. The search starts at a specified
        /// character position.
        /// </summary>
        /// <param name="str">
        /// The string in which to find the non specified character.
        /// </param>
        /// <param name="values">
        /// A Unicode character array containing one or more characters to seek.
        /// </param>
        /// <param name="startIndex">
        /// The search starting position.
        /// </param>
        /// <returns>
        /// The index position of the first character not in anyOf was found;
        /// otherwise, -1 if no character not in anyOf was found.
        /// </returns>
        public static int IndexNotOfAny(this string str, char[] values, int startIndex)
        {
            //Validation
            if (str == null)
                throw new ArgumentNullException("str");

            return IndexNotOfAny(str, values, startIndex, str.Length - startIndex);
        }

        /// <summary>
        /// Reports the index of the first character which is not a character in a
        /// specified array of Unicode characters. The search starts at a specified
        /// character position and examines a specified number of character
        /// positions.
        /// </summary>
        /// <param name="str">
        /// The string in which to find the non specified character.
        /// </param>
        /// <param name="values">
        /// A Unicode character array containing one or more characters to seek.
        /// </param>
        /// <param name="startIndex">
        /// The search starting position.
        /// </param>
        /// <param name="count">
        /// The number of character positions to examine.
        /// </param>
        /// <returns>
        /// The index position of the first character not in anyOf was found;
        /// otherwise, -1 if no character not in anyOf was found.
        /// </returns>
        public static int IndexNotOfAny(this string str, char[] values, int startIndex, int count)
        {
            //Validation
            if (str == null)
                throw new ArgumentNullException("str");

            if (values == null)
                throw new ArgumentNullException("values");

            if ((startIndex < 0) || (startIndex >= str.Length))
                throw new ArgumentOutOfRangeException("startIndex",
                    ResourceProvider.ArgMustBeBetween("startIndex", "0", "str.Length"));

            if ((count < 0) || ((startIndex + count) > str.Length))
                throw new ArgumentOutOfRangeException("count",
                    ResourceProvider.ArgMustBeBetween("count", "0", "str.Length - startIndex"));


            IList valuesList = ArrayList.FixedSize(values);

            char[] chrArr = str.ToCharArray();
            for (int i = startIndex; i < startIndex + count; i++)
            {
                if (!valuesList.Contains(chrArr[i]))
                    return i;
            }
            return -1;
        }
        #endregion

        #region LastIndexNotOfAny

        /// <summary>
        /// Reports the index of the last character which is not a character in a
        /// specified array of Unicode characters.
        /// </summary>
        /// <param name="str">
        /// The string in which to find the non specified character.
        /// </param>
        /// <param name="values">
        /// A Unicode character array containing one or more characters to seek.
        /// </param>
        /// <returns>
        /// The index position of the last character not in anyOf found;
        /// otherwise, -1 if no character not in anyOf was found.
        /// </returns>
        public static int LastIndexNotOfAny(this string str, char[] values)
        {
            //Validation
            if (str == null)
                throw new ArgumentNullException("str");

            return LastIndexNotOfAny(str, values, str.Length - 1, str.Length);
        }

        /// <summary>
        /// Reports the index of the first character which is not a character in a
        /// specified array of Unicode characters. The search starts at a specified
        /// character position.
        /// </summary>
        /// <param name="str">
        /// The string in which to find the non specified character.
        /// </param>
        /// <param name="values">
        /// A Unicode character array containing one or more characters to seek.
        /// </param>
        /// <param name="startIndex">
        /// The search starting position.
        /// </param>
        /// <returns>
        /// The index position of the last character not in anyOf found;
        /// otherwise, -1 if no character not in anyOf was found.
        /// </returns>
        public static int LastIndexNotOfAny(this string str, char[] values, int startIndex)
        {
            //Validation
            if (str == null)
                throw new ArgumentNullException("str");

            return LastIndexNotOfAny(str, values, startIndex, startIndex + 1);
        }

        /// <summary>
        /// Reports the index of the first character which is not a character in a
        /// specified array of Unicode characters. The search starts at a specified
        /// character position and examines a specified number of character
        /// positions.
        /// </summary>
        /// <param name="str">
        /// The string in which to find the non specified character.
        /// </param>
        /// <param name="values">
        /// A Unicode character array containing one or more characters to seek.
        /// </param>
        /// <param name="startIndex">
        /// The search starting position.
        /// </param>
        /// <param name="count">
        /// The number of character positions to examine.
        /// </param>
        /// <returns>
        /// The index position of the last character not in anyOf found;
        /// otherwise, -1 if no character not in anyOf was found.
        /// </returns>
        public static int LastIndexNotOfAny(this string str, char[] values, int startIndex, int count)
        {
            //Validation
            if (str == null)
                throw new ArgumentNullException("str");

            if (values == null)
                throw new ArgumentNullException("values");

            if ((startIndex < 0) || (startIndex >= str.Length))
                throw new ArgumentOutOfRangeException("startIndex",
                    ResourceProvider.ArgMustBeBetween("startIndex", "0", "str.Length"));

            if ((count < 0) || (count > startIndex))
                throw new ArgumentOutOfRangeException("count",
                    ResourceProvider.ArgMustBeBetween("count", "0", "startIndex"));


            IList valuesList = ArrayList.FixedSize(values);

            char[] chrArr = str.ToCharArray();
            int stopIndex = (startIndex - count);
            for (int i = startIndex; i > stopIndex; i--)
            {
                if (!valuesList.Contains(chrArr[i]))
                    return i;
            }
            return -1;
        }
        #endregion

        #region IndexOfWhitespace

        /// <summary>
        /// Reports the index of the first occurrence of a whitespace character in
        /// this string.
        /// </summary>
        /// <param name="str">
        /// The string in which to find the next white space character.
        /// </param>
        /// <returns>
        /// The index position of value if a whitespace character is found, or -1
        /// if not.
        /// </returns>
        public static int IndexOfWhiteSpace(this string str)
        {
            return str.IndexOfAny(WhiteSpace);
        }

        /// <summary>
        /// Reports the index of the first occurrence of a whitespace character in
        /// this string. The search starts at a specified character position.
        /// </summary>
        /// <param name="str">
        /// The string in which to find the next white space character.
        /// </param>
        /// <param name="startIndex">
        /// The search starting position.
        /// </param>
        /// <returns>
        /// The index position of value if a whitespace character is found, or -1
        /// if not.
        /// </returns>
        public static int IndexOfWhiteSpace(this string str, int startIndex)
        {
            return str.IndexOfAny(WhiteSpace, startIndex);
        }

        /// <summary>
        /// Reports the index of the first occurrence of a whitespace character in
        /// this string. The search starts at a specified character position and
        /// examines a specified number of character positions.
        /// </summary>
        /// <param name="str">
        /// The string in which to find the next white space character.
        /// </param>
        /// <param name="startIndex">
        /// The search starting position.
        /// </param>
        /// <param name="count">
        /// The number of character positions to examine.
        /// </param>
        /// <returns>
        /// The index position of value if a whitespace character is found, or -1
        /// if not.
        /// </returns>
        public static int IndexOfWhiteSpace(this string str, int startIndex, int count)
        {
            return str.IndexOfAny(WhiteSpace, startIndex, count);
        }
        #endregion

        #region LastIndexOfWhitespace

        /// <summary>
        /// Reports the index of the last occurrence of a whitespace character in
        /// this string.
        /// </summary>
        /// <param name="str">
        /// The string in which to find the next white space character.
        /// </param>
        /// <returns>
        /// The index position of value if a whitespace character is found, or -1
        /// if not.
        /// </returns>
        public static int LastIndexOfWhiteSpace(this string str)
        {
            return str.LastIndexOfAny(WhiteSpace);
        }

        /// <summary>
        /// Reports the index of the last occurrence of a whitespace character in
        /// this string. The search starts at a specified character position.
        /// </summary>
        /// <param name="str">
        /// The string in which to find the next white space character.
        /// </param>
        /// <param name="startIndex">
        /// The search starting position.
        /// </param>
        /// <returns>
        /// The index position of value if a whitespace character is found, or -1
        /// if not.
        /// </returns>
        public static int LastIndexOfWhiteSpace(this string str, int startIndex)
        {
            return str.LastIndexOfAny(WhiteSpace, startIndex);
        }

        /// <summary>
        /// Reports the index of the last occurrence of a whitespace character in
        /// this string. The search starts at a specified character position and
        /// examines a specified number of character positions.
        /// </summary>
        /// <param name="str">
        /// The string in which to find the next white space character.
        /// </param>
        /// <param name="startIndex">
        /// The search starting position.
        /// </param>
        /// <param name="count">
        /// The number of character positions to examine.
        /// </param>
        /// <returns>
        /// The index position of value if a whitespace character is found, or -1
        /// if not.
        /// </returns>
        public static int LastIndexOfWhiteSpace(this string str, int startIndex, int count)
        {
            return str.LastIndexOfAny(WhiteSpace, startIndex, count);
        }
        #endregion

        #region IndexNotOfWhitespace

        /// <summary>
        /// Reports the index of the first occurrence of a non-whitespace character
        /// in this string.
        /// </summary>
        /// <param name="str">
        /// The string in which to find the next non-whitespace character.
        /// </param>
        /// <returns>
        /// The index position of value if a non-whitespace character is found, or
        /// -1 if not.
        /// </returns>
        public static int IndexNotOfWhitespace(this string str)
        {
            return IndexNotOfAny(str, WhiteSpace);
        }

        /// <summary>
        /// Reports the index of the first occurrence of a non-whitespace character
        /// in this string. The search starts at a specified character position.
        /// </summary>
        /// <param name="str">
        /// The string in which to find the next non-whitespace character.
        /// </param>
        /// <param name="startIndex">
        /// The search starting position.
        /// </param>
        /// <returns>
        /// The index position of value if a non-whitespace character is found, or
        /// -1 if not.
        /// </returns>
        public static int IndexNotOfWhitespace(this string str, int startIndex)
        {
            return IndexNotOfAny(str, WhiteSpace, startIndex);
        }

        /// <summary>
        /// Reports the index of the first occurrence of a non-whitespace character
        /// in this string. The search starts at a specified character position and
        /// examines a specified number of character positions.
        /// </summary>
        /// <param name="str">
        /// The string in which to find the next non-whitespace character.
        /// </param>
        /// <param name="startIndex">
        /// The search starting position.
        /// </param>
        /// <param name="count">
        /// The number of character positions to examine.
        /// </param>
        /// <returns>
        /// The index position of value if a non-whitespace character is found, or
        /// -1 if not.
        /// </returns>
        public static int IndexNotOfWhitespace(this string str, int startIndex, int count)
        {
            return IndexNotOfAny(str, WhiteSpace, startIndex, count);
        }
        #endregion

        #region LastIndexNotOfWhitespace

        /// <summary>
        /// Reports the index of the last occurrence of a non-whitespace character
        /// in this string.
        /// </summary>
        /// <param name="str">
        /// The string in which to find the next non-whitespace character.
        /// </param>
        /// <returns>
        /// The index position of value if a non-whitespace character is found, or
        /// -1 if not.
        /// </returns>
        public static int LastIndexNotOfWhitespace(this string str)
        {
            return LastIndexNotOfAny(str, WhiteSpace);
        }

        /// <summary>
        /// Reports the index of the last occurrence of a non-whitespace character
        /// in this string. The search starts at a specified character position.
        /// </summary>
        /// <param name="str">
        /// The string in which to find the next non-whitespace character.
        /// </param>
        /// <param name="startIndex">
        /// The search starting position.
        /// </param>
        /// <returns>
        /// The index position of value if a non-whitespace character is found, or
        /// -1 if not.
        /// </returns>
        public static int LastIndexNotOfWhitespace(this string str, int startIndex)
        {
            return LastIndexNotOfAny(str, WhiteSpace, startIndex);
        }

        /// <summary>
        /// Reports the index of the last occurrence of a non-whitespace character
        /// in this string. The search starts at a specified character position and
        /// examines a specified number of character positions.
        /// </summary>
        /// <param name="str">
        /// The string in which to find the next non-whitespace character.
        /// </param>
        /// <param name="startIndex">
        /// The search starting position.
        /// </param>
        /// <param name="count">
        /// The number of character positions to examine.
        /// </param>
        /// <returns>
        /// The index position of value if a non-whitespace character is found, or
        /// -1 if not.
        /// </returns>
        public static int LastIndexNotOfWhitespace(this string str, int startIndex, int count)
        {
            return LastIndexNotOfAny(str, WhiteSpace, startIndex, count);
        }
        #endregion

        #region IndexOfWord

        /// <summary>
        /// Reports the index of the beginning of the next word in the string.
        /// </summary>
        /// <param name="str">
        /// The string in which to find the index of the beginning of the next
        /// word.
        /// </param>
        /// <param name="startIndex">
        /// The search starting position.
        /// </param>
        /// <returns>
        /// The index position of the beginning of the next word if found, or
        /// -1 if not.
        /// </returns>
        /// <remarks>
        ///  <para>
        ///   The start of a word is defined by a non-whitespace character occuring
        ///   after a whitespace character. The first word begins at index 0.
        ///  </para>
        /// </remarks>
        public static int IndexOfWord(this string str, int startIndex)
        {
            return IndexOfWord(str, startIndex, true);
        }

        /// <summary>
        /// Reports the index of the beginning or end of the next word in the string.
        /// </summary>
        /// <param name="str">
        /// The string in which to find the index of the beginning or end of the next
        /// word.
        /// </param>
        /// <param name="startIndex">
        /// The search starting position.
        /// </param>
        /// <param name="findWordBeginning">
        /// 'true' if the beginning of the word is to be required; or 'false' if
        /// the end of the word is required.
        /// </param>
        /// <returns>
        /// The index position of the start of the next word if found, or
        /// -1 if not.
        /// </returns>
        /// <remarks>
        ///  <para>
        ///   The start of a word is defined by a non-whitespace character occuring
        ///   after a whitespace character. The first word begins at index 0.
        ///  </para>
        ///  <para>
        ///   The end of a word is defined by a whitespace character occurring
        ///   before a whitespace character.
        ///  </para>
        /// </remarks>
        public static int IndexOfWord(this string str, int startIndex, bool findWordBeginning)
        {
            //Validation
            if (str == null)
                throw new ArgumentNullException("str");

            if ((startIndex < 0) || (startIndex >= str.Length))
                throw new ArgumentOutOfRangeException("startIndex",
                    ResourceProvider.ArgMustBeBetween("startIndex", "0", "'str.Length'"));


            Regex regex;
            if (findWordBeginning)
            {
                if (startIndex == 0)
                    return 0;

                regex = BeginWordRegex;
            }
            else
            {
                regex = EndWordRegex;
            }

            MatchCollection matches = regex.Matches(str, startIndex);
            if (matches.Count == 0)
                return -1;

            Match match = matches[0];
            return match.Index + 1;
        }
        #endregion

        #region LastIndexOfWord

        /// <summary>
        /// Reports the index of the beginning of the previous word in the
        /// string.
        /// </summary>
        /// <param name="str">
        /// The string in which to find the index of the beginning of the
        /// previous word.
        /// </param>
        /// <param name="startIndex">
        /// The search starting position.
        /// </param>
        /// <returns>
        /// The index position of the start of the previous word if found, or
        /// -1 if not.
        /// </returns>
        /// <remarks>
        ///  <para>
        ///   The start of a word is defined by a non-whitespace character occuring
        ///   after a whitespace character. The first word begins at index 0.
        ///  </para>
        /// </remarks>
        public static int LastIndexOfWord(this string str, int startIndex)
        {
            return LastIndexOfWord(str, startIndex, true);
        }

        /// <summary>
        /// Reports the index of the beginning or end of the previous word in the
        /// string.
        /// </summary>
        /// <param name="str">
        /// The string in which to find the index of the beginning or end of the
        /// previous word.
        /// </param>
        /// <param name="startIndex">
        /// The search starting position.
        /// </param>
        /// <param name="findWordBeginning">
        /// 'true' if the beginning of the word is to be required; or 'false' if
        /// the end of the word is required.
        /// </param>
        /// <returns>
        /// The index position of the start of the previous word if found, or
        /// -1 if not.
        /// </returns>
        /// <remarks>
        ///  <para>
        ///   The start of a word is defined by a non-whitespace character occuring
        ///   after a whitespace character. The first word begins at index 0.
        ///  </para>
        ///  <para>
        ///   The end of a word is defined by a whitespace character occurring
        ///   before a whitespace character.
        ///  </para>
        /// </remarks>
        public static int LastIndexOfWord(this string str, int startIndex, bool findWordBeginning)
        {
            //Validation
            if (str == null)
                throw new ArgumentNullException("str");

            if ((startIndex < 0) || (startIndex >= str.Length))
                throw new ArgumentOutOfRangeException("startIndex",
                    ResourceProvider.ArgMustBeBetween("startIndex", "0", "'str.Length'"));


            Regex regex;
            if (findWordBeginning)
            {
                if (startIndex == 0)
                    return 0;

                regex = BeginWordRegex;
            }
            else
            {
                regex = EndWordRegex;
            }

            string tmp = str.Substring(0, startIndex - 1);
            MatchCollection matches = regex.Matches(tmp);
            if (matches.Count == 0)
                return -1;

            Match match = matches[matches.Count - 1];
            return match.Index + 1;
        }
        #endregion
        #endregion

        #region Private Members

        /// <summary>
        /// Gets the Regex which describes the beginning of a word.
        /// </summary>
        private static Regex BeginWordRegex
        {
            get
            {
                if (beginWordRegex == null)
                    beginWordRegex = new Regex(@"(\s\S)");
                return beginWordRegex;
            }
        }

        /// <summary>
        /// Gets the Regex which describes the end of a word.
        /// </summary>
        private static Regex EndWordRegex
        {
            get
            {
                if (endWordRegex == null)
                    endWordRegex = new Regex(@"(\S\s)");
                return endWordRegex;
            }
        }
        #endregion
    }
}