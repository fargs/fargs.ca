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
using WebApp.Library;
using System.Collections.Generic;

namespace WebApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        OrvosiEntities db = new OrvosiEntities();

        // MVC
        public async Task<ActionResult> Index(string ServiceProviderId)
        {
            Guid? serviceProviderGuid;
            var user = await db.Users.SingleOrDefaultAsync(c => c.UserName == User.Identity.Name);
            if (user.RoleId == Roles.SuperAdmin && ServiceProviderId != null)
            {
                serviceProviderGuid = new Guid(ServiceProviderId);
            }
            else
            {
                serviceProviderGuid = new Guid(user.Id);
            }

            var requests = db.GetDashboardServiceRequest(serviceProviderGuid, SystemTime.Now().Date).ToList();

            var vm = new vm.IndexViewModel();
            vm.User = user;
            vm.ThisWeekCards = GetCards(requests, 1);
            vm.ThisWeekTotal = vm.ThisWeekCards.Sum(c => c.Summary.RequestCount);
            vm.NextWeekCards = GetCards(requests, 2);
            vm.NextWeekTotal = vm.NextWeekCards.Sum(c => c.Summary.RequestCount);

            //var tasks = db.ServiceRequestTasks.Where(srt => srt.CompletedDate == null);
            //if (user.RoleId != Roles.SuperAdmin)
            //{
            //    tasks = tasks.Where(srt => srt.AssignedTo == user.Id);
            //}
            //vm.TaskCards = tasks.GroupBy(srt => new { srt.TaskId, srt.TaskName, srt.Sequence })
            //    .Select(c => new vm.IndexViewModel.TaskCard() { CardId = c.Key.TaskId.ToString(), TaskName = c.Key.TaskName, Sequence = c.Key.Sequence.Value, Total = c.Count() })
            //    .OrderBy(c => c.Sequence)
            //    .ToList();

            //foreach (var item in tasks)
            //{
            //    vm.AddTask(item);
            //}

            ViewBag.PhysicianName = user.DisplayName;
            ViewBag.UserId = user.Id;
            ViewBag.ServiceProviderId = serviceProviderGuid;

            return View(vm);
        }

        private static List<vm.IndexViewModel.DateCard> GetCards(List<GetDashboardServiceRequest_Result> requests, byte WeekNumber)
        {
            var cards = new List<vm.IndexViewModel.DateCard>();
            var days = requests.Where(r => r.WeekNumber == WeekNumber).Select(c => new { Day = c.AppointmentDate.Value, WeekNumber = c.WeekNumber }).Distinct();

            foreach (var day in days)
            {
                var dayRequests = requests.Where(c => c.AppointmentDate == day.Day.Date).OrderBy(c => c.StartTime).ToList();
                var first = dayRequests.First();
                var summary = new GetDashboardSchedule_Result()
                {
                    AddressName = first.AddressName,
                    AppointmentDate = day.Day,
                    City = first.City,
                    CompanyName = first.CompanyName,
                    StartTime = first.StartTime,
                    EndTime = dayRequests.Last().EndTime,
                    TimelineId = first.TimelineId,
                    WeekNumber = first.WeekNumber,
                    RequestCount = dayRequests.Count
                };
                cards.Add(new vm.IndexViewModel.DateCard
                {
                    Summary = summary,
                    ServiceRequests = dayRequests
                });
            }
            return cards;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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