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
            var claim = obj.GetClaimsIdentity().FindFirstValue("Features");
            if (claim != null)
            {
                return JsonConvert.DeserializeObject<short[]>(claim);
            }
            return null;
        }

        public static IEnumerable<LookupViewModel<Guid>> GetPhysicians(this IIdentity obj)
        {
            var claim = obj.GetClaimsIdentity().FindFirstValue("Physicians");
            if (claim != null)
            {
                return JsonConvert.DeserializeObject<IEnumerable<LookupViewModel<Guid>>>(claim);
            }
            return null;
        }
        public static Guid? GetPhysicianId(this IIdentity identity)
        {
            var claim = identity.GetClaimsIdentity().FindFirstValue("PhysicianId");
            if (string.IsNullOrEmpty(claim) || claim == null)
            {
                return null;
            }
            return new Guid(claim);
        }
        public static LookupViewModel<Guid> GetPhysician(this IIdentity identity)
        {
            var claim = identity.GetClaimsIdentity().FindFirstValue("Physician");
            if (claim != null)
            {
                return JsonConvert.DeserializeObject<IEnumerable<LookupViewModel<Guid>>>(claim).FirstOrDefault();
            }
            return null;
        }

        public static UserContextViewModel GetLoggedInUserContext(this IIdentity obj)
        {
            var claim = obj.GetClaimsIdentity().FindFirstValue("UserContext");
            if (claim == null)
            {
                return new UserContextViewModel
                {
                    Id = obj.GetGuidUserId(),
                    Name = obj.GetDisplayName()
                };
            }
            return JsonConvert.DeserializeObject<UserContextViewModel>(claim);
        }

        public static UserContextViewModel GetUserContext(this IIdentity obj)
        {
            var claim = obj.GetClaimsIdentity().FindFirstValue("UserContext");
            if (claim == null)
            {
                return new UserContextViewModel
                {
                    Id = obj.GetGuidUserId(),
                    Name = obj.GetDisplayName()
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
        public static string GetEmail(this IIdentity obj)
        {
            var claim = obj.GetClaimsIdentity().FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(claim))
            {
                return string.Empty;
            }
            return claim;
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
        public static string GetInitials(this IIdentity obj)
        {
            var claim = obj.GetClaimsIdentity().FindFirstValue("Initials");
            if (string.IsNullOrEmpty(claim))
            {
                return string.Empty;
            }
            return claim;
        }
        public static string GetColorCode(this IIdentity obj)
        {
            var claim = obj.GetClaimsIdentity().FindFirstValue("ColorCode");
            if (string.IsNullOrEmpty(claim))
            {
                return string.Empty;
            }
            return claim;
        }
        public static LookupViewModel<Guid> GetRole(this IIdentity identity)
        {
            var claim = identity.GetClaimsIdentity().FindFirstValue("Role");
            if (claim != null)
            {
                return JsonConvert.DeserializeObject<IEnumerable<LookupViewModel<Guid>>>(claim).FirstOrDefault();
            }
            return null;
        }

        public static bool IOwnThis(this IIdentity identity, Guid ownerId)
        {
            return identity.GetGuidUserId() == ownerId;
        }
    }
}