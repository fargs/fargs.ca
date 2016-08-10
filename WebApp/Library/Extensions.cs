using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using Model;
using Model.Enums;
using System.Security.Principal;
using System.Security.Claims;
using System.Text;
using Box.V2.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using WebApp.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace WebApp.Library.Extensions
{
    public static class Extensions
    {
        public static string ToShortTimeSafe(this TimeSpan timeSpan)
        {
            return new DateTime().Add(timeSpan).ToShortTimeString();
        }

        public static string ToShortTimeSafe(this TimeSpan? timeSpan)
        {
            return timeSpan == null ? string.Empty : timeSpan.Value.ToShortTimeSafe();
        }
        public static IEnumerable<DateTime> GetDateRangeTo(this DateTime self, DateTime toDate)
        {
            var range = Enumerable.Range(0, new TimeSpan(toDate.Ticks - self.Ticks).Days);

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
                return request.Url.Scheme + "://" + request.Url.Authority + VirtualPathUtility.ToAbsolute("~/");
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

        public static string ToOrvosiDateTimeFormat(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd HH:mm");
        }

        public static string ToOrvosiDateTimeFormat(this DateTime? value)
        {
            return value.HasValue ? value.Value.ToString("yyyy-MM-dd HH:mm") : string.Empty;
        }

        public static List<BoxItem> Entries(this BoxFolder value)
        {
            return value.ItemCollection == null ? new List<BoxItem>() : value.ItemCollection.Entries;
        }

        public static string ToJson(this object obj)
        {
            JsonSerializer js = JsonSerializer.Create(new JsonSerializerSettings());
            var jw = new StringWriter();
            js.Serialize(jw, obj);
            return jw.ToString();
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
            if (daysPast > obj.Day) // number of days since monday is greater than the current day number
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

        #region IIdentity

        public static bool IsAdmin(this IIdentity identity)
        {
            var adminRoles = new Guid[2] 
            {
                Roles.CaseCoordinator,
                Roles.SuperAdmin
            };
            return adminRoles.Contains(identity.GetRoleId());
        }
        public static Guid GetGuidUserId(this IIdentity identity)
        {
            Guid result = Guid.Empty;
            Guid.TryParse(identity.GetUserId(), out result);
            return result;
        }

        public static ClaimsIdentity GetClaimsIdentity(this IIdentity obj)
        {
            return obj as ClaimsIdentity;
        }

        public static ApplicationUser GetApplicationUser(this IIdentity obj)
        {
            return obj as ApplicationUser;
        }

        public static Guid GetRoleId(this IIdentity obj)
        {
            var claim = obj.GetClaimsIdentity().FindFirstValue("RoleId");
            if (claim == null)
            {
                return Guid.Empty;
            }
            return new Guid(claim);

        }

        public static string GetDisplayName(this IIdentity obj)
        {
            var claim = obj.GetClaimsIdentity().FindFirstValue("DisplayName");
            if (string.IsNullOrEmpty(claim))
            {
                return string.Empty;
            }
            return claim;

        }

        public static string GetRoleName(this IIdentity obj)
        {
            var claim = obj.GetClaimsIdentity().FindFirstValue(ClaimTypes.Role);
            if (string.IsNullOrEmpty(claim))
            {
                return string.Empty;
            }
            return claim;

        }

        public static List<Guid> GetRoles(this IIdentity obj)
        {
            var roles = obj.GetClaimsIdentity().FindFirstValue("Roles");
            if (roles == null)
            {
                return new List<Guid>();
            }
            return roles.Split('|').Select(s => Guid.Parse(s)).ToList();

        }

        public static bool IOwnThis(this IIdentity identity, Guid ownerId)
        {
            return identity.GetGuidUserId() == ownerId;
        }

        #endregion
    }
}

namespace WebApp.Library.Extensions.Model
{
    public static class Extensions
    {
        public static bool IsPhysician(this User obj)
        {
            return obj.RoleId == Roles.Physician ? true : false;
        }

        public static bool IsCompanyAdmin(this User obj)
        {
            return obj.RoleId == Roles.Company ? true : false;
        }

        public static bool IsExamWorksCompany(this PhysicianCompany obj)
        {
            return obj.ParentId == ParentCompanies.Examworks ? true : false;
        }

        public static bool IsScmCompany(this PhysicianCompany obj)
        {
            return obj.ParentId == ParentCompanies.SCM ? true : false;
        }

        public static BoxFolder GetBoxFolder(this GetServiceRequestResources_Result obj, string folderId)
        {
            var box = new BoxManager();
            return box.GetFolder(folderId, obj.BoxUserId).Result;
        }
    }
    
}
