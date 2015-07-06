using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.Developers.Controllers
{
    public class HomeController : Controller
    {
        // GET: Developers/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Design()
        {
            return View();
        }
    }
}