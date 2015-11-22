using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
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
            var identity = (User.Identity as ClaimsIdentity);
            _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            _roleManager = HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();

            _db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var model = new vm.DashboardViewModel();

            model.UserRoleName = identity.FindFirst(ClaimTypes.Role).Value;
            model.UserDisplayName = user.DisplayName;
            model.SchedulingProcess = schedulingProcess;
            model.Physicians = GetPhysicians();
            

            var company = _db.Companies.FirstOrDefault(c => c.Id == user.CompanyId);
            if (company != null)
            {
                model.UserCompanyDisplayName = company.Name;
                model.UserCompanyLogoCssClass = company.LogoCssClass;

                //if (model.SchedulingProcess == "ByTime")
                //{
                //    model.BookingPageName = company.MasterBookingPageByTime;
                //}
                //else 
                if (model.SchedulingProcess == "Teleconference")
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

            //var list = from sc in _db.ServiceCatalogue
            //           join s in _db.Services on sc.ServiceId equals s.Id
            //           where sc.PhysicianId == id
            //           select new vm.Service { Id = s.Id, Name = s.Name };
            var list = _db.Services.Where(s => s.ServiceCategoryId == 5).Select(c => new vm.Service()
            {
                Id = c.Id,
                Name = c.Name
            });
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SubmitSpecialRequest(SpecialRequestFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(model);
            }

            // send an email

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
            var users = _userManager.Users.Where(u => u.Roles.Any(r => r.RoleId == role.Id)).ToList();
            return users.Select(u => new ViewModels.Physician()
            {
                Id = u.Id,
                DisplayName = u.DisplayName
            });
        }
    }
}