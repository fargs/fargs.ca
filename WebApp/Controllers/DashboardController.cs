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
using WebApp.ViewModels.ServiceRequestViewModels;
using System.Net;
using System.Data.Entity;
using Orvosi.Data.Filters;

namespace WebApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private Orvosi.Data.OrvosiDbContext context = new Orvosi.Data.OrvosiDbContext();

        public ActionResult Index(Guid? serviceProviderId, bool showClosed = false, bool onlyMine = true)
        {
            return RedirectToAction("Agenda");
        }

        public async Task<ActionResult> Agenda(Guid? serviceProviderId, DateTime? day, int? serviceRequestId = null)
        {
            // Set date range variables used in where conditions

            var now = SystemTime.UtcNow().ToLocalTimeZone(TimeZones.EasternStandardTime);
            var dayOrDefault = GetDayOrDefault(day);
            var userId = GetServiceProviderIdOrDefault(serviceProviderId);

            var loggedInUserId = User.Identity.GetGuidUserId();
            var baseUrl = Request.GetBaseUrl();

            // a list of responsible roles that is used to filter out the task list
            var rolesThatShouldBeSeen = new Guid?[3] { AspNetRoles.Physician, AspNetRoles.IntakeAssistant, AspNetRoles.DocumentReviewer };

            var query = context.ServiceRequests
                .AreAssignedToUser(userId)
                .AreScheduledThisDay(dayOrDefault.Date)
                .Where(sr => !sr.CancelledDate.HasValue);

            // either return a refreshed view of a single request or return the list of all requests grouped by day
            if (serviceRequestId.HasValue) query = query.Where(s => s.Id == serviceRequestId);

            var result = query.Select(sr => new Orvosi.Shared.Model.ServiceRequest
            {
                Id = sr.Id,
                ClaimantName = sr.ClaimantName,
                DueDate = sr.DueDate,
                AppointmentDate = sr.AppointmentDate,
                Now = now,
                StartTime = sr.StartTime,
                CancelledDate = sr.CancelledDate,
                IsClosed = sr.IsClosed,
                Notes = sr.Notes,
                BoxCaseFolderId = sr.BoxCaseFolderId,
                PhysicianId = sr.PhysicianId,
                Address = new Orvosi.Shared.Model.Address
                {
                    Id = sr.Address.Id,
                    Name = sr.Address.Name,
                    City = sr.Address.City_CityId.Name,
                    ProvinceCode = sr.Address.Province.ProvinceCode,
                    TimeZone = sr.Address.TimeZone
                },
                Company = new Orvosi.Shared.Model.Company
                {
                    Id = sr.Company.Id,
                    Name = sr.Company.Name
                },
                Service = new Orvosi.Shared.Model.Service
                {
                    Id = sr.Service.Id,
                    Name = sr.Service.Name,
                    Code = sr.Service.Code
                },
                ServiceRequestMessages = sr.ServiceRequestMessages.OrderBy(srm => srm.PostedDate).Select(srm => new Orvosi.Shared.Model.ServiceRequestMessage
                {
                    Id = srm.Id,
                    TimeZone = srm.ServiceRequest.Address == null ? TimeZones.EasternStandardTime : srm.ServiceRequest.Address.TimeZone,
                    Message = srm.Message,
                    PostedDate = srm.PostedDate,
                    PostedBy = new Orvosi.Shared.Model.Person
                    {
                        Id = srm.AspNetUser.Id,
                        Title = srm.AspNetUser.Title,
                        FirstName = srm.AspNetUser.FirstName,
                        LastName = srm.AspNetUser.LastName,
                        Email = srm.AspNetUser.Email,
                        ColorCode = srm.AspNetUser.ColorCode,
                        Role = srm.AspNetUser.AspNetUserRoles.Select(r => new Orvosi.Shared.Model.UserRole
                        {
                            Id = r.AspNetRole.Id,
                            Name = r.AspNetRole.Name
                        }).FirstOrDefault()
                    }
                }),
                ServiceRequestTasks = sr.ServiceRequestTasks
                    .Where(srt => srt.AssignedTo == userId || rolesThatShouldBeSeen.Contains(srt.ResponsibleRoleId))
                    .OrderBy(srt => srt.Sequence)
                    .Select(t => new Orvosi.Shared.Model.ServiceRequestTask
                    {
                        Id = t.Id,
                        ServiceRequestId = t.ServiceRequestId,
                        AppointmentDate = sr.AppointmentDate,
                        DueDate = t.DueDate,
                        Now = now,
                        ProcessTask = new Orvosi.Shared.Model.ProcessTask
                        {
                            Id = t.OTask.Id,
                            Name = t.OTask.Name,
                            Sequence = t.OTask.Sequence.Value,
                            ResponsibleRole = t.OTask.AspNetRole == null ? null : new Orvosi.Shared.Model.UserRole
                            {
                                Id = t.OTask.AspNetRole.Id,
                                Name = t.OTask.AspNetRole.Name
                            }
                        },
                        AssignedTo = new Orvosi.Shared.Model.Person
                        {
                            Id = t.AspNetUser_AssignedTo == null ? (Guid?)null : t.AspNetUser_AssignedTo.Id,
                            Title = t.AspNetUser_AssignedTo.Title,
                            FirstName = t.AspNetUser_AssignedTo.FirstName,
                            LastName = t.AspNetUser_AssignedTo.LastName,
                            Email = t.AspNetUser_AssignedTo.Email,
                            ColorCode = t.AspNetUser_AssignedTo.ColorCode,
                            Role = t.AspNetUser_AssignedTo.AspNetUserRoles.Select(r => new Orvosi.Shared.Model.UserRole
                            {
                                Id = r.AspNetRole.Id,
                                Name = r.AspNetRole.Name
                            }).FirstOrDefault()
                        },
                        CompletedBy = new Orvosi.Shared.Model.Person
                        {
                            Id = t.AspNetUser_CompletedBy == null ? (Guid?)null : t.AspNetUser_CompletedBy.Id,
                            Title = t.AspNetUser_CompletedBy.Title,
                            FirstName = t.AspNetUser_CompletedBy.FirstName,
                            LastName = t.AspNetUser_CompletedBy.LastName,
                            Email = t.AspNetUser_CompletedBy.Email,
                            ColorCode = t.AspNetUser_CompletedBy.ColorCode,
                            Role = t.AspNetUser_CompletedBy.AspNetUserRoles.Select(r => new Orvosi.Shared.Model.UserRole
                            {
                                Id = r.AspNetRole.Id,
                                Name = r.AspNetRole.Name
                            }).FirstOrDefault()
                        },
                        CompletedDate = t.CompletedDate,
                        IsObsolete = t.IsObsolete,
                        Dependencies = t.Child.Select(c => new Orvosi.Shared.Model.ServiceRequestTaskDependent
                        {
                            TaskId = c.TaskId.Value,
                            CompletedDate = c.CompletedDate,
                            IsObsolete = c.IsObsolete
                        })
                    })
            }).ToList();

            // SHORT CIRCUIT: we have what we need to refresh a single request so return it. This is expected to be called via an ajax call
            if (serviceRequestId.HasValue && Request.IsAjaxRequest()) return PartialView("_ServiceRequestTasks_Today", result.First());

            // Populate the view model
            var vm = new ViewModels.DashboardViewModels.AgendaViewModel();

            // group into hierarchy
            vm.DayFolder = (from sr in result
                            group sr by sr.AppointmentDate.Value into days
                            select new Orvosi.Shared.Model.DayFolder
                            {
                                DayAndTime = days.Key,
                                Address = days.First().Address,
                                ServiceRequests = days
                            }).FirstOrDefault();

            // This is used to join the discussion rooms
            var serviceRequestIds = result
                                .Select(srt => srt.Id)
                                .Distinct()
                                .ToArray();

            vm.Day = dayOrDefault.Date;
            vm.ServiceRequestMessageJSViewModel = new ViewModels.ServiceRequestMessageJSViewModel() { QueryString = Request.QueryString, ServiceRequestIds = serviceRequestIds };
            vm.SelectedUserId = userId;
            vm.UserSelectList = await (from user in context.AspNetUsers
                                       from userRole in context.AspNetUserRoles
                                       from role in context.AspNetRoles
                                       where user.Id == userRole.UserId && role.Id == userRole.RoleId
                                       select new SelectListItem
                                       {
                                           Text = user.FirstName + " " + user.LastName,
                                           Value = user.Id.ToString(),
                                           Group = new SelectListGroup() { Name = role.Name }
                                       }).ToListAsync();

            return new NegotiatedResult("Agenda", vm);
        }

        public async Task<ActionResult> DueDates(Guid? serviceProviderId, short[] selectedTaskTypes = null, int? serviceRequestId = null)
        {
            // Set date range variables used in where conditions
            var now = SystemTime.UtcNow();
            var loggedInUserId = User.Identity.GetGuidUserId();
            var baseUrl = Request.GetBaseUrl();

            Guid userId = User.Identity.GetGuidUserId();
            // Admins can see the Service Provider dropdown and view other's dashboards. Otherwise, it displays the data of the current user.
            if (User.Identity.IsAdmin() && serviceProviderId.HasValue)
            {
                userId = serviceProviderId.Value;
            }

            // a list of responsible roles that is used to filter out the task list
            var rolesThatShouldBeSeen = new Guid?[3] { AspNetRoles.Physician, AspNetRoles.IntakeAssistant, AspNetRoles.DocumentReviewer };

            // get a list of service request Ids from the users active tasks
            // also passed to the view for signalr to create a discussion room for each request
            var serviceRequestIdsQuery = context.ServiceRequestTasks
                                .AreAssignedToUser(userId)
                                .AreActive();

            selectedTaskTypes = selectedTaskTypes == null ? new short[0] : selectedTaskTypes;
            if (selectedTaskTypes.Any()) serviceRequestIdsQuery = serviceRequestIdsQuery.Where(srt => selectedTaskTypes.Contains(srt.TaskId.Value));  // Cannot be refactored because the list is not an IQueryable <need to look into this>

            var serviceRequestIds = serviceRequestIdsQuery
                                .Select(srt => srt.ServiceRequestId)
                                .Distinct()
                                .ToArray();

            // get all the service requests where the user has an active task
            var source = context.ServiceRequests.AsQueryable();

            // either return a refreshed view of a single request or return the list of all requests grouped by day
            source = serviceRequestId.HasValue ?
                source.Where(s => s.Id == serviceRequestId) :
                source.Where(sr => serviceRequestIds.Contains(sr.Id));

            // retrieve the required information from the database and project it to the view model
            var result = source.Select(sr => new Orvosi.Shared.Model.ServiceRequest
            {
                Id = sr.Id,
                ClaimantName = sr.ClaimantName,
                DueDate = sr.DueDate,
                AppointmentDate = sr.AppointmentDate,
                Now = now,
                StartTime = sr.StartTime,
                CancelledDate = sr.CancelledDate,
                IsClosed = sr.IsClosed,
                Notes = sr.Notes,
                BoxCaseFolderId = sr.BoxCaseFolderId,
                PhysicianId = sr.PhysicianId,
                Company = new Orvosi.Shared.Model.Company
                {
                    Id = sr.Company.Id,
                    Name = sr.Company.Name
                },
                Service = new Orvosi.Shared.Model.Service
                {
                    Id = sr.Service.Id,
                    Name = sr.Service.Name,
                    Code = sr.Service.Code
                },
                ServiceRequestMessages = sr.ServiceRequestMessages.OrderBy(srm => srm.PostedDate).Select(srm => new Orvosi.Shared.Model.ServiceRequestMessage
                {
                    Id = srm.Id,
                    TimeZone = srm.ServiceRequest.Address == null ? TimeZones.EasternStandardTime : srm.ServiceRequest.Address.TimeZone,
                    Message = srm.Message,
                    PostedDate = srm.PostedDate,
                    PostedBy = new Orvosi.Shared.Model.Person
                    {
                        Id = srm.AspNetUser.Id,
                        Title = srm.AspNetUser.Title,
                        FirstName = srm.AspNetUser.FirstName,
                        LastName = srm.AspNetUser.LastName,
                        Email = srm.AspNetUser.Email,
                        ColorCode = srm.AspNetUser.ColorCode,
                        Role = srm.AspNetUser.AspNetUserRoles.Select(r => new Orvosi.Shared.Model.UserRole
                        {
                            Id = r.AspNetRole.Id,
                            Name = r.AspNetRole.Name
                        }).FirstOrDefault()
                    }
                }),
                ServiceRequestTasks = sr.ServiceRequestTasks
                    .Where(srt => srt.AssignedTo == userId || rolesThatShouldBeSeen.Contains(srt.ResponsibleRoleId))
                    //.Where(srt => srt.TaskId != Tasks.AssessmentDay)
                    .OrderBy(srt => srt.Sequence)
                    .Select(t => new Orvosi.Shared.Model.ServiceRequestTask
                    {
                        Id = t.Id,
                        ServiceRequestId = t.ServiceRequestId,
                        AppointmentDate = sr.AppointmentDate,
                        DueDate = t.DueDate,
                        Now = now,
                        ProcessTask = new Orvosi.Shared.Model.ProcessTask
                        {
                            Id = t.OTask.Id,
                            Name = t.OTask.Name,
                            Sequence = t.OTask.Sequence.Value,
                            ResponsibleRole = t.OTask.AspNetRole == null ? null : new Orvosi.Shared.Model.UserRole
                            {
                                Id = t.OTask.AspNetRole.Id,
                                Name = t.OTask.AspNetRole.Name
                            }
                        },
                        AssignedTo = new Orvosi.Shared.Model.Person
                        {
                            Id = t.AspNetUser_AssignedTo == null ? (Guid?)null : t.AspNetUser_AssignedTo.Id,
                            Title = t.AspNetUser_AssignedTo.Title,
                            FirstName = t.AspNetUser_AssignedTo.FirstName,
                            LastName = t.AspNetUser_AssignedTo.LastName,
                            Email = t.AspNetUser_AssignedTo.Email,
                            ColorCode = t.AspNetUser_AssignedTo.ColorCode,
                            Role = t.AspNetUser_AssignedTo.AspNetUserRoles.Select(r => new Orvosi.Shared.Model.UserRole
                            {
                                Id = r.AspNetRole.Id,
                                Name = r.AspNetRole.Name
                            }).FirstOrDefault()
                        },
                        CompletedBy = new Orvosi.Shared.Model.Person
                        {
                            Id = t.AspNetUser_CompletedBy == null ? (Guid?)null : t.AspNetUser_CompletedBy.Id,
                            Title = t.AspNetUser_CompletedBy.Title,
                            FirstName = t.AspNetUser_CompletedBy.FirstName,
                            LastName = t.AspNetUser_CompletedBy.LastName,
                            Email = t.AspNetUser_CompletedBy.Email,
                            ColorCode = t.AspNetUser_CompletedBy.ColorCode,
                            Role = t.AspNetUser_CompletedBy.AspNetUserRoles.Select(r => new Orvosi.Shared.Model.UserRole
                            {
                                Id = r.AspNetRole.Id,
                                Name = r.AspNetRole.Name
                            }).FirstOrDefault()
                        },
                        CompletedDate = t.CompletedDate,
                        IsObsolete = t.IsObsolete,
                        Dependencies = t.Child.Select(c => new Orvosi.Shared.Model.ServiceRequestTaskDependent
                        {
                            TaskId = c.TaskId.Value,
                            CompletedDate = c.CompletedDate,
                            IsObsolete = c.IsObsolete
                        })
                    })
            }).ToList();

            // we have what we need to refresh a single request so return it. This is expected to be called via an ajax call
            if (serviceRequestId.HasValue && Request.IsAjaxRequest()) return PartialView("_ServiceRequestTasks_Today", result.First());

            // otherwise create a view model and load the due dates page.
            var vm = new dvm.DueDatesViewModel();

            // group the results by day
            vm.DueDates = from s in result
                          group s by s.DueDate into days
                          select new Orvosi.Shared.Model.DueDateDayFolder
                          {
                              DayAndTime = days.Key,
                              Company = days.Min(c => c.Company.Name),
                              ServiceRequests = days
                          };

            // This page uses the TaskFilter component and must therefore supply the view model for it
            var taskFilters = context.ServiceRequestTasks
                                .AreAssignedToUser(userId)
                                .AreActive()
                                .Select(srt => new ViewModels.TaskIndexDto { Id = srt.TaskId, Name = srt.TaskName, Sequence = srt.OTask.Sequence })
                                .Distinct()
                                .OrderBy(srt => srt.Sequence.Value)
                                .ToList();

            vm.TaskFilterViewModel = new ViewModels.TaskFilterViewModel() { Tasks = taskFilters, SelectedTaskTypes = selectedTaskTypes, SelectedUserId = userId };

            // This page uses the Discussion component and must therefore supply the view model for it
            vm.ServiceRequestMessageJSViewModel = new ViewModels.ServiceRequestMessageJSViewModel() { QueryString = Request.QueryString, ServiceRequestIds = serviceRequestIds };
            vm.SelectedUserId = userId;
            vm.UserSelectList = await (from user in context.AspNetUsers
                                       from userRole in context.AspNetUserRoles
                                       from role in context.AspNetRoles
                                       where user.Id == userRole.UserId && role.Id == userRole.RoleId
                                       select new SelectListItem
                                       {
                                           Text = user.FirstName + " " + user.LastName,
                                           Value = user.Id.ToString(),
                                           Group = new SelectListGroup() { Name = role.Name }
                                       }).ToListAsync();

            // return the Due Dates view
            return PartialView("DueDates", vm);
        }

        public async Task<ActionResult> Schedule(Guid? serviceProviderId, short[] selectedTaskTypes = null, int? serviceRequestId = null)
        {
            var now = SystemTime.UtcNow();
            var loggedInUserId = User.Identity.GetGuidUserId();
            var baseUrl = Request.GetBaseUrl();

            Guid userId = User.Identity.GetGuidUserId();
            // Admins can see the Service Provider dropdown and view other's dashboards. Otherwise, it displays the data of the current user.
            if (User.Identity.IsAdmin() && serviceProviderId.HasValue)
            {
                userId = serviceProviderId.Value;
            }

            // a list of responsible roles that is used to filter out the task list
            var rolesThatShouldBeSeen = new Guid?[3] { AspNetRoles.Physician, AspNetRoles.IntakeAssistant, AspNetRoles.DocumentReviewer };

            // get a list of service request Ids from the users active tasks
            // also passed to the view for signalr to create a discussion room for each request
            var serviceRequestIdsQuery = context.ServiceRequestTasks
                                .AreAssignedToUser(userId)
                                .AreActive()
                                .Where(sr => sr.ServiceRequest.AppointmentDate.HasValue); 

            selectedTaskTypes = selectedTaskTypes == null ? new short[0] : selectedTaskTypes;
            if (selectedTaskTypes.Any()) serviceRequestIdsQuery = serviceRequestIdsQuery.Where(srt => selectedTaskTypes.Contains(srt.TaskId.Value));  // Cannot be refactored because the list is not an IQueryable <need to look into this>

            var serviceRequestIds = serviceRequestIdsQuery
                                .Select(srt => srt.ServiceRequestId)
                                .Distinct()
                                .ToArray();

            var source = context.ServiceRequests.AsQueryable();

            // either return a refreshed view of a single request or return the list of all requests grouped by day
            source = serviceRequestId.HasValue ?
                source.Where(s => s.Id == serviceRequestId) :
                source.Where(sr => serviceRequestIds.Contains(sr.Id));

            // order the results
            source = from s in source
                     orderby s.AppointmentDate, s.StartTime
                     select s;

            // retrieve the required information from the database and project it to the view model
            var result = source.Select(sr => new Orvosi.Shared.Model.ServiceRequest
            {
                Id = sr.Id,
                ClaimantName = sr.ClaimantName,
                DueDate = sr.DueDate,
                AppointmentDate = sr.AppointmentDate,
                Now = now,
                StartTime = sr.StartTime,
                CancelledDate = sr.CancelledDate,
                IsClosed = sr.IsClosed,
                Notes = sr.Notes,
                BoxCaseFolderId = sr.BoxCaseFolderId,
                PhysicianId = sr.PhysicianId,
                Address = new Orvosi.Shared.Model.Address
                {
                    Id = sr.Address.Id,
                    Name = sr.Address.Name,
                    City = sr.Address.City_CityId.Name,
                    ProvinceCode = sr.Address.Province.ProvinceCode,
                    TimeZone = sr.Address.TimeZone
                },
                Company = new Orvosi.Shared.Model.Company
                {
                    Id = sr.Company.Id,
                    Name = sr.Company.Name
                },
                Service = new Orvosi.Shared.Model.Service
                {
                    Id = sr.Service.Id,
                    Name = sr.Service.Name,
                    Code = sr.Service.Code
                },
                ServiceRequestMessages = sr.ServiceRequestMessages.OrderBy(srm => srm.PostedDate).Select(srm => new Orvosi.Shared.Model.ServiceRequestMessage
                {
                    Id = srm.Id,
                    TimeZone = srm.ServiceRequest.Address == null ? TimeZones.EasternStandardTime : srm.ServiceRequest.Address.TimeZone,
                    Message = srm.Message,
                    PostedDate = srm.PostedDate,
                    PostedBy = new Orvosi.Shared.Model.Person
                    {
                        Id = srm.AspNetUser.Id,
                        Title = srm.AspNetUser.Title,
                        FirstName = srm.AspNetUser.FirstName,
                        LastName = srm.AspNetUser.LastName,
                        Email = srm.AspNetUser.Email,
                        ColorCode = srm.AspNetUser.ColorCode,
                        Role = srm.AspNetUser.AspNetUserRoles.Select(r => new Orvosi.Shared.Model.UserRole
                        {
                            Id = r.AspNetRole.Id,
                            Name = r.AspNetRole.Name
                        }).FirstOrDefault()
                    }
                }),
                ServiceRequestTasks = sr.ServiceRequestTasks
                    .Where(srt => srt.AssignedTo == userId || rolesThatShouldBeSeen.Contains(srt.ResponsibleRoleId))
                    //.Where(srt => srt.TaskId != Tasks.AssessmentDay)
                    .OrderBy(srt => srt.Sequence)
                    .Select(t => new Orvosi.Shared.Model.ServiceRequestTask
                    {
                        Id = t.Id,
                        ServiceRequestId = t.ServiceRequestId,
                        AppointmentDate = sr.AppointmentDate,
                        DueDate = t.DueDate,
                        Now = now,
                        ProcessTask = new Orvosi.Shared.Model.ProcessTask
                        {
                            Id = t.OTask.Id,
                            Name = t.OTask.Name,
                            Sequence = t.OTask.Sequence.Value,
                            ResponsibleRole = t.OTask.AspNetRole == null ? null : new Orvosi.Shared.Model.UserRole
                            {
                                Id = t.OTask.AspNetRole.Id,
                                Name = t.OTask.AspNetRole.Name
                            }
                        },
                        AssignedTo = new Orvosi.Shared.Model.Person
                        {
                            Id = t.AspNetUser_AssignedTo == null ? (Guid?)null : t.AspNetUser_AssignedTo.Id,
                            Title = t.AspNetUser_AssignedTo.Title,
                            FirstName = t.AspNetUser_AssignedTo.FirstName,
                            LastName = t.AspNetUser_AssignedTo.LastName,
                            Email = t.AspNetUser_AssignedTo.Email,
                            ColorCode = t.AspNetUser_AssignedTo.ColorCode,
                            Role = t.AspNetUser_AssignedTo.AspNetUserRoles.Select(r => new Orvosi.Shared.Model.UserRole
                            {
                                Id = r.AspNetRole.Id,
                                Name = r.AspNetRole.Name
                            }).FirstOrDefault()
                        },
                        CompletedBy = new Orvosi.Shared.Model.Person
                        {
                            Id = t.AspNetUser_CompletedBy == null ? (Guid?)null : t.AspNetUser_CompletedBy.Id,
                            Title = t.AspNetUser_CompletedBy.Title,
                            FirstName = t.AspNetUser_CompletedBy.FirstName,
                            LastName = t.AspNetUser_CompletedBy.LastName,
                            Email = t.AspNetUser_CompletedBy.Email,
                            ColorCode = t.AspNetUser_CompletedBy.ColorCode,
                            Role = t.AspNetUser_CompletedBy.AspNetUserRoles.Select(r => new Orvosi.Shared.Model.UserRole
                            {
                                Id = r.AspNetRole.Id,
                                Name = r.AspNetRole.Name
                            }).FirstOrDefault()
                        },
                        CompletedDate = t.CompletedDate,
                        IsObsolete = t.IsObsolete,
                        Dependencies = t.Child.Select(c => new Orvosi.Shared.Model.ServiceRequestTaskDependent
                        {
                            TaskId = c.TaskId.Value,
                            CompletedDate = c.CompletedDate,
                            IsObsolete = c.IsObsolete
                        })
                    }),
                People = sr.ServiceRequestTasks
                    .Where(srt => srt.TaskId != Tasks.AssessmentDay)
                    .GroupBy(t => new Orvosi.Shared.Model.Person
                    {
                        Id = t.AspNetUser_AssignedTo == null ? (Guid?)null : t.AspNetUser_AssignedTo.Id,
                        Title = t.AspNetUser_AssignedTo.Title,
                        FirstName = t.AspNetUser_AssignedTo.FirstName,
                        LastName = t.AspNetUser_AssignedTo.LastName,
                        Email = t.AspNetUser_AssignedTo.Email,
                        ColorCode = t.AspNetUser_AssignedTo.ColorCode,
                        Role = t.AspNetUser_AssignedTo.AspNetUserRoles.Select(r => new Orvosi.Shared.Model.UserRole
                        {
                            Id = r.AspNetRole.Id,
                            Name = r.AspNetRole.Name
                        }).FirstOrDefault()
                    }).Select(group => group.Key)
            }).ToList();

            // we have what we need to refresh a single request so return it. This is expected to be called via an ajax call
            if (serviceRequestId.HasValue && Request.IsAjaxRequest()) return PartialView("_ServiceRequestTasks_Today", result.First());

            // otherwise create a view model and load the due dates page.
            var vm = new ViewModels.DashboardViewModels.ScheduleViewModel();

            // get the start of this and next week to highlight them in the list
            var startOfWeek = now.Date.GetStartOfWeek();
            var endOfWeek = now.Date.GetEndOfWeek();
            var startOfNextWeek = now.Date.GetStartOfNextWeek();
            var endOfNextWeek = now.Date.GetEndOfNextWeek();

            var firstDate = result.Min(a => a.AppointmentDate);
            var lastDate = result.Max(a => a.AppointmentDate);

            var weeks = firstDate.GetValueOrDefault(SystemTime.Now()).GetDateRangeTo(lastDate.GetValueOrDefault(SystemTime.Now()))
                .Select(w => new WeekFolder
                {
                    WeekFolderName = w.ToWeekFolderName(),
                    StartDate = w.GetStartOfWeekWithinMonth(),
                    EndDate = w.GetEndOfWeekWithinMonth()
                }).Distinct(new WeekFolderEquals());

            // group the results by week
            vm.WeekFolders = from w in weeks
                             from a in result
                             where a.AppointmentDate >= w.StartDate && a.AppointmentDate <= w.EndDate
                             group a by new { w.WeekFolderName, w.StartDate, w.EndDate } into wf
                             select new Orvosi.Shared.Model.WeekFolder
                             {
                                 WeekFolderName = wf.Key.WeekFolderName,
                                 StartDate = wf.Key.StartDate,
                                 EndDate = wf.Key.EndDate,
                                 DayFolders = from o in result
                                              where o.AppointmentDate >= wf.Key.StartDate && o.AppointmentDate <= wf.Key.EndDate
                                              group o by o.AppointmentDate into days
                                              select new Orvosi.Shared.Model.DayFolder
                                              {
                                                  DayAndTime = days.Key,
                                                  Address = days.First().Address,
                                                  Company = days.First().Company.Name,
                                                  ServiceRequests = days
                                              }
                             };
            // This page uses the TaskFilter component and must therefore supply the view model for it
            var taskFilters = context.ServiceRequestTasks
                                .AreAssignedToUser(userId)
                                .AreActive()
                                .Select(srt => new ViewModels.TaskIndexDto { Id = srt.TaskId, Name = srt.TaskName, Sequence = srt.OTask.Sequence })
                                .Distinct()
                                .OrderBy(srt => srt.Sequence.Value)
                                .ToList();

            vm.TaskFilterViewModel = new ViewModels.TaskFilterViewModel() { Tasks = taskFilters, SelectedTaskTypes = selectedTaskTypes, SelectedUserId = userId };

            // This page uses the Discussion component and must therefore supply the view model for it
            vm.ServiceRequestMessageJSViewModel = new ViewModels.ServiceRequestMessageJSViewModel() { QueryString = Request.QueryString, ServiceRequestIds = serviceRequestIds };
            vm.SelectedUserId = userId;
            vm.UserSelectList = await (from user in context.AspNetUsers
                                       from userRole in context.AspNetUserRoles
                                       from role in context.AspNetRoles
                                       where user.Id == userRole.UserId && role.Id == userRole.RoleId
                                       select new SelectListItem
                                       {
                                           Text = user.FirstName + " " + user.LastName,
                                           Value = user.Id.ToString(),
                                           Group = new SelectListGroup() { Name = role.Name }
                                       }).ToListAsync();

            // return the Due Dates view
            return PartialView("Schedule", vm);
        }

        public async Task<ActionResult> Additionals(Guid? serviceProviderId)
        {
            // Set date range variables used in where conditions
            var now = SystemTime.Now();
            var loggedInUserId = User.Identity.GetGuidUserId();
            var baseUrl = Request.GetBaseUrl();

            var serviceProviderIdOrDefault = GetServiceProviderIdOrDefault(serviceProviderId);

            var requests = await context.GetAssignedServiceRequestsAsync(serviceProviderIdOrDefault, now, false, null);

            // Populate the view model
            var vm = new dvm.IndexViewModel();

            vm.AddOns = ServiceRequestMapper.MapToAddOns(requests, now, serviceProviderIdOrDefault, baseUrl);

            // Additional view data.
            vm.SelectedUserId = serviceProviderIdOrDefault;
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

        public ActionResult RefreshServiceRequestTasks(int serviceRequestId, Guid serviceProviderId)
        {
            var now = SystemTime.UtcNow();
            // This should be updated to include only the data required to refresh the _ServiceRequestTasks_Today view. 
            // Currently the Agenda and DueDates page are using the ServiceRequestId parametere on the DueDates and Agenda action methods to get the data.
            var result = context.ServiceRequests.Where(sr => sr.Id == serviceRequestId)
                .Select(sr => new Orvosi.Shared.Model.ServiceRequest
                {
                    Id = sr.Id,
                    ClaimantName = sr.ClaimantName,
                    ServiceRequestTasks = sr.ServiceRequestTasks
                        .Where(srt => srt.AssignedTo == serviceProviderId)
                        .OrderBy(srt => srt.Sequence)
                        .Select(t => new Orvosi.Shared.Model.ServiceRequestTask
                        {
                            Id = t.Id,
                            ServiceRequestId = t.ServiceRequestId,
                            AppointmentDate = t.ServiceRequest.AppointmentDate,
                            DueDate = t.DueDate,
                            Now = now,
                            ProcessTask = new Orvosi.Shared.Model.ProcessTask
                            {
                                Id = t.OTask.Id,
                                Name = t.OTask.Name,
                                Sequence = t.OTask.Sequence.Value,
                                ResponsibleRole = t.OTask.AspNetRole == null ? null : new Orvosi.Shared.Model.UserRole
                                {
                                    Id = t.OTask.AspNetRole.Id,
                                    Name = t.OTask.AspNetRole.Name
                                }
                            },
                            AssignedTo = new Orvosi.Shared.Model.Person
                            {
                                Id = t.AspNetUser_AssignedTo == null ? (Guid?)null : t.AspNetUser_AssignedTo.Id,
                                Title = t.AspNetUser_AssignedTo.Title,
                                FirstName = t.AspNetUser_AssignedTo.FirstName,
                                LastName = t.AspNetUser_AssignedTo.LastName,
                                Email = t.AspNetUser_AssignedTo.Email,
                                ColorCode = t.AspNetUser_AssignedTo.ColorCode,
                                Role = t.AspNetUser_AssignedTo.AspNetUserRoles.Select(r => new Orvosi.Shared.Model.UserRole
                                {
                                    Id = r.AspNetRole.Id,
                                    Name = r.AspNetRole.Name
                                }).FirstOrDefault()
                            },
                            CompletedBy = new Orvosi.Shared.Model.Person
                            {
                                Id = t.AspNetUser_CompletedBy == null ? (Guid?)null : t.AspNetUser_CompletedBy.Id,
                                Title = t.AspNetUser_CompletedBy.Title,
                                FirstName = t.AspNetUser_CompletedBy.FirstName,
                                LastName = t.AspNetUser_CompletedBy.LastName,
                                Email = t.AspNetUser_CompletedBy.Email,
                                ColorCode = t.AspNetUser_CompletedBy.ColorCode,
                                Role = t.AspNetUser_CompletedBy.AspNetUserRoles.Select(r => new Orvosi.Shared.Model.UserRole
                                {
                                    Id = r.AspNetRole.Id,
                                    Name = r.AspNetRole.Name
                                }).FirstOrDefault()
                            },
                            CompletedDate = t.CompletedDate,
                            IsObsolete = t.IsObsolete,
                            Dependencies = t.Child.Select(c => new Orvosi.Shared.Model.ServiceRequestTaskDependent
                            {
                                TaskId = c.TaskId.Value,
                                CompletedDate = c.CompletedDate,
                                IsObsolete = c.IsObsolete
                            })
                        })
                }).Single();
            return PartialView("_ServiceRequestTasks_Today", result);
        }

        public ActionResult RefreshServiceStatus(int serviceRequestId)
        {
            var context = new Orvosi.Data.OrvosiDbContext();
            var request = context.ServiceRequests.Select(sr => new m.ServiceRequest
            {
                Id = sr.Id,
                IsLateCancellation = sr.IsLateCancellation,
                IsNoShow = sr.IsNoShow,
                CancelledDate = sr.CancelledDate
            })
            .First(sr => sr.Id == serviceRequestId);

            return PartialView("_ServiceStatus", request);
        }

        public ActionResult RefreshAgendaSummaryCount(Guid? serviceProviderId, DateTime? day)
        {
            var dayOrDefault = GetDayOrDefault(day);
            var serviceProviderIdOrDefault = GetServiceProviderIdOrDefault(serviceProviderId);

            var count = Models.ServiceRequestModels2.ServiceRequestMapper2.ScheduleThisDayCount(serviceProviderIdOrDefault, dayOrDefault);

            return PartialView("_SummaryCount", count);
        }

        public ActionResult RefreshDueDateSummaryCount(Guid? serviceProviderId)
        {
            var now = SystemTime.UtcNow().ToLocalTimeZone(TimeZones.EasternStandardTime);
            var serviceProviderIdOrDefault = GetServiceProviderIdOrDefault(serviceProviderId);

            var count = Models.ServiceRequestModels2.ServiceRequestMapper2.DueDatesCount(serviceProviderIdOrDefault, now);

            return PartialView("_SummaryCount", count);
        }

        public ActionResult RefreshScheduleSummaryCount(Guid? serviceProviderId)
        {
            var userId = GetServiceProviderIdOrDefault(serviceProviderId);
            
            var count = context.ServiceRequestTasks
                                .AreAssignedToUser(userId)
                                .AreActive()
                                .Where(srt => srt.ServiceRequest.AppointmentDate.HasValue)
                                .Select(srt => srt.ServiceRequestId)
                                .Distinct()
                                .Count();

            return PartialView("_SummaryCount", count);
        }

        public ActionResult RefreshAdditionalsSummaryCount(Guid? serviceProviderId)
        {
            var serviceProviderIdOrDefault = GetServiceProviderIdOrDefault(serviceProviderId);

            var count = Models.ServiceRequestModels2.ServiceRequestMapper2.AdditionalsCount(serviceProviderIdOrDefault);

            return PartialView("_SummaryCount", count);
        }

        private static DateTime GetDayOrDefault(DateTime? day)
        {
            return day.HasValue ? day.Value : SystemTime.UtcNow().ToLocalTimeZone(TimeZones.EasternStandardTime);
        }

        private Guid GetServiceProviderIdOrDefault(Guid? serviceProviderId)
        {
            return (User.Identity.IsAdmin() && serviceProviderId.HasValue) ? serviceProviderId.Value : User.Identity.GetGuidUserId();
        }

        public async Task<ActionResult> RefreshNote(int serviceRequestId)
        {
            var context = new Orvosi.Data.OrvosiDbContext();
            var note = await context.ServiceRequests.FindAsync(serviceRequestId);
            return PartialView("_Note", new NoteViewModel() { ServiceRequestId = note.Id, Note = note.Notes });
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
            serviceRequestTask.CompletedBy = isChecked ? loggedInUserId : (Guid?)null;
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
            return new HttpStatusCodeResult(HttpStatusCode.OK);

            //if (includeSummaries)
            //{
            //    var request = await context.GetAssignedServiceRequestsAsync(serviceRequestTask.AssignedTo, now, false, null);
            //    var result = ServiceRequestMapper.MapToWeekFolders(request, now, loggedInUserId, baseUrl);
            //    return Json(new { WeekFolders = result });
            //}
            //else if (isAddOn)
            //{
            //    var request = await context.GetAssignedServiceRequestsAsync(serviceRequestTask.AssignedTo, now, false, serviceRequestTask.ServiceRequestId);
            //    var result = ServiceRequestMapper.MapToAddOn(request, now, loggedInUserId, baseUrl);
            //    return Json(result);
            //}
            //else
            //{
            //    var request = await context.GetAssignedServiceRequestsAsync(serviceRequestTask.AssignedTo, now, false, serviceRequestTask.ServiceRequestId);
            //    var result = ServiceRequestMapper.MapToAssessment(request, now, loggedInUserId, baseUrl);
            //    return Json(result);
            //}
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

            return new HttpStatusCodeResult(HttpStatusCode.OK);
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