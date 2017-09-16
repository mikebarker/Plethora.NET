using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Plethora.SearchBar.Definitions.WellKnown
{
    public delegate bool TryParseCultureFunction(string text, CultureInfo cultureInfo, out object value);

    public class DateDataTypeDefinition : DataTypeDefinition
    {
        private static readonly Dictionary<string, TryParseCultureFunction> alternatePairs =
            new Dictionary<string, TryParseCultureFunction>();

        static DateDataTypeDefinition()
        {
            RegisterPatternParserPair(@"today[-+]\d*", TodayMinusN_ParseFunction);
            RegisterPatternParserPair(@"T[-+]\d*", TodayMinusN_ParseFunction);
            RegisterPatternParserPair(@"today", TodayParseFunction);
            RegisterPatternParserPair(@"yesterday", YesterdayParseFunction);
            RegisterPatternParserPair(@"tomorrow", TomorrowParseFunction);
            RegisterPatternParserPair(@"this week", ThisWeekParseFunction);
            RegisterPatternParserPair(@"last week", LastWeekParseFunction);
            RegisterPatternParserPair(@"next week", NextWeekParseFunction);
            RegisterPatternParserPair(@"this month", ThisMonthParseFunction);
            RegisterPatternParserPair(@"last month", LastMonthParseFunction);
            RegisterPatternParserPair(@"next month", NextMonthParseFunction);
        }

        public DateDataTypeDefinition()
            : this(CultureInfo.CurrentCulture)
        {
        }

        public DateDataTypeDefinition(CultureInfo cultureInfo)
            : base("date", ConstructRegexPattern(cultureInfo), GetParseDate(cultureInfo))
        {
        }

        private static string ConstructRegexPattern(CultureInfo cultureInfo)
        {
            string[] patterns = new[]
            {
                "yyyy-MM-dd",
                "dd MMM yyyy",
                "dd MMM yy",
                "dd MMM",
                "MMM dd",
                cultureInfo.DateTimeFormat.MonthDayPattern,
                cultureInfo.DateTimeFormat.YearMonthPattern,
                cultureInfo.DateTimeFormat.ShortDatePattern,
                cultureInfo.DateTimeFormat.LongDatePattern,
            };

            IEnumerable<string> regexPatterns = patterns
                .Distinct()
                .Select(p => Regex.Escape(p))
                .Select(p => ApplyRegexSubstitutions(p, cultureInfo))
                .ToList();

            IEnumerable<string> alternatePatterns = alternatePairs.Keys;

            string datePattern = string.Join("|", alternatePatterns.Concat(regexPatterns));

            return datePattern;
        }

        private static string ApplyRegexSubstitutions(string datePattern, CultureInfo cultureInfo)
        {
            // -------------------------------------------------------------------------------------------
            // NOTE: Being too correct with the regex can cause very slow performance.
            // Better to be "context-free" and approximate the pattern, then fall back on the TryParse
            // function later.
            // -------------------------------------------------------------------------------------------

            var monthNames = cultureInfo.DateTimeFormat.MonthNames
                .Concat(cultureInfo.DateTimeFormat.MonthGenitiveNames)
                .Concat(cultureInfo.DateTimeFormat.AbbreviatedMonthNames)
                .Concat(cultureInfo.DateTimeFormat.AbbreviatedMonthGenitiveNames)
                .Where(month => !string.IsNullOrEmpty(month))
                .Distinct();

            int minLength = monthNames.Min(name => name.Length);
            int maxLength = monthNames.Max(name => name.Length);
            string monthNamesPattern = string.Format(@"\b\w{{{0},{1}}}\b", minLength, maxLength);

            datePattern = datePattern.Replace("yyyy", @"\d{4}");
            datePattern = datePattern.Replace("yy", @"\d{2}");

            datePattern = datePattern.Replace("MMMM", monthNamesPattern);
            datePattern = datePattern.Replace("MMM", monthNamesPattern);
            datePattern = datePattern.Replace("MM", @"\d{1,2}");

            datePattern = datePattern.Replace("dd", @"\d{1,2}");

            return datePattern;
        }


        public static void RegisterPatternParserPair(string pattern, TryParseCultureFunction tryParseFunction)
        {
            alternatePairs.Add(pattern, tryParseFunction);
        }

        private static TryParseFunction GetParseDate(CultureInfo cultureInfo)
        {
            return delegate(string text, out object value)
            {
                bool result;

                if (alternatePairs.TryGetValue(text, out TryParseCultureFunction tryParseFunction))
                {
                    result = tryParseFunction(text, cultureInfo, out value);
                    return result;
                }

                foreach (var pair in alternatePairs)
                {
                    string pattern = pair.Key;
                    tryParseFunction = pair.Value;

                    bool isMatch = Regex.IsMatch(text, pattern, RegexBuilder.Options);

                    if (isMatch)
                    {
                        result = tryParseFunction(text, cultureInfo, out value);
                        return result;
                    }
                }

                result = DateTime.TryParse(text, cultureInfo, DateTimeStyles.None, out DateTime dateValue);
                value = dateValue;
                return result;
            };
        }

        #region Parse Functions

        public static TryParseCultureFunction TodayParseFunction
        {
            get
            {
                return delegate(string text, CultureInfo cultureInfo, out object value)
                {
                    value = DateTime.Today;
                    return true;
                };
            }
        }

        public static TryParseCultureFunction YesterdayParseFunction
        {
            get
            {
                return delegate (string text, CultureInfo cultureInfo, out object value)
                {
                    value = DateTime.Today.AddDays(-1);
                    return true;
                };
            }
        }

        public static TryParseCultureFunction TomorrowParseFunction
        {
            get
            {
                return delegate (string text, CultureInfo cultureInfo, out object value)
                {
                    value = DateTime.Today.AddDays(1);
                    return true;
                };
            }
        }

        public static TryParseCultureFunction ThisWeekParseFunction
        {
            get
            {
                return delegate (string text, CultureInfo cultureInfo, out object value)
                {
                    value = ThisWeek(cultureInfo);
                    return true;
                };
            }
        }

        public static TryParseCultureFunction LastWeekParseFunction
        {
            get
            {
                return delegate (string text, CultureInfo cultureInfo, out object value)
                {
                    DateTimeRange thisWeek = ThisWeek(cultureInfo);
                    DateTimeRange lastWeek = new DateTimeRange(thisWeek.Min.AddDays(-7), thisWeek.Max.AddDays(-7));
                    value = lastWeek;
                    return true;
                };
            }
        }

        public static TryParseCultureFunction NextWeekParseFunction
        {
            get
            {
                return delegate (string text, CultureInfo cultureInfo, out object value)
                {
                    DateTimeRange thisWeek = ThisWeek(cultureInfo);
                    DateTimeRange lastWeek = new DateTimeRange(thisWeek.Min.AddDays(7), thisWeek.Max.AddDays(7));
                    value = lastWeek;
                    return true;
                };
            }
        }

        public static TryParseCultureFunction ThisMonthParseFunction
        {
            get
            {
                return delegate (string text, CultureInfo cultureInfo, out object value)
                {
                    DateTime today = DateTime.Today;
                    int year = today.Year;
                    int month = today.Month;

                    value = new DateTimeRange(
                        new DateTime(year, month, 1),
                        new DateTime(year, month, DateTime.DaysInMonth(year, month)));

                    return true;
                };
            }
        }

        public static TryParseCultureFunction LastMonthParseFunction
        {
            get
            {
                return delegate (string text, CultureInfo cultureInfo, out object value)
                {
                    DateTime today = DateTime.Today;
                    int year = today.Year;
                    int month = today.Month;

                    month -= 1;
                    if (month == 0)
                    {
                        month = 12;
                        year -= 1;
                    }

                    value = new DateTimeRange(
                        new DateTime(year, month, 1),
                        new DateTime(year, month, DateTime.DaysInMonth(year, month)));

                    return true;
                };
            }
        }

        public static TryParseCultureFunction NextMonthParseFunction
        {
            get
            {
                return delegate (string text, CultureInfo cultureInfo, out object value)
                {
                    DateTime today = DateTime.Today;
                    int year = today.Year;
                    int month = today.Month;

                    month += 1;
                    if (month == 13)
                    {
                        month = 1;
                        year += 1;
                    }

                    value = new DateTimeRange(
                        new DateTime(year, month, 1),
                        new DateTime(year, month, DateTime.DaysInMonth(year, month)));

                    return true;
                };
            }
        }

        public static TryParseCultureFunction TodayMinusN_ParseFunction
        {
            get
            {
                return delegate (string text, CultureInfo cultureInfo, out object value)
                {
                    int index = text.IndexOfAny(new[] {'-', '+'});
                    if (index < 0)
                    {
                        value = null;
                        return false;
                    }

                    string nString = text.Substring(index);

                    if (!int.TryParse(nString, NumberStyles.Integer, cultureInfo, out int n))
                    {
                        value = null;
                        return false;
                    }

                    value = DateTime.Today.AddDays(n);
                    return true;
                };
            }
        }

        #endregion

        private static DateTimeRange ThisWeek(CultureInfo cultureInfo)
        {
            DateTime today = DateTime.Today;

            DayOfWeek firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            DayOfWeek lastDayOfWeek = (firstDayOfWeek == DayOfWeek.Sunday)
                ? DayOfWeek.Saturday
                : firstDayOfWeek - 1;

            DayOfWeek dayOfWeek = today.DayOfWeek;

            int daysToFirstDayOfWeek = dayOfWeek - firstDayOfWeek;
            int daysToLastDayOfWeek = lastDayOfWeek - dayOfWeek;

            if (daysToFirstDayOfWeek < 0)
                daysToFirstDayOfWeek += 7;

            if (daysToLastDayOfWeek < 0)
                daysToLastDayOfWeek += 7;

            DateTime firstDateOfWeek = today.AddDays(-(daysToFirstDayOfWeek));
            DateTime lastDateOfWeek = today.AddDays(daysToLastDayOfWeek);


            DateTimeRange range = new DateTimeRange(firstDateOfWeek, lastDateOfWeek);
            return range;
        }
    }
}
