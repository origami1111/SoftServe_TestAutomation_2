using System;
using System.Collections.Generic;

namespace WHAT_API
{
    public class PatternGenerator
    {
        public static Pattern GetRelativeMonthlyPattern(int interval, MonthIndex? index,
            params DayOfWeek[] daysOfWeek) =>
            new Pattern()
            {
                Type = PatternType.RelativeMonthly,
                Interval = interval,
                DaysOfWeek = daysOfWeek,
                Index = index
            };

        public static Pattern GetAbsoluteMonthlyPattern(int interval, params int[] dates) =>
            new Pattern()
            {
                Type = PatternType.RelativeMonthly,
                Interval = interval,
                Dates = dates,
            };

        public static Pattern GetDailyPattern(int interval) =>
            new Pattern()
            {
                Type = PatternType.Daily,
                Interval = interval
            };

        public static Pattern GetWeeklyPattern(int interval, DayOfWeek[] daysOfWeek) =>
            new Pattern()
            {
                Type = PatternType.Weekly,
                Interval = interval,
                DaysOfWeek = daysOfWeek
            };
    }
}
