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

namespace WebApp.Controllers
{
    public class BaseController : Controller
    {
        protected WorkService service;
        protected OrvosiDbContext db;
        protected IIdentity identity;
        protected UserContextViewModel userContext;
        protected Guid userId;
        protected Guid physicianId;
        protected Guid roleId;
        protected DateTime now;

        public BaseController()
        {
            db = new OrvosiDbContext();
            now = SystemTime.Now();
        }

        string _userName = string.Empty;
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            identity = requestContext.HttpContext.User.Identity;
            userId = User.Identity.GetGuidUserId();
            userContext = User.Identity.GetUserContext();
            physicianId = userContext.Id;
            roleId = User.Identity.GetRoleId();
            service = new WorkService(this.db, this.identity);
            ViewData["Now"] = now;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
                this.service.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}