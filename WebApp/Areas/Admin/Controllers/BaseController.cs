using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;


namespace WebApp.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        public BaseController(IIdentity identity, DateTime now)
        {
            this.identity = identity;
            this.now = now;
        }

        protected IIdentity identity { get; private set; }
        protected DateTime now { get; private set; }
    }
}