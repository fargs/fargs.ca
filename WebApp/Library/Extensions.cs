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

namespace WebApp.Library.Extensions
{
    public static class Extensions
    {

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

        public static string GetBaseUrl(this HttpRequestBase request)
        {
            if (request.Url == (Uri)null)
                return string.Empty;
            else
                return request.Url.Scheme + "://" + request.Url.Authority + VirtualPathUtility.ToAbsolute("~/");
        }

        public static string ToOrvosiDateFormat(this DateTime value)
        {
            return value.GetDateTimeFormats('d')[5];
        }

        public static string ToJson(this object obj)
        {
            JsonSerializer js = JsonSerializer.Create(new JsonSerializerSettings());
            var jw = new StringWriter();
            js.Serialize(jw, obj);
            return jw.ToString();
        }

        public static ClaimsIdentity GetClaimsIdentity(this IIdentity obj)
        {
            return obj as ClaimsIdentity;
        }

        public static string GetRoleId(this IIdentity obj)
        {
            var claim = obj.GetClaimsIdentity().Claims.SingleOrDefault(c => c.Type == "RoleId");
            if (claim == null)
            {
                return string.Empty;
            }
            return claim.Value;

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

        public static DateTime GetStartOfWeek(this DateTime obj)
        {
            return obj.AddDays(obj.GetDaysOfWeekPast() * -1);
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
    }
}