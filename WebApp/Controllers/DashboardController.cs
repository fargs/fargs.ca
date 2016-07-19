using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.ViewModels.DashboardViewModels;
using Orvosi.Data;
using Orvosi.Shared.Enums;
using System.Data.Entity;
using System.Globalization;
using WebApp.Library.Extensions;
using System.Collections.Generic;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using WebApp.Library;
using Westwind.Web.Mvc;

namespace WebApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        //OrvosiEntities db = new OrvosiEntities();
        private OrvosiDbContext context = new OrvosiDbContext();

        // MVC
        public async Task<ActionResult> Index(Guid? serviceProviderId, string orderTasksBy = "DueDate", bool onlyMine = true)
        {
            // Set date range variables used in where conditions
            var now = SystemTime.Now();
            var startOfWeek = now.Date.GetStartOfWeek();
            var endOfWeek = now.Date.GetEndOfWeek();
            var startOfNextWeek = now.Date.GetStartOfNextWeek();
            var endOfNextWeek = now.Date.GetEndOfNextWeek();

            List<API_GetAssignedServiceRequestsReturnModel> requests = null;

            // This pulls all the data required by the dashboard in one call to the database
            //var requests = context.ServiceRequests
            //    .Include(sr => sr.Service)
            //    .Include(sr => sr.Address.City_CityId)
            //    .Include(sr => sr.Company)
            //    .Include(sr => sr.AvailableSlot)
            //    .Include(sr => sr.CaseCoordinator)
            //    .Include(sr => sr.DocumentReviewer)
            //    .Include(sr => sr.IntakeAssistant)
            //    .Include(sr => sr.Physician.AspNetUser)
            //    .Where(sr => sr.AppointmentDate >= startOfWeek && sr.AppointmentDate < endOfNextWeek);

            Guid? userId = User.Identity.GetGuidUserId();
            // Admins can see the Service Provider dropdown and view other's dashboards. Otherwise, it displays the data of the current user.
            if (User.Identity.IsAdmin() && serviceProviderId.HasValue)
            {
                userId = serviceProviderId.Value;
            }

            requests = await context.API_GetAssignedServiceRequestsAsync(userId, now);

            // Populate the view model
            var vm = new IndexViewModel(requests, now, userId.Value, this.ControllerContext);

            // Additional view data.
            vm.SelectedUserId = serviceProviderId;
            vm.UserSelectList = (from user in context.AspNetUsers
                                from userRole in context.AspNetUserRoles
                                from role in context.AspNetRoles
                                where user.Id == userRole.UserId && role.Id == userRole.RoleId
                                select new SelectListItem
                                {
                                    Text = user.FirstName + " " + user.LastName,
                                    Value = user.Id.ToString(),
                                    Group = new SelectListGroup() { Name = role.Name }
                                }).ToList();

            var useKnockoutView = true;
            if (useKnockoutView)
            {
                return new NegotiatedResult("IndexKO", vm);
            }
            return View(vm);
        }

        //private static List<IndexViewModel.DayFolder> GetCards(List<API_GetAssignedServiceRequestsReturnModel> requests, byte WeekNumber)
        //{
        //    var cards = new List<IndexViewModel.DayFolder>();
        //    var days = requests.Select(c => new { Day = c.AppointmentDate.Value, WeekNumber = WeekNumber }).Distinct();

        //    foreach (var day in days)
        //    {
        //        var dayRequests = requests.Where(c => c.AppointmentDate == day.Day.Date).OrderBy(c => c.StartTime);
        //        var first = dayRequests.First();
        //        var last = dayRequests.Last();
        //        var card = new IndexViewModel.AssessmentCard
        //        {
        //            Address = first.Ad,
        //            Day = day.Day,
        //            City = first.Address.City_CityId.Name,
        //            Company = first.Company.Name,
        //            StartTime = first.AvailableSlot.StartTime,
        //            EndTime = last.AvailableSlot.EndTime.GetValueOrDefault(first.AvailableSlot.StartTime.Add(new TimeSpan(1))),
        //            RequestCount = dayRequests.Count(),
        //            ServiceRequests = dayRequests
        //        };
        //        cards.Add(card);
        //    }
        //    return cards;
        //}
    }

}