using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Assessments.Models.Home;

namespace WebApp.Areas.Assessments.Controllers
{
    public class HomeController : Controller
    {

        Model.OrvosiEntities db = new Model.OrvosiEntities();

        // GET: Assessments/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Book()
        {
            return View();
        }
    }
}