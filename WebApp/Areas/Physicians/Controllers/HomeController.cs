using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.Physicians.Controllers
{
    public class HomeController : Controller
    {
        // GET: Physician/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}