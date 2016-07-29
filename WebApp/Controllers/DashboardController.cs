using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using dvm = WebApp.ViewModels.DashboardViewModels;
using Orvosi.Data;
using Orvosi.Shared.Enums;
using WebApp.Library.Extensions;
using WebApp.Library;
using Westwind.Web.Mvc;

namespace WebApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private OrvosiDbContext context = new OrvosiDbContext();

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
            var vm = new dvm.IndexViewModel(requests, now, userId.Value, this.ControllerContext);

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

        [HttpPost]
        public async Task<ActionResult> ToggleNoShow(int serviceRequestId, bool isChecked, Guid? serviceProviderGuid)
        {
            var now = SystemTime.Now();
            Guid? userId = User.Identity.GetGuidUserId();

            var serviceRequest = await context.ServiceRequests.FindAsync(serviceRequestId);

            if (serviceRequest == null)
                return HttpNotFound();
            
            serviceRequest.IsNoShow = isChecked;
            serviceRequest.ModifiedDate = SystemTime.Now();
            serviceRequest.ModifiedUser = User.Identity.Name;

            foreach (var task in serviceRequest.ServiceRequestTasks.Where(t => t.CompletedDate == null && t.TaskId != Tasks.SubmitInvoice && t.TaskId != Tasks.CloseCase))
            {
                task.IsObsolete = isChecked;
                task.ModifiedDate = SystemTime.Now();
                task.ModifiedUser = User.Identity.Name;
            }

            await context.SaveChangesAsync();

            return await Index(serviceProviderGuid);
        }

        [HttpGet]
        public async Task<ActionResult> TaskHierarchy(int serviceRequestId, int? taskId = null)
        {
            var now = SystemTime.Now();
            Guid? userId = User.Identity.GetGuidUserId();

            var requests = await context.API_GetServiceRequestAsync(serviceRequestId, now);

            requests = requests.Where(o => o.ResponsibleRoleId != Roles.CaseCoordinator || o.TaskId == Tasks.SaveMedBrief).ToList();
            requests = requests.Where(c => c.TaskStatusId == TaskStatuses.Waiting || c.TaskStatusId == TaskStatuses.ToDo).ToList();

            var vm = new dvm.TaskListViewModel(requests, taskId);

            return PartialView("_TaskHierarchy", vm);
        }

        [HttpGet]
        public async Task<ActionResult> TaskList(int serviceRequestId)
        {
            var now = SystemTime.Now();
            Guid? userId = User.Identity.GetGuidUserId();

            var requests = await context.API_GetServiceRequestAsync(serviceRequestId, now);
            var assessment = new dvm.Assessment
            {
                ClaimantName = requests.First().ClaimantName,
                Tasks = from o in requests
                        where o.ResponsibleRoleId != Roles.CaseCoordinator || o.TaskId == Tasks.SaveMedBrief
                        orderby o.TaskSequence
                        select new dvm.Task
                        {
                            Id = o.Id,
                            Name = o.TaskName,
                            CompletedDate = o.CompletedDate,
                            StatusId = o.TaskStatusId.Value,
                            Status = o.TaskStatusName,
                            AssignedTo = o.AssignedTo,
                            AssignedToDisplayName = o.AssignedToDisplayName,
                            AssignedToColorCode = o.AssignedToColorCode,
                            AssignedToInitials = o.AssignedToInitials,
                            IsComplete = o.TaskStatusId.Value == TaskStatuses.Done,
                            ServiceRequestId = o.ServiceRequestId

                        }
            };

            return PartialView("_TaskList", assessment);
        }


    }

}