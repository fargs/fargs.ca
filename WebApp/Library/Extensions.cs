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