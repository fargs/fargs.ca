using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.Process.Controllers
{
    public class HomeController : Controller
    {
        // GET: Process/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}