using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.Reports.Controllers
{
    public class HomeController : Controller
    {
        // GET: Reports/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}