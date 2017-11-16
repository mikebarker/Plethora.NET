using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Plethora.SearchBar.Definitions.WellKnown
{
    public class DateRangeDataTypeDefinition : DataTypeDefinition
    {
        private static readonly Dictionary<string, TryParseCultureFunction> alternatePairs =
            new Dictionary<string, TryParseCultureFunction>();

        static DateRangeDataTypeDefinition()
        {
            RegisterPatternParserPair(@"this week", ThisWeekParseFunction);
            RegisterPatternParserPair(@"last week", LastWeekParseFunction);
            RegisterPatternParserPair(@"next week", NextWeekParseFunction);
            RegisterPatternParserPair(@"this month", ThisMonthParseFunction);
            RegisterPatternParserPair(@"last month", LastMonthParseFunction);
            RegisterPatternParserPair(@"next month", NextMonthParseFunction);
        }

        public DateRangeDataTypeDefinition()
            : this(CultureInfo.CurrentCulture)
        {
        }

        public DateRangeDataTypeDefinition(CultureInfo cultureInfo)
            : base("date", ConstructRegexPattern(cultureInfo), GetParseDate(cultureInfo))
        {
        }

        private static string ConstructRegexPattern(CultureInfo cultureInfo)
        {
            IEnumerable<string> alternatePatterns = alternatePairs.Keys;

            string datePattern = string.Join("|", alternatePatterns);

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

                value = null;
                return false;
            };
        }

        #region Parse Functions

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
