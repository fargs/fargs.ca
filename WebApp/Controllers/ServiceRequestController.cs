﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Orvosi.Data;
using Orvosi.Shared.Enums;
using WebApp.ViewModels.ServiceRequestViewModels;
using System.Security.Claims;
using WebApp.Library;
using Dropbox.Api;
using Dropbox.Api.Files;
using Dropbox.Api.Sharing;
using Dropbox.Api.Team;
using Box.V2.Models;
using e = Orvosi.Shared.Enums;
using WebApp.Library.Extensions;
using MoreLinq;
using WebApp.ViewModels;
using WebApp.Library.Projections;
using Orvosi.Data.Filters;
using Orvosi.Shared.Filters;
using Features = Orvosi.Shared.Enums.Features;
using WebApp.Library.Filters;
using WebApp.ViewModels.UIElements;
using LinqKit;
using m = WebApp.Models;
using WebApp.FormModels;
using WebApp.ViewModels.CalendarViewModels;
using WebApp.ViewDataModels;
using WebApp.ViewModels.ServiceRequestTaskViewModels;

namespace WebApp.Controllers
{
    public delegate void NoShowToggledHandler(object sender, EventArgs e);

    [AuthorizeRole(Feature = Features.ServiceRequest.View)]
    public class ServiceRequestController : BaseController
    {
        private OrvosiDropbox _dropbox;

        public OrvosiDropbox dropbox
        {
            get
            {
                if (_dropbox == null)
                {
                    _dropbox = new OrvosiDropbox();
                    return _dropbox;
                }
                return _dropbox;
            }
        }

        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public async Task<ActionResult> Index(WebApp.ViewModels.ServiceRequestViewModels.FilterArgs filterArgs)
        {
            var vm = new WebApp.ViewModels.ServiceRequestViewModels.IndexViewModel();
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

            var userId = User.Identity.GetGuidUserId();

            // if the user is an administrator and the option showAll is true, then show all
            if (User.Identity.GetRoleId() != AspNetRoles.SuperAdmin || (User.Identity.GetRoleId() != AspNetRoles.SuperAdmin && filterArgs.ShowAll == false))
            {
                sr = sr.Where(c => c.CaseCoordinatorId == userId || c.IntakeAssistantId == userId || c.DocumentReviewerId == userId || c.PhysicianId == userId);
            }

            if (User.Identity.GetRoleId() != AspNetRoles.SuperAdmin || User.Identity.GetRoleId() != AspNetRoles.CaseCoordinator || User.Identity.GetRoleId() != AspNetRoles.DocumentReviewer || User.Identity.GetRoleId() != AspNetRoles.IntakeAssistant)
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

        public async Task<ActionResult> Dashboard()
        {
            var userId = User.Identity.GetGuidUserId();

            var list = await db.ServiceRequests
                .Where(sr => sr.CaseCoordinatorId == userId || sr.IntakeAssistantId == userId || sr.DocumentReviewerId == userId || sr.PhysicianId == userId)
                .ToListAsync();

            return View(list);
        }

        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public ActionResult Details2(int serviceRequestId)
        {
            var dto =  db.ServiceRequests
                .WithId(serviceRequestId)
                .Select(m.ServiceRequestDto.FromServiceRequestEntity.Expand())
                .Single();

            var viewModel = CaseViewModel.FromServiceRequestDto.Invoke(dto);

            return PartialView(viewModel);
        }

        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public async Task<ActionResult> Details(int id)
        {
            var dto = db.ServiceRequests
                .WithId(id)
                .CanAccess(userId, physicianId, roleId)
                .Select(m.ServiceRequestDto.FromServiceRequestEntity.Expand())
                .SingleOrDefault();

            if (dto == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            var viewModel = CaseViewModel.FromServiceRequestDto.Invoke(dto);

            ViewData.TaskListArgs_Set(new TaskListArgs
            {
                ServiceRequestId = id,
                ViewTarget = ViewTarget.Details,
                ViewFilter = TaskListViewModelFilter.AllTasks
            });

            return View(viewModel);
        }

        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public ActionResult Agenda(DateTime selectedDate, ViewTarget viewOptions = ViewTarget.Agenda)
        {
            // Set date range variables used in where conditions
            var dto = db.ServiceRequests
                .AreScheduledThisDay(selectedDate)
                .AreNotCancellations()
                .CanAccess(userId, physicianId, roleId)
                .Select(m.ServiceRequestDto.FromServiceRequestEntity.Expand())
                .OrderBy(sr => sr.AppointmentDate).ThenBy(sr => sr.StartTime)
                .ToList();

            var caseViewModels = dto.AsQueryable()
                .Select(CaseViewModel.FromServiceRequestDto.Expand());

            var dayViewModel = caseViewModels
                .GroupBy(c => c.AppointmentDate.Value)
                .AsQueryable()
                .Select(DayViewModel.FromServiceRequestDtoGroupingDto.Expand())
                .SingleOrDefault();

            return PartialView(dayViewModel);
        }

        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        [HttpPost]
        public ActionResult DueDate(DueDateArgs args)//DateTime selectedDate, short[] selectedTaskTypes = null, CaseViewOptions viewOptions = CaseViewOptions.Agenda)
        {
            var dto = db.ServiceRequestTasks
                .AreDueBetween(args.TaskListArgs.DateRange.StartDate, args.TaskListArgs.DateRange.EndDate.Value)
                .AreAssignedToUser(userId)
                .WithTaskIds(args.TaskListArgs.TaskIds)
                .AreActive()
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
        public ActionResult ScheduleDateRange(TaskListArgs args)
        {
            var ids = db.ServiceRequestTasks
                .AreScheduledBetween(args.DateRange.StartDate, args.DateRange.EndDate.Value)
                .AreAssignedToUser(userId)
                .WithTaskIds(args.TaskIds)
                .AreActive()
                .Where(srt => srt.ServiceRequest.AppointmentDate.HasValue)
                .GroupBy(srt => srt.ServiceRequestId)
                .Select(grp => new
                {
                    ServiceRequestId = grp.Key,
                    NextTaskStatusId = grp.Min(srt => srt.TaskStatusId)
                });

            var dto = db.ServiceRequests
                .AreNotCancellations()
                .Where(sr => sr.AppointmentDate.HasValue)
                .Where(sr => ids.Select(s => s.ServiceRequestId).Contains(sr.Id))
                .Select(m.ServiceRequestDto.FromServiceRequestEntityForCase.Expand())
                .ToList();

            var viewModel = dto
                .AsQueryable()
                .Select(CaseViewModel.FromServiceRequestDto.Expand());

            var dayViewModel = viewModel
                .GroupBy(c => c.AppointmentDate.Value)
                .AsQueryable()
                .Select(DayViewModel.FromServiceRequestDtoGroupingDto.Expand());

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
        public ActionResult Additionals(TaskListArgs args)
        {
            var dto = db.ServiceRequestTasks
                .AreAssignedToUser(userId)
                .WithTaskIds(args.TaskIds)
                .AreActive()
                .Where(srt => !srt.ServiceRequest.AppointmentDate.HasValue)
                .GroupBy(srt => srt.ServiceRequest)
                .Select(grp => grp.Key)
                .Select(m.ServiceRequestDto.FromServiceRequestEntityForCase.Expand())
                .ToList();

            var viewModel = dto
                .AsQueryable()
                .Select(CaseViewModel.FromServiceRequestDto.Expand());

            return PartialView(viewModel);
        }
        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public ActionResult CaseLink(CaseLinkArgs args)
        {
            var dto = db.ServiceRequests
                .WithId(args.ServiceRequestId)
                .Select(m.ServiceRequestDto.FromServiceRequestEntityForCase.Expand())
                .SingleOrDefault();

            if (dto == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            var viewModel = CaseViewModel.FromServiceRequestDto.Invoke(dto);
            
            return PartialView(viewModel);
        }

        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public ActionResult Case(int serviceRequestId, ViewTarget viewOptions = ViewTarget.Details)
        {
            var dto = db.ServiceRequests
                .WithId(serviceRequestId)
                .CanAccess(userId, physicianId, roleId)
                .Select(m.ServiceRequestDto.FromServiceRequestEntityForCase.Expand())
                .SingleOrDefault();

            if (dto == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            var viewModel = CaseViewModel.FromServiceRequestDto.Invoke(dto);

            ViewData.ViewTarget_Set(viewOptions);

            return PartialView(viewModel);
        }

        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public async Task<ActionResult> ActionMenu(int serviceRequestId)
        {
            var dto = await db.ServiceRequests
                .WithId(serviceRequestId)
                .CanAccess(userId, physicianId, roleId)
                .Select(m.ServiceRequestDto.FromServiceRequestEntityForCase.Expand())
                .SingleOrDefaultAsync();

            if (dto == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            var viewModel = CaseViewModel.FromServiceRequestDto.Invoke(dto);

            return PartialView(viewModel);
        }

        [AuthorizeRole(Feature = Features.ServiceRequest.ChangeCompanyOrService)]
        public ActionResult ChangeCompany(int id)
        {
            var vm = db.ServiceRequests
                .Where(sr => sr.Id == id)
                .Select(sr => new ChangeCompanyViewModel
                {
                    ServiceRequestId = sr.Id,
                    ClaimantName = sr.ClaimantName,
                    CompanyId = sr.CompanyId,
                    ServiceId = sr.ServiceId,
                    HasInvoices = sr.InvoiceDetails.Any()
                })
                .First();

            vm.CompanySelectList = db.Companies
                .Where(c => c.IsParent == false)
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Group = new SelectListGroup() { Name = c.ParentId.ToString() }
                })
                .ToList();

            vm.ServiceSelectList = db.Services
                .Where(c => c.ServicePortfolioId == e.ServicePortfolios.Physician)
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Group = new SelectListGroup() { Name = c.ServiceCategory.Name }
                })
                .ToList();


            // pre validation rules
            if (vm.HasInvoices)
            {
                ModelState.AddModelError("CompanyId", "This request has a pending invoice to the original company. Please delete all invoices and try again.");
            }

            return View(vm);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.ChangeCompanyOrService)]
        public async Task<ActionResult> ChangeCompany(ChangeCompanyFormViewModel form)
        {
            var record = db.ServiceRequests
                .Where(sr => sr.Id == form.ServiceRequestId)
                .Single();

            // Get the service catalogue
            var address = await db.Addresses.FirstOrDefaultAsync(c => c.Id == record.AddressId);
            var serviceCatalogues = db.GetServiceCatalogueForCompany(record.PhysicianId, record.CompanyId).ToList();
            var serviceCatalogueInMemoryQuery = serviceCatalogues.Where(c => c.ServiceId == record.ServiceId);
            if (address != null)
                serviceCatalogueInMemoryQuery.Where(sr => sr.LocationId == address.LocationId);
            var serviceCatalogue = serviceCatalogueInMemoryQuery.FirstOrDefault();
            if (serviceCatalogue == null || !serviceCatalogue.Price.HasValue)
            {
                this.ModelState.AddModelError("ServiceId", "This service has not been offered to this company at this location.");
            }

            // Get the no show and late cancellation rates for this company
            var company = db.Companies.FirstOrDefault(c => c.Id == record.CompanyId);
            var rates = db.GetServiceCatalogueRate(record.PhysicianId, company?.ObjectGuid).First();
            if (rates == null || !rates.NoShowRate.HasValue || !rates.LateCancellationRate.HasValue)
            {
                this.ModelState.AddModelError("ServiceId", "No Show Rates or Late Cancellation Rates have not been set for this company.");
            }

            if (record.InvoiceDetails.Any())
            {
                ModelState.AddModelError("CompanyId", "This request has a pending invoice to the original company. Please delete all invoices and try again.");
            }

            if (!ModelState.IsValid)
            {
                return View("ChangeCompany", form);
            }

            // update the company
            record.CompanyId = form.CompanyId;
            record.ServiceId = form.ServiceId;
            record.ServiceCatalogueId = serviceCatalogue.ServiceCatalogueId;
            record.ServiceCataloguePrice = serviceCatalogue.Price;
            record.ModifiedDate = SystemTime.UtcNow();
            record.ModifiedUser = User.Identity.GetGuidUserId().ToString();

            await db.SaveChangesAsync();
            return RedirectToAction("Details", new { id = record.Id });
        }

        [AuthorizeRole(Feature = Features.ServiceRequest.ChangeProcessTemplate)]
        public ActionResult ChangeProcessTemplate(int id)
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
        public ActionResult ChangeProcessTemplate(ChangeProcessTemplateViewModel model)
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

            foreach (var template in requestTemplate.ServiceRequestTemplateTasks)
            {
                var st = new Orvosi.Data.ServiceRequestTask();
                st.DueDateBase = template.OTask.DueDateBase;
                st.DueDateDiff = template.OTask.DueDateDiff;
                st.Guidance = template.OTask.Guidance;
                st.ObjectGuid = Guid.NewGuid();
                st.ResponsibleRoleId = template.ResponsibleRoleId;
                st.Sequence = template.Sequence;
                st.ShortName = template.OTask.ShortName;
                st.TaskId = template.OTask.Id;
                st.TaskName = template.OTask.Name;
                st.ModifiedDate = SystemTime.Now();
                st.ModifiedUser = User.Identity.Name;
                st.ServiceRequestTemplateTaskId = template.Id;
                st.TaskType = template.OTask.TaskType;
                st.Workload = template.OTask.Workload;
                st.DueDate = GetTaskDueDate(template.DueDateType, serviceRequest.AppointmentDate, serviceRequest.DueDate);
                // Assign tasks to physician and case coordinator to start
                st.AssignedTo = GetTaskAssignment(
                    template.ResponsibleRoleId,
                    serviceRequest.PhysicianId,
                    serviceRequest.CaseCoordinatorId,
                    serviceRequest.IntakeAssistantId,
                    serviceRequest.DocumentReviewerId);

                serviceRequest.ServiceRequestTasks.Add(st);
            }

            serviceRequest.UpdateIsClosed();

            db.SaveChanges();

            // Clone the related tasks
            foreach (var taskTemplate in requestTemplate.ServiceRequestTemplateTasks)
            {
                foreach (var dependentTemplate in taskTemplate.Child)
                {
                    var task = serviceRequest.ServiceRequestTasks.First(srt => srt.ServiceRequestTemplateTaskId == taskTemplate.Id);
                    var dependent = serviceRequest.ServiceRequestTasks.First(srt => srt.ServiceRequestTemplateTaskId == dependentTemplate.Id);
                    task.Child.Add(dependent);
                }
            }

            db.SaveChanges();

            return RedirectToAction("Details", new { id = serviceRequest.Id });
        }

        [AuthorizeRole(Feature = Features.Availability.Reschedule)]
        public ActionResult Reschedule(int id)
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
            await db.SaveChangesAsync();
            return RedirectToAction("Details", new { id = sr.Id });
        }

        [AuthorizeRole(Feature = Features.Availability.BookAssessment)]
        public ActionResult Availability() => View();

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
            bool overrideServiceCatalogueMissingError = false;
            if (!string.IsNullOrEmpty(Request.Form.Get("OverrideServiceCatalogueMissingError")))
            {
                overrideServiceCatalogueMissingError = Request.Form.Get("OverrideServiceCatalogueMissingError").Contains("true");
            }

            // Get the service catalogue
            var address = await db.Addresses.FirstOrDefaultAsync(c => c.Id == sr.AddressId);
            var serviceCatalogues = db.GetServiceCatalogueForCompany(sr.PhysicianId, sr.CompanyId).ToList();
            var serviceCatalogue = serviceCatalogues.FirstOrDefault(c => c.LocationId == address.LocationId && c.ServiceId == sr.ServiceId);
            if (serviceCatalogue == null || !serviceCatalogue.Price.HasValue)
            {
                if (!overrideServiceCatalogueMissingError)
                {
                    this.ModelState.AddModelError("ServiceId", "This service has not been offered to this company at this location.");
                    ViewBag.ServiceIdHasErrors = true;
                }
            }

            // Get the no show and late cancellation rates for this company
            var company = db.Companies.FirstOrDefault(c => c.Id == sr.CompanyId);
            var rates = db.GetServiceCatalogueRate(sr.PhysicianId, company?.ObjectGuid).First();
            if (rates == null || !rates.NoShowRate.HasValue || !rates.LateCancellationRate.HasValue)
            {
                if (!overrideServiceCatalogueMissingError)
                {
                    this.ModelState.AddModelError("ServiceId", "No Show Rates or Late Cancellation Rates have not been set for this company.");
                    ViewBag.ServiceIdHasErrors = true;
                }
            }

            if (ModelState.IsValid)
            {
                var slot = await db.AvailableSlots.FindAsync(sr.AvailableSlotId);
                sr.ServiceId = sr.ServiceId;
                sr.PhysicianId = selectedPhysicianId;
                sr.CompanyId = company.Id;
                sr.AddressId = address.Id;
                sr.ServiceCatalogueId = serviceCatalogue == null ? null : serviceCatalogue.ServiceCatalogueId;
                sr.AvailableSlotId = sr.AvailableSlotId;
                sr.StartTime = slot.StartTime;
                sr.EndTime = slot.EndTime;
                sr.ServiceCataloguePrice = serviceCatalogue == null ? null : serviceCatalogue.Price;
                sr.NoShowRate = rates.NoShowRate;
                sr.LateCancellationRate = rates.LateCancellationRate;
                sr.ModifiedUser = User.Identity.Name;
                sr.ModifiedDate = SystemTime.Now();
                sr.CreatedUser = User.Identity.Name;
                sr.CreatedDate = SystemTime.Now();

                var requestTemplate = await db.ServiceRequestTemplates.FindAsync(sr.ServiceRequestTemplateId);

                foreach (var template in requestTemplate.ServiceRequestTemplateTasks)
                {
                    var st = new Orvosi.Data.ServiceRequestTask();
                    st.DueDateBase = template.OTask.DueDateBase;
                    st.DueDateDiff = template.OTask.DueDateDiff;
                    st.Guidance = template.OTask.Guidance;
                    st.ObjectGuid = Guid.NewGuid();
                    st.ResponsibleRoleId = template.ResponsibleRoleId;
                    st.Sequence = template.Sequence;
                    st.ShortName = template.OTask.ShortName;
                    st.TaskId = template.OTask.Id;
                    st.TaskName = template.OTask.Name;
                    st.ModifiedDate = SystemTime.Now();
                    st.ModifiedUser = User.Identity.Name;
                    // Assign tasks to physician and case coordinator to start
                    st.AssignedTo = (template.ResponsibleRoleId == AspNetRoles.CaseCoordinator ? sr.CaseCoordinatorId : (template.ResponsibleRoleId == AspNetRoles.Physician ? sr.PhysicianId as Nullable<Guid> : null));
                    st.ServiceRequestTemplateTaskId = template.Id;
                    st.TaskType = template.OTask.TaskType;
                    st.Workload = template.OTask.Workload;
                    st.DueDate = GetTaskDueDate(template.DueDateType, sr.AppointmentDate, sr.DueDate);

                    sr.ServiceRequestTasks.Add(st);
                }

                sr.UpdateIsClosed();

                db.ServiceRequests.Add(sr);

                await db.SaveChangesAsync();

                // Clone the related tasks
                foreach (var taskTemplate in requestTemplate.ServiceRequestTemplateTasks)
                {
                    foreach (var dependentTemplate in taskTemplate.Child)
                    {
                        var task = sr.ServiceRequestTasks.First(srt => srt.ServiceRequestTemplateTaskId == taskTemplate.Id);
                        var dependent = sr.ServiceRequestTasks.First(srt => srt.ServiceRequestTemplateTaskId == dependentTemplate.Id);
                        task.Child.Add(dependent);
                    }
                }

                await db.SaveChangesAsync();

                return RedirectToAction("Details", new { id = sr.Id });
            }
            return await Create(selectedAvailableDayId, selectedPhysicianId, ViewBag.ServiceIdHasErrors);

        }

        [HttpGet]
        public ActionResult CreateSuccess(CreateSuccessViewModel obj)
        {
            return View(obj);
        }

        [AuthorizeRole(Feature = Features.ServiceRequest.SubmitRequest)]
        public async Task<ActionResult> CreateAddOn(bool serviceIdHasErrors = false)
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
           
            bool overrideServiceCatalogueMissingError = false;
            if (!string.IsNullOrEmpty(Request.Form.Get("OverrideServiceCatalogueMissingError")))
            {
                overrideServiceCatalogueMissingError = Request.Form.Get("OverrideServiceCatalogueMissingError").Contains("true");
            }

            var company = await db.Companies.FirstOrDefaultAsync(c => c.Id == sr.CompanyId);

            var serviceCatalogues = db.GetServiceCatalogueForCompany(sr.PhysicianId, sr.CompanyId).ToList();

            var serviceCatalogue = serviceCatalogues.SingleOrDefault(c => c.ServiceId == sr.ServiceId && c.LocationId == 0);
            if (serviceCatalogue == null || !serviceCatalogue.Price.HasValue)
            {
                if (!overrideServiceCatalogueMissingError)
                {
                    this.ModelState.AddModelError("ServiceId", "This service has not been offered to this company at this location.");
                    ViewBag.ServiceIdHasErrors = true;
                }
            }

            var rates = db.GetServiceCatalogueRate(sr.PhysicianId, company?.ObjectGuid).First();
            if (rates == null || !rates.NoShowRate.HasValue || !rates.LateCancellationRate.HasValue)
            {
                this.ModelState.AddModelError("ServiceId", "No Show Rates or Late Cancellation Rates have not been set for this company.");
            }

            if (ModelState.IsValid)
            {
                sr.ServiceCatalogueId = serviceCatalogue == null ? null : serviceCatalogue.ServiceCatalogueId;
                sr.DueDate = sr.DueDate;
                sr.CaseCoordinatorId = sr.CaseCoordinatorId;
                sr.ClaimantName = sr.ClaimantName;
                sr.CompanyReferenceId = sr.CompanyReferenceId;
                sr.RequestedDate = sr.RequestedDate;
                sr.CompanyId = sr.CompanyId;
                sr.PhysicianId = sr.PhysicianId;
                sr.ServiceId = sr.ServiceId;
                sr.ServiceCataloguePrice = serviceCatalogue == null ? null : serviceCatalogue.Price;
                sr.NoShowRate = rates.NoShowRate;
                sr.LateCancellationRate = rates.LateCancellationRate;
                sr.ServiceRequestTemplateId = sr.ServiceRequestTemplateId;
                sr.ModifiedUser = User.Identity.Name;
                sr.ModifiedDate = SystemTime.Now();
                sr.CreatedUser = User.Identity.Name;
                sr.CreatedDate = SystemTime.Now();

                var requestTemplate = await db.ServiceRequestTemplates.FindAsync(sr.ServiceRequestTemplateId);

                foreach (var template in requestTemplate.ServiceRequestTemplateTasks)
                {
                    var st = new Orvosi.Data.ServiceRequestTask();
                    st.DueDateBase = template.OTask.DueDateBase;
                    st.DueDateDiff = template.OTask.DueDateDiff;
                    st.Guidance = template.OTask.Guidance;
                    st.ObjectGuid = Guid.NewGuid();
                    st.ResponsibleRoleId = template.ResponsibleRoleId;
                    st.Sequence = template.Sequence;
                    st.ShortName = template.OTask.ShortName;
                    st.TaskId = template.OTask.Id;
                    st.TaskName = template.OTask.Name;
                    st.ModifiedDate = SystemTime.Now();
                    st.ModifiedUser = User.Identity.Name;
                    // Assign tasks to physician and case coordinator to start
                    st.AssignedTo = (template.ResponsibleRoleId == AspNetRoles.CaseCoordinator ? sr.CaseCoordinatorId : (template.ResponsibleRoleId == AspNetRoles.Physician ? sr.PhysicianId as Nullable<Guid> : null));
                    st.ServiceRequestTemplateTaskId = template.Id;
                    st.TaskType = template.OTask.TaskType;
                    st.Workload = template.OTask.Workload;

                    sr.ServiceRequestTasks.Add(st);
                }

                sr.UpdateIsClosed();

                db.ServiceRequests.Add(sr);

                await db.SaveChangesAsync();

                // Clone the related tasks
                foreach (var taskTemplate in requestTemplate.ServiceRequestTemplateTasks)
                {
                    foreach (var dependentTemplate in taskTemplate.Child)
                    {
                        var task = sr.ServiceRequestTasks.First(srt => srt.ServiceRequestTemplateTaskId == taskTemplate.Id);
                        var dependent = sr.ServiceRequestTasks.First(srt => srt.ServiceRequestTemplateTaskId == dependentTemplate.Id);
                        task.Child.Add(dependent);
                    }
                }

                await db.SaveChangesAsync();

                return RedirectToAction("Details", new { id = sr.Id });
            }

            return await CreateAddOn(ViewBag.ServiceIdHasErrors);
        }

        [HttpGet]
        public ActionResult RefreshCompanyDropDown(Guid physicianId)
        {
            var viewDataService = new ViewDataService(User.Identity);
            var companySelectList = viewDataService.GetPhysicianCompanySelectList(physicianId);
            return PartialView("_CreateAddOnCompanyDropDown", companySelectList);
        }

        [HttpGet]
        public ActionResult RefreshServiceDropDown(Guid physicianId)
        {
            var viewDataService = new ViewDataService(User.Identity);
            var selectList = viewDataService.GetPhysicianServiceSelectList(physicianId);
            return PartialView("_CreateAddOnServiceDropDown", selectList);
        }

        [HttpGet]
        public ActionResult RefreshCaseCoordinatorDropDown(Guid physicianId)
        {
            var viewDataService = new ViewDataService(User.Identity);
            var selectList = viewDataService.GetPhysicianCaseCoordinatorSelectList(physicianId);
            return PartialView("_CreateAddOnCaseCoordinatorDropDown", selectList);
        }

        [HttpGet]
        public ActionResult RefreshProcessTemplateDropDown(Guid physicianId)
        {
            var viewDataService = new ViewDataService(User.Identity);
            var selectList = viewDataService.GetPhysicianProcessTemplateSelectList(physicianId);
            return PartialView("_CreateAddOnProcessTemplateDropDown", selectList);
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
                obj.DueDate = sr.DueDate;
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

        [AuthorizeRole(Feature = Features.ServiceRequest.Cancel)]
        public async Task<ActionResult> Cancel(int serviceRequestId)
        {
            ServiceRequest serviceRequest = await db.ServiceRequests.FindAsync(serviceRequestId);

            if (serviceRequest == null)
            {
                return HttpNotFound();
            }
            // TODO: Update the calendar and dropbox folder if appropriate.

            return View(serviceRequest);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[AuthorizeRole(Feature = Features.ServiceRequest.Cancel)]
        //public async Task<ActionResult> Cancel(CancellationForm form)
        //{
        //    var serviceRequest = await db.ServiceRequests.FindAsync(form.ServiceRequestId);
        //    if (serviceRequest == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        serviceRequest.CancelledDate = form.CancelledDate;
        //        serviceRequest.IsLateCancellation = form.IsLate == "on" ? true : false;
        //        serviceRequest.Notes = string.Concat(serviceRequest.Notes, '\n', form.Notes);

        //        serviceRequest.UpdateToDoTasksToObsolete();

        //        serviceRequest.UpdateIsClosed();

        //        serviceRequest.UpdateInvoice(db);

        //        await db.SaveChangesAsync();

        //        return RedirectToAction("Details", new { id = serviceRequest.Id });
        //    }
        //    return View(serviceRequest);
        //}

        [AuthorizeRole(Feature = Features.ServiceRequest.Cancel)]
        public async Task<ActionResult> UndoCancel(int serviceRequestId)
        {
            var serviceRequest = await db.ServiceRequests.FindAsync(serviceRequestId);
            if (serviceRequest == null)
            {
                return HttpNotFound();
            }

            serviceRequest.CancelledDate = null;
            serviceRequest.IsLateCancellation = false;
            serviceRequest.Notes = string.Concat(serviceRequest.Notes, '\n', "Cancellation Undone");

            serviceRequest.UpdateObsoleteTasksToToDo();

            serviceRequest.UpdateIsClosed();

            serviceRequest.UpdateInvoice(db);

            await db.SaveChangesAsync();

            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.ToggleNoShow)]
        public async Task<ActionResult> ToggleNoShow(NoShowForm form)
        {
            await service.NoShow(form);

            return Json(new
            {
                serviceRequestId = form.ServiceRequestId
            });
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.ToggleNoShow)]
        public async Task<ActionResult> ToggleOnHold(OnHoldForm form)
        {
            await service.OnHold(form);

            return Json(new
            {
                serviceRequestId = form.ServiceRequestId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(Feature = Features.ServiceRequest.ToggleNoShow)]
        public async Task<ActionResult> NoShow()
        {
            var serviceRequestId = int.Parse(Request.Form.Get("ServiceRequestId"));
            var serviceRequest = await db.ServiceRequests.FindAsync(serviceRequestId);
            if (serviceRequest == null)
            {
                return HttpNotFound();
            }

            serviceRequest.IsNoShow = true;

            serviceRequest.UpdateToDoTasksToObsolete();

            serviceRequest.UpdateIsClosed();

            serviceRequest.UpdateInvoice(db);

            await db.SaveChangesAsync();

            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(Feature = Features.ServiceRequest.ToggleNoShow)]
        public async Task<ActionResult> UndoNoShow()
        {
            var serviceRequestId = int.Parse(Request.Form.Get("ServiceRequestId"));
            var serviceRequest = await db.ServiceRequests.FindAsync(serviceRequestId);
            if (serviceRequest == null)
            {
                return HttpNotFound();
            }

            serviceRequest.IsNoShow = false;

            serviceRequest.UpdateObsoleteTasksToToDo();

            serviceRequest.UpdateIsClosed();

            serviceRequest.UpdateInvoice(db);

            await db.SaveChangesAsync();

            return Redirect(Request.UrlReferrer.ToString());
        }

        [AuthorizeRole(Feature = Features.ServiceRequest.ViewInvoiceNote)]
        public async Task<ActionResult> RefreshNote(int serviceRequestId)
        {
            var context = new Orvosi.Data.OrvosiDbContext();
            var note = await context.ServiceRequests.FindAsync(serviceRequestId);
            return PartialView("_Note", new NoteViewModel() { ServiceRequestId = note.Id, Note = note.Notes });
        }

        #region Box

        public async Task<ActionResult> BoxManager(int serviceRequestId)
        {
            // Get the box folder id for the case
            var serviceRequest = db.ServiceRequests
                .WithId(serviceRequestId)
                .Select(ServiceRequestProjections.ForBoxManager())
                .Single();

            if (string.IsNullOrEmpty(serviceRequest.BoxCaseFolderId))
            {
                ModelState.AddModelError("", "BoxCaseFolderId is set to null.");
                return new HttpNotFoundResult();
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
                    boxResource.BoxFolder = await box.GetFolder(serviceRequest.BoxCaseFolderId, resource.BoxUserId);
                }
                resources.Add(boxResource);
            }

            var vm = new BoxManagerViewModel();
            vm.ServiceRequestId = serviceRequestId;
            vm.Reconciliations = result;
            vm.Resources = resources;
            vm.BoxFolderCollaborations = boxFolderCollaborations;
            vm.BoxFolder = boxFolder;
            vm.ExpectedFolderName = serviceRequest.CaseFolderName;
            return View(vm);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest_Box.UpdateFolder)]
        public ActionResult UpdateBoxCaseFolderName(int serviceRequestId)
        {
            // Get the request
            var serviceRequest = db.ServiceRequests
                .WithId(serviceRequestId)
                .Select(ServiceRequestProjections.ForBoxManager())
                .Single();

            var box = new BoxManager();
            var caseFolder = box.RenameCaseFolder(serviceRequest.BoxCaseFolderId, serviceRequest.CaseFolderName);
            
            // Redirect to display the Box Folder
            return RedirectToAction("Details", new { serviceRequestId = serviceRequestId });
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest_Box.CreateFolder)]
        public ActionResult CreateBoxCaseFolder(int ServiceRequestId)
        {
            // Get the request
            var serviceRequest = db.ServiceRequests
                .WithId(ServiceRequestId)
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
            var sr = db.ServiceRequests.Find(ServiceRequestId);
            sr.BoxCaseFolderId = caseFolder.Id;
            db.SaveChanges();

            // Redirect to display the Box Folder
            return RedirectToAction("Details", new { id = ServiceRequestId });
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest_Box.AddCollaborator)]
        public ActionResult ShareBoxFolder(int ServiceRequestId, string FolderId, Guid UserId)
        {
            var resources = db.GetServiceRequestResources(ServiceRequestId);
            var resource = resources.Single(r => r.Id == UserId);

            var box = new BoxManager();
            var collaboration = box.AddCollaboration(FolderId, resource.BoxUserId, resource.Email);
            db.ServiceRequestBoxCollaborations.Add(
                new ServiceRequestBoxCollaboration()
                {
                    BoxCollaborationId = collaboration.Id,
                    ServiceRequestId = ServiceRequestId,
                    UserId = UserId,
                    ModifiedUser = User.Identity.Name,
                    ModifiedDate = SystemTime.Now()
                }
            );
            db.SaveChanges();

            box.UpdateSyncState(collaboration.Item.Id, resource.BoxUserId, BoxSyncStateType.synced);

            return RedirectToAction("BoxManager", new { serviceRequestId = ServiceRequestId });

        }

        [AuthorizeRole(Feature = Features.ServiceRequest_Box.RemoveCollaborator)]
        public ActionResult UnshareBoxFolder(int ServiceRequestId, string CollaborationId)
        {

            var box = new BoxManager();
            var success = box.RemoveCollaboration(CollaborationId);

            if (success)
            {
                var collaboration = db.ServiceRequestBoxCollaborations.SingleOrDefault(b => b.BoxCollaborationId == CollaborationId);
                db.ServiceRequestBoxCollaborations.Remove(collaboration);
                db.SaveChanges();
            }
            return RedirectToAction("BoxManager", new { serviceRequestId = ServiceRequestId });

        }

        public ActionResult AcceptBoxFolder(int ServiceRequestId, Guid UserId, string CollaborationId)
        {
            string boxUserId;

            var user = db.Profiles.Single(p => p.Id == UserId);
            boxUserId = user.BoxUserId;
            var box = new BoxManager();
            var collaboration = box.AcceptCollaboration(CollaborationId, boxUserId);
            return RedirectToAction("Details", new { id = ServiceRequestId });
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest_Box.SyncUnsyncCollaborator)]
        public ActionResult UnsyncBoxFolder(int ServiceRequestId, string FolderId, string BoxUserId)
        {
            var box = new BoxManager();
            box.UpdateSyncState(FolderId, BoxUserId, BoxSyncStateType.not_synced);
            return RedirectToAction("BoxManager", new { serviceRequestId = ServiceRequestId });
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest_Box.SyncUnsyncCollaborator)]
        public ActionResult SyncBoxFolder(int ServiceRequestId, string FolderId, string BoxUserId)
        {
            var box = new BoxManager();
            box.UpdateSyncState(FolderId, BoxUserId, BoxSyncStateType.synced);
            return RedirectToAction("BoxManager", new { serviceRequestId = ServiceRequestId });
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

        private DateTime? GetTaskDueDate(string dueDateType, DateTime? appointmentDate, DateTime? dueDate)
        {
            switch (dueDateType)
            {
                case DueDateTypes.AppointmentDate:
                    return appointmentDate;
                case DueDateTypes.ReportDueDate:
                    return dueDate;
                default:
                    return null;
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
        public async Task<ActionResult> GetAllServiceRequestIds()
        {
            var ids = db.ServiceRequests.Select(sr => sr.Id).OrderBy(sr => sr).ToList();
            return Json(ids, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.UpdateTaskStatus)]
        public async Task<ActionResult> UpdateDependentTaskStatuses(int serviceRequestId)
        {
            var service = new WorkService(db, User.Identity);

            await service.UpdateDependentTaskStatuses(serviceRequestId);

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