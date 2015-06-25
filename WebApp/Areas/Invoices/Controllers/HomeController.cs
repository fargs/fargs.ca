using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.Invoices.Controllers
{
    public class HomeController : Controller
    {
        // GET: Invoices/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}