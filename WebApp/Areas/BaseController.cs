using System;
using System.Security.Principal;
using System.Web.Mvc;
using WebApp.Library.Extensions;
using WebApp.ViewModels;

namespace WebApp.Areas.Shared
{
    public class BaseController : Controller
    {
        protected IIdentity identity;
        protected UserContextViewModel loggedInUserContext;
        protected UserContextViewModel physicianContext;
        protected Guid loggedInUserId;
        protected Guid loggedInRoleId;
        protected Guid? physicianId;
        protected Guid physicianOrLoggedInUserId;
        protected DateTime now;

        public BaseController()
        {
        }

        public BaseController(DateTime now, IPrincipal principal)
        {
            this.now = now;
            identity = principal.Identity;
            loggedInUserContext = identity.GetLoggedInUserContext();
            physicianContext = principal.Identity.GetPhysicianContext();
            loggedInUserId = principal.Identity.GetGuidUserId();
            physicianId = physicianContext == null ? (Guid?)null : physicianContext.Id;
            physicianOrLoggedInUserId = physicianId.GetValueOrDefault(loggedInUserId);
            loggedInRoleId = identity.GetRoleId();
        }

    }
}