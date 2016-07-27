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

        public ActionResult Work()
        {
            return View();
        }

        // MVC
        public async Task<ActionResult> Index(Guid? serviceProviderId, string orderTasksBy = "DueDate", bool onlyMine = true)
        {
            // Set date range variables used in where conditions
            var now = SystemTime.Now();
            var startOfWeek = now.Date.GetStartOfWeek();
            var endOfWeek = now.Date.GetEndOfWeek();
            var startOfNextWeek = now.Date.GetStartOfNextWeek();
            var endOfNextWeek = now.Date.GetEndOfNextWeek();

            Guid? userId = User.Identity.GetGuidUserId();
            // Admins can see the Service Provider dropdown and view other's dashboards. Otherwise, it displays the data of the current user.
            if (User.Identity.IsAdmin() && serviceProviderId.HasValue)
            {
                userId = serviceProviderId.Value;
            }

            var requests = await context.API_GetAssignedServiceRequestsAsync(userId, now);

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

            return new NegotiatedResult("Index", vm);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateTaskStatus(int taskId, bool isChecked, Guid? serviceProviderGuid)
        {
            var now = SystemTime.Now();
            Guid? userId = User.Identity.GetGuidUserId();

            var serviceRequestTask = await context.ServiceRequestTasks.FindAsync(taskId);

            if (serviceRequestTask == null)
                return HttpNotFound();

            if (isChecked)
                serviceRequestTask.CompletedDate = SystemTime.Now();
            else
                serviceRequestTask.CompletedDate = null;

            serviceRequestTask.ModifiedDate = SystemTime.Now();
            serviceRequestTask.ModifiedUser = User.Identity.Name;
            await context.SaveChangesAsync();

            return await Index(serviceProviderGuid);
        }

        [HttpGet]
        public async Task<ActionResult> TaskHierarchy(int serviceRequestId, int? taskId = null)
        {
            var now = SystemTime.Now();
            Guid? userId = User.Identity.GetGuidUserId();

            var requests = await context.API_GetServiceRequestAsync(serviceRequestId, now);

            var vm = new TaskListViewModel(requests, taskId);

            return PartialView("_TaskList", vm);
        }



    }

}