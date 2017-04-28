using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using Orvosi.Data;
using Orvosi.Shared.Enums;
using System.Security.Principal;
using System.Security.Claims;
using System.Text;
using Box.V2.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using WebApp.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Collections.Specialized;
using System.Runtime.Serialization.Formatters.Binary;
using WebApp.ViewModels;
using System.Web.Mvc;
using Humanizer;

namespace WebApp.Library.Extensions
{
    public static class Extensions
    {

        public static Dictionary<string, object> ToDictionary(this NameValueCollection nvc, bool handleMultipleValuesPerKey)
        {
            var result = new Dictionary<string, object>();
            foreach (string key in nvc.Keys)
            {
                if (handleMultipleValuesPerKey)
                {
                    string[] values = nvc.GetValues(key);
                    if (values.Length == 1)
                    {
                        result.Add(key, values[0]);
                    }
                    else
                    {
                        result.Add(key, values);
                    }
                }
                else
                {
                    result.Add(key, nvc[key]);
                }
            }

            return result;
        }

        public static string ToShortTimeSafe(this TimeSpan timeSpan)
        {
            return new DateTime().Add(timeSpan).ToShortTimeString();
        }

        public static string ToShortTimeSafe(this TimeSpan? timeSpan)
        {
            return timeSpan == null ? string.Empty : timeSpan.Value.ToShortTimeSafe();
        }
        public static DateTime ToLocalTimeZone(this DateTime timeUtc, string timeZone)
        {
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            return TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);
        }
        public static DateTime ToTimeZoneIana(this DateTime time, string timeZoneIana)
        {
            //TODO: This mapping info is already in the Timezone table. Needs to be loaded in and cached
            string timeZone = TimeZones.EasternStandardTime;
            switch (timeZoneIana)
            {
                case "America/Vancouver":
                    timeZone = TimeZones.PacificStandardTime;
                    break;
                default:
                    break;
            }
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

        public static byte ToTimeline(this DateTime value, DateTime now)
        {
            byte result = Timeline.Future;
            if (value < now)
                result = Timeline.Past;
            else if (value == now)
                result = Timeline.Present;
            return result;
        }

        public static string GetBaseUrl(this HttpRequestBase request)
        {
            if (request.Url == (Uri)null)
                return string.Empty;
            else
                return request.Url.Scheme + "://" + request.Url.Authority + VirtualPathUtility.ToAbsolute("~");
        }

        public static string ToMonthFolderName(this DateTime value)
        {
            return string.Format("{0} {1}", value.Month.ToString().PadLeft(2, '0'), value.ToString("MMM").ToUpper());
        }

        public static string ToWeekFolderName(this DateTime value)
        {
            return string.Format("{0} {1}-{2}", value.ToString("MMM").ToUpper(), value.GetStartOfWeekWithinMonth().Day.ToString().PadLeft(2, '0'), value.GetEndOfWeekWithinMonth().Day.ToString().PadLeft(2, '0'));
        }

        public static string ToOrvosiDateFormat(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd");
        }

        public static string ToOrvosiDateFormat(this DateTime? value)
        {
            return value.HasValue ? value.Value.ToString("yyyy-MM-dd") : string.Empty;
        }
        public static string ToHumanizedDate(this DateTime? value)
        {
            return value.HasValue ? value.Value.Humanize() : "ASAP";
        }
        public static string ToOrvosiDateShortFormat(this DateTime value)
        {
            return value.ToString("MMM dd");
        }

        public static string ToOrvosiDateTimeFormat(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd hh:mm tt");
        }

        public static string ToOrvosiDateTimeShortFormat(this DateTime value)
        {
            return value.ToString("MMM dd hh:mm tt");
        }

        public static string ToOrvosiDateTimeFormat(this DateTime value, TimeSpan startTime)
        {
            return value.AddTicks(startTime.Ticks + 1).ToOrvosiDateTimeFormat();
        }

        public static string ToOrvosiDateTimeFormat(this DateTime? value)
        {
            return value.HasValue ? value.Value.ToOrvosiDateTimeFormat() : string.Empty;
        }

        public static string ToOrvosiDateTimeFormat(this DateTime? value, TimeSpan startTime)
        {
            return value.HasValue ? value.Value.ToOrvosiDateTimeFormat(startTime) : string.Empty;
        }

        public static string ToOrvosiLongDateFormat(this DateTime value)
        {
            return value.ToString("ddd dd, MMM");
        }

        public static List<BoxItem> Entries(this BoxFolder value)
        {
            return value.ItemCollection == null ? new List<BoxItem>() : value.ItemCollection.Entries;
        }

        public static string ToJson(this object obj) =>
            JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

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

        public static StringBuilder RemoveLine(this StringBuilder @this, string lineText)
        {
            string[] stringSeparators = new string[] { "\r\n" };
            var arr = @this.ToString().Split(stringSeparators, StringSplitOptions.None).ToList();
            var index = arr.FindIndex(c => c.Contains(lineText));
            arr.RemoveAt(index);
            return @this.Clear().Append(string.Join("\r\n", arr));
        }

        public static HtmlString ToCSV(this IEnumerable<string> values)
        {
            var result = new HtmlString("'" + string.Join("','", values) + "'");
            return result;
        }

        
    }
}

namespace WebApp.Library.Extensions.Model
{
    public static class Extensions
    {
        public static bool IsPhysician(this AspNetUserView obj)
        {
            return obj.RoleId == AspNetRoles.Physician ? true : false;
        }

        public static bool IsCompanyAdmin(this AspNetUserView obj)
        {
            return obj.RoleId == AspNetRoles.Company ? true : false;
        }

        public static bool IsExamWorksCompany(this PhysicianCompanyView obj)
        {
            return obj.ParentId == ParentCompanies.Examworks ? true : false;
        }

        public static bool IsScmCompany(this PhysicianCompanyView obj)
        {
            return obj.ParentId == ParentCompanies.SCM ? true : false;
        }

        public static BoxFolder GetBoxFolder(this GetServiceRequestResourcesReturnModel obj, string folderId)
        {
            var box = new BoxManager();
            return box.GetFolder(folderId, obj.BoxUserId).Result;
        }
    }

}
