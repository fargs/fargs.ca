using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Admin.ViewModels.AvailabilityViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    public class AvailabilityController : Controller
    {
        OrvosiEntities db = new OrvosiEntities();

        // GET: Admin/Availability
        public ActionResult Index(string physicianId)
        {
            var physician = db.Physicians.Single(u => u.Id == physicianId);
            if (physician == null)
            {
                throw new Exception("User does not exist");
            }

            var availableDays = db.AvailableDays
                .Where(ad => ad.PhysicianId == physicianId).OrderBy(ad => ad.Day)
                .ToList();

            var availableDaysJson = availableDays.Select(c => new FullCalendarEvent()
            {
                title = c.CompanyName + " - " + c.LocationName,

                start = c.Day.GetDateTimeFormats('d')[0]
            }).ToList();

            var vm = new IndexViewModel()
            {
                Physician = physician,
                AvailableDays = availableDaysJson
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