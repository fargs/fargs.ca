using System;
using System.Collections.Generic;
using System.Linq;

namespace Fargs.Portal.Extensions
{
    public enum Timeline
    {
        Past, Present, Future
    }
    public static class DateTimeExtensions
    {
        public static DateTime ToLocalTimeZone(this DateTime timeUtc, string timeZone)
        {
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            return TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);
        }
        public static DateTime ToTimeZoneIana(this DateTime time, string timeZoneIana)
        {
            if (string.IsNullOrEmpty(timeZoneIana)) throw new Exception($"Event on {time.ToIsoDateAndTimeFormat()} does not have a timezone set.");

            //TODO: This mapping info is already in the Timezone table. Needs to be loaded in and cached
            var timeZone = TimeZoneConverter.TZConvert.IanaToWindows(timeZoneIana);
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            return TimeZoneInfo.ConvertTime(time, cstZone);
        }
        public static IEnumerable<DateTime> GetDateRangeTo(this DateTime self, DateTime toDate)
        {
            if (self.Date == toDate.Date)
                return new List<DateTime> { self.Date };

            var range = Enumerable.Range(0, new TimeSpan(toDate.Ticks - self.Ticks).Days + 1);

            return from p in range
                   select self.Date.AddDays(p);
        }
        public static int Quarter(this DateTime dateTime)
        {
            return Convert.ToInt16((dateTime.Month - 1) / 3) + 1;
        }
        public static Timeline ToTimeline(this DateTime value, DateTime now)
        {
            var result = Timeline.Future;
            if (value < now)
                result = Timeline.Past;
            else if (value == now)
                result = Timeline.Present;
            return result;
        }
        public static string ToMonthFolderName(this DateTime value)
        {
            return string.Format("{0} {1}", value.Month.ToString().PadLeft(2, '0'), value.ToString("MMM").ToUpper());
        }
        public static string ToWeekFolderName(this DateTime value)
        {
            return string.Format("{0} {1}-{2}", value.ToString("MMM").ToUpper(), value.GetStartOfWeekWithinMonth().Day.ToString().PadLeft(2, '0'), value.GetEndOfWeekWithinMonth().Day.ToString().PadLeft(2, '0'));
        }
        public static string ToIsoDate(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd");
        }
        public static string ToIsoDate(this DateTime? value)
        {
            return value.HasValue ? value.ToIsoDate() : string.Empty;
        }
        public static string ToMonthDayFormat(this DateTime value)
        {
            return value.ToString("MMM dd");
        }
        public static string ToIsoDateAndTimeFormat(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd hh:mm tt");
        }
        public static string ToMonthDayAndTimeFormat(this DateTime value)
        {
            return value.ToString("MMM dd hh:mm tt");
        }
        public static int GetRestOfWeek(this DateTime obj)
        {
            int restOfWeek = 0;
            switch (obj.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    restOfWeek = 0;
                    break;
                case DayOfWeek.Monday:
                    restOfWeek = 6;
                    break;
                case DayOfWeek.Tuesday:
                    restOfWeek = 5;
                    break;
                case DayOfWeek.Wednesday:
                    restOfWeek = 4;
                    break;
                case DayOfWeek.Thursday:
                    restOfWeek = 3;
                    break;
                case DayOfWeek.Friday:
                    restOfWeek = 2;
                    break;
                case DayOfWeek.Saturday:
                    restOfWeek = 1;
                    break;
                default:
                    restOfWeek = 0;
                    break;
            }
            return restOfWeek;
        }
        public static int GetRestOfWeekWithinMonth(this DateTime obj)
        {
            var restOfWeek = obj.GetRestOfWeek();
            var daysInMonth = DateTime.DaysInMonth(obj.Year, obj.Month);

            // if the end of the month falls in the week
            if (obj.Day + restOfWeek > daysInMonth)
            {
                restOfWeek = daysInMonth - obj.Day;
            }
            return restOfWeek;
        }
        public static int GetDaysOfWeekPast(this DateTime obj)
        {
            int daysOfWeekPast = 0;
            switch (obj.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    daysOfWeekPast = 6;
                    break;
                case DayOfWeek.Monday:
                    daysOfWeekPast = 0;
                    break;
                case DayOfWeek.Tuesday:
                    daysOfWeekPast = 1;
                    break;
                case DayOfWeek.Wednesday:
                    daysOfWeekPast = 2;
                    break;
                case DayOfWeek.Thursday:
                    daysOfWeekPast = 3;
                    break;
                case DayOfWeek.Friday:
                    daysOfWeekPast = 4;
                    break;
                case DayOfWeek.Saturday:
                    daysOfWeekPast = 5;
                    break;
                default:
                    daysOfWeekPast = 0;
                    break;
            }
            return daysOfWeekPast;
        }
        public static DateTime GetEndOfWeek(this DateTime obj)
        {
            return obj.AddDays(obj.GetRestOfWeek());
        }
        public static DateTime GetEndOfWeekWithinMonth(this DateTime obj)
        {
            return obj.AddDays(obj.GetRestOfWeekWithinMonth());
        }
        public static DateTime GetStartOfWeek(this DateTime obj)
        {
            return obj.AddDays(obj.GetDaysOfWeekPast() * -1);
        }
        public static DateTime GetStartOfWeekWithinMonth(this DateTime obj)
        {
            var daysPast = obj.GetDaysOfWeekPast();
            if (daysPast >= obj.Day) // number of days since monday is greater than the current day number
            {
                return new DateTime(obj.Year, obj.Month, 1);
            }
            return obj.AddDays(obj.GetDaysOfWeekPast() * -1);
        }
        public static DateTime GetStartOfNextWeek(this DateTime obj)
        {
            return obj.AddDays(obj.GetRestOfWeek() + 1);
        }
        public static DateTime GetEndOfNextWeek(this DateTime obj)
        {
            return obj.AddDays(obj.GetRestOfWeek() + 7);
        }
    }
}