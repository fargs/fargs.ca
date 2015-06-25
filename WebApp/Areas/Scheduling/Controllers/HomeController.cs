using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.Scheduling.Controllers
{
    public class HomeController : Controller
    {
        // GET: Scheduling/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}