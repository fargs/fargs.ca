using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using vm = WebApp.ViewModels;
using enums = WebApp.Models.Enums;

namespace WebApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private ApplicationDbContext _db;

        // MVC
        public async Task<ActionResult> Index(string schedulingProcess = "ByPhysician")
        {
            _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            _roleManager = HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            _db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var model = new vm.DashboardViewModel();
            model.UserDisplayName = user.DisplayName;
            model.SchedulingProcess = schedulingProcess;
            model.Physicians = GetPhysicians();
            if (user.Roles.Count > 0)
            {
                var roleId = user.Roles.First().RoleId;
                model.UserRoleName = _roleManager.Roles.SingleOrDefault(c => c.Id == roleId).Name;
            }
            var company = _db.Companies.SingleOrDefault(c => c.Id == user.CompanyId);
            if (company != null)
            {
                model.UserCompanyDisplayName = company.Name;
                model.UserCompanyLogoCssClass = company.LogoCssClass;

                if (model.SchedulingProcess == "ByTime")
                {
                    model.BookingPageName = company.MasterBookingPageByTime;
                }
                else if (model.SchedulingProcess == "Teleconference")
                {
                    model.BookingPageName = company.MasterBookingPageTeleconference;
                }
                else if (model.SchedulingProcess == "ByPhysician")
                {
                    model.BookingPageName = company.MasterBookingPageByPhysician;
                }
                else if (model.SchedulingProcess == "SpecialRequest")
                {
                    model.BookingPageName = null;
                }
            }
            return View(model);
        }

        // API
        [HttpGet]
        public JsonResult GetServiceCatalogue(string id)
        {
            _db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            var list = from sc in _db.ServiceCatalogue
                       join s in _db.Services on sc.ServiceId equals s.Id
                       where sc.PhysicianId == id
                       select new vm.Service { Id = s.Id, Name = s.Name };
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SubmitSpecialRequest(SpecialRequestFormViewModel vm)
        {
            var result = new
            {
                success = true,
                message = "Request submitted successfully"
            };
            return Json(result);
        }

        private IEnumerable<ViewModels.Physician> GetPhysicians()
        {
            var role = _roleManager.Roles.SingleOrDefault(r => r.Id == enums.Roles.Physician);
            var users = _userManager.Users.Where(u => u.Roles.Any(r => r.RoleId == role.Id));
            return users.Select(u => new ViewModels.Physician()
            {
                Id = u.Id,
                DisplayName = u.FirstName
            });
        }
    }
}