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

        public static DateTime GetEndOfWeek(this DateTime obj)
        {
            return obj.AddDays(obj.GetRestOfWeek());
        }

        public static StringBuilder RemoveLine(this StringBuilder @this, string lineText)
        {
            string[] stringSeparators = new string[] { "\r\n" };
            var arr = @this.ToString().Split(stringSeparators, StringSplitOptions.None).ToList();
            var index = arr.FindIndex(c => c.Contains(lineText));
            arr.RemoveAt(index);
            return @this.Clear().Append(string.Join("\r\n", arr));
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