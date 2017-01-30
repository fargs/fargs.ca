using Orvosi.Data;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebApp.Library.Extensions;
using WebApp.Library.Projections;

namespace WebApp.Library.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class AuthorizeRoleAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        /// <summary>
        /// This is an array of acceptable features to access a method. 
        /// If the user has any of the features they are allowed (to check that they have all of the features use multiple attributes)
        /// </summary>
        public short Feature { get; set; } = 0; // nullable types are not allowed as attribute parameters ... default to 0
        public short[] Features { get; set; } = null;
        public ActionResult UnauthorizedView() => new ViewResult() { ViewName = "~/Views/Shared/Unauthorized.cshtml" };
        public ActionResult UnauthorizedPartialView() => new PartialViewResult() { ViewName = "~/Views/Shared/Unauthorized.cshtml" };
        public ActionResult Unauthorized(System.Web.Mvc.AuthorizationContext filterContext)
        {
            return filterContext.HttpContext.Request.IsAjaxRequest() ? new HttpStatusCodeResult(HttpStatusCode.Forbidden) : UnauthorizedView();
        }

        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            var identity = filterContext.HttpContext.User.Identity;

            if (!identity.IsAuthenticated)
            {
                filterContext.Result = Unauthorized(filterContext);
                return;
            }

            var roleId = identity.GetRoleId();

            short[] roleFeatures;
            // should inject this using dependency injection so it is not building up and tearing down the connection multiple times within a request.
            using (var context = new OrvosiDbContext())
            {
                if (roleId == AspNetRoles.SuperAdmin) // Features list is used to hide/show elements in the views so the entire list is needed.
                    roleFeatures = context.Features.Select(srf => srf.Id).ToArray();
                else
                    roleFeatures = context.AspNetRolesFeatures.Where(srf => srf.AspNetRolesId == roleId).Select(srf => srf.FeatureId).ToArray();

                identity.GetApplicationUser().Features = roleFeatures;

                //if (identity.GetApplicationUser().Physicians == null)
                //{
                    identity.GetApplicationUser().Physicians = GetPhysicians(identity, context);
                //}
            }
            

            // Super Admin role has access to all features
            if (roleId == Orvosi.Shared.Enums.AspNetRoles.SuperAdmin) return;

            // Roles with no features are unauthorized to all features
            if (!roleFeatures.Any())
            {
                filterContext.Result = Unauthorized(filterContext);
            }
            // Where the Feature has been set on the Attribute controller action, use that to determine if the Role has access to that feature
            else if (Feature != 0)
            {
                if (!roleFeatures.Contains(Feature))
                {
                    filterContext.Result = Unauthorized(filterContext);
                    return;
                }
            }
            // Where a list of Features has been set on the Attribute controller action, use that to determine if the Role has access to that ALL of those features
            else if (Features != null)
            {
                if (Features.Any(f => roleFeatures.Contains(f)))
                {
                    filterContext.Result = Unauthorized(filterContext);
                    return;
                }
            }
        }

        private Guid[] GetPhysicians(IIdentity identity, IOrvosiDbContext context)
        {
            var userId = identity.GetGuidUserId();
            IQueryable<AspNetUser> userSelectListQuery;

            if (identity.GetIsAppTester())
            {
                userSelectListQuery = context.AspNetUsers
                    .Where(u => u.AspNetUserRoles.Any(r => r.RoleId == AspNetRoles.Physician));
            }
            else
            {
                userSelectListQuery = context.Collaborators
                    .Join(context.AspNetUsers,
                        c => c.UserId,
                        u => u.Id,
                        (c, u) => new { c.User, CollaboratorUserId = c.CollaboratorUserId })
                    .Where(u => u.CollaboratorUserId == userId)
                    .Select(u => u.User);
            }

            return userSelectListQuery
                .Select(u => u.Id)
                .ToArray();
        }
    }
}
