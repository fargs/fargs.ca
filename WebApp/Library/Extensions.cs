using Box.V2.Models;
using Orvosi.Data;
using Orvosi.Shared.Enums;

namespace WebApp.Library.Extensions
{
    public static class Extensions
    {

        

        

        

        
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
