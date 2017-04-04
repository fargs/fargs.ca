using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Library.Extensions
{
    public static class IIdentityExtensions
    {
        public static bool IsAdmin(this IIdentity identity)
        {
            var adminRoles = new Guid[2]
            {
                AspNetRoles.CaseCoordinator,
                AspNetRoles.SuperAdmin
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
            return HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(obj.GetGuidUserId());

        }

        public static short[] GetFeatures(this IIdentity obj)
        {
            return obj.GetApplicationUser().Features;
        }

        public static Guid[] GetPhysicians(this IIdentity obj)
        {
            return obj.GetApplicationUser().Physicians;
        }

        public static UserContextViewModel GetPhysicianContext(this IIdentity obj)
        {
            var claim = obj.GetClaimsIdentity().FindFirstValue("UserContext");
            if (claim != null)
            {
                return JsonConvert.DeserializeObject<UserContextViewModel>(claim);
            }
            return null;
        }

        public static UserContextViewModel GetUserContext(this IIdentity obj)
        {
            var claim = obj.GetClaimsIdentity().FindFirstValue("UserContext");
            if (claim == null)
            {
                return new UserContextViewModel
                {
                    Id = obj.GetGuidUserId(),
                    DisplayName = obj.GetDisplayName()
                };
            }
            return JsonConvert.DeserializeObject<UserContextViewModel>(claim);
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

        public static bool IsImpersonating(this IIdentity identity)
        {
            return identity.GetClaimsIdentity().HasClaim("UserImpersonation", "true");
        }

        public static Guid GetOriginalUserId(this IIdentity identity)
        {
            var claim = identity.GetClaimsIdentity().FindFirstValue("OriginalUserId");
            if (claim == null)
            {
                return Guid.Empty;
            }
            return new Guid(claim);

        }

        public static bool GetIsAppTester(this IIdentity obj)
        {
            var claim = obj.GetClaimsIdentity().FindFirstValue("IsAppTester");
            if (claim == null)
            {
                return false;
            }
            return bool.Parse(claim);
        }

        public static bool GetOriginalIsAppTester(this IIdentity obj)
        {
            var claim = obj.GetClaimsIdentity().FindFirstValue("OriginalIsAppTester");
            if (claim == null)
            {
                return obj.GetIsAppTester();
            }
            return bool.Parse(claim);
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
    }
}