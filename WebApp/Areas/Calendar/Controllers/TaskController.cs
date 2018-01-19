using LinqKit;
using Orvosi.Data;
using Orvosi.Data.Filters;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebApp.Controllers;
using WebApp.Library;
using WebApp.Library.Filters;
using WebApp.Models;
using WebApp.ViewDataModels;
using WebApp.ViewModels;
using WebApp.ViewModels.CalendarViewModels;
using Features = Orvosi.Shared.Enums.Features;

namespace WebApp.Areas.Calendar.Controllers
{
    public class TaskController : BaseController
    {
        private OrvosiDbContext db;
        private WorkService service;

        public TaskController(OrvosiDbContext db, WorkService service, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
            this.service = service;
        }

        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public PartialViewResult GetByDay(DateTime day)
        {
            // Set date range variables used in where conditions
            var dto = db.ServiceRequests
                .AsExpandable()
                .AreScheduledThisDay(day)
                .AreNotCancellations()
                .CanAccess(loggedInUserId, physicianId, loggedInRoleId)
                .Select(ServiceRequestDto.FromServiceRequestEntityForCaseLinks(loggedInUserId))
                .OrderBy(sr => sr.AppointmentDate).ThenBy(sr => sr.StartTime)
                .ToList();

            var caseViewModels = dto.AsQueryable()
                .Select(CaseLinkViewModel.FromServiceRequestDto.Expand());

            var dayViewModel = caseViewModels
                .GroupBy(c => c.AppointmentDate.Value)
                .AsQueryable()
                .Select(DayViewModel.FromServiceRequestDtoGroupingDtoForCaseLinks.Expand())
                .SingleOrDefault();
            
            return PartialView(dayViewModel);
        }

        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public PartialViewResult GetAssessment(int serviceRequestId)
        {
            var dto = db.ServiceRequests
                .WithId(serviceRequestId)
                .CanAccess(loggedInUserId, physicianId, loggedInRoleId)
                .Select(ServiceRequestDto.FromServiceRequestEntityV2(loggedInUserId))
                .SingleOrDefault();

            if (dto == null)
            {
                return PartialView("Unauthorized");
            }

            var viewModel = CaseViewModel.FromServiceRequestDto.Invoke(dto);

            var args = new TaskListArgs
            {
                ServiceRequestId = serviceRequestId,
                ViewTarget = ViewTarget.Details,
                ViewFilter = TaskListViewModelFilter.AllTasks
            };

            if (loggedInRoleId == AspNetRoles.SuperAdmin || loggedInRoleId == AspNetRoles.Physician || loggedInRoleId == AspNetRoles.CaseCoordinator)
            {
                args.ViewFilter = TaskListViewModelFilter.AllTasks;
            }
            else
            {
                args.ViewFilter = TaskListViewModelFilter.CriticalPathOrAssignedToUser;
            }
            ViewData.TaskListArgs_Set(args);

            ViewData.ViewTarget_Set(args.ViewTarget);
            ViewData.ViewFilter_Set(args.ViewFilter);

            return PartialView("~/Views/ServiceRequest/_Details.cshtml", viewModel);
        }
    }
}