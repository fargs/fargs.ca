using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Staff.ViewModels.Home;

namespace WebApp.Areas.Staff.Controllers
{
    public class HomeController : BaseController
    {
        OrvosiEntities db = new OrvosiEntities();
        // GET: Physician/Home
        public ActionResult Index(Guid staffId, byte lookAhead)
        {
            var now = DateTime.Today;
            var lookAheadDate = now.AddDays(lookAhead);
            var vm = new IndexViewModel();
            //vm.Today = db.ServiceRequests.Where(p => (p.IntakeAssistantId == staffId || p.CaseCoordinatorId == staffId || p.DocumentReviewerId == staffId) && p.AppointmentDate == now)
            //    .OrderBy(c => c.AppointmentDate)
            //    .OrderBy(c => c.StartTime)
            //    .ToList();
            //vm.Upcoming = db.ServiceRequests.Where(p => (p.IntakeAssistantId == staffId || p.CaseCoordinatorId == staffId || p.DocumentReviewerId == staffId) && p.AppointmentDate > now && p.AppointmentDate <= lookAheadDate)
            //    .OrderBy(c => c.AppointmentDate)
            //    .OrderBy(c => c.StartTime)
            //    .ToList();

            ViewBag.LookAhead = lookAhead;
            ViewBag.StaffName = (User.Identity as ClaimsIdentity).FindFirst("DisplayName").Value;

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

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var vm = new DetailsViewModel();

            vm.ServiceRequest = await db.ServiceRequests.FindAsync(id);
            vm.ServiceRequestTasks = db.ServiceRequestTasks.Where(sr => sr.ServiceRequestId == id && !sr.IsObsolete).ToList();

            if (vm.ServiceRequest == null)
            {
                return HttpNotFound();
            }

            vm.User = db.Users.Single(c => c.UserName == User.Identity.Name);

            return View(vm);
        }
    }
}