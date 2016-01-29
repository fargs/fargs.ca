using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;

namespace WebApp.Controllers
{
    public class AvailabilityController : Controller
    {
        OrvosiEntities db = new OrvosiEntities();

        // GET: Availability
        public ActionResult Index()
        {
            // TODO: Get the company information of the current user
            //var profile = db.GetCompany(User.Identity.Name.Split('|')[0]);
            return View();
        }
    }
}