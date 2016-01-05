using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Physicians.ViewModels.Home;

namespace WebApp.Areas.Physicians.Controllers
{
    public class HomeController : Controller
    {
        OrvosiEntities db = new OrvosiEntities();
        // GET: Physician/Home
        public ActionResult Index(string physicianId, byte lookAhead)
        {
            var now = DateTime.Today;
            var lookAheadDate = now.AddDays(lookAhead);
            var vm = new IndexViewModel();
            vm.Today = db.ServiceRequests.Where(p => p.PhysicianId == physicianId && p.AppointmentDate == now)
                .OrderBy(c => c.AppointmentDate)
                .OrderBy(c => c.StartTime)
                .ToList();
            vm.Upcoming = db.ServiceRequests.Where(p => p.PhysicianId == physicianId && p.AppointmentDate > now && p.AppointmentDate <= lookAheadDate)
                .OrderBy(c => c.AppointmentDate)
                .OrderBy(c => c.StartTime)
                .ToList();

            var identity = (User.Identity as ClaimsIdentity);
            ViewBag.LookAhead = lookAhead;
            ViewBag.PhysicianName = identity.FindFirst("DisplayName").Value;

            var user = db.Users.Single(c => c.UserName == User.Identity.Name);
            ViewBag.UserId = user.Id;

            return View(vm);
        }

        public async Task<ActionResult> Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceRequest serviceRequest = await db.ServiceRequests.FindAsync(id);
            if (serviceRequest == null)
            {
                return HttpNotFound();
            }
            var user = db.Users.Single(c => c.UserName == User.Identity.Name);
            ViewBag.UserId = user.Id;
            return View(serviceRequest);
        }
    }
}