using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Admin.ViewModels.PhysicianLocationAreasViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    public class PhysicianLocationAreaController : Controller
    {
        OrvosiEntities db = new OrvosiEntities();

        public ActionResult Index(string physicianId)
        {
            var user = db.Users.Single(u => u.Id == physicianId);
            if (user == null)
            {
                throw new Exception("User does not exist");
            }

            var pla = db.PhysicianLocationAreas.Where(p => p.PhysicianId == physicianId).ToList();

            var la = db.LocationAreas.ToList();

            var vm = new IndexViewModel()
            {
                User = user,
                PhysicianLocationAreas = pla,
                LocationAreas = la
            };

            return View(vm);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}