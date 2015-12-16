using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Admin.ViewModels.PhysicianLicenceViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    public class PhysicianLicenceController : Controller
    {
        OrvosiEntities db = new OrvosiEntities();

        // GET: Admin/PhysicianLicence
        public ActionResult Index(string userId)
        {
            var user = db.Users.Single(u => u.Id == userId);
            if (user == null)
            {
                throw new Exception("User does not exist");
            }

            // drop down lists
            var licences = db.PhysicianLicenses.Where(p => p.PhysicianId == userId).ToList();

            var vm = new IndexViewModel()
            {
                User = user,
                Licences = licences
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