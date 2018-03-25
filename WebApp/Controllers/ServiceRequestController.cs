using Box.V2.Models;
using FluentDateTime;
using LinqKit;
using MoreLinq;
using Orvosi.Data;
using Orvosi.Data.Filters;
using Orvosi.Shared.Enums;
using Orvosi.Shared.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.FormModels;
using WebApp.Library;
using WebApp.Library.Extensions;
using WebApp.Library.Filters;
using WebApp.Library.Projections;
using WebApp.ViewDataModels;
using WebApp.ViewModels;
using WebApp.ViewModels.CalendarViewModels;
using WebApp.ViewModels.ServiceRequestViewModels;
using e = Orvosi.Shared.Enums;
using Features = Orvosi.Shared.Enums.Features;
using m = WebApp.Models;

namespace WebApp.Controllers
{
    public delegate void NoShowToggledHandler(object sender, EventArgs e);

    [AuthorizeRole(Feature = Features.ServiceRequest.View)]
    public class ServiceRequestController : BaseController
    {
        private OrvosiDbContext db;
        private WorkService service;
        private ViewDataService viewDataService;

        public ServiceRequestController(OrvosiDbContext db, WorkService service, ViewDataService viewDataService, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
            this.service = service;
            this.viewDataService = viewDataService;
        }

        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public async Task<ViewResult> Index(FilterArgs filterArgs)
        {
            var vm = new IndexViewModel();
            // get the user
            filterArgs.ShowAll = (filterArgs.ShowAll ?? true);
            filterArgs.Sort = (filterArgs.Sort ?? "Oldest");
            filterArgs.StatusId = (filterArgs.StatusId ?? e.ServiceRequestStatus.Open);

            var sr = db.ServiceRequestViews.AsQueryable();

            if (!string.IsNullOrEmpty(filterArgs.Ids))
            {
                var ids = Array.ConvertAll(filterArgs.Ids.Split(','), s => int.Parse(s));
                sr = sr.Where(c => ids.Contains(c.Id));
            }
            else if (!string.IsNullOrEmpty(filterArgs.ClaimantName))
            {
                sr = sr.Where(c => c.ClaimantName.Contains(filterArgs.ClaimantName) || c.CompanyReferenceId.Contains(filterArgs.ClaimantName));
            }
            else if (filterArgs.StatusId.HasValue)
            {
                sr = sr.Where(c => c.ServiceRequestStatusId == filterArgs.StatusId);
            }

            // if the user is an administrator and the option showAll is true, then show all
            if (loggedInRoleId != AspNetRoles.SuperAdmin || (loggedInRoleId != AspNetRoles.SuperAdmin && filterArgs.ShowAll == false))
            {
                sr = sr.Where(c => c.CaseCoordinatorId == loggedInUserId || c.IntakeAssistantId == loggedInUserId || c.DocumentReviewerId == loggedInUserId || c.PhysicianId == loggedInUserId);
            }

            if (loggedInRoleId != AspNetRoles.SuperAdmin || loggedInRoleId != AspNetRoles.CaseCoordinator || loggedInRoleId != AspNetRoles.DocumentReviewer || loggedInRoleId != AspNetRoles.IntakeAssistant)
            {
                if (filterArgs.PhysicianId.HasValue)
                {
                    sr = sr.Where(c => c.PhysicianId == filterArgs.PhysicianId);
                }
            }

            if (filterArgs.Sort == "Newest")
            {
                sr = sr.OrderByDescending(c => c.AppointmentDate).ThenBy(c => c.StartTime.Value);
            }
            else
            {
                sr = sr.OrderBy(c => c.AppointmentDate).ThenBy(c => c.StartTime.Value);
            }

            // order the requests from oldest to newest
            vm.ServiceRequests = await sr.ToListAsync();

            var serviceRequestIds = vm.ServiceRequests.Select(item => item.Id);
            vm.ServiceRequestTasks = await db.ServiceRequestTasks
                    .Include(srt => srt.AspNetUser_AssignedTo)
                    .Include(srt => srt.OTask)
                .Where(srt => serviceRequestIds.Contains(srt.ServiceRequestId)
                    && srt.TaskId != Tasks.AssessmentDay)
                .ToListAsync();

            vm.FilterArgs = filterArgs;

            return View(vm);
        }

        public async Task<ViewResult> Dashboard()
        {
            var list = await db.ServiceRequests
                .Where(sr => sr.CaseCoordinatorId == loggedInUserId || sr.IntakeAssistantId == loggedInUserId || sr.DocumentReviewerId == loggedInUserId || sr.PhysicianId == loggedInUserId)
                .ToListAsync();

            return View(list);
        }

        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public PartialViewResult Details2(int serviceRequestId)
        {
            var dto =  db.ServiceRequests
                .WithId(serviceRequestId)
                .Select(m.ServiceRequestDto.FromServiceRequestEntity.Expand())
                .Single();

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

            return PartialView("Details", viewModel);
        }

        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public ViewResult Details(int id)
        {
            var dto = db.ServiceRequests
                .AsExpandable()
                .WithId(id)
                .CanAccess(loggedInUserId, physicianId, loggedInRoleId)
                .Select(m.ServiceRequestDto.FromServiceRequestEntityV2(loggedInUserId))
                //.Select(m.ServiceRequestDto.FromServiceRequestEntity.Expand())
                .SingleOrDefault();

            if (dto == null)
            {
                return View("Unauthorized");
            }

            var viewModel = CaseViewModel.FromServiceRequestDto.Invoke(dto);

            var args = new TaskListArgs
            {
                ServiceRequestId = id,
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

            return View(viewModel);
        }

        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public PartialViewResult Agenda(DateTime selectedDate, ViewTarget viewOptions = ViewTarget.Agenda)
        {
            // Set date range variables used in where conditions
            var dto = db.ServiceRequests
                .AsExpandable()
                .AreScheduledThisDay(selectedDate)
                .AreNotCancellations()
                .CanAccess(loggedInUserId, physicianId, loggedInRoleId)
                .Select(m.ServiceRequestDto.FromServiceRequestEntityForCaseV2(loggedInUserId))
                .OrderBy(sr => sr.AppointmentDate).ThenBy(sr => sr.StartTime)
                .ToList();

            var caseViewModels = dto.AsQueryable()
                .Select(CaseViewModel.FromServiceRequestDto.Expand());

            var dayViewModel = caseViewModels
                .GroupBy(c => c.AppointmentDate.Value)
                .AsQueryable()
                .Select(DayViewModel.FromServiceRequestDtoGroupingDtoForCases.Expand())
                .SingleOrDefault();

            ViewData.ViewTarget_Set(ViewTarget.Agenda);
            ViewData.ViewFilter_Set(TaskListViewModelFilter.CriticalPathOrAssignedToUser);

            return PartialView(dayViewModel);
        }

        public PartialViewResult GetInvoices(int serviceRequestId)
        {
            var invoiceIds = db.InvoiceDetails.Where(id => id.ServiceRequestId == serviceRequestId).Select(id => id.InvoiceId);
            var invoices = db.Invoices.Where(i => invoiceIds.Contains(i.Id)).Select(m.InvoiceDto.FromInvoiceEntity.Expand()).ToList();

            var viewModel = invoices.AsQueryable().Select(WebApp.ViewModels.InvoiceViewModels.InvoiceViewModel.FromInvoiceDto.Expand());

            return PartialView("InvoiceList", viewModel);
        }

        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        [HttpPost]
        public PartialViewResult DueDate(DueDateArgs args)//DateTime selectedDate, short[] selectedTaskTypes = null, CaseViewOptions viewOptions = CaseViewOptions.Agenda)
        {
            var dto = db.ServiceRequestTasks
                .AreDueBetween(args.TaskListArgs.DateRange.StartDate, args.TaskListArgs.DateRange.EndDate.Value)
                .AreAssignedToUser(loggedInUserId)
                .WithTaskIds(args.TaskListArgs.TaskIds)
                .AreActiveOrDone()
                .Where(srt => srt.DueDate.HasValue)
                .Select(srt => srt.ServiceRequestId)
                .Distinct()
                .ToList();

            var caseLinkArgs = dto.Select(sr => new CaseLinkArgs
            {
                ServiceRequestId = sr,
                ViewTarget = ViewTarget.DueDates
            });

            var taskListArgs = dto.Select(sr => new TaskListArgs
            {
                AssignedTo = args.TaskListArgs.AssignedTo,
                DateRange = args.TaskListArgs.DateRange,
                ViewTarget = args.ViewTarget, // Copy the default to pass into the task list
                ServiceRequestId = sr, // this is the service request id for each task list
                TaskIds = args.TaskListArgs.TaskIds,
                TaskStatusIds = args.TaskListArgs.TaskStatusIds,
                ViewFilter = args.TaskListArgs.ViewFilter
            });

            var viewModel = new DueDateViewModel
            {
                DueDateArgs = args,
                TaskListArgs = taskListArgs,
            };

            return PartialView(viewModel);
        }

        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public PartialViewResult ScheduleDateRange(TaskListArgs args)
        {
            var dto = db.ServiceRequests
                .AsExpandable()
                .AreScheduledBetween(args.DateRange.StartDate, args.DateRange.EndDate.Value)
                .CanAccess(loggedInUserId, physicianId, loggedInRoleId)
                .AreNotClosed()
                .HaveAppointment()
                .Select(m.ServiceRequestDto.FromServiceRequestEntityForCaseLinks(loggedInUserId))
                .ToList();

            var viewModel = dto
                .AsQueryable()
                .Select(CaseLinkViewModel.FromServiceRequestDto.Expand());

            var dayViewModel = viewModel
                .GroupBy(c => c.AppointmentDate.Value)
                .AsQueryable()
                .Select(DayViewModel.FromServiceRequestDtoGroupingDtoForCaseLinks.Expand());

            //var viewModel = dto.Select(sr => new TaskListArgs
            //{
            //    AssignedTo = args.AssignedTo,
            //    DateRange = args.DateRange,
            //    Options = args.Options,
            //    ServiceRequestId = sr,
            //    TaskIds = args.TaskIds,
            //    TaskStatusIds = args.TaskStatusIds,
            //    ViewOptions = args.ViewOptions
            //});

            return PartialView(dayViewModel);
        }

        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public PartialViewResult Additionals(TaskListArgs args)
        {
            var dto = db.ServiceRequests
                .AsExpandable()
                .CanAccess(loggedInUserId, physicianId, loggedInRoleId)
                .AreNotClosed()
                .HaveNoAppointment()
                .Select(m.ServiceRequestDto.FromServiceRequestEntityForCaseLinks(loggedInUserId))
                .ToList();

            var viewModel = dto
                .AsQueryable()
                .Select(CaseLinkViewModel.FromServiceRequestDto.Expand());

            //var dto = db.ServiceRequestTasks
            //    .AreAssignedToUser(userId)
            //    .WithTaskIds(args.TaskIds)
            //    .AreActive()
            //    .Where(srt => !srt.ServiceRequest.AppointmentDate.HasValue)
            //    .GroupBy(srt => srt.ServiceRequest)
            //    .Select(grp => grp.Key)
            //    .Select(m.ServiceRequestDto.FromServiceRequestEntityForCase.Expand())
            //    .ToList();

            //var viewModel = dto
            //    .AsQueryable()
            //    .Select(CaseLinkViewModel.FromServiceRequestDto.Expand());

            return PartialView(viewModel);
        }

        [HttpGet]
        public async Task<JsonResult> GetAppointmentDate(int serviceRequestId)
        {
            var result = await db.ServiceRequests
                .HaveAppointment()
                .WithId(serviceRequestId)
                .Select(sr => new
                {
                    sr.Id,
                    sr.AppointmentDate,
                })
                .SingleAsync();

            return Json(new
            {
                Id = result.Id,
                AppointmentDate = result.AppointmentDate.ToOrvosiDateFormat(),
                FirstDayOfWeek = result.AppointmentDate.Value.FirstDayOfWeek().ToOrvosiDateFormat(),
                FirstDayOfWeekTicks = result.AppointmentDate.Value.FirstDayOfWeek().Ticks
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult AdditionalCount()
        {
            var count = db.ServiceRequests
                .CanAccess(loggedInUserId, physicianId, loggedInRoleId)
                .AreNotClosed()
                .HaveNoAppointment()
                .Count();

            return PartialView("~/Views/Dashboard/_AdditionalsHeading.cshtml", count);
        }

        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public PartialViewResult CaseLink(CaseLinkArgs args)
        {
            var dto = db.ServiceRequests
                .WithId(args.ServiceRequestId)
                .Select(m.ServiceRequestDto.FromServiceRequestEntityForCase.Expand())
                .SingleOrDefault();

            if (dto == null)
            {
                return PartialView("Unauthorized");
            }

            var viewModel = CaseViewModel.FromServiceRequestDto.Invoke(dto);
            
            return PartialView(viewModel);
        }

        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public PartialViewResult Case(int serviceRequestId, ViewTarget viewOptions = ViewTarget.Details)
        {
            var dto = db.ServiceRequests
                .WithId(serviceRequestId)
                .CanAccess(loggedInUserId, physicianId, loggedInRoleId)
                .Select(m.ServiceRequestDto.FromServiceRequestEntityForCase.Expand())
                .SingleOrDefault();

            if (dto == null)
            {
                return PartialView("Unauthorized");
            }

            var viewModel = CaseViewModel.FromServiceRequestDto.Invoke(dto);

            ViewData.ViewTarget_Set(viewOptions);

            return PartialView(viewModel);
        }

        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public async Task<PartialViewResult> ActionMenu(int serviceRequestId)
        {
            var dto = await db.ServiceRequests
                .WithId(serviceRequestId)
                .CanAccess(loggedInUserId, physicianId, loggedInRoleId)
                .Select(m.ServiceRequestDto.FromServiceRequestEntityForCase.Expand())
                .SingleOrDefaultAsync();

            if (dto == null)
            {
                return PartialView("Unauthorized");
            }

            var viewModel = CaseViewModel.FromServiceRequestDto.Invoke(dto);

            return PartialView(viewModel);
        }

        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public async Task<PartialViewResult> AgendaActionMenu(int serviceRequestId)
        {
            var dto = await db.ServiceRequests
                .WithId(serviceRequestId)
                .CanAccess(loggedInUserId, physicianId, loggedInRoleId)
                .Select(m.ServiceRequestDto.FromServiceRequestEntityForCase.Expand())
                .SingleOrDefaultAsync();

            if (dto == null)
            {
                return PartialView("Unauthorized");
            }

            var viewModel = CaseViewModel.FromServiceRequestDto.Invoke(dto);

            return PartialView(viewModel);
        }

        [AuthorizeRole(Feature = Features.ServiceRequest.ChangeCompanyOrService)]
        public async Task<ActionResult> ShowChangeCompanyForm(int serviceRequestId)
        {
            var dto = await db.ServiceRequests.FindAsync(serviceRequestId);

            var form = new WebApp.FormModels.ChangeCompanyForm();
            form.ServiceRequestId = dto.Id;
            form.CompanyId = dto.CompanyId;
            form.PhysicianId = dto.PhysicianId;

            return PartialView("~/Views/ServiceRequest/Company/_CompanyModalForm.cshtml", form);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.ChangeCompanyOrService)]
        public async Task<ActionResult> ChangeCompany(ChangeCompanyForm form)
        {
            if (ModelState.IsValid)
            {
                //var response = await service.Reschedule(form);
                //response.AddToModelState(ModelState, null);
                await service.ChangeCompany(form);
                return Json(new
                {
                    serviceRequestId = form.ServiceRequestId
                });
            }
            Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            return PartialView("~/Views/ServiceRequest/Company/_CompanyModalForm.cshtml", form);
        }

        public async Task<PartialViewResult> ShowChangeServiceForm(int serviceRequestId)
        {
            var dto = await db.ServiceRequests.FindAsync(serviceRequestId);

            var form = new ChangeServiceForm();
            form.ServiceRequestId = dto.Id;
            form.ServiceId = dto.ServiceId;
            form.MedicolegalTypeId = dto.MedicolegalTypeId;
            form.PhysicianId = dto.PhysicianId;

            return PartialView("~/Views/ServiceRequest/Service/_ServiceModalForm.cshtml", form);
        }
        [HttpPost]
        public async Task<ActionResult> ChangeService(ChangeServiceForm form)
        {
            if (ModelState.IsValid)
            {
                //var response = await service.Reschedule(form);
                //response.AddToModelState(ModelState, null);
                await service.ChangeService(form);
                return Json(new
                {
                    serviceRequestId = form.ServiceRequestId
                });
            }
            Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            return PartialView("~/Views/ServiceRequest/Service/_ServiceModalForm.cshtml", form);
        }

        public async Task<PartialViewResult> ShowChangeAddressForm(int serviceRequestId)
        {
            var dto = await db.ServiceRequests.FindAsync(serviceRequestId);

            var form = new ChangeAddressForm();
            form.ServiceRequestId = dto.Id;
            form.AddressId = dto.AddressId;
            form.PhysicianId = dto.PhysicianId;

            return PartialView("~/Views/ServiceRequest/Address/_AddressModalForm.cshtml", form);
        }
        [HttpPost]
        public async Task<ActionResult> ChangeAddress(ChangeAddressForm form)
        {
            if (ModelState.IsValid)
            {
                //var response = await service.Reschedule(form);
                //response.AddToModelState(ModelState, null);
                await service.ChangeAddress(form);
                return Json(new
                {
                    serviceRequestId = form.ServiceRequestId
                });
            }
            Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            return PartialView("~/Views/ServiceRequest/Address/_AddressModalForm.cshtml", form);
        }
        public async Task<ActionResult> ShowChangeClaimantForm(int serviceRequestId)
        {
            var dto = await db.ServiceRequests.FindAsync(serviceRequestId);

            var form = new ChangeClaimantForm();
            form.ServiceRequestId = dto.Id;
            form.ClaimantName = dto.ClaimantName;

            return PartialView("~/Views/ServiceRequest/Claimant/_ClaimantModalForm.cshtml", form);
        }
        [HttpPost]
        public async Task<ActionResult> ChangeClaimant(ChangeClaimantForm form)
        {
            if (ModelState.IsValid)
            {
                //var response = await service.Reschedule(form);
                //response.AddToModelState(ModelState, null);
                await service.ChangeClaimant(form);
                return Json(new
                {
                    serviceRequestId = form.ServiceRequestId
                });
            }
            Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            return PartialView("~/Views/ServiceRequest/Claimant/_ClaimantModalForm.cshtml", form);
        }
        [AuthorizeRole(Feature = Features.ServiceRequest.ChangeProcessTemplate)]
        public ViewResult ChangeProcessTemplate(int id)
        {
            var serviceRequest = db.ServiceRequests.Single(sr => sr.Id == id);

            var model = new ChangeProcessTemplateViewModel()
            {
                ServiceRequestId = serviceRequest.Id,
                CurrentServiceRequestTemplate = serviceRequest.ServiceRequestTemplate == null ? null : new SelectListItem()
                {
                    Text = serviceRequest.ServiceRequestTemplate.Name,
                    Value = serviceRequest.ServiceRequestTemplate.Id.ToString()
                },
                PhysicianId = serviceRequest.PhysicianId
            };

            return View(model);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.ChangeProcessTemplate)]
        public async Task<ActionResult> ChangeProcessTemplate(ChangeProcessTemplateViewModel model)
        {
            var serviceRequest = db.ServiceRequests.Single(sr => sr.Id == model.ServiceRequestId);
            if (serviceRequest.ServiceRequestTemplateId == model.NewServiceRequestTemplateId)
            {
                ModelState.AddModelError("NewServiceRequestTemplateId", "New Template must be different than the current template");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var tasks = serviceRequest.ServiceRequestTasks.ToList();
            // remove  TODO: improve this by removing foreach loops
            foreach (var task in tasks)
            {
                foreach (var item in task.Child.ToList())
                {
                    task.Child.Remove(item);
                }
                foreach (var item in task.Parent.ToList())
                {
                    task.Parent.Remove(item);
                }
                // Parents are removed using referential integrity at the database level.
                db.ServiceRequestTasks.Remove(task);
            }

            // add

            serviceRequest.ServiceRequestTemplateId = model.NewServiceRequestTemplateId;

            var requestTemplate = db.ServiceRequestTemplates.Find(serviceRequest.ServiceRequestTemplateId);

            foreach (var template in requestTemplate.ServiceRequestTemplateTasks.AsQueryable().AreNotDeleted())
            {
                var st = new Orvosi.Data.ServiceRequestTask();
                st.Guidance = template.OTask.Guidance;
                st.ObjectGuid = Guid.NewGuid();
                st.ResponsibleRoleId = template.ResponsibleRoleId;
                st.Sequence = template.Sequence;
                st.ShortName = template.OTask.ShortName;
                st.TaskId = template.OTask.Id;
                st.TaskName = template.OTask.Name;
                st.ModifiedDate = now;
                st.ModifiedUser = loggedInUserId.ToString();
                st.CreatedDate = now;
                st.CreatedUser = loggedInUserId.ToString();
                st.ServiceRequestTemplateTaskId = template.Id;
                st.TaskType = template.OTask.TaskType;
                st.Workload = template.OTask.Workload;
                st.DueDate = GetTaskDueDate(serviceRequest.AppointmentDate, serviceRequest.DueDate, template);
                st.DueDateDurationFromBaseline = template.DueDateDurationFromBaseline;
                st.TaskStatusId = TaskStatuses.ToDo;
                st.TaskStatusChangedBy = loggedInUserId;
                st.TaskStatusChangedDate = now;
                st.IsCriticalPath = template.IsCriticalPath;
                st.EffectiveDateDurationFromBaseline = template.EffectiveDateDurationFromBaseline;
                st.EffectiveDate = GetEffectiveDate(serviceRequest.AppointmentDate.HasValue ? serviceRequest.AppointmentDate : serviceRequest.DueDate, template);
                // Assign tasks to physician and case coordinator to start
                st.AssignedTo = GetTaskAssignment(
                    template.ResponsibleRoleId,
                    serviceRequest.PhysicianId,
                    serviceRequest.CaseCoordinatorId,
                    serviceRequest.IntakeAssistantId,
                    serviceRequest.DocumentReviewerId);

                serviceRequest.ServiceRequestTasks.Add(st);
            }

            db.SaveChanges();

            // Clone the related tasks
            foreach (var taskTemplate in requestTemplate.ServiceRequestTemplateTasks.AsQueryable().AreNotDeleted())
            {
                foreach (var dependentTemplate in taskTemplate.Child.AsQueryable().AreNotDeleted())
                {
                    var task = serviceRequest.ServiceRequestTasks.First(srt => srt.ServiceRequestTemplateTaskId == taskTemplate.Id);
                    var dependent = serviceRequest.ServiceRequestTasks.First(srt => srt.ServiceRequestTemplateTaskId == dependentTemplate.Id);
                    task.Child.Add(dependent);
                }
            }

            db.SaveChanges();

            await service.UpdateDependentTaskStatuses(serviceRequest.Id);

            return RedirectToAction("Details", new { id = serviceRequest.Id });
        }

        [AuthorizeRole(Feature = Features.Availability.Reschedule)]
        public ViewResult Reschedule(int id)
        {
            var model = db.ServiceRequests.Single(c => c.Id == id);
            return View(model);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.Availability.Reschedule)]
        public async Task<ActionResult> Reschedule(ServiceRequest serviceRequest)
        {
            var sr = db.ServiceRequests.Single(c => c.Id == serviceRequest.Id);
            var slot = db.AvailableSlots.Single(c => c.Id == serviceRequest.AvailableSlotId);
            sr.AvailableSlotId = serviceRequest.AvailableSlotId;
            sr.AppointmentDate = slot.AvailableDay.Day;
            sr.StartTime = slot.StartTime;
            sr.EndTime = slot.EndTime;
            sr.AddressId = serviceRequest.AddressId;
            sr.ModifiedDate = SystemTime.Now();
            sr.ModifiedUser = User.Identity.Name;

            foreach (var t in sr.ServiceRequestTasks.AsQueryable().AreActive().Where(srt => srt.ServiceRequestTemplateTaskId.HasValue))
            {
                t.DueDate = GetTaskDueDate(sr.AppointmentDate, sr.DueDate, t.ServiceRequestTemplateTask);
                t.EffectiveDate = GetEffectiveDate(sr.AppointmentDate, t.ServiceRequestTemplateTask);
            }

            await db.SaveChangesAsync();
            return RedirectToAction("Details", new { id = sr.Id });
        }

        [AuthorizeRole(Feature = Features.Availability.BookAssessment)]
        public ViewResult Availability() => View();

        [HttpPost]
        [AuthorizeRole(Feature = Features.Availability.BookAssessment)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Availability(AvailabilityForm form)
        {
            var ad = await db.AvailableDays
                .FirstOrDefaultAsync(c => c.PhysicianId == form.PhysicianId && c.Day == form.AppointmentDate);

            if (ad == null)
            {
                this.ModelState.AddModelError("AppointmentDate", string.Format("Not available on this day."));
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction("Create", new { availableDayId = ad.Id, physicianId = form.PhysicianId });
            }
            return Availability();
        }

        [HttpGet]
        [AuthorizeRole(Feature = Features.Availability.BookAssessment)]
        public async Task<ActionResult> ShowBookingForm(int availableSlotId)
        {
            var dto = await db.AvailableSlots
                .Where(a => a.Id == availableSlotId)
                .Select(m.AvailableSlotDto.FromAvailableSlotEntityForBooking.Expand())
                .SingleAsync();

            var form = BookingForm.FromAvailableSlotDto.Invoke(dto);

            form.DueDate = form.AvailableSlotViewModel.AvailableDay.Day.AddDays(3);

            return PartialView("~/Views/ServiceRequest/Booking.cshtml", form);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.Availability.BookAssessment)]
        public async Task<ActionResult> BookAssessment(BookingForm form)
        {
            if (ModelState.IsValid)
            {
                var id = await service.BookAssessment(form);
                return Json(new
                {
                    serviceRequestId = id
                });
            }
            return PartialView("~/Views/ServiceRequest/Booking.cshtml", form);
        }

        [HttpGet]
        [AuthorizeRole(Feature = Features.Availability.BookAssessment)]
        public async Task<ActionResult> Create(int availableDayId, Guid physicianId, bool serviceIdHasErrors = false)
        {
            var availableDay = await db.AvailableDays.FindAsync(availableDayId);
            var physician = await db.Physicians.FindAsync(physicianId);
            var serviceRequest = new Orvosi.Data.ServiceRequest();
            serviceRequest.PhysicianId = physicianId;
            serviceRequest.AppointmentDate = availableDay.Day;

            var vm = new CreateViewModel();

            vm.SelectedAvailableDay = availableDay;
            vm.SelectedPhysician = physician;
            vm.ServiceRequest = serviceRequest;

            vm.ServiceSelectList = db.Services.Where(c => c.ServicePortfolioId == e.ServicePortfolios.Physician && c.ServiceCategoryId == e.ServiceCategories.IndependentMedicalExam)
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Group = new SelectListGroup() { Name = c.ServiceCategory.Name }
                });

            vm.AvailableSlotSelectList = db.AvailableSlots
                .Where(c => c.AvailableDayId == availableDayId)
                .Select(slot => new
                {
                    Id = slot.Id,
                    StartTime = slot.StartTime,
                    ServiceRequests = slot.ServiceRequests
                        .Where(sr => !sr.CancelledDate.HasValue)
                        .Select(sr => new
                        {
                            sr.Id,
                            sr.ClaimantName,
                            Service = sr.Service.Code,
                            Company = sr.Company.Name,
                            Location = new
                            {
                                City = sr.Address.City_CityId.Name,
                                BuildingName = sr.Address.Name
                            }
                        })
                })
                .AsEnumerable()
                .Select(c => new SelectListItem()
                {
                    Text = c.StartTime.ToString(@"hh\:mm")  + " - " + (c.ServiceRequests.Count() > 1 ? "Multiple Assessments" : c.ServiceRequests.Count() == 0 ? "Available" : $"{c.ServiceRequests.Single().Service} - {c.ServiceRequests.Single().Company} - {c.ServiceRequests.Single().Location.City} - {c.ServiceRequests.Single().Location.BuildingName} - {c.ServiceRequests.Single().ClaimantName}"),
                    Value = c.Id.ToString()
                })
                .OrderBy(c => c.Text);

            vm.CompanySelectList = db.Companies
                .Where(c => c.IsParent == false)
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Group = new SelectListGroup() { Name = c.ParentId.ToString() }
                });

            var addresses = new DataHelper().LoadAddressesWithOwner(db);
            vm.AddressSelectList = addresses
                .Where(loc => loc.Address.AddressTypeId != e.AddressTypes.BillingAddress)
                .AsEnumerable()
                .Select(c => new SelectListItem()
                {
                    Text = string.Format("{0} - {1}", c.Owner, c.Address.Name),
                    Value = c.Address.Id.ToString(),
                    Group = new SelectListGroup() { Name = c.Address.City_CityId.Name }
                })
                .OrderBy(c => c.Group.Name).ThenBy(c => c.Text);

            vm.StaffSelectList = db.AspNetUsers
                .Where(u => u.AspNetUserRoles.FirstOrDefault().RoleId == AspNetRoles.CaseCoordinator || u.AspNetUserRoles.FirstOrDefault().RoleId == AspNetRoles.SuperAdmin)
                .AsEnumerable()
                .Select(c => new SelectListItem()
                {
                    Text = c.GetDisplayName(),
                    Value = c.Id.ToString()
                });

            vm.ServiceRequestTemplateSelectList = db.ServiceRequestTemplates
                .AsEnumerable()
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });

            ViewBag.ServiceIdHasErrors = serviceIdHasErrors;

            return View(vm);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.Availability.BookAssessment)]
        public async Task<ActionResult> Create(Orvosi.Data.ServiceRequest sr)
        {
            // These are the original parameters required by the Get method. AvailableDayId is not a property on ServiceRequest so it was added as a hidden field on the form. PhysicianId was added to be consistent.
            var selectedAvailableDayId = int.Parse(Request.Form.Get("SelectedAvailableDayId"));
            var selectedPhysicianId = new Guid(Request.Form.Get("SelectedPhysicianId"));
            //bool overrideServiceCatalogueMissingError = false;
            //if (!string.IsNullOrEmpty(Request.Form.Get("OverrideServiceCatalogueMissingError")))
            //{
            //    overrideServiceCatalogueMissingError = Request.Form.Get("OverrideServiceCatalogueMissingError").Contains("true");
            //}

            // Get the service catalogue
            var address = await db.Addresses.FirstOrDefaultAsync(c => c.Id == sr.AddressId);
            //var serviceCatalogues = db.GetServiceCatalogueForCompany(sr.PhysicianId, sr.CompanyId).ToList();
            //var serviceCatalogue = serviceCatalogues.FirstOrDefault(c => c.LocationId == address.LocationId && c.ServiceId == sr.ServiceId);
            //if (serviceCatalogue == null || !serviceCatalogue.Price.HasValue)
            //{
            //    if (!overrideServiceCatalogueMissingError)
            //    {
            //        this.ModelState.AddModelError("ServiceId", "This service has not been offered to this company at this location.");
            //        ViewBag.ServiceIdHasErrors = true;
            //    }
            //}

            // Get the no show and late cancellation rates for this company
            var company = db.Companies.FirstOrDefault(c => c.Id == sr.CompanyId);
            //var rates = db.GetServiceCatalogueRate(sr.PhysicianId, company?.ObjectGuid).First();
            //if (rates == null || !rates.NoShowRate.HasValue || !rates.LateCancellationRate.HasValue)
            //{
            //    if (!overrideServiceCatalogueMissingError)
            //    {
            //        this.ModelState.AddModelError("ServiceId", "No Show Rates or Late Cancellation Rates have not been set for this company.");
            //        ViewBag.ServiceIdHasErrors = true;
            //    }
            //}

            if (ModelState.IsValid)
            {
                var slot = await db.AvailableSlots.FindAsync(sr.AvailableSlotId);
                sr.ServiceRequestStatusId = ServiceRequestStatuses.Active;
                sr.ServiceRequestStatusChangedBy = loggedInUserId;
                sr.ServiceRequestStatusChangedDate = now;
                sr.RequestedDate = now;
                sr.ServiceId = sr.ServiceId;
                sr.PhysicianId = selectedPhysicianId;
                sr.CompanyId = company.Id;
                sr.AddressId = address.Id;
                //sr.ServiceCatalogueId = serviceCatalogue == null ? null : serviceCatalogue.ServiceCatalogueId;
                sr.AvailableSlotId = sr.AvailableSlotId;
                sr.StartTime = slot.StartTime;
                sr.EndTime = slot.EndTime;
                sr.DueDate = sr.DueDate;
                //sr.ServiceCataloguePrice = serviceCatalogue == null ? null : serviceCatalogue.Price;
                //sr.NoShowRate = rates.NoShowRate;
                //sr.LateCancellationRate = rates.LateCancellationRate;
                sr.ModifiedUser = loggedInUserId.ToString();
                sr.ModifiedDate = now;
                sr.CreatedUser = loggedInUserId.ToString();
                sr.CreatedDate = now;

                // clone the workflow template tasks
                var requestTemplate = await db.ServiceRequestTemplates.FindAsync(sr.ServiceRequestTemplateId);
                foreach (var template in requestTemplate.ServiceRequestTemplateTasks.AsQueryable().AreNotDeleted())
                {
                    var st = new Orvosi.Data.ServiceRequestTask();
                    st.Guidance = template.OTask.Guidance;
                    st.ObjectGuid = Guid.NewGuid();
                    st.ResponsibleRoleId = template.ResponsibleRoleId;
                    st.Sequence = template.Sequence;
                    st.ShortName = template.OTask.ShortName;
                    st.TaskId = template.OTask.Id;
                    st.TaskName = template.OTask.Name;
                    st.ModifiedDate = now;
                    st.ModifiedUser = loggedInUserId.ToString();
                    st.CreatedDate = now;
                    st.CreatedUser = loggedInUserId.ToString();
                    // Assign tasks to physician and case coordinator to start
                    st.AssignedTo = (template.ResponsibleRoleId == AspNetRoles.CaseCoordinator ? sr.CaseCoordinatorId : (template.ResponsibleRoleId == AspNetRoles.Physician ? sr.PhysicianId as Nullable<Guid> : null));
                    st.ServiceRequestTemplateTaskId = template.Id;
                    st.TaskType = template.OTask.TaskType;
                    st.Workload = template.OTask.Workload;
                    st.DueDateDurationFromBaseline = template.DueDateDurationFromBaseline;
                    st.DueDate = GetTaskDueDate(sr.AppointmentDate, sr.DueDate, template);
                    st.TaskStatusId = TaskStatuses.ToDo;
                    st.TaskStatusChangedBy = loggedInUserId;
                    st.TaskStatusChangedDate = now;
                    st.IsCriticalPath = template.IsCriticalPath;
                    st.EffectiveDateDurationFromBaseline = template.EffectiveDateDurationFromBaseline;
                    st.EffectiveDate = GetEffectiveDate(sr.AppointmentDate, template);

                    sr.ServiceRequestTasks.Add(st);
                }

                sr.UpdateIsClosed();

                db.ServiceRequests.Add(sr);

                await db.SaveChangesAsync();

                // Clone the task dependencies
                foreach (var taskTemplate in requestTemplate.ServiceRequestTemplateTasks.AsQueryable().AreNotDeleted())
                {
                    foreach (var dependentTemplate in taskTemplate.Child)
                    {
                        var task = sr.ServiceRequestTasks.First(srt => srt.ServiceRequestTemplateTaskId == taskTemplate.Id);
                        var dependent = sr.ServiceRequestTasks.First(srt => srt.ServiceRequestTemplateTaskId == dependentTemplate.Id);
                        task.Child.Add(dependent);
                    }
                }

                await db.SaveChangesAsync();

                await service.UpdateDependentTaskStatuses(sr.Id);
                await service.UpdateServiceRequestStatus(sr.Id);

                return RedirectToAction("Details", new { id = sr.Id });
            }
            return await Create(selectedAvailableDayId, selectedPhysicianId, ViewBag.ServiceIdHasErrors);

        }

        [HttpGet]
        public ViewResult CreateSuccess(CreateSuccessViewModel obj)
        {
            return View(obj);
        }

        [AuthorizeRole(Feature = Features.ServiceRequest.SubmitRequest)]
        public ViewResult CreateAddOn(bool serviceIdHasErrors = false)
        {
            var vm = new CreateViewModel();

            vm.ServiceSelectList = db.Services.Where(c => c.ServicePortfolioId == e.ServicePortfolios.Physician && c.ServiceCategoryId == ServiceCategories.AddOn)
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Group = new SelectListGroup() { Name = c.ServiceCategory.Name }
                });

            vm.StaffSelectList = db.AspNetUsers
                .Where(u => u.AspNetUserRoles.FirstOrDefault().RoleId == AspNetRoles.CaseCoordinator || u.AspNetUserRoles.FirstOrDefault().RoleId == AspNetRoles.SuperAdmin)
                .AsEnumerable()
                .Select(c => new SelectListItem()
                {
                    Text = c.GetDisplayName(),
                    Value = c.Id.ToString()
                });

            vm.ServiceRequestTemplateSelectList = db.ServiceRequestTemplates
                .AsEnumerable()
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });

            ViewBag.ServiceIdHasErrors = serviceIdHasErrors;

            return View(vm);
        }

        [AuthorizeRole(Feature = Features.ServiceRequest.SubmitRequest)]
        [HttpPost]
        public async Task<ActionResult> CreateAddOn(Orvosi.Data.ServiceRequest sr)
        {
            var serviceCategoryId = db.Services.Single(s => s.Id == sr.ServiceId).ServiceCategoryId;

            if (serviceCategoryId != ServiceCategories.AddOn)
            {
                this.ModelState.AddModelError("ServiceId", "Service must be an AddOn.");
            }
           
            //bool overrideServiceCatalogueMissingError = false;
            //if (!string.IsNullOrEmpty(Request.Form.Get("OverrideServiceCatalogueMissingError")))
            //{
            //    overrideServiceCatalogueMissingError = Request.Form.Get("OverrideServiceCatalogueMissingError").Contains("true");
            //}

            var company = await db.Companies.FirstOrDefaultAsync(c => c.Id == sr.CompanyId);

            //var serviceCatalogues = db.GetServiceCatalogueForCompany(sr.PhysicianId, sr.CompanyId).ToList();

            //var serviceCatalogue = serviceCatalogues.SingleOrDefault(c => c.ServiceId == sr.ServiceId && c.LocationId == 0);
            //if (serviceCatalogue == null || !serviceCatalogue.Price.HasValue)
            //{
            //    if (!overrideServiceCatalogueMissingError)
            //    {
            //        this.ModelState.AddModelError("ServiceId", "This service has not been offered to this company at this location.");
            //        ViewBag.ServiceIdHasErrors = true;
            //    }
            //}

            //var rates = db.GetServiceCatalogueRate(sr.PhysicianId, company?.ObjectGuid).First();
            //if (rates == null || !rates.NoShowRate.HasValue || !rates.LateCancellationRate.HasValue)
            //{
            //    this.ModelState.AddModelError("ServiceId", "No Show Rates or Late Cancellation Rates have not been set for this company.");
            //}

            if (ModelState.IsValid)
            {
                sr.ServiceRequestStatusId = ServiceRequestStatuses.Active;
                sr.ServiceRequestStatusChangedBy = loggedInUserId;
                sr.ServiceRequestStatusChangedDate = now;
                //sr.ServiceCatalogueId = serviceCatalogue == null ? null : serviceCatalogue.ServiceCatalogueId;
                sr.DueDate = sr.DueDate;
                sr.CaseCoordinatorId = sr.CaseCoordinatorId;
                sr.RequestedDate = now;
                sr.ClaimantName = sr.ClaimantName;
                sr.CompanyReferenceId = sr.CompanyReferenceId;
                sr.RequestedDate = sr.RequestedDate;
                sr.CompanyId = sr.CompanyId;
                sr.PhysicianId = sr.PhysicianId;
                sr.ServiceId = sr.ServiceId;
                //sr.ServiceCataloguePrice = serviceCatalogue == null ? null : serviceCatalogue.Price;
                //sr.NoShowRate = rates.NoShowRate;
                //sr.LateCancellationRate = rates.LateCancellationRate;
                sr.ServiceRequestTemplateId = sr.ServiceRequestTemplateId;
                sr.ModifiedUser = User.Identity.Name;
                sr.ModifiedDate = SystemTime.Now();
                sr.CreatedUser = User.Identity.Name;
                sr.CreatedDate = SystemTime.Now();

                var requestTemplate = await db.ServiceRequestTemplates.FindAsync(sr.ServiceRequestTemplateId);

                foreach (var template in requestTemplate.ServiceRequestTemplateTasks.AsQueryable().AreNotDeleted())
                {
                    var st = new Orvosi.Data.ServiceRequestTask();
                    st.Guidance = template.OTask.Guidance;
                    st.ObjectGuid = Guid.NewGuid();
                    st.ResponsibleRoleId = template.ResponsibleRoleId;
                    st.Sequence = template.Sequence;
                    st.ShortName = template.OTask.ShortName;
                    st.TaskId = template.OTask.Id;
                    st.TaskName = template.OTask.Name;
                    st.ModifiedDate = now;
                    st.ModifiedUser = loggedInUserId.ToString();
                    st.CreatedDate = now;
                    st.CreatedUser = loggedInUserId.ToString();
                    // Assign tasks to physician and case coordinator to start
                    st.AssignedTo = (template.ResponsibleRoleId == AspNetRoles.CaseCoordinator ? sr.CaseCoordinatorId : (template.ResponsibleRoleId == AspNetRoles.Physician ? sr.PhysicianId as Nullable<Guid> : null));
                    st.ServiceRequestTemplateTaskId = template.Id;
                    st.TaskType = template.OTask.TaskType;
                    st.Workload = template.OTask.Workload;
                    st.DueDateDurationFromBaseline = template.DueDateDurationFromBaseline;
                    st.DueDate = GetTaskDueDate(sr.AppointmentDate, sr.DueDate, template);
                    st.TaskStatusId = TaskStatuses.ToDo;
                    st.TaskStatusChangedBy = loggedInUserId;
                    st.TaskStatusChangedDate = now;
                    st.IsCriticalPath = template.IsCriticalPath;
                    st.EffectiveDateDurationFromBaseline = template.EffectiveDateDurationFromBaseline;
                    st.EffectiveDate = GetEffectiveDate(sr.DueDate, template);

                    sr.ServiceRequestTasks.Add(st);
                }

                sr.UpdateIsClosed();

                db.ServiceRequests.Add(sr);

                await db.SaveChangesAsync();

                // Clone the related tasks
                foreach (var taskTemplate in requestTemplate.ServiceRequestTemplateTasks.AsQueryable().AreNotDeleted())
                {
                    foreach (var dependentTemplate in taskTemplate.Child)
                    {
                        var task = sr.ServiceRequestTasks.First(srt => srt.ServiceRequestTemplateTaskId == taskTemplate.Id);
                        var dependent = sr.ServiceRequestTasks.First(srt => srt.ServiceRequestTemplateTaskId == dependentTemplate.Id);
                        task.Child.Add(dependent);
                    }
                }

                await db.SaveChangesAsync();

                await service.UpdateDependentTaskStatuses(sr.Id);
                await service.UpdateServiceRequestStatus(sr.Id);


                return RedirectToAction("Details", new { id = sr.Id });
            }

            return await CreateAddOn(ViewBag.ServiceIdHasErrors);
        }

        [HttpGet]
        public PartialViewResult RefreshCompanyDropDown(Guid physicianId)
        {
            var companySelectList = viewDataService.GetPhysicianCompanySelectList(physicianId);
            return PartialView("_CreateAddOnCompanyDropDown", companySelectList);
        }

        [HttpGet]
        public PartialViewResult RefreshServiceDropDown(Guid physicianId)
        {
            var selectList = viewDataService.GetPhysicianServiceSelectList(physicianId);
            return PartialView("_CreateAddOnServiceDropDown", selectList);
        }

        [HttpGet]
        public PartialViewResult RefreshCaseCoordinatorDropDown(Guid physicianId)
        {
            var selectList = viewDataService.GetPhysicianCaseCoordinatorSelectList(physicianId);
            return PartialView("_CreateAddOnCaseCoordinatorDropDown", selectList);
        }

        [HttpGet]
        public PartialViewResult RefreshProcessTemplateDropDown(Guid physicianId)
        {
            var selectList = viewDataService.GetPhysicianProcessTemplateSelectList(physicianId);
            return PartialView("_CreateAddOnProcessTemplateDropDown", selectList);
        }

        [AuthorizeRole(Feature = Features.ServiceRequest.AssignResources)]
        public async Task<ActionResult> AssignRequiredResourcesToTasks(int serviceRequestId)
        {
            await service.AssignRequiredResourcesToTasks(serviceRequestId);

            return Json(new
            {
                serviceRequestId = serviceRequestId
            });
        }

        [AuthorizeRole(Feature = Features.ServiceRequest.AssignResources)]
        public async Task<ActionResult> ResourceAssignment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var serviceRequest = await db.ServiceRequests.FindAsync(id);

            if (serviceRequest == null)
            {
                return HttpNotFound();
            }
            
            var vm = new ResourceAssignmentViewModel()
            {
                ServiceRequestId = serviceRequest.Id,
                PhysicianId = serviceRequest.PhysicianId,
                CaseCoordinatorId = serviceRequest.CaseCoordinatorId,
                DocumentReviewerId = serviceRequest.DocumentReviewerId,
                IntakeAssistantId = serviceRequest.IntakeAssistantId
            };

            // TODO: Update the calendar and dropbox folder if appropriate.

            return View(vm);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(Feature = Features.ServiceRequest.AssignResources)]
        public async Task<ActionResult> ResourceAssignment(ResourceAssignmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                // get the tracked object from the database
                var obj = await db.ServiceRequests.FindAsync(model.ServiceRequestId);

                // update the resource assignments
                obj.CaseCoordinatorId = model.CaseCoordinatorId;
                obj.DocumentReviewerId = model.DocumentReviewerId;
                obj.IntakeAssistantId = model.IntakeAssistantId;

                foreach (var task in obj.ServiceRequestTasks)
                {
                    if (task.ResponsibleRoleId == AspNetRoles.CaseCoordinator)
                        task.AssignedTo = obj.CaseCoordinatorId;
                    else if (task.ResponsibleRoleId == AspNetRoles.IntakeAssistant)
                        task.AssignedTo = obj.IntakeAssistantId;
                    else if (task.ResponsibleRoleId == AspNetRoles.DocumentReviewer)
                        task.AssignedTo = obj.DocumentReviewerId;
                    else if (task.ResponsibleRoleId == AspNetRoles.Physician)
                        task.AssignedTo = obj.PhysicianId;
                }

                await db.SaveChangesAsync();

                return RedirectToAction("Details", new { id = obj.Id });
            }
            return View(model);
        }

        [AuthorizeRole(Feature = Features.ServiceRequest.Edit)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ServiceRequest serviceRequest = await db.ServiceRequests.FindAsync(id);

            if (serviceRequest == null)
            {
                return HttpNotFound();
            }

            // TODO: Update the calendar and dropbox folder if appropriate.

            return View(new EditViewModel
            {
                ServiceRequestId = serviceRequest.Id,
                ClaimantName = serviceRequest.ClaimantName,
                DueDate = serviceRequest.DueDate,
                CompanyReferenceId = serviceRequest.CompanyReferenceId,
                AdditionalNotes = serviceRequest.Notes,
                RequestedBy = serviceRequest.RequestedBy,
                RequestedDate = serviceRequest.RequestedDate
            });
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(Feature = Features.ServiceRequest.Edit)]
        public async Task<ActionResult> Edit(EditViewModel sr)
        {
            if (ModelState.IsValid)
            {
                // get the tracked object from the database
                var obj = await db.ServiceRequests.SingleOrDefaultAsync(c => c.Id == sr.ServiceRequestId);

                // update the resource assignments
                obj.ClaimantName = sr.ClaimantName;
                obj.CompanyReferenceId = sr.CompanyReferenceId;
                obj.Notes = sr.AdditionalNotes;
                obj.RequestedBy = sr.RequestedBy;
                obj.RequestedDate = sr.RequestedDate;
                obj.ModifiedDate = SystemTime.Now();
                obj.ModifiedUser = User.Identity.GetGuidUserId().ToString();

                await db.SaveChangesAsync();

                return RedirectToAction("Details", new { id = sr.ServiceRequestId });
            }
            return View(sr);
        }

        [AuthorizeRole(Feature = Features.ServiceRequest.Delete)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceRequest serviceRequest = await db.ServiceRequests.FindAsync(id);
            if (serviceRequest == null)
            {
                return HttpNotFound();
            }
            return View(serviceRequest);
        }

        // POST: ServiceRequest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(Feature = Features.ServiceRequest.Delete)]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ServiceRequest serviceRequest = await db.ServiceRequests.FindAsync(id);

            if (!serviceRequest.CancelledDate.HasValue)
            {
                this.ModelState.AddModelError("", "Service Request must be cancelled before being deleted.");
                return View("Delete");
            }

            serviceRequest.IsDeleted = true;
            serviceRequest.ModifiedDate = SystemTime.Now();
            serviceRequest.ModifiedUser = User.Identity.GetGuidUserId().ToString();
            await db.SaveChangesAsync();

            //TODO: Unshare and delete folders from dropbox
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.ToggleNoShow)]
        public async Task<JsonResult> ToggleNoShow(NoShowForm form)
        {
            await service.NoShow(form);

            return Json(new
            {
                serviceRequestId = form.ServiceRequestId
            });
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.ToggleNoShow)]
        public async Task<JsonResult> ToggleOnHold(OnHoldForm form)
        {
            await service.OnHold(form);

            return Json(new
            {
                serviceRequestId = form.ServiceRequestId
            });
        }

        [AuthorizeRole(Feature = Features.ServiceRequest.ViewInvoiceNote)]
        public async Task<PartialViewResult> RefreshNote(int serviceRequestId, bool allowEdit = false)
        {
            var context = new Orvosi.Data.OrvosiDbContext();
            var note = await context.ServiceRequests.FindAsync(serviceRequestId);
            return PartialView("~/Views/Note/_Note.cshtml", new NoteViewModel() { ServiceRequestId = note.Id, Note = note.Notes, AllowEdit = allowEdit });
        }

        #region Box

        public async Task<PartialViewResult> BoxManager(int serviceRequestId)
        {
            var vm = new BoxManagerViewModel();
            vm.ServiceRequestId = serviceRequestId;

            // Get the box folder id for the case
            var serviceRequest = db.ServiceRequests
                .WithId(serviceRequestId)
                .Select(ServiceRequestProjections.ForBoxManager())
                .Single();

            if (string.IsNullOrEmpty(serviceRequest.BoxCaseFolderId))
            {
                return PartialView("~/Views/Box/BoxManager.cshtml", vm);
            }

            var orvosiBoxFolderCollaborations = db.ServiceRequestBoxCollaborations
                .Where(bc => bc.ServiceRequestId == serviceRequestId)
                .ToList();

            var box = new BoxManager();
            var boxFolder = box.GetFolder(serviceRequest.BoxCaseFolderId);
            var boxFolderCollaborations = box.GetCollaborations(serviceRequest.BoxCaseFolderId).Entries.ToList();

            var result = orvosiBoxFolderCollaborations
                .FullGroupJoin(boxFolderCollaborations,
                    o => o.BoxCollaborationId,
                    b => b.Id,
                    (key, o, b) => new BoxCollaborationFullOuterJoinResult
                    {
                        ServiceRequestId = serviceRequestId,
                        BoxCollaborationId = key,
                        OrvosiCollaborations = o,
                        BoxCollaborations = b
                    }).ToList();

            var orvosiResources = db.ServiceRequestTasks
                    .Where(srt => srt.ServiceRequestId == serviceRequestId)
                    .Where(t => t.AssignedTo.HasValue)
                    .Select(sr => sr.AspNetUser_AssignedTo)
                    .Distinct();

            var resources = new List<BoxResource>();
            foreach (var resource in orvosiResources)
            {
                var boxResource = new BoxResource() { Resource = resource };
                if (string.IsNullOrEmpty(boxResource.Resource.BoxUserId))
                {
                    boxResource.BoxFolder = null;
                }
                else
                {
                    // this should catch errors from box and handle user accounts that have been removed from box.
                    boxResource.BoxFolder = await box.GetFolder(serviceRequest.BoxCaseFolderId, resource.BoxUserId);
                }
                resources.Add(boxResource);
            }

            vm.ServiceRequestId = serviceRequestId;
            vm.Reconciliations = result;
            vm.Resources = resources;
            vm.BoxFolderCollaborations = boxFolderCollaborations;
            vm.BoxFolder = boxFolder;
            vm.ExpectedFolderName = serviceRequest.CaseFolderName;
            return PartialView("~/Views/Box/BoxManager.cshtml", vm);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest_Box.UpdateFolder)]
        public JsonResult UpdateBoxCaseFolderName(int serviceRequestId)
        {
            // Get the request
            var serviceRequest = db.ServiceRequests
                .WithId(serviceRequestId)
                .Select(ServiceRequestProjections.ForBoxManager())
                .Single();

            var box = new BoxManager();
            var caseFolder = box.RenameCaseFolder(serviceRequest.BoxCaseFolderId, serviceRequest.CaseFolderName);
            
            // Redirect to display the Box Folder
            return Json(serviceRequestId);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest_Box.CreateFolder)]
        public ActionResult CreateBoxCaseFolder(int serviceRequestId)
        {
            // Get the request
            var serviceRequest = db.ServiceRequests
                .WithId(serviceRequestId)
                .Select(ServiceRequestProjections.ForBoxManager())
                .Single();

            // Get the request and assert they have a Box Folder Id
            var physician = db.AspNetUsers.Single(p => p.Id == serviceRequest.Physician.Id);
            //var physicianBoxFolderId = "7027883033"; // This overrides to HanSolo box folder while developing. Comment out for production.
            var physicianBoxFolderId = serviceRequest.Physician.BoxFolderId;
            if (string.IsNullOrEmpty(physicianBoxFolderId))
                return PartialView("Error", "Physician does not have a cases folder setup in Box.");

            // Create the case folder
            var box = new BoxManager();
            BoxFolder caseFolder;
            if (serviceRequest.Service.ServiceCategoryId == ServiceCategories.AddOn)
            {
                var province = db.GetCompanyProvince(serviceRequest.Company.Id).FirstOrDefault();
                if (province == null)
                {
                    province = new GetCompanyProvinceReturnModel() { ProvinceID = 0, ProvinceName = "Ontario" };
                }
                caseFolder = box.CreateAddOnFolder(physicianBoxFolderId, province.ProvinceName, serviceRequest.DueDate.Value, serviceRequest.CaseFolderName, serviceRequest.Physician.BoxAddOnTemplateFolderId);
            }
            else
            {
                // Get the province which is used in the case folder path
                var province = db.Provinces.Single(p => p.Id == serviceRequest.Address.ProvinceId);
                caseFolder = box.CreateCaseFolder(physicianBoxFolderId, province.ProvinceName, serviceRequest.AppointmentDate.Value, serviceRequest.CaseFolderName, serviceRequest.Physician.BoxCaseTemplateFolderId);
            }

            // Persist the new case folder Id to the database.
            var sr = db.ServiceRequests.Find(serviceRequestId);
            sr.BoxCaseFolderId = caseFolder.Id;
            db.SaveChanges();

            // Redirect to display the Box Folder
            return Json(serviceRequestId);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest_Box.AddCollaborator)]
        public JsonResult ShareBoxFolder(int serviceRequestId, string FolderId, Guid UserId)
        {
            var resources = db.GetServiceRequestResources(serviceRequestId);
            var resource = resources.Single(r => r.Id == UserId);

            var box = new BoxManager();
            var collaboration = box.AddCollaboration(FolderId, resource.BoxUserId, resource.Email);
            db.ServiceRequestBoxCollaborations.Add(
                new ServiceRequestBoxCollaboration()
                {
                    BoxCollaborationId = collaboration.Id,
                    ServiceRequestId = serviceRequestId,
                    UserId = UserId,
                    ModifiedUser = User.Identity.Name,
                    ModifiedDate = SystemTime.Now()
                }
            );
            db.SaveChanges();

            box.UpdateSyncState(collaboration.Item.Id, resource.BoxUserId, BoxSyncStateType.synced);

            return Json(serviceRequestId);

        }

        [AuthorizeRole(Feature = Features.ServiceRequest_Box.RemoveCollaborator)]
        public JsonResult UnshareBoxFolder(int serviceRequestId, string CollaborationId)
        {

            var box = new BoxManager();
            var success = box.RemoveCollaboration(CollaborationId);

            if (success)
            {
                var collaboration = db.ServiceRequestBoxCollaborations.SingleOrDefault(b => b.BoxCollaborationId == CollaborationId);
                db.ServiceRequestBoxCollaborations.Remove(collaboration);
                db.SaveChanges();
            }
            return Json(serviceRequestId);
        }

        [AuthorizeRole(Feature = Features.ServiceRequest_Box.RemoveCollaborator)]
        public JsonResult DeleteOrvosiBoxCollaboration(int id)
        {
            var collaboration = db.ServiceRequestBoxCollaborations.Single(b => b.Id == id);
            var serviceRequestId = collaboration.ServiceRequestId;
            db.ServiceRequestBoxCollaborations.Remove(collaboration);
            db.SaveChanges();
            return Json(serviceRequestId);
        }


        public JsonResult AcceptBoxFolder(int serviceRequestId, Guid UserId, string CollaborationId)
        {
            string boxUserId;

            var user = db.Profiles.Single(p => p.Id == UserId);
            boxUserId = user.BoxUserId;
            var box = new BoxManager();
            var collaboration = box.AcceptCollaboration(CollaborationId, boxUserId);
            return Json(serviceRequestId);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest_Box.SyncUnsyncCollaborator)]
        public JsonResult UnsyncBoxFolder(int serviceRequestId, string FolderId, string BoxUserId)
        {
            var box = new BoxManager();
            box.UpdateSyncState(FolderId, BoxUserId, BoxSyncStateType.not_synced);
            return Json(serviceRequestId);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest_Box.SyncUnsyncCollaborator)]
        public JsonResult SyncBoxFolder(int serviceRequestId, string FolderId, string BoxUserId)
        {
            var box = new BoxManager();
            box.UpdateSyncState(FolderId, BoxUserId, BoxSyncStateType.synced);
            return Json(serviceRequestId);
        }

        #endregion

        #region Private

        private async Task GetPhysicianDropDownData()
        {
            var physicians = await db.Physicians
                .Select(p => new
                {
                    p.AspNetUser.FirstName,
                    p.AspNetUser.LastName,
                    p.AspNetUser.Title,
                    p.Id,
                    PhysicianSpecialty = p.PhysicianSpeciality.Name
                }).ToListAsync();

            ViewBag.Physicians = physicians.Select(c => new SelectListItem()
            {
                Text = ValueConverters.GetDisplayName(c.Title, c.FirstName, c.LastName),
                Value = c.Id.ToString(),
                Group = new SelectListGroup() { Name = c.PhysicianSpecialty }
            });
        }

        private async Task GetCreateDropdownlistData(AvailableDay availableDay)
        {
            //var companies = ctx.Companies
            //    .Where(c => c.IsParent == false);

            //if (availableDay != null && availableDay.CompanyIsParent.Value)
            //{
            //    companies = companies.Where(c => c.ParentId == availableDay.CompanyId);
            //}

            ViewBag.Services = await db.Services
                .Where(c => c.ServicePortfolioId == ServicePortfolios.Physician)
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Group = new SelectListGroup() { Name = c.ServiceCategory.Name }
                }).ToListAsync();

            var slots = await db.AvailableSlotViews
                .Where(c => c.AvailableDayId == availableDay.Id).ToListAsync();

            ViewBag.AvailableSlots = slots.Select(c => new SelectListItem()
            {
                Text = c.StartTime.ToString(@"hh\:mm") + " - " + c.Title,
                Value = c.Id.ToString()
            })
            .OrderBy(c => c.Text)
            .ToList();

            ViewBag.Companies = await db.Companies
                .Where(c => c.IsParent == false)
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Group = new SelectListGroup() { Name = c.Parent.Name }
                }).ToListAsync();

            var l = new WebApp.Library.DataHelper().LoadAddressesWithOwner(db);
            ViewBag.Locations = l
                .Where(a => a.Address.AddressTypeId != AddressTypes.BillingAddress)
                .Select(c => new SelectListItem()
            {
                Text = string.Format("{0} - {1} - {2}", c.Owner, c.Address.City_CityId.Name, c.Address.Name),
                Value = c.Address.Id.ToString(),
                Group = new SelectListGroup() { Name = c.Owner }
            });

            ViewBag.Staff = db.AspNetUsers
                .Where(u => u.GetRoleId() == AspNetRoles.CaseCoordinator || u.GetRoleId() == AspNetRoles.DocumentReviewer || u.GetRoleId() == AspNetRoles.IntakeAssistant || u.GetRoleId() == AspNetRoles.SuperAdmin)
                .AsEnumerable()
                .Select(c => new SelectListItem()
                {
                    Text = ValueConverters.GetDisplayName(c.Title, c.FirstName, c.LastName),
                    Value = c.Id.ToString()
                });
        }

        private DateTime? GetTaskDueDate(DateTime? appointmentDate, DateTime? reportDueDate, ServiceRequestTemplateTask taskTemplate)
        {
            DateTime? taskDueDate;
            if (appointmentDate.HasValue && taskTemplate.TaskId == Tasks.AssessmentDay) // assessment day task is set to the appointment date
            {
                taskDueDate = appointmentDate.Value;
            }
            else if (reportDueDate.HasValue && taskTemplate.TaskId == Tasks.SubmitReport) // submit report task is set to the report due date
            {
                taskDueDate = reportDueDate.Value;
            }
            else // for all other tasks, we calculate the due date accordingly
            {
                if (!taskTemplate.DueDateDurationFromBaseline.HasValue) // If there is no duration, return null (ASAP) NOTE: 0 must be set explicitly to have it match the baseline date.
                {
                    taskDueDate = null; 
                }
                else
                {
                    if (taskTemplate.DueDateType == DueDateTypes.AppointmentDate)
                    {
                        taskDueDate = appointmentDate.Value.AddDays(taskTemplate.DueDateDurationFromBaseline.Value);
                    }
                    else if (taskTemplate.DueDateType == DueDateTypes.ReportDueDate)
                    {
                        taskDueDate = reportDueDate.Value.AddDays(taskTemplate.DueDateDurationFromBaseline.Value);
                        if (appointmentDate.HasValue && taskDueDate < appointmentDate)
                        {
                            taskDueDate = appointmentDate;
                        }
                    }
                    else
                    {
                        taskDueDate = null;
                    }
                }
            }

            return taskDueDate;
        }

        private DateTime GetEffectiveDate(DateTime? baselineDate, ServiceRequestTemplateTask taskTemplate)
        {
            if (!baselineDate.HasValue)
            {
                throw new Exception("Baseline Date is required to calculate Due Dates and Effective Dates.");
            }

            if (taskTemplate.IsBaselineDate) // BASELINE
            {
                return baselineDate.Value;
            }
            else if (taskTemplate.EffectiveDateDurationFromBaseline.HasValue) // HAS A DURATION FROM BASELINE
            {
                return baselineDate.Value.AddDays(taskTemplate.EffectiveDateDurationFromBaseline.Value);
            }
            else // ASAP
            {
                return now;
            }
        }

        private Guid? GetTaskAssignment(Guid? responsibleRoleId, Guid physicianId, Guid? caseCoordinatorId, Guid? intakeAssistantId, Guid? documentReviewerId)
        {
            if (responsibleRoleId == AspNetRoles.Physician)
            {
                return physicianId;
            }
            else if (responsibleRoleId == AspNetRoles.CaseCoordinator)
            {
                return caseCoordinatorId;
            }
            else if (responsibleRoleId == AspNetRoles.IntakeAssistant)
            {
                return intakeAssistantId;
            }
            else if (responsibleRoleId == AspNetRoles.DocumentReviewer)
            {
                return documentReviewerId;
            }
            else
            {
                return null;
            }
        }

        #endregion



        [HttpGet]
        //[AuthorizeRole(Feature = Features.SysAdmin.ManageTasks)]
        public JsonResult GetAllServiceRequestIds()
        {
            var ids = db.ServiceRequests.Select(sr => sr.Id).OrderBy(sr => sr).ToList();
            return Json(ids, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.UpdateTaskStatus)]
        public async Task<JsonResult> UpdateDependentTaskStatuses(int serviceRequestId)
        {
            await service.UpdateDependentTaskStatuses(serviceRequestId);

            return Json(new
            {
                serviceRequestId = serviceRequestId
            });
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.UpdateTaskStatus)]
        public async Task<JsonResult> UpdateServiceRequestStatuses(int serviceRequestId)
        {
            await service.UpdateServiceRequestStatus(serviceRequestId);

            return Json(new
            {
                serviceRequestId = serviceRequestId
            });
        }
    }
}

public class BoxCollaborationFullOuterJoinResult
{
    public string BoxCollaborationId { get; set; }
    public IEnumerable<ServiceRequestBoxCollaboration> OrvosiCollaborations { get; set; }
    public IEnumerable<BoxCollaboration> BoxCollaborations { get; set; }
    public int ServiceRequestId { get; internal set; }
}