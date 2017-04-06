﻿using System;
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
using WebApp.Library.Filters;
using Features = Orvosi.Shared.Enums.Features;
using WebApp.Models;
using LinqKit;
using WebApp.ViewModels;
using WebApp.ViewModels.CalendarViewModels;
using WebApp.ViewDataModels;
using WebApp.ViewModels.ServiceRequestTaskViewModels;
using FluentDateTime;

namespace WebApp.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToAction("Agenda");
        }

        [AuthorizeRole(Feature = Features.Work.Agenda)]
        public ActionResult Agenda(DateTime? selectedDate)
        {
            ViewData["selectedDate"] = selectedDate.GetValueOrDefault(SystemTime.Now()).ToOrvosiDateFormat();
            return View();
        }

        [AuthorizeRole(Feature = Features.Work.DueDates)]
        public async Task<ActionResult> DueDates(TaskListArgs args)
        {
            args.ViewTarget = ViewTarget.DueDates;
            args.ViewFilter = TaskListViewModelFilter.MyActiveTasks;
            
            return View(args);
        }

        [AuthorizeRole(Feature = Features.Work.Schedule)]
        public async Task<ActionResult> Schedule(TaskListArgs args)
        {
            var data = db.ServiceRequests
                .CanAccess(userId, physicianId, roleId)
                .AreNotClosed()
                .HaveAppointment()
                .AreNotCancellations()
                .Select(ServiceRequestDto.FromServiceRequestEntityForSchedule(userId))
                .ToList();

            var dayViewModels = data
                .GroupBy(srt => srt.AppointmentDate.Value)
                .Select(c => new DayViewModel
                {
                    Day = c.Key,
                    TaskStatusSummary = new dvm.TaskStatusSummaryViewModel
                    {
                        Count = c.Count(),
                        ToDoCount = c.Count(sr => sr.NextTaskStatusForUser == null ? false : sr.NextTaskStatusForUser.Id == TaskStatuses.ToDo),
                        WaitingCount = c.Count(sr => sr.NextTaskStatusForUser == null ? false : sr.NextTaskStatusForUser.Id == TaskStatuses.Waiting),
                        DoneCount = c.Count(sr => sr.NextTaskStatusForUser == null ? false : sr.NextTaskStatusForUser.Id == TaskStatuses.Done)
                    }
                })
                .ToList();

            var viewModel =
                from day in dayViewModels
                let week = day.Day.FirstDayOfWeek()
                group new { day } by week into weekGrp
                orderby weekGrp.Key
                select new DayViewModel
                {
                    Day = weekGrp.Key,
                    TaskStatusSummary = new dvm.TaskStatusSummaryViewModel
                    {
                        Count = weekGrp.Sum(sr => sr.day.TaskStatusSummary.Count),
                        ToDoCount = weekGrp.Sum(sr => sr.day.TaskStatusSummary.ToDoCount),
                        WaitingCount = weekGrp.Sum(sr => sr.day.TaskStatusSummary.WaitingCount),
                        DoneCount = weekGrp.Sum(sr => sr.day.TaskStatusSummary.OnHoldCount)
                    }
                };

            ViewData.TaskListArgs_Set(args);

            // otherwise create a view model and load the due dates page.
            var vm = new ViewModels.DashboardViewModels.ScheduleViewModel();
            vm.WeekFolders = viewModel;

            // return the Due Dates view
            return PartialView("Schedule", vm);
        }

        [AuthorizeRole(Feature = Features.Work.Additionals)]
        public ActionResult Additionals()
        {
            return View();   
        }



        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
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

        [AuthorizeRole(Feature = Features.Work.Agenda)]
        public ActionResult RefreshAgendaSummaryCount(Guid? serviceProviderId, DateTime? day)
        {
            var dayOrDefault = GetDayOrDefault(day);
            var serviceProviderIdOrDefault = User.Identity.GetUserContext().Id;

            var count = Models.ServiceRequestModels2.ServiceRequestMapper2.ScheduleThisDayCount(serviceProviderIdOrDefault, dayOrDefault);

            return PartialView("_SummaryCount", count);
        }

        [AuthorizeRole(Feature = Features.Work.DueDates)]
        public ActionResult RefreshDueDateSummaryCount(Guid? serviceProviderId)
        {
            var now = SystemTime.UtcNow().ToLocalTimeZone(TimeZones.EasternStandardTime);
            var serviceProviderIdOrDefault = User.Identity.GetUserContext().Id;

            var count = Models.ServiceRequestModels2.ServiceRequestMapper2.DueDatesCount(serviceProviderIdOrDefault, now);

            return PartialView("_SummaryCount", count);
        }

        [AuthorizeRole(Feature = Features.Work.Schedule)]
        public ActionResult RefreshScheduleSummaryCount(Guid? serviceProviderId)
        {
            Guid userId = User.Identity.GetUserContext().Id;

            var count = db.ServiceRequestTasks
                                .AreAssignedToUser(userId)
                                .AreActive()
                                .Where(srt => srt.ServiceRequest.AppointmentDate.HasValue)
                                .Select(srt => srt.ServiceRequestId)
                                .Distinct()
                                .Count();

            return PartialView("_SummaryCount", count);
        }

        [AuthorizeRole(Feature = Features.Work.Additionals)]
        public ActionResult RefreshAdditionalsSummaryCount(Guid? serviceProviderId)
        {
            var serviceProviderIdOrDefault = User.Identity.GetUserContext().Id;

            var count = Models.ServiceRequestModels2.ServiceRequestMapper2.AdditionalsCount(serviceProviderIdOrDefault);

            return PartialView("_SummaryCount", count);
        }

        [AuthorizeRole(Feature = Features.ServiceRequest.ViewInvoiceNote)]
        public async Task<ActionResult> RefreshNote(int serviceRequestId)
        {
            var context = new Orvosi.Data.OrvosiDbContext();
            var note = await context.ServiceRequests.FindAsync(serviceRequestId);
            return PartialView("_Note", new NoteViewModel() { ServiceRequestId = note.Id, Note = note.Notes });
        }



        //[HttpPost]
        //[AuthorizeRole(Feature = Features.ServiceRequest.ToggleNoShow)]
        //public async Task<ActionResult> ToggleNoShow(int serviceRequestId, bool isChecked, Guid? serviceProviderGuid)
        //{
            
        //    var serviceRequest = await context.ServiceRequests.FindAsync(serviceRequestId);
        //    if (serviceRequest == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    serviceRequest.IsNoShow = isChecked;

        //    if (isChecked)
        //    {
        //        serviceRequest.MarkActiveTasksAsObsolete();
        //    }
        //    else
        //    {
        //        serviceRequest.MarkObsoleteTasksAsActive();
        //    }

        //    serviceRequest.UpdateIsClosed();

        //    serviceRequest.UpdateInvoice(context);

        //    await context.SaveChangesAsync();

        //    return new HttpStatusCodeResult(HttpStatusCode.OK);
        //}



        [HttpGet]
        public async Task<ActionResult> TaskHierarchy(int serviceRequestId, int? taskId = null)
        {
            var now = SystemTime.Now();
            Guid? userId = User.Identity.GetGuidUserId();

            var requests = await db.GetAssignedServiceRequestsAsync(null, now, null, serviceRequestId);

            //requests = requests.Where(o => o.ResponsibleRoleId != Roles.CaseCoordinator || o.TaskId == Tasks.SaveMedBrief).ToList();
            //requests = requests.Where(c => c.TaskStatusId == TaskStatuses.Waiting || c.TaskStatusId == TaskStatuses.ToDo).ToList();

            var vm = new dvm.TaskListViewModel(requests, taskId, User.Identity.GetRoleId());

            return PartialView("_TaskHierarchy", vm);
        }


        [HttpGet]
        [AuthorizeRole(Feature = Features.ServiceRequest.LiveChat)]
        public async Task<ActionResult> Discussion(int serviceRequestId)
        {
            var now = SystemTime.Now();

            var requests = await db.GetServiceRequestAsync(serviceRequestId, now);
            var assessment = new Assessment
            {
                Id = requests.First().Id,
                ClaimantName = requests.First().ClaimantName
            };

            return PartialView("_DiscussionModal", assessment);
        }

        private static DateTime GetDayOrDefault(DateTime? day)
        {
            return day.HasValue ? day.Value : SystemTime.UtcNow().ToLocalTimeZone(TimeZones.EasternStandardTime);
        }
    }
}