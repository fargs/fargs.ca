using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Super Admin, Physician, Intake Coordinator, Admin")]
    [RequireHttps]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (this.Request.IsAuthenticated)
                return RedirectToAction("Index", "Dashboard");

            return View();
        }

        [AllowAnonymous]
        public ActionResult Landing()
        {
            return View("Index");
        }

        [AllowAnonymous]
        public ActionResult Index2()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Services()
        {
            return View();
        }

        public ActionResult ReleaseHistory()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult TestHelper()
        {
            return View();
        }
    }
}