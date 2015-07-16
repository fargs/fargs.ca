using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class AppointmentController : Controller
    {
        // GET: Assessment
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Book()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Book(int id)
        {
            return View();
        }
    }
}