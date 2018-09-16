using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebApp.Library;
using WebApp.Library.Extensions;
using WebApp.ViewModels;
using FluentDateTime;
using WebApp.Library.Filters;

namespace WebApp.Controllers
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