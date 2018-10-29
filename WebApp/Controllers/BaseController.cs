﻿using System;
using System.Security.Principal;
using System.Web.Mvc;
using WebApp.Library.Extensions;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class BaseController : Controller
    {
        protected IIdentity identity;
        protected UserContextViewModel loggedInUserContext;
        protected LookupViewModel<Guid> physicianContext;
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
            physicianContext = principal.Identity.GetPhysician();
            loggedInUserId = principal.Identity.GetGuidUserId();
            physicianId = principal.Identity.GetPhysicianId();
            physicianOrLoggedInUserId = physicianId.GetValueOrDefault(loggedInUserId);
            loggedInRoleId = identity.GetRoleId();
        }

    }
}