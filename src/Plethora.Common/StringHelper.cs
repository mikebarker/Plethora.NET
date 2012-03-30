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
                    List<char> whitespaceList = new List<char>(32);

                    //The loop should include the upper bound char.MaxValue
                    // (i.e. the test should be i <= char.MaxValue) but since when i
                    // reaches char.MaxValue then i++ will result in char.MinValue, for
                    // which the test condition will evaluate to true. This will result in
                    // an infinite loop.
                    // Therefore the test for i == char.MaxValue is done outside the loop.
                    //
                    // One could say that char.MaxValue is constant and is not a whitespace
                    // character, and therefore exclude it here. However, that would make this
                    // function dependant on the implementation of char.
                    for (char i = char.MinValue; i < char.MaxValue; i++)
                    {
                        if (char.IsWhiteSpace(i))
                            whitespaceList.Add(i);
                    }

                    if (char.IsWhiteSpace(char.MaxValue))
                        whitespaceList.Add(char.MaxValue);

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

            if ((count < 0) || (startIndex - count < -1))
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

        #region IndexOfWhiteSpace

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
            //Validation
            if (str == null)
                throw new ArgumentNullException("str");

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
            //Validation
            if (str == null)
                throw new ArgumentNullException("str");

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
            //Validation
            if (str == null)
                throw new ArgumentNullException("str");

            return str.IndexOfAny(WhiteSpace, startIndex, count);
        }
        #endregion

        #region LastIndexOfWhiteSpace

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
            //Validation
            if (str == null)
                throw new ArgumentNullException("str");

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
            //Validation
            if (str == null)
                throw new ArgumentNullException("str");

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
            //Validation
            if (str == null)
                throw new ArgumentNullException("str");

            return str.LastIndexOfAny(WhiteSpace, startIndex, count);
        }
        #endregion

        #region IndexNotOfWhiteSpace

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
        public static int IndexNotOfWhiteSpace(this string str)
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
        public static int IndexNotOfWhiteSpace(this string str, int startIndex)
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
        public static int IndexNotOfWhiteSpace(this string str, int startIndex, int count)
        {
            return IndexNotOfAny(str, WhiteSpace, startIndex, count);
        }
        #endregion

        #region LastIndexNotOfWhiteSpace

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
        public static int LastIndexNotOfWhiteSpace(this string str)
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
        public static int LastIndexNotOfWhiteSpace(this string str, int startIndex)
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
        public static int LastIndexNotOfWhiteSpace(this string str, int startIndex, int count)
        {
            return LastIndexNotOfAny(str, WhiteSpace, startIndex, count);
        }
        #endregion

        #region IndexOfWord

        /// <summary>
        /// Reports the index of the beginning of the first word in the string.
        /// </summary>
        /// <param name="str">
        /// The string in which to find the index of the beginning of the firstword.
        /// </param>
        /// <returns>
        /// The index position of the beginning of the next word if found, or
        /// -1 if not.
        /// </returns>
        /// <see cref="IndexOfWord(string,int,bool)"/>
        public static int IndexOfWord(this string str)
        {
            return IndexOfWord(str, 0, true);
        }

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
        /// <see cref="IndexOfWord(string,int,bool)"/>
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
        ///   A 'word' in this context is defined as a sub-string consisting of the
        ///   characters a-z, A-Z, 0-9, and underscore (_).
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
            int indexOffset;
            if (findWordBeginning)
            {
                regex = BeginWordRegex;
                indexOffset = 1;
            }
            else
            {
                regex = EndWordRegex;
                indexOffset = 0;
            }

            MatchCollection matches = regex.Matches(str, startIndex);
            if (matches.Count == 0)
                return -1;

            Match match = matches[0];
            return match.Index + indexOffset;
        }
        #endregion

        #region LastIndexOfWord

        /// <summary>
        /// Reports the index of the beginning of the last word in the
        /// string.
        /// </summary>
        /// <param name="str">
        /// The string in which to find the index of the beginning of the
        /// last word.
        /// </param>
        /// <returns>
        /// The index position of the start of the last word if found, or
        /// -1 if not.
        /// </returns>
        /// <see cref="LastIndexOfWord(string,int,bool)"/>
        public static int LastIndexOfWord(this string str)
        {
            //Validation
            if (str == null)
                throw new ArgumentNullException("str");


            return LastIndexOfWord(str, str.Length - 1, true);
        }

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
        /// <see cref="LastIndexOfWord(string,int,bool)"/>
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
        ///   A 'word' in this context is defined as a sub-string consisting of the
        ///   characters a-z, A-Z, 0-9, and underscore (_).
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
            int indexOffset;
            if (findWordBeginning)
            {
                regex = BeginWordRegex;
                indexOffset = 1;
            }
            else
            {
                regex = EndWordRegex;
                indexOffset = 0;
            }

            string tmp = str.Substring(0, startIndex + 1);
            MatchCollection matches = regex.Matches(tmp);
            if (matches.Count == 0)
                return -1;

            Match match = matches[matches.Count - 1];

            //Special case for finding the end of a word and not considering cropped text.
            if ((!findWordBeginning) && (match.Index == tmp.Length-1))
            {
                //test if the original string contained an end-of-word at this location.
                if (str.Length > tmp.Length)
                {
                    var nextChar = str[match.Index + 1];
                    if (((nextChar >= 'a') && (nextChar <= 'z'))||
                        ((nextChar >= 'A') && (nextChar <= 'Z')) ||
                        ((nextChar == '_')))
                    {
                        if (matches.Count < 2)
                            return -1;

                        //Use the previous match
                        match = matches[matches.Count - 2];
                    }
                    else
                    {
                        return match.Index;
                    }
                }
                else
                {
                    return match.Index;
                }
            }

            return match.Index + indexOffset;
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
                    beginWordRegex = new Regex(@"(\W|^)\w", RegexOptions.Compiled);
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
                    endWordRegex = new Regex(@"\w(\W|$)", RegexOptions.Compiled);
                return endWordRegex;
            }
        }
        #endregion
    }
}