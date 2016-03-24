using Model;
using Model.Enums;
using WebApp.ViewModels.ServiceRequestTaskViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace WebApp.Controllers
{
    [Authorize]
    public class ServiceRequestTaskController : Controller
    {
        private OrvosiEntities db = new OrvosiEntities();
        // GET: ServiceRequestTasks
        public ActionResult Index(FilterArgs filterArgs)
        {
            // get the user
            var user = db.Users.Single(u => u.UserName == User.Identity.Name);

            var sr = db.ServiceRequestTasks.AsQueryable<ServiceRequestTask>();

            if (user.RoleId == Roles.SuperAdmin && filterArgs.ShowAll.Value) { }
            else
            {
                sr.Where(c => c.AssignedTo == user.Id);
            }

            var vm = new IndexViewModel()
            {
                User = user,
                Tasks = sr.ToList()
            };

            return View(vm);
        }

        public ActionResult TaskList(int? serviceRequestId)
        {
            // get the user
            var user = db.Users.Single(u => u.UserName == User.Identity.Name);

            if (user.RoleId != Roles.SuperAdmin && user.RoleId != Roles.CaseCoordinator && !db.ServiceRequestTasks.Any(srt => srt.AssignedTo == user.Id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            var tasks = db.ServiceRequestTasks.Where(srt => srt.ServiceRequestId == serviceRequestId);

            var vm = new IndexViewModel()
            {
                User = user,
                Tasks = tasks.ToList()
            };

            return PartialView("TaskList", vm);
        }
    }
}