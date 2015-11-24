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
using System.Net.Mail;
using System.Text;

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
            model.UserEmail = identity.FindFirst(ClaimTypes.Email).Value;
            model.UserDisplayName = user.DisplayName;
            model.SchedulingProcess = schedulingProcess;
            model.Physicians = GetPhysicians();
            model.Services = GetServices();
            
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
                //if (model.SchedulingProcess == "Teleconference")
                //{
                //    model.BookingPageName = company.MasterBookingPageTeleconference;
                //}
                //else 
                if (model.SchedulingProcess == "ByPhysician")
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
        public async Task<JsonResult> SubmitSpecialRequest(vm.SpecialRequestFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    model = model,
                    message = "Errors occurred"
                });
            }

            var identity = (User.Identity as ClaimsIdentity);
            // save the record to the database
            _db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            var specialRequest = AutoMapper.Mapper.Map<SpecialRequest>(model);
            specialRequest.ModifiedUserName = identity.Name;
            specialRequest.ModifiedUserId = identity.FindFirst(ClaimTypes.Sid).Value;

            _db.SpecialRequests.Add(specialRequest);
            model.ActionState = (byte)await _db.SaveChangesAsync();

            if (model.ActionState == enums.ActionStates.Saved)
            {
                // the current user that is submitting the special request (someone from the company typcially)
                var from = identity.FindFirst(ClaimTypes.Email).Value;
                var to = "scheduling@orvosi.ca";
                // Get the physician selected
                _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var physician = await _userManager.FindByIdAsync(model.PhysicianId);
                var subject = string.Format("Special Request - {0}", physician.UserName.Split('@').First().ToString());
                var b = new StringBuilder();
                b.AppendLine("REQUESTOR INFO");
                b.AppendLine(string.Format("Requestor Name: {0}", identity.FindFirst("DisplayName").Value));
                b.AppendLine(string.Format("Requestor Email: {0}", identity.FindFirst(ClaimTypes.Email).Value));
                b.AppendLine();
                b.AppendLine("PHYSICIAN INFO");
                b.AppendLine(string.Format("Physician Name: {0}", physician.DisplayName));
                b.AppendLine(string.Format("Physician Email: {0}", physician.Email));
                b.AppendLine();
                b.AppendLine("REQUEST");
                var service = _db.Services.SingleOrDefault(c => c.Id == model.ServiceId);
                if (service != null)
                {
                    b.AppendLine(string.Format("Service: {0}", "<Not Set>"));
                }
                else
                {
                    b.AppendLine(string.Format("Service: {0}", service.Name));
                }
                b.AppendLine(string.Format("Timeframe: {0}", model.Timeframe));
                b.AppendLine("Additional Notes:");
                b.AppendLine(model.AdditionalNotes);

                var message = new MailMessage(from, to, subject, b.ToString());

                var messenger = MessagingService.GetService();
                await messenger.SendAsync(message);
            }

            var result = new
            {
                success = true,
                model = model,
                message = "Request submitted successfully"
            };
            return Json(result);
        }

        private SelectList GetPhysicians()
        {
            var users = _userManager.Users
                .Where(u => u.Roles.Any(r => r.RoleId == enums.Roles.Physician)).ToList();
            var physicians = users
                .Select(u => new SelectListItem() { Value = u.Id, Text = u.DisplayName })
                .ToList();
            return new SelectList(physicians, "Value", "Text");
        }

        private SelectList GetServices()
        {
            var services = _db.Services
                .Where(s => s.ServiceCategoryId == 5)
                .Select(u => new SelectListItem() { Value = u.Id.ToString(), Text = u.Name })
                .ToList();
            return new SelectList(services, "Value", "Text");
        }
    }
}