using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.Staff.Controllers
{
    public class HomeController : BaseController
    {
        OrvosiEntities db = new OrvosiEntities();
        // GET: Staff/Home
        public ActionResult Index()
        {

            return View();
        }
    }
}