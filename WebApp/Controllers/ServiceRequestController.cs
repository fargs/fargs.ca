using System;
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

namespace WebApp.Controllers
{
    public delegate void NoShowToggledHandler(object sender, EventArgs e);

    [Authorize]
    public class ServiceRequestController : Controller
    {
        private OrvosiDbContext ctx = new OrvosiDbContext();
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

        public async Task<ActionResult> Index(FilterArgs filterArgs)
        {
            var vm = new IndexViewModel();
            // get the user
            filterArgs.ShowAll = (filterArgs.ShowAll ?? true);
            filterArgs.Sort = (filterArgs.Sort ?? "Oldest");
            filterArgs.StatusId = (filterArgs.StatusId ?? e.ServiceRequestStatus.Open);

            var sr = ctx.ServiceRequestViews.AsQueryable();

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
            vm.ServiceRequestTasks = await ctx.ServiceRequestTasks
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

            var list = await ctx.ServiceRequests
                .Where(sr => sr.CaseCoordinatorId == userId || sr.IntakeAssistantId == userId || sr.DocumentReviewerId == userId || sr.PhysicianId == userId)
                .ToListAsync();

            return View(list);
        }

        // GET: ServiceRequest/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var now = SystemTime.Now();
            var userId = User.Identity.GetGuidUserId();

            var vm = new DetailsViewModel();

            vm.ServiceRequest = ctx.ServiceRequests
                .Where(sr => sr.Id == id)
                .Select(ServiceRequestProjections.AllDetails(userId, now))
                .SingleOrDefault();

            if (vm.ServiceRequest == null)
            {
                return HttpNotFound();
            }

            //vm.UserSelectList = (from user in ctx.AspNetUsers
            vm.UserSelectList = (from user in ctx.AspNetUsers
                                 from userRole in ctx.AspNetUserRoles
                                 from role in ctx.AspNetRoles
                                 where user.Id == userRole.UserId && role.Id == userRole.RoleId && (role.Id == AspNetRoles.SuperAdmin || role.Id == AspNetRoles.CaseCoordinator || role.Id == AspNetRoles.DocumentReviewer || role.Id == AspNetRoles.IntakeAssistant || user.Id == vm.ServiceRequest.PhysicianId)
                                 select new SelectListItem
                                 {
                                     Text = user.FirstName + " " + user.LastName,
                                     Value = user.Id.ToString(),
                                     Group = new SelectListGroup() { Name = role.Name }
                                 }).ToList();

            return View(vm);
        }

        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public ActionResult ChangeCompany(int id)
        {
            var vm = ctx.ServiceRequests
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

            vm.CompanySelectList = ctx.Companies
                .Where(c => c.IsParent == false)
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Group = new SelectListGroup() { Name = c.ParentId.ToString() }
                })
                .ToList();

            vm.ServiceSelectList = ctx.Services.Where(c => c.ServicePortfolioId == e.ServicePortfolios.Physician && c.ServiceCategoryId == e.ServiceCategories.IndependentMedicalExam)
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
        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> ChangeCompany(ChangeCompanyFormViewModel form)
        {
            var record = ctx.ServiceRequests
                .Where(sr => sr.Id == form.ServiceRequestId)
                .Single();

            // Get the service catalogue
            var address = await ctx.Addresses.FirstOrDefaultAsync(c => c.Id == record.AddressId);
            var serviceCatalogues = ctx.GetServiceCatalogueForCompany(record.PhysicianId, record.CompanyId).ToList();
            var serviceCatalogue = serviceCatalogues.FirstOrDefault(c => c.LocationId == address.LocationId && c.ServiceId == record.ServiceId);
            if (serviceCatalogue == null || !serviceCatalogue.Price.HasValue)
            {
                this.ModelState.AddModelError("ServiceId", "This service has not been offered to this company at this location.");
            }

            // Get the no show and late cancellation rates for this company
            var company = ctx.Companies.FirstOrDefault(c => c.Id == record.CompanyId);
            var rates = ctx.GetServiceCatalogueRate(record.PhysicianId, company?.ObjectGuid).First();
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

            await ctx.SaveChangesAsync();
            return RedirectToAction("Details", new { id = record.Id });
        }

        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public ActionResult Reschedule(int id)
        {
            var model = ctx.ServiceRequests.Single(c => c.Id == id);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> Reschedule(ServiceRequest serviceRequest)
        {
            var sr = ctx.ServiceRequests.Single(c => c.Id == serviceRequest.Id);
            var slot = ctx.AvailableSlots.Single(c => c.Id == serviceRequest.AvailableSlotId);
            sr.AvailableSlotId = serviceRequest.AvailableSlotId;
            sr.AppointmentDate = slot.AvailableDay.Day;
            sr.StartTime = slot.StartTime;
            sr.EndTime = slot.EndTime;
            sr.AddressId = serviceRequest.AddressId;
            sr.ModifiedDate = SystemTime.Now();
            sr.ModifiedUser = User.Identity.Name;
            await ctx.SaveChangesAsync();
            return RedirectToAction("Details", new { id = sr.Id });
        }

        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public ActionResult Availability() => View();

        [HttpPost]
        [Authorize(Roles = "Case Coordinator, Super Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Availability(AvailabilityForm form)
        {
            var ad = await ctx.AvailableDays
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
        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> Create(int availableDayId, Guid physicianId, bool serviceIdHasErrors = false)
        {
            var availableDay = await ctx.AvailableDays.FindAsync(availableDayId);
            var physician = await ctx.Physicians.FindAsync(physicianId);
            var serviceRequest = new Orvosi.Data.ServiceRequest();
            serviceRequest.PhysicianId = physicianId;
            serviceRequest.AppointmentDate = availableDay.Day;

            var vm = new CreateViewModel();

            vm.SelectedAvailableDay = availableDay;
            vm.SelectedPhysician = physician;
            vm.ServiceRequest = serviceRequest;

            vm.ServiceSelectList = ctx.Services.Where(c => c.ServicePortfolioId == e.ServicePortfolios.Physician && c.ServiceCategoryId == e.ServiceCategories.IndependentMedicalExam)
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Group = new SelectListGroup() { Name = c.ServiceCategory.Name }
                });

            vm.AvailableSlotSelectList = ctx.AvailableSlots
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

            vm.CompanySelectList = ctx.Companies
                .Where(c => c.IsParent == false)
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Group = new SelectListGroup() { Name = c.ParentId.ToString() }
                });

            var addresses = new DataHelper().LoadAddressesWithOwner(ctx);
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

            vm.StaffSelectList = ctx.AspNetUsers
                .Where(u => u.AspNetUserRoles.FirstOrDefault().RoleId == AspNetRoles.CaseCoordinator || u.AspNetUserRoles.FirstOrDefault().RoleId == AspNetRoles.SuperAdmin)
                .AsEnumerable()
                .Select(c => new SelectListItem()
                {
                    Text = c.GetDisplayName(),
                    Value = c.Id.ToString()
                });

            vm.ServiceRequestTemplateSelectList = ctx.ServiceRequestTemplates
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
        [Authorize(Roles = "Case Coordinator, Super Admin")]
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
            var address = await ctx.Addresses.FirstOrDefaultAsync(c => c.Id == sr.AddressId);
            var serviceCatalogues = ctx.GetServiceCatalogueForCompany(sr.PhysicianId, sr.CompanyId).ToList();
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
            var company = ctx.Companies.FirstOrDefault(c => c.Id == sr.CompanyId);
            var rates = ctx.GetServiceCatalogueRate(sr.PhysicianId, company?.ObjectGuid).First();
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
                var slot = await ctx.AvailableSlots.FindAsync(sr.AvailableSlotId);
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

                var requestTemplate = await ctx.ServiceRequestTemplates.FindAsync(sr.ServiceRequestTemplateId);

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

                ctx.ServiceRequests.Add(sr);

                await ctx.SaveChangesAsync();

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

                await ctx.SaveChangesAsync();

                return RedirectToAction("Details", new { id = sr.Id });
            }
            return await Create(selectedAvailableDayId, selectedPhysicianId, ViewBag.ServiceIdHasErrors);

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

        [HttpGet]
        public ActionResult CreateSuccess(CreateSuccessViewModel obj)
        {
            return View(obj);
        }

        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public ActionResult CreateAddOn(bool serviceIdHasErrors = false)
        {
            var vm = new CreateViewModel();

            vm.ServiceSelectList = ctx.Services.Where(c => c.ServicePortfolioId == e.ServicePortfolios.Physician && c.ServiceCategoryId == ServiceCategories.AddOn)
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Group = new SelectListGroup() { Name = c.ServiceCategory.Name }
                });

            vm.CompanySelectList = ctx.Companies
                .Where(c => c.IsParent == false)
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Group = new SelectListGroup() { Name = c.ParentId.ToString() }
                });

            vm.PhysicianSelectList = ctx.AspNetUsers
                .Where(u => u.AspNetUserRoles.FirstOrDefault().AspNetRole.RoleCategoryId == e.RoleCategories.Physician)
                .AsEnumerable()
                .Select(c => new SelectListItem()
                {
                    Text = c.GetDisplayName(),
                    Value = c.Id.ToString()
                });

            vm.StaffSelectList = ctx.AspNetUsers
                .Where(u => u.AspNetUserRoles.FirstOrDefault().RoleId == AspNetRoles.CaseCoordinator || u.AspNetUserRoles.FirstOrDefault().RoleId == AspNetRoles.SuperAdmin)
                .AsEnumerable()
                .Select(c => new SelectListItem()
                {
                    Text = c.GetDisplayName(),
                    Value = c.Id.ToString()
                });

            vm.ServiceRequestTemplateSelectList = ctx.ServiceRequestTemplates
                .AsEnumerable()
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });

            ViewBag.ServiceIdHasErrors = serviceIdHasErrors;

            return View(vm);
        }

        [Authorize(Roles = "Case Coordinator, Super Admin")]
        [HttpPost]
        public async Task<ActionResult> CreateAddOn(Orvosi.Data.ServiceRequest sr)
        {
            var serviceCategoryId = ctx.Services.Single(s => s.Id == sr.ServiceId).ServiceCategoryId;

            if (serviceCategoryId != ServiceCategories.AddOn)
            {
                this.ModelState.AddModelError("ServiceId", "Service must be an AddOn.");
            }
           
            bool overrideServiceCatalogueMissingError = false;
            if (!string.IsNullOrEmpty(Request.Form.Get("OverrideServiceCatalogueMissingError")))
            {
                overrideServiceCatalogueMissingError = Request.Form.Get("OverrideServiceCatalogueMissingError").Contains("true");
            }

            var company = await ctx.Companies.FirstOrDefaultAsync(c => c.Id == sr.CompanyId);

            var serviceCatalogues = ctx.GetServiceCatalogueForCompany(sr.PhysicianId, sr.CompanyId).ToList();

            var serviceCatalogue = serviceCatalogues.SingleOrDefault(c => c.ServiceId == sr.ServiceId && c.LocationId == 0);
            if (serviceCatalogue == null || !serviceCatalogue.Price.HasValue)
            {
                if (!overrideServiceCatalogueMissingError)
                {
                    this.ModelState.AddModelError("ServiceId", "This service has not been offered to this company at this location.");
                    ViewBag.ServiceIdHasErrors = true;
                }
            }

            var rates = ctx.GetServiceCatalogueRate(sr.PhysicianId, company?.ObjectGuid).First();
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

                var requestTemplate = await ctx.ServiceRequestTemplates.FindAsync(sr.ServiceRequestTemplateId);

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

                ctx.ServiceRequests.Add(sr);

                await ctx.SaveChangesAsync();

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

                await ctx.SaveChangesAsync();

                return RedirectToAction("Details", new { id = sr.Id });
            }

            return CreateAddOn(ViewBag.ServiceIdHasErrors);
        }

        private Guid? GetTaskAssignment(Guid? responsibleRoleId, Guid physicianId, Guid? caseCoordinatorId)
        {
            if (responsibleRoleId == AspNetRoles.Physician)
            {
                return physicianId;
            }
            else if (responsibleRoleId == AspNetRoles.CaseCoordinator)
            {
                return caseCoordinatorId;
            }
            else
            {
                return null;
            }
        }

        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> ResourceAssignment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var serviceRequest = await ctx.ServiceRequests.FindAsync(id);

            if (serviceRequest == null)
            {
                return HttpNotFound();
            }

            var userSelectList = ctx.AspNetUsers
                .Where(u => u.AspNetUserRoles.Any() && u.AspNetUserRoles.Any(ur => ur.AspNetRole.RoleCategoryId != e.RoleCategories.Company))
                .AsEnumerable()
                .Select(c => new SelectListItem()
                {
                    Text = c.GetDisplayName(),
                    Value = c.Id.ToString(),
                    Group = new SelectListGroup() { Name = c.AspNetUserRoles.FirstOrDefault().AspNetRole.Name }
                });

            var vm = new ResourceAssignmentViewModel()
            {
                ServiceRequestId = serviceRequest.Id,
                CaseCoordinatorId = serviceRequest.CaseCoordinatorId,
                DocumentReviewerId = serviceRequest.DocumentReviewerId,
                IntakeAssistantId = serviceRequest.IntakeAssistantId,
                UserSelectList = userSelectList
            };

            // TODO: Update the calendar and dropbox folder if appropriate.

            return View(vm);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> ResourceAssignment(ResourceAssignmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                // get the tracked object from the database
                var obj = await ctx.ServiceRequests.FindAsync(model.ServiceRequestId);

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

                await ctx.SaveChangesAsync();

                return RedirectToAction("Details", new { id = obj.Id });
            }
            return View(model);
        }

        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ServiceRequest serviceRequest = await ctx.ServiceRequests.FindAsync(id);

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
        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> Edit(EditViewModel sr)
        {
            if (ModelState.IsValid)
            {
                // get the tracked object from the database
                var obj = await ctx.ServiceRequests.SingleOrDefaultAsync(c => c.Id == sr.ServiceRequestId);

                // update the resource assignments
                obj.ClaimantName = sr.ClaimantName;
                obj.CompanyReferenceId = sr.CompanyReferenceId;
                obj.DueDate = sr.DueDate;
                obj.Notes = sr.AdditionalNotes;
                obj.RequestedBy = sr.RequestedBy;
                obj.RequestedDate = sr.RequestedDate;
                obj.ModifiedDate = SystemTime.Now();
                obj.ModifiedUser = User.Identity.GetGuidUserId().ToString();

                await ctx.SaveChangesAsync();

                return RedirectToAction("Details", new { id = sr.ServiceRequestId });
            }
            return View(sr);
        }

        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceRequest serviceRequest = await ctx.ServiceRequests.FindAsync(id);
            if (serviceRequest == null)
            {
                return HttpNotFound();
            }
            return View(serviceRequest);
        }

        // POST: ServiceRequest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ServiceRequest serviceRequest = await ctx.ServiceRequests.FindAsync(id);
            ctx.ServiceRequests.Remove(serviceRequest);
            await ctx.SaveChangesAsync();

            //TODO: Unshare and delete folders from dropbox
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> Cancel(int serviceRequestId)
        {
            ServiceRequest serviceRequest = await ctx.ServiceRequests.FindAsync(serviceRequestId);

            if (serviceRequest == null)
            {
                return HttpNotFound();
            }
            // TODO: Update the calendar and dropbox folder if appropriate.

            return View(serviceRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> Cancel(CancellationForm form)
        {
            var serviceRequest = await ctx.ServiceRequests.FindAsync(form.ServiceRequestId);
            if (serviceRequest == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                serviceRequest.CancelledDate = form.CancelledDate;
                serviceRequest.IsLateCancellation = form.IsLate == "on" ? true : false;
                serviceRequest.Notes = string.Concat(serviceRequest.Notes, '\n', form.Notes);

                serviceRequest.MarkActiveTasksAsObsolete();

                serviceRequest.UpdateIsClosed();

                serviceRequest.UpdateInvoice(ctx);

                await ctx.SaveChangesAsync();

                return RedirectToAction("Details", new { id = serviceRequest.Id });
            }
            return View(serviceRequest);
        }

        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> UndoCancel(int serviceRequestId)
        {
            var serviceRequest = await ctx.ServiceRequests.FindAsync(serviceRequestId);
            if (serviceRequest == null)
            {
                return HttpNotFound();
            }

            serviceRequest.CancelledDate = null;
            serviceRequest.IsLateCancellation = false;
            serviceRequest.Notes = string.Concat(serviceRequest.Notes, '\n', "Cancellation Undone");

            serviceRequest.MarkObsoleteTasksAsActive();

            serviceRequest.UpdateIsClosed();

            serviceRequest.UpdateInvoice(ctx);

            await ctx.SaveChangesAsync();

            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NoShow()
        {
            var serviceRequestId = int.Parse(Request.Form.Get("ServiceRequestId"));
            var serviceRequest = await ctx.ServiceRequests.FindAsync(serviceRequestId);
            if (serviceRequest == null)
            {
                return HttpNotFound();
            }

            serviceRequest.IsNoShow = true;

            serviceRequest.MarkActiveTasksAsObsolete();

            serviceRequest.UpdateIsClosed();

            serviceRequest.UpdateInvoice(ctx);

            await ctx.SaveChangesAsync();

            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UndoNoShow()
        {
            var serviceRequestId = int.Parse(Request.Form.Get("ServiceRequestId"));
            var serviceRequest = await ctx.ServiceRequests.FindAsync(serviceRequestId);
            if (serviceRequest == null)
            {
                return HttpNotFound();
            }

            serviceRequest.IsNoShow = false;

            serviceRequest.MarkObsoleteTasksAsActive();

            serviceRequest.UpdateIsClosed();

            serviceRequest.UpdateInvoice(ctx);

            await ctx.SaveChangesAsync();

            return Redirect(Request.UrlReferrer.ToString());
        }

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
            var serviceRequest = ctx.ServiceRequests.Find(serviceRequestId);

            if (string.IsNullOrEmpty(serviceRequest.BoxCaseFolderId))
            {
                ModelState.AddModelError("", "BoxCaseFolderId is set to null.");
                return new HttpNotFoundResult();
            }

            var orvosiBoxFolderCollaborations = serviceRequest
                .ServiceRequestBoxCollaborations
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

            var orvosiResources = serviceRequest
                .ServiceRequestTasks
                    .Where(t => t.AssignedTo.HasValue)
                    .Select(sr => sr.AspNetUser_AssignedTo)
                    .Distinct();

            var resources = new List<BoxResource>();
            foreach (var resource in orvosiResources)
            {
                var boxResource = new BoxResource() { Resource = resource };
                boxResource.BoxFolder = await box.GetFolder(serviceRequest.BoxCaseFolderId, resource.BoxUserId);
                resources.Add(boxResource);
            }

            var vm = new BoxManagerViewModel();
            vm.ServiceRequestId = serviceRequestId;
            vm.Reconciliations = result;
            vm.Resources = resources;
            vm.BoxFolderCollaborations = boxFolderCollaborations;
            vm.BoxFolder = boxFolder;
            vm.ExpectedFolderName = serviceRequest.GetCaseFolderName();
            return View(vm);
        }

        [HttpPost]
        public ActionResult UpdateBoxCaseFolderName(int serviceRequestId)
        {
            // Get the request
            var request = ctx.ServiceRequests.Single(sr => sr.Id == serviceRequestId);

            var box = new BoxManager();
            var caseFolder = box.RenameCaseFolder(request.BoxCaseFolderId, request.GetCaseFolderName());
            
            // Redirect to display the Box Folder
            return RedirectToAction("Details", new { serviceRequestId = serviceRequestId });
        }

        [HttpPost]
        public ActionResult CreateBoxCaseFolder(int ServiceRequestId)
        {
            // Get the request
            var request = ctx.ServiceRequests.Single(sr => sr.Id == ServiceRequestId);

            // Get the request and assert they have a Box Folder Id
            var physician = ctx.AspNetUsers.Single(p => p.Id == request.PhysicianId);
            //var physicianBoxFolderId = "7027883033"; // This overrides to HanSolo box folder while developing. Comment out for production.
            var physicianBoxFolderId = physician.BoxFolderId;
            if (string.IsNullOrEmpty(physicianBoxFolderId))
                return PartialView("Error", "Physician does not have a cases folder setup in Box.");

            // Create the case folder
            var box = new BoxManager();
            BoxFolder caseFolder;
            if (request.Service.ServiceCategoryId == ServiceCategories.AddOn)
            {
                var province = ctx.GetCompanyProvince(request.CompanyId).FirstOrDefault();
                if (province == null)
                {
                    province = new GetCompanyProvinceReturnModel() { ProvinceID = 0, ProvinceName = "Ontario" };
                }
                caseFolder = box.CreateAddOnFolder(physicianBoxFolderId, province.ProvinceName, request.DueDate.Value, request.GetCaseFolderName(), physician.Physician.BoxAddOnTemplateFolderId);
            }
            else
            {
                // Get the province which is used in the case folder path
                var province = ctx.Provinces.Single(p => p.Id == request.Address.ProvinceId);
                caseFolder = box.CreateCaseFolder(physicianBoxFolderId, province.ProvinceName, request.AppointmentDate.Value, request.GetCaseFolderName(), physician.Physician.BoxCaseTemplateFolderId);
            }

            // Persist the new case folder Id to the database.
            request.BoxCaseFolderId = caseFolder.Id;
            ctx.SaveChanges();

            // Redirect to display the Box Folder
            return RedirectToAction("Details", new { id = ServiceRequestId });
        }

        [HttpPost]
        public ActionResult ShareBoxFolder(int ServiceRequestId, string FolderId, Guid UserId)
        {
            var resources = ctx.GetServiceRequestResources(ServiceRequestId);
            var resource = resources.Single(r => r.Id == UserId);

            var box = new BoxManager();
            var collaboration = box.AddCollaboration(FolderId, resource.BoxUserId, resource.Email);
            ctx.ServiceRequestBoxCollaborations.Add(
                new ServiceRequestBoxCollaboration()
                {
                    BoxCollaborationId = collaboration.Id,
                    ServiceRequestId = ServiceRequestId,
                    UserId = UserId,
                    ModifiedUser = User.Identity.Name,
                    ModifiedDate = SystemTime.Now()
                }
            );
            ctx.SaveChanges();

            box.UpdateSyncState(collaboration.Item.Id, resource.BoxUserId, BoxSyncStateType.synced);

            return RedirectToAction("BoxManager", new { serviceRequestId = ServiceRequestId });

        }

        public ActionResult UnshareBoxFolder(int ServiceRequestId, string CollaborationId)
        {

            var box = new BoxManager();
            var success = box.RemoveCollaboration(CollaborationId);

            if (success)
            {
                var collaboration = ctx.ServiceRequestBoxCollaborations.SingleOrDefault(b => b.BoxCollaborationId == CollaborationId);
                ctx.ServiceRequestBoxCollaborations.Remove(collaboration);
                ctx.SaveChanges();
            }
            return RedirectToAction("BoxManager", new { serviceRequestId = ServiceRequestId });

        }

        public ActionResult AcceptBoxFolder(int ServiceRequestId, Guid UserId, string CollaborationId)
        {
            string boxUserId;

            var user = ctx.Profiles.Single(p => p.Id == UserId);
            boxUserId = user.BoxUserId;
            var box = new BoxManager();
            var collaboration = box.AcceptCollaboration(CollaborationId, boxUserId);
            return RedirectToAction("Details", new { id = ServiceRequestId });
        }

        [HttpPost]
        public ActionResult UnsyncBoxFolder(int ServiceRequestId, string FolderId, string BoxUserId)
        {
            var box = new BoxManager();
            box.UpdateSyncState(FolderId, BoxUserId, BoxSyncStateType.not_synced);
            return RedirectToAction("BoxManager", new { serviceRequestId = ServiceRequestId });
        }

        [HttpPost]
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
            var physicians = await ctx.Physicians
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

            ViewBag.Services = await ctx.Services
                .Where(c => c.ServicePortfolioId == ServicePortfolios.Physician)
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Group = new SelectListGroup() { Name = c.ServiceCategory.Name }
                }).ToListAsync();

            var slots = await ctx.AvailableSlotViews
                .Where(c => c.AvailableDayId == availableDay.Id).ToListAsync();

            ViewBag.AvailableSlots = slots.Select(c => new SelectListItem()
            {
                Text = c.StartTime.ToString(@"hh\:mm") + " - " + c.Title,
                Value = c.Id.ToString()
            })
            .OrderBy(c => c.Text)
            .ToList();

            ViewBag.Companies = await ctx.Companies
                .Where(c => c.IsParent == false)
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Group = new SelectListGroup() { Name = c.Parent.Name }
                }).ToListAsync();

            var l = new WebApp.Library.DataHelper().LoadAddressesWithOwner(ctx);
            ViewBag.Locations = l
                .Where(a => a.Address.AddressTypeId != AddressTypes.BillingAddress)
                .Select(c => new SelectListItem()
            {
                Text = string.Format("{0} - {1} - {2}", c.Owner, c.Address.City_CityId.Name, c.Address.Name),
                Value = c.Address.Id.ToString(),
                Group = new SelectListGroup() { Name = c.Owner }
            });

            ViewBag.Staff = ctx.AspNetUsers
                .Where(u => u.GetRoleId() == AspNetRoles.CaseCoordinator || u.GetRoleId() == AspNetRoles.DocumentReviewer || u.GetRoleId() == AspNetRoles.IntakeAssistant || u.GetRoleId() == AspNetRoles.SuperAdmin)
                .AsEnumerable()
                .Select(c => new SelectListItem()
                {
                    Text = ValueConverters.GetDisplayName(c.Title, c.FirstName, c.LastName),
                    Value = c.Id.ToString()
                });
        }

        #endregion


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ctx.Dispose();
            }
            base.Dispose(disposing);
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