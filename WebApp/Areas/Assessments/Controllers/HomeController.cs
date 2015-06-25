using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.Assessments.Controllers
{
    public class HomeController : Controller
    {
        // GET: Assessments/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}