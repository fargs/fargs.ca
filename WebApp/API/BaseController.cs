using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http;
using WebApp.Library;
using WebApp.Library.Extensions;

namespace WebApp.API
{
    public class BaseController : ApiController
    {
        protected OrvosiDbContext db;
        protected IIdentity identity;
        protected Guid userId;
        protected Guid? physicianId;
        protected Guid roleId;
        protected DateTime now;

        public BaseController()
        {
            now = SystemTime.Now();
        }

        string _userName = string.Empty;
        protected override void Initialize(System.Web.Http.Controllers.HttpControllerContext context)
        {
            base.Initialize(context);

            db = ContextPerRequest.db;
            identity = context.RequestContext.Principal.Identity;
            userId = User.Identity.GetGuidUserId();
            var userContext = User.Identity.GetPhysicianContext();
            physicianId = userContext == null ? (Guid?)null : userContext.Id;
            roleId = User.Identity.GetRoleId();
        }
    }
}