using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using WebApp.Models;
using vm = WebApp.ViewModels.DashboardViewModels;
using System.Net.Mail;
using System.Text;
using Model;
using Model.Enums;
using WebApp.Library.Extensions;
using System.Data.Entity;

namespace WebApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        OrvosiEntities db = new OrvosiEntities();

        // MVC
        public async Task<ActionResult> Index(byte lookAhead, string ImpersonateUserId = "")
        {
            var user = await db.Users.SingleOrDefaultAsync(c => c.UserName == User.Identity.Name);

            var now = DateTime.Today;
            var lookAheadDate = now.AddDays(lookAhead);

            var query = db.ServiceRequests;

            if (user.RoleId == Roles.Physician)
            {
                query.Where(c => c.PhysicianId == user.Id);
            }
            else if (user.RoleId == Roles.IntakeAssistant)
            {
                query.Where(c => c.IntakeAssistantId.ToString() == user.Id);
            }
            else if (user.RoleId == Roles.IntakeAssistant || user.RoleId == Roles.DocumentReviewer)
            {

            }
            else if (user.RoleId == Roles.CaseCoordinator)
            {
                query.Where(c => c.IntakeAssistantId.ToString() == user.Id);
            }
            else if (user.RoleId == Roles.SuperAdmin)
            {
                query.Where(c => c.IntakeAssistantId.ToString() == user.Id);
            }
            else
            {
                throw new Exception("User in this role is not supported at this time.");
            }

            var today = query
                .Where(c => c.AppointmentDate == DateTime.Today);

            var upcoming = query
                .Where(p => p.AppointmentDate > now && p.AppointmentDate <= lookAheadDate);

            var vm = new vm.IndexViewModel();

            vm.Today = today
                .OrderBy(c => c.AppointmentDate)
                .OrderBy(c => c.StartTime)
                .ToList();

            vm.Upcoming = upcoming
                .OrderBy(c => c.AppointmentDate)
                .OrderBy(c => c.StartTime)
                .ToList();

            ViewBag.LookAhead = lookAhead;
            ViewBag.PhysicianName = user.DisplayName;
            ViewBag.UserId = user.Id;

            return View(vm);
        }

        // API
        [HttpGet]
        public JsonResult GetServiceCatalogue(string id)
        {
            using (var db = new OrvosiEntities())
            {
                var list = db.Services.Where(s => s.ServiceCategoryId == 5).Select(c => new vm.Service()
                {
                    Id = c.Id,
                    Name = c.Name
                });
                return Json(list.ToList(), JsonRequestBehavior.AllowGet);
            }
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

            var db = new OrvosiEntities();

            var identity = (User.Identity as ClaimsIdentity);
            // save the record to the database

            var specialRequest = AutoMapper.Mapper.Map<SpecialRequest>(model);
            specialRequest.ModifiedUserName = identity.Name;
            specialRequest.ModifiedUserId = identity.FindFirst(ClaimTypes.Sid).Value;

            db.SpecialRequests.Add(specialRequest);
            model.ActionState = (byte)await db.SaveChangesAsync();

            if (model.ActionState == ActionStates.Saved)
            {
                // the current user that is submitting the special request (someone from the company typcially)
                var from = identity.FindFirst(ClaimTypes.Email).Value;
                var to = "support@orvosi.ca";
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
                var service = db.Services.SingleOrDefault(c => c.Id == model.ServiceId);
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
                .Where(u => u.Roles.Any(r => r.RoleId == Roles.Physician)).ToList();
            var physicians = users
                .Select(u => new SelectListItem() { Value = u.Id, Text = u.DisplayName })
                .ToList();
            return new SelectList(physicians, "Value", "Text");
        }

        private SelectList GetServices()
        {
            using (var db = new OrvosiEntities())
            {
                var services = db.Services
                    .Where(s => s.ServiceCategoryId == 5)
                    .Select(u => new SelectListItem() { Value = u.Id.ToString(), Text = u.Name })
                    .ToList();
                return new SelectList(services, "Value", "Text");
            }
        }
    }
}