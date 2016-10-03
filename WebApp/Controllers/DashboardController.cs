using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using dvm = WebApp.ViewModels.DashboardViewModels;
using Orvosi.Shared.Enums;
using WebApp.Library.Extensions;
using WebApp.Library;
using WebApp.Models.ServiceRequestModels;
using m = Orvosi.Shared.Model;
using Westwind.Web.Mvc;

namespace WebApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private Orvosi.Data.OrvosiDbContext context = new Orvosi.Data.OrvosiDbContext();

        public async Task<ActionResult> Index(Guid? serviceProviderId, bool showClosed = false, bool onlyMine = true)
        {
            return RedirectToAction("Today");
            //// Set date range variables used in where conditions
            //var now = SystemTime.Now();
            //var loggedInUserId = User.Identity.GetGuidUserId();
            //var baseUrl = Request.GetBaseUrl();

            //Guid? userId = User.Identity.GetGuidUserId();
            //// Admins can see the Service Provider dropdown and view other's dashboards. Otherwise, it displays the data of the current user.
            //if (User.Identity.IsAdmin() && serviceProviderId.HasValue)
            //{
            //    userId = serviceProviderId.Value;
            //}

            //var requests = await context.GetAssignedServiceRequestsAsync(userId, now, showClosed, null);

            //// Populate the view model
            //var vm = new dvm.IndexViewModel();

            //vm.WeekFolders = ServiceRequestMapper.MapToWeekFolders(requests, now, loggedInUserId, baseUrl);
            //vm.AddOns = ServiceRequestMapper.MapToAddOns(requests, now, loggedInUserId, baseUrl);
            //vm.Today = ServiceRequestMapper.MapToToday(requests, now, loggedInUserId, baseUrl);
            //vm.DueDates = ServiceRequestMapper.MapToDueDates(requests, now, loggedInUserId, baseUrl);

            //// Additional view data.
            //vm.SelectedUserId = userId;
            //vm.UserSelectList = (from user in context.AspNetUsers
            //                    from userRole in context.AspNetUserRoles
            //                    from role in context.AspNetRoles
            //                    where user.Id == userRole.UserId && role.Id == userRole.RoleId
            //                    select new SelectListItem
            //                    {
            //                        Text = user.FirstName + " " + user.LastName,
            //                        Value = user.Id.ToString(),
            //                        Group = new SelectListGroup() { Name = role.Name }
            //                    }).ToList();
            
            //return new NegotiatedResult("Index", vm);
            
        }

        public async Task<ActionResult> Today(Guid? serviceProviderId)
        {
            // Set date range variables used in where conditions
            var now = SystemTime.Now();
            var loggedInUserId = User.Identity.GetGuidUserId();
            var baseUrl = Request.GetBaseUrl();

            Guid userId = User.Identity.GetGuidUserId();
            // Admins can see the Service Provider dropdown and view other's dashboards. Otherwise, it displays the data of the current user.
            if (User.Identity.IsAdmin() && serviceProviderId.HasValue)
            {
                userId = serviceProviderId.Value;
            }

            var requests = await context.GetAssignedServiceRequestsAsync(userId, now, false, null);

            // Populate the view model
            var vm = new dvm.IndexViewModel();

            vm.Today = ServiceRequestMapper.MapToToday(requests, now, userId, baseUrl);
            
            // Additional view data.
            vm.SelectedUserId = userId;
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

            return new NegotiatedResult("Today", vm);
        }

        public async Task<ActionResult> DueDates(Guid? serviceProviderId)
        {
            // Set date range variables used in where conditions
            var now = SystemTime.Now();
            var loggedInUserId = User.Identity.GetGuidUserId();
            var baseUrl = Request.GetBaseUrl();

            Guid? userId = User.Identity.GetGuidUserId();
            // Admins can see the Service Provider dropdown and view other's dashboards. Otherwise, it displays the data of the current user.
            if (User.Identity.IsAdmin() && serviceProviderId.HasValue)
            {
                userId = serviceProviderId.Value;
            }

            var requests = await context.GetAssignedServiceRequestsAsync(userId, now, false, null);

            // Populate the view model
            var vm = new dvm.IndexViewModel();

            vm.DueDates = ServiceRequestMapper.MapToDueDates(requests, now, loggedInUserId, baseUrl);

            // Additional view data.
            vm.SelectedUserId = userId;
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

            return new NegotiatedResult("DueDates", vm);
        }

        public async Task<ActionResult> Schedule(Guid? serviceProviderId)
        {
            // Set date range variables used in where conditions
            var now = SystemTime.Now();
            var loggedInUserId = User.Identity.GetGuidUserId();
            var baseUrl = Request.GetBaseUrl();

            Guid userId = User.Identity.GetGuidUserId();
            // Admins can see the Service Provider dropdown and view other's dashboards. Otherwise, it displays the data of the current user.
            if (User.Identity.IsAdmin() && serviceProviderId.HasValue)
            {
                userId = serviceProviderId.Value;
            }

            var requests = await context.GetAssignedServiceRequestsAsync(userId, now, false, null);

            // Populate the view model
            var vm = new dvm.IndexViewModel();

            vm.WeekFolders = ServiceRequestMapper.MapToWeekFolders(requests, now, userId, baseUrl);

            // Additional view data.
            vm.SelectedUserId = userId;
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

            return new NegotiatedResult("Schedule", vm);
        }

        public async Task<ActionResult> Additionals(Guid? serviceProviderId)
        {
            // Set date range variables used in where conditions
            var now = SystemTime.Now();
            var loggedInUserId = User.Identity.GetGuidUserId();
            var baseUrl = Request.GetBaseUrl();

            Guid userId = User.Identity.GetGuidUserId();
            // Admins can see the Service Provider dropdown and view other's dashboards. Otherwise, it displays the data of the current user.
            if (User.Identity.IsAdmin() && serviceProviderId.HasValue)
            {
                userId = serviceProviderId.Value;
            }

            var requests = await context.GetAssignedServiceRequestsAsync(userId, now, false, null);

            // Populate the view model
            var vm = new dvm.IndexViewModel();

            vm.AddOns = ServiceRequestMapper.MapToAddOns(requests, now, userId, baseUrl);

            // Additional view data.
            vm.SelectedUserId = userId;
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

            return new NegotiatedResult("Additionals", vm);
        }

        public async Task<ActionResult> DueDates2(Guid? serviceProviderId)
        {
            // Set date range variables used in where conditions
            var now = SystemTime.Now();
            var loggedInUserId = User.Identity.GetGuidUserId();
            var baseUrl = Request.GetBaseUrl();

            Guid userId = User.Identity.GetGuidUserId();
            // Admins can see the Service Provider dropdown and view other's dashboards. Otherwise, it displays the data of the current user.
            if (User.Identity.IsAdmin() && serviceProviderId.HasValue)
            {
                userId = serviceProviderId.Value;
            }

            // Populate the view model
            var vm = new dvm.IndexViewModel();

            vm.DueDates2 = Models.ServiceRequestModels2.ServiceRequestMapper2.MapToDueDates(userId, now, baseUrl);

            // Additional view data.
            vm.SelectedUserId = userId;
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

            return new NegotiatedResult("DueDates2", vm);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateTaskStatus2(int taskId, bool isChecked, Guid? serviceProviderGuid, bool includeSummaries = false, bool isAddOn = false)
        {
            var now = SystemTime.Now();
            var loggedInUserId = User.Identity.GetGuidUserId();
            var baseUrl = Request.GetBaseUrl();

            var serviceRequestTask = await context.ServiceRequestTasks.FindAsync(taskId);
            if (serviceRequestTask == null)
            {
                return HttpNotFound();
            }
            serviceRequestTask.CompletedDate = isChecked ? SystemTime.Now() : (DateTime?)null;
            serviceRequestTask.CompletedBy = isChecked ? User.Identity.GetGuidUserId() : (Guid?)null;
            serviceRequestTask.ModifiedDate = SystemTime.Now();
            serviceRequestTask.ModifiedUser = User.Identity.Name;
            serviceRequestTask.ServiceRequest.UpdateIsClosed();
            await context.SaveChangesAsync();

            if (includeSummaries)
            {
                var request = await context.GetAssignedServiceRequestsAsync(serviceRequestTask.AssignedTo, now, false, null);
                var result = ServiceRequestMapper.MapToWeekFolders(request, now, loggedInUserId, baseUrl);
                return Json(new { WeekFolders = result });
            }
            else if (isAddOn)
            {
                //var request = await context.GetAssignedServiceRequestsAsync(serviceRequestTask.AssignedTo, now, false, serviceRequestTask.ServiceRequestId);
                var result = Models.ServiceRequestModels2.ServiceRequestMapper2.MapToServiceRequest(serviceRequestTask.ServiceRequestId, now, loggedInUserId, baseUrl);
                return Json(result);
            }
            else
            {
                var result = Models.ServiceRequestModels2.ServiceRequestMapper2.MapToServiceRequest(serviceRequestTask.ServiceRequestId, now, loggedInUserId, baseUrl);
                return Json(result);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateTaskStatus(int taskId, bool isChecked, Guid? serviceProviderGuid, bool includeSummaries = false, bool isAddOn = false)
        {
            var now = SystemTime.Now();
            var loggedInUserId = User.Identity.GetGuidUserId();
            var baseUrl = Request.GetBaseUrl();

            var serviceRequestTask = await context.ServiceRequestTasks.FindAsync(taskId);
            if (serviceRequestTask == null)
            {
                return HttpNotFound();
            }
            serviceRequestTask.CompletedDate = isChecked ? SystemTime.Now() : (DateTime?)null;
            serviceRequestTask.CompletedBy = isChecked ? User.Identity.GetGuidUserId() : (Guid?)null;
            serviceRequestTask.ModifiedDate = SystemTime.Now();
            serviceRequestTask.ModifiedUser = User.Identity.Name;
            serviceRequestTask.ServiceRequest.UpdateIsClosed();
            await context.SaveChangesAsync();

            if (includeSummaries)
            {
                var request = await context.GetAssignedServiceRequestsAsync(serviceRequestTask.AssignedTo, now, false, null);
                var result = ServiceRequestMapper.MapToWeekFolders(request, now, loggedInUserId, baseUrl);
                return Json(new { WeekFolders = result });
            }
            else if (isAddOn)
            {
                var request = await context.GetAssignedServiceRequestsAsync(serviceRequestTask.AssignedTo, now, false, serviceRequestTask.ServiceRequestId);
                var result = ServiceRequestMapper.MapToAddOn(request, now, loggedInUserId, baseUrl);
                return Json(result);
            }
            else
            {
                var request = await context.GetAssignedServiceRequestsAsync(serviceRequestTask.AssignedTo, now, false, serviceRequestTask.ServiceRequestId);
                var result = ServiceRequestMapper.MapToAssessment(request, now, loggedInUserId, baseUrl);
                return Json(result);
            }
        }

        [HttpPost]
        public async Task<ActionResult> ToggleNoShow(int serviceRequestId, bool isChecked, Guid? serviceProviderGuid)
        {
            var serviceRequest = await context.ServiceRequests.FindAsync(serviceRequestId);
            if (serviceRequest == null)
            {
                return HttpNotFound();
            }

            serviceRequest.IsNoShow = isChecked;

            if (isChecked)
            {
                serviceRequest.MarkActiveTasksAsObsolete();
            }
            else
            {
                serviceRequest.MarkObsoleteTasksAsActive();
            }

            serviceRequest.UpdateIsClosed();

            serviceRequest.UpdateInvoice(context);

            await context.SaveChangesAsync();

            return await Index(serviceProviderGuid);
        }

        [HttpGet]
        public async Task<ActionResult> TaskHierarchy(int serviceRequestId, int? taskId = null)
        {
            var now = SystemTime.Now();
            Guid? userId = User.Identity.GetGuidUserId();

            var requests = await context.GetAssignedServiceRequestsAsync(null, now, null, serviceRequestId);

            //requests = requests.Where(o => o.ResponsibleRoleId != Roles.CaseCoordinator || o.TaskId == Tasks.SaveMedBrief).ToList();
            //requests = requests.Where(c => c.TaskStatusId == TaskStatuses.Waiting || c.TaskStatusId == TaskStatuses.ToDo).ToList();

            var vm = new dvm.TaskListViewModel(requests, taskId, User.Identity.GetRoleId());

            return PartialView("_TaskHierarchy", vm);
        }

        [HttpGet]
        public async Task<ActionResult> TaskList(int serviceRequestId)
        {
            var now = SystemTime.Now();
            Guid? userId = User.Identity.GetGuidUserId();

            var requests = await context.GetServiceRequestAsync(serviceRequestId, now);
            var assessment = new Assessment
            {
                ClaimantName = requests.First().ClaimantName,
                Tasks = from o in requests
                        //where o.ResponsibleRoleId != Roles.CaseCoordinator || o.TaskId == Tasks.SaveMedBrief
                        orderby o.TaskSequence
                        select new ServiceRequestTask
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

        [HttpGet]
        public async Task<ActionResult> Discussion(int serviceRequestId)
        {
            var now = SystemTime.Now();

            var requests = await context.GetServiceRequestAsync(serviceRequestId, now);
            var assessment = new Assessment
            {
                Id = requests.First().Id,
                ClaimantName = requests.First().ClaimantName
            };

            return PartialView("_DiscussionModal", assessment);
        }


    }

}