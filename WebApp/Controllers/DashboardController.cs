using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using vm = WebApp.ViewModels.DashboardViewModels;
using Model;
using Model.Enums;
using System.Data.Entity;
using System.Globalization;
using WebApp.Library.Extensions;

namespace WebApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        OrvosiEntities db = new OrvosiEntities();

        // MVC
        public async Task<ActionResult> Index()
        {
            var user = await db.Users.SingleOrDefaultAsync(c => c.UserName == User.Identity.Name);
            Guid? userGuid = new Guid(user.Id);

            var now = DateTime.Today;
            var nextDay = now.AddDays(1);
            var endOfWeek = now.GetEndOfWeek();
            var endOfNextWeek = endOfWeek.AddDays(7);

            var query = db.ServiceRequests;

            if (user.RoleCategoryId != RoleCategory.Admin)
            {
                query.Where(c => c.PhysicianId == user.Id
                    || c.IntakeAssistantId == userGuid
                    || c.DocumentReviewerId == userGuid
                    || c.CaseCoordinatorId == userGuid);
            }

            var today = query
                .Where(c => c.AppointmentDate == now);

            var tomorrow = query
                .Where(c => c.AppointmentDate == nextDay);

            var restOfWeek = query
                .Where(p => p.AppointmentDate > nextDay && p.AppointmentDate <= endOfWeek);

            var nextWeek = query
                .Where(p => p.AppointmentDate > endOfWeek && p.AppointmentDate <= endOfNextWeek);

            var vm = new vm.IndexViewModel();

            vm.Today = today
                .OrderBy(c => c.AppointmentDate)
                .OrderBy(c => c.StartTime)
                .ToList();

            vm.Tomorrow = tomorrow
                .OrderBy(c => c.AppointmentDate)
                .OrderBy(c => c.StartTime)
                .ToList();

            vm.RestOfWeek = restOfWeek
                .OrderBy(c => c.AppointmentDate)
                .OrderBy(c => c.StartTime)
                .ToList();

            vm.NextWeek = nextWeek
                .OrderBy(c => c.AppointmentDate)
                .OrderBy(c => c.StartTime)
                .ToList();

            ViewBag.PhysicianName = user.DisplayName;
            ViewBag.UserId = user.Id;

            return View(vm);
        }

        // API
        //[HttpGet]
        //public JsonResult GetServiceCatalogue(string id)
        //{
        //    using (var db = new OrvosiEntities())
        //    {
        //        var list = db.Services.Where(s => s.ServiceCategoryId == 5).Select(c => new vm.Service()
        //        {
        //            Id = c.Id,
        //            Name = c.Name
        //        });
        //        return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        //    }
        //}

        //[HttpPost]
        //public async Task<JsonResult> SubmitSpecialRequest(vm.SpecialRequestFormViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Json(new
        //        {
        //            success = false,
        //            model = model,
        //            message = "Errors occurred"
        //        });
        //    }

        //    var db = new OrvosiEntities();

        //    var identity = (User.Identity as ClaimsIdentity);
        //    // save the record to the database

        //    var specialRequest = AutoMapper.Mapper.Map<SpecialRequest>(model);
        //    specialRequest.ModifiedUserName = identity.Name;
        //    specialRequest.ModifiedUserId = identity.FindFirst(ClaimTypes.Sid).Value;

        //    db.SpecialRequests.Add(specialRequest);
        //    model.ActionState = (byte)await db.SaveChangesAsync();

        //    if (model.ActionState == ActionStates.Saved)
        //    {
        //        // the current user that is submitting the special request (someone from the company typcially)
        //        var from = identity.FindFirst(ClaimTypes.Email).Value;
        //        var to = "support@orvosi.ca";
        //        // Get the physician selected
        //        _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //        var physician = await _userManager.FindByIdAsync(model.PhysicianId);
        //        var subject = string.Format("Special Request - {0}", physician.UserName.Split('@').First().ToString());
        //        var b = new StringBuilder();
        //        b.AppendLine("REQUESTOR INFO");
        //        b.AppendLine(string.Format("Requestor Name: {0}", identity.FindFirst("DisplayName").Value));
        //        b.AppendLine(string.Format("Requestor Email: {0}", identity.FindFirst(ClaimTypes.Email).Value));
        //        b.AppendLine();
        //        b.AppendLine("PHYSICIAN INFO");
        //        b.AppendLine(string.Format("Physician Name: {0}", physician.DisplayName));
        //        b.AppendLine(string.Format("Physician Email: {0}", physician.Email));
        //        b.AppendLine();
        //        b.AppendLine("REQUEST");
        //        var service = db.Services.SingleOrDefault(c => c.Id == model.ServiceId);
        //        if (service != null)
        //        {
        //            b.AppendLine(string.Format("Service: {0}", "<Not Set>"));
        //        }
        //        else
        //        {
        //            b.AppendLine(string.Format("Service: {0}", service.Name));
        //        }
        //        b.AppendLine(string.Format("Timeframe: {0}", model.Timeframe));
        //        b.AppendLine("Additional Notes:");
        //        b.AppendLine(model.AdditionalNotes);

        //        var message = new MailMessage(from, to, subject, b.ToString());

        //        var messenger = MessagingService.GetService();
        //        await messenger.SendAsync(message);
        //    }

        //    var result = new
        //    {
        //        success = true,
        //        model = model,
        //        message = "Request submitted successfully"
        //    };
        //    return Json(result);
        //}
    }
}