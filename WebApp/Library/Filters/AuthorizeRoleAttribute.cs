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
using WebApp.Models;

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
        public string Feature { get; set; } = string.Empty; // nullable types are not allowed as attribute parameters ... default to 0
        public string[] Features { get; set; } = null;
        public ActionResult UnauthorizedView() => new ViewResult() { ViewName = "~/Views/Shared/Unauthorized.cshtml" };
        public ActionResult UnauthorizedPartialView() => new PartialViewResult() { ViewName = "~/Views/Shared/Unauthorized.cshtml" };
        public ActionResult Unauthorized(System.Web.Mvc.AuthorizationContext filterContext)
        {
            return filterContext.HttpContext.Request.IsAjaxRequest() ? new HttpStatusCodeResult(HttpStatusCode.Forbidden) : UnauthorizedView();
        }

        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            if (filterContext.IsChildAction) return;

            var identity = filterContext.HttpContext.User.Identity;

            if (!identity.IsAuthenticated)
            {
                filterContext.Result = Unauthorized(filterContext);
                return;
            }

            var roleId = identity.GetRoleId();

            var roleFeatures = identity.GetFeatures();
            if (roleFeatures == null)
            {
                return;
            }


            // Super Admin role has access to all features
            if (roleId == Orvosi.Shared.Enums.AspNetRoles.SuperAdmin) return;

            // Roles with no features are unauthorized to all features
            if (!roleFeatures.Any())
            {
                filterContext.Result = Unauthorized(filterContext);
            }
            // Where the Feature has been set on the Attribute controller action, use that to determine if the Role has access to that feature
            else if (string.IsNullOrEmpty(Feature))
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
    }
}
