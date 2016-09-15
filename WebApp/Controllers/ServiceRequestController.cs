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

namespace WebApp.Controllers
{
    public delegate void NoShowToggledHandler(object sender, EventArgs e);

    [Authorize]
    public class ServiceRequestController : Controller
    {
        private OrvosiDbContext context = new OrvosiDbContext();
        private Orvosi.Data.OrvosiDbContext ctx = new Orvosi.Data.OrvosiDbContext();
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

            var sr = context.ServiceRequestViews.AsQueryable();

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
            vm.ServiceRequestTasks = await context.ServiceRequestTasks
                    .Include(srt => srt.AspNetUser)
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

            var list = await context.ServiceRequests
                .Where(sr => sr.CaseCoordinatorId == userId || sr.IntakeAssistantId == userId || sr.DocumentReviewerId == userId || sr.PhysicianId == userId)
                .ToListAsync();

            return View(list);
        }

        // GET: ServiceRequest/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var vm = new DetailsViewModel();

            vm.ServiceRequest = await ctx.ServiceRequests.FindAsync(id);

            // Get the data set
            var source = await ctx.GetAssignedServiceRequestsAsync(null, SystemTime.Now(), null, id);

            if (vm.ServiceRequest.Service.ServiceCategoryId == ServiceCategories.IndependentMedicalExam)
            {
                vm.ServiceRequestMapped = WebApp.Models.ServiceRequestModels.ServiceRequestMapper.MapToAssessment(source, SystemTime.Now(), User.Identity.GetGuidUserId(), HttpContext.Server.MapPath("~/ServiceRequestTask/Details"));
            }
            else if (vm.ServiceRequest.Service.ServiceCategoryId == ServiceCategories.AddOn)
            {
                vm.ServiceRequestMapped = WebApp.Models.ServiceRequestModels.ServiceRequestMapper.MapToAddOn(source, SystemTime.Now(), User.Identity.GetGuidUserId(), HttpContext.Server.MapPath("~/ServiceRequestTask/Details"));
            }
            
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

        public async Task<ActionResult> BoxManager(int serviceRequestId)
        {
            var serviceRequest = await ctx.ServiceRequests.FindAsync(serviceRequestId);
            var resources = serviceRequest.ServiceRequestTasks.Select(sr => sr.AspNetUser).Distinct();

            var boxCaseFolderId = ctx.ServiceRequests.First(sr => sr.Id == serviceRequestId).BoxCaseFolderId;

            var vm = new BoxManagerViewModel();
            vm.ServiceRequestId = serviceRequestId;

            var box = new BoxManager();
            if (!string.IsNullOrEmpty(boxCaseFolderId))
            {
                vm.BoxFolder = box.GetFolder(boxCaseFolderId);
                vm.BoxFolderCollaborations = box.GetCollaborations(boxCaseFolderId);
                foreach (var resource in resources)
                {
                    var boxResource = new BoxResource() { Resource = resource };
                    boxResource.BoxFolder = await box.GetFolder(boxCaseFolderId, resource.BoxUserId);
                    vm.Resources.Add(boxResource);
                }
            }
            return View(vm);
        }


        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public ActionResult Reschedule(int id)
        {
            var model = context.ServiceRequests.Single(c => c.Id == id);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> Reschedule(ServiceRequest serviceRequest)
        {
            var sr = context.ServiceRequests.Single(c => c.Id == serviceRequest.Id);
            var slot = context.AvailableSlots.Single(c => c.Id == serviceRequest.AvailableSlotId);
            sr.AvailableSlotId = serviceRequest.AvailableSlotId;
            sr.AppointmentDate = slot.AvailableDay.Day;
            sr.StartTime = slot.StartTime;
            sr.EndTime = slot.EndTime;
            sr.AddressId = serviceRequest.AddressId;
            sr.ModifiedDate = SystemTime.Now();
            sr.ModifiedUser = User.Identity.Name;
            await context.SaveChangesAsync();
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
        public async Task<ActionResult> Create(int availableDayId, Guid physicianId)
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

            vm.ServiceSelectList = ctx.Services.Where(c => c.ServicePortfolioId == e.ServicePortfolios.Physician)
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Group = new SelectListGroup() { Name = c.ServiceCategory.Name }
                });

            vm.AvailableSlotSelectList = ctx.AvailableSlots
                .Where(c => c.AvailableDayId == availableDayId)
                .AsEnumerable()
                .Select(c => new SelectListItem()
                {
                    Text = c.StartTime.ToString(@"hh\:mm") + " - " + c.GetTitle(),
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

            vm.AddressSelectList = ctx.Addresses
                .Where(loc => loc.AddressTypeId != e.AddressTypes.BillingAddress)
                .AsEnumerable()
                .Select(c => new SelectListItem()
                {
                    Text = string.Format("{0}", c.Name),
                    Value = c.Id.ToString(),
                    Group = new SelectListGroup() { Name = c.City_CityId.Name }
                });

            vm.StaffSelectList = ctx.AspNetUsers
                .Where(u => u.AspNetUserRoles.FirstOrDefault().AspNetRole.RoleCategoryId == e.RoleCategories.Staff || u.AspNetUserRoles.FirstOrDefault().AspNetRole.RoleCategoryId == e.RoleCategories.Admin)
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

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> Create(Orvosi.Data.ServiceRequest sr)
        {
            // These are the original parameters required by the Get method. AvailableDayId is not a property on ServiceRequest so it was added as a hidden field on the form. PhysicianId was added to be consistent.
            var selectedAvailableDayId = int.Parse(Request.Form.Get("SelectedAvailableDayId"));
            var selectedPhysicianId = new Guid(Request.Form.Get("SelectedPhysicianId"));

            // Get the service catalogue
            var address = await ctx.Addresses.FirstOrDefaultAsync(c => c.Id == sr.AddressId);
            var serviceCatalogues = context.GetServiceCatalogueForCompany(sr.PhysicianId, sr.CompanyId).ToList();
            var serviceCatalogue = serviceCatalogues.FirstOrDefault(c => c.LocationId == address.LocationId && c.ServiceId == sr.ServiceId);
            if (serviceCatalogue == null || !serviceCatalogue.Price.HasValue)
            {
                this.ModelState.AddModelError("ServiceId", "This service has not been offered to this company at this location.");
            }

            // Get the no show and late cancellation rates for this company
            var company = ctx.Companies.FirstOrDefault(c => c.Id == sr.CompanyId);
            var rates = context.GetServiceCatalogueRate(sr.PhysicianId, company?.ObjectGuid).First();
            if (rates == null || !rates.NoShowRate.HasValue || !rates.LateCancellationRate.HasValue)
            {
                this.ModelState.AddModelError("ServiceId", "No Show Rates or Late Cancellation Rates have not been set for this company.");
            }

            if (ModelState.IsValid)
            {
                var slot = await ctx.AvailableSlots.FindAsync(sr.AvailableSlotId);
                sr.ServiceId = serviceCatalogue.ServiceId;
                sr.PhysicianId = selectedPhysicianId;
                sr.CompanyId = company.Id;
                sr.AddressId = address.Id;
                sr.ServiceCatalogueId = serviceCatalogue.ServiceCatalogueId;
                sr.AvailableSlotId = sr.AvailableSlotId;
                sr.StartTime = slot.StartTime;
                sr.EndTime = slot.EndTime;
                sr.ServiceCataloguePrice = serviceCatalogue.Price;
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
                    st.ResponsibleRoleId = template.OTask.ResponsibleRoleId;
                    st.Sequence = template.Sequence;
                    st.ShortName = template.OTask.ShortName;
                    st.TaskId = template.OTask.Id;
                    st.TaskName = template.OTask.Name;
                    st.ModifiedDate = SystemTime.Now();
                    st.ModifiedUser = User.Identity.Name;
                    // Assign tasks to physician and case coordinator to start
                    st.AssignedTo = (template.OTask.ResponsibleRoleId == AspNetRoles.CaseCoordinator ? sr.CaseCoordinatorId : (template.OTask.ResponsibleRoleId == AspNetRoles.Physician ? sr.PhysicianId as Nullable<Guid> : null));
                    st.ServiceRequestTemplateTaskId = template.Id;
                    sr.ServiceRequestTasks.Add(st);
                }

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
            return await Create(selectedAvailableDayId, selectedPhysicianId);

            ////CREATE THE INVOICE
            //var serviceRequest = obj;

            //var serviceProvider = await context.BillableEntities.SingleOrDefaultAsync(c => c.EntityGuid == serviceRequest.PhysicianId);
            //var customer = await context.BillableEntities.SingleOrDefaultAsync(c => c.EntityGuid == serviceRequest.CompanyGuid.Value);

            //var invoiceNumber = context.GetNextInvoiceNumber().SingleOrDefault();
            //var invoiceDate = serviceRequest.AppointmentDate.Value;

            //var invoice = new Invoice();
            //invoice.BuildInvoice(serviceProvider, customer, invoiceNumber, invoiceDate, User.Identity.Name);

            //// Create or update the invoice detail for the service request
            //InvoiceDetail invoiceDetail;

            //invoiceDetail = context.InvoiceDetails.SingleOrDefault(c => c.ServiceRequestId == serviceRequest.Id);
            //if (invoiceDetail == null)
            //{
            //    invoiceDetail = new InvoiceDetail();
            //    invoiceDetail.BuildInvoiceDetailFromServiceRequest(serviceRequest, User.Identity.Name);
            //    invoice.InvoiceDetails.Add(invoiceDetail);
            //}
            //else
            //{
            //    invoiceDetail.BuildInvoiceDetailFromServiceRequest(serviceRequest, User.Identity.Name);
            //}

            //invoice.CalculateTotal();
            //context.Invoices.Add(invoice);

            //var validationResults = context.GetValidationErrors();
            //if (validationResults.Count() > 0)
            //{
            //    foreach (var validationResult in validationResults)
            //    {
            //        foreach (var error in validationResult.ValidationErrors)
            //        {
            //            additionalErrors.Add(new ModelError(error.ErrorMessage));
            //        }
            //    }
            //}
            //else
            //{
            //    await context.SaveChangesAsync();
            //}

            /*  TODO: Create the calendar event in the physician booking calendar.
                TITLE will equal the Calendar Title field.
                WHERE will be populated with the location address.
                DESCRIPTION will be populated with:
                    Case Details: 
                    https://orvosi.ca/servicerequest/details/[id]

                    Case Folder:
                    [Case Folder Name field]

                NOTE: Invitees will be set with the resources are assigned. Not now.

                TODO: Create the DropBox folder.
                TITLE will equal the Case Folder Name field.
                PATH will equal Cases/[physician user name]/[yyyy-mm]/[case folder name]

                NOTE: Share permissions will be granted when the resources are assigned.
            */

            ///***********************************
            //Dropbox
            //***********************************/
            //var dropbox = new OrvosiDropbox();
            //var client = await dropbox.GetServiceAccountClientAsync();
            //Metadata folder = null;
            //List<MembersGetInfoItem> members = new List<MembersGetInfoItem>();
            //try
            //{

            //    // Copy the case template folder
            //    var destination = obj.DocumentFolderLink;
            //    folder = await client.Files.CopyAsync(new RelocationArg("/cases/_casefoldertemplate", destination));
            //    // Share the folder
            //    var caseCoordinator = await db.Users.SingleAsync(c => c.Id == sr.CaseCoordinatorId.Value.ToString());
            //    var share = await client.Sharing.ShareFolderAsync(new ShareFolderArg(folder.PathLower, MemberPolicy.Team.Instance, AclUpdatePolicy.Editors.Instance));
            //    var sharedFolderId = share.AsComplete.Value.SharedFolderId;

            //    var physician = await db.Users.SingleAsync(c => c.Id == obj.PhysicianId);
            //    await DropboxAddMember(dropbox, client, caseCoordinator.Email, sharedFolderId);
            //    await DropboxAddMember(dropbox, client, physician.Email, sharedFolderId);

            //    // Get the folder
            //    folder = await client.Files.GetMetadataAsync(folder.AsFolder.PathLower);

            //    // Get the shared folder members
            //    members = await GetSharedFolderMembers(dropbox, client, sharedFolderId);

            //}
            //catch (Exception ex)
            //{
            //    additionalErrors.Add(new ModelError(ex.Message));
            //}
            // Create the calendar event

            //// mark the first task as complete
            //using (var db = new OrvosiDbContext(User.Identity.Name))
            //{
            //    var serviceRequestTask = await db.ServiceRequestTasks.SingleOrDefaultAsync(c => c.ServiceRequestId == obj.Id && c.TaskId == Tasks.CreateCaseFolder && !c.IsObsolete);
            //    serviceRequestTask.CompletedDate = SystemTime.Now();
            //    await db.SaveChangesAsync();
            //}

            //var model = new CreateSuccessViewModel()
            //{
            //    ServiceRequest = obj,
            //    Folder = null,// folder,
            //    Members = null, //members,
            //    Invoice = invoice,
            //    Errors = additionalErrors
            //};

        }

        [HttpGet]
        public ActionResult CreateSuccess(CreateSuccessViewModel obj)
        {
            return View(obj);
        }

        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public ActionResult CreateAddOn()
        {
            var vm = new CreateViewModel();

            vm.ServiceSelectList = ctx.Services.Where(c => c.ServicePortfolioId == e.ServicePortfolios.Physician)
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
                .Where(u => u.AspNetUserRoles.FirstOrDefault().AspNetRole.RoleCategoryId == e.RoleCategories.Staff || u.AspNetUserRoles.FirstOrDefault().AspNetRole.RoleCategoryId == e.RoleCategories.Admin)
                .AsEnumerable()
                .Select(c => new SelectListItem()
                {
                    Text = c.GetDisplayName(),
                    Value = c.Id.ToString()
                });

            return View(vm);
        }

        [Authorize(Roles = "Case Coordinator, Super Admin")]
        [HttpPost]
        public async Task<ActionResult> CreateAddOn(Orvosi.Data.ServiceRequest sr)
        {
            if (sr.ServiceId != Services.Addendum && sr.ServiceId != Services.PaperReview)
            {
                this.ModelState.AddModelError("ServiceId", "Service must be an AddOn.");
            }

            var company = await ctx.Companies.FirstOrDefaultAsync(c => c.Id == sr.CompanyId);

            var serviceCatalogues = context.GetServiceCatalogueForCompany(sr.PhysicianId, sr.CompanyId).ToList();

            var serviceCatalogue = serviceCatalogues.SingleOrDefault(c => c.ServiceId == sr.ServiceId && c.LocationId == 0);
            if (serviceCatalogue == null || !serviceCatalogue.Price.HasValue)
            {
                this.ModelState.AddModelError("ServiceId", "This service has not been offered to this company at this location.");
            }

            var rates = context.GetServiceCatalogueRate(sr.PhysicianId, company?.ObjectGuid).First();
            if (rates == null || !rates.NoShowRate.HasValue || !rates.LateCancellationRate.HasValue)
            {
                this.ModelState.AddModelError("ServiceId", "No Show Rates or Late Cancellation Rates have not been set for this company.");
            }

            if (ModelState.IsValid)
            {
                sr.ServiceCatalogueId = serviceCatalogue.ServiceCatalogueId;
                sr.DueDate = sr.DueDate;
                sr.CaseCoordinatorId = sr.CaseCoordinatorId;
                sr.ClaimantName = sr.ClaimantName;
                sr.CompanyReferenceId = sr.CompanyReferenceId;
                sr.RequestedDate = sr.RequestedDate;
                sr.CompanyId = sr.CompanyId;
                sr.PhysicianId = sr.PhysicianId;
                sr.ServiceId = serviceCatalogue.ServiceId;
                sr.ServiceCataloguePrice = serviceCatalogue.Price;
                sr.NoShowRate = rates.NoShowRate;
                sr.LateCancellationRate = rates.LateCancellationRate;
                sr.ModifiedUser = User.Identity.Name;
                sr.ModifiedDate = SystemTime.Now();

                var service = await ctx.Services.FindAsync(serviceCatalogue.ServiceId);
                var tasks = ctx.OTasks.Where(t => t.ServiceCategoryId == service.ServiceCategoryId);

                foreach (var task in tasks)
                {
                    var st = new Orvosi.Data.ServiceRequestTask();
                    st.DependsOn = task.DependsOn;
                    st.DueDateBase = task.DueDateBase;
                    st.DueDateDiff = task.DueDateDiff;
                    st.Guidance = task.Guidance;
                    st.ObjectGuid = task.ObjectGuid;
                    st.ResponsibleRoleId = task.ResponsibleRoleId;
                    st.AssignedTo = GetTaskAssignment(task.ResponsibleRoleId, sr.PhysicianId, sr.CaseCoordinatorId);
                    st.Sequence = task.Sequence;
                    st.ShortName = task.ShortName;
                    st.TaskId = task.Id;
                    st.TaskName = task.Name;
                    st.ModifiedDate = SystemTime.Now();
                    st.ModifiedUser = User.Identity.Name;

                    sr.ServiceRequestTasks.Add(st);
                }

                ctx.ServiceRequests.Add(sr);
                await ctx.SaveChangesAsync();

                return RedirectToAction("Details", new { id = sr.Id });
            }

            return CreateAddOn();
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
                .Where(u => u.AspNetUserRoles.FirstOrDefault().AspNetRole.RoleCategoryId == e.RoleCategories.Staff || u.AspNetUserRoles.FirstOrDefault().AspNetRole.RoleCategoryId == e.RoleCategories.Admin)
                .AsEnumerable()
                .Select(c => new SelectListItem()
                {
                    Text = c.GetDisplayName(),
                    Value = c.Id.ToString()
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

            ServiceRequest serviceRequest = await context.ServiceRequests.FindAsync(id);

            if (serviceRequest == null)
            {
                return HttpNotFound();
            }

            // TODO: Update the calendar and dropbox folder if appropriate.

            return View(serviceRequest);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> Edit(ServiceRequest sr)
        {
            if (ModelState.IsValid)
            {
                using (context = new OrvosiDbContext(User.Identity.Name))
                {
                    // get the tracked object from the database
                    var obj = await context.ServiceRequests.SingleOrDefaultAsync(c => c.Id == sr.Id);

                    // update the resource assignments
                    obj.ClaimantName = sr.ClaimantName;
                    obj.CompanyReferenceId = sr.CompanyReferenceId;
                    obj.DueDate = sr.DueDate;
                    obj.Notes = sr.Notes;

                    await context.SaveChangesAsync();

                    return RedirectToAction("Details", new { id = obj.Id });
                }
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
            ServiceRequest serviceRequest = await context.ServiceRequests.FindAsync(id);
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
            ServiceRequest serviceRequest = await context.ServiceRequests.FindAsync(id);
            context.ServiceRequests.Remove(serviceRequest);
            await context.SaveChangesAsync();

            //TODO: Unshare and delete folders from dropbox
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> Cancel(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ServiceRequest serviceRequest = await context.ServiceRequests.FindAsync(id);

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
            if (!form.Id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var serviceRequest = await context.ServiceRequests.FindAsync(form.Id);
            if (serviceRequest == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                context.ToggleCancellation(form.Id, form.CancelledDate, form.IsLate == "on" ? true : false, string.Concat(serviceRequest.Notes, '\n', form.Notes));

                serviceRequest.IsLateCancellation = form.IsLate == "on" ? true : false;

                if (serviceRequest.InvoiceDetails.Count > 0)
                {
                    var detail = serviceRequest.InvoiceDetails.First();
                    if (serviceRequest.IsLateCancellation)
                    {
                        var rates = context.GetServiceCatalogueRate(detail.Invoice.ServiceProviderGuid, detail.Invoice.CustomerGuid).First();
                        detail.ApplyDiscount(DiscountTypes.LateCancellation, rates.LateCancellationRate);
                    }
                    else
                    {
                        detail.RemoveDiscount();
                    }

                    detail.Invoice.CalculateTotal();
                    await context.SaveChangesAsync();
                }

                return RedirectToAction("Index");
            }
            return View(serviceRequest);
        }

        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> UndoCancel(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var serviceRequest = await context.ServiceRequests.FindAsync(id);
            if (serviceRequest == null)
            {
                return HttpNotFound();
            }
            context.ToggleCancellation(id, null, false, string.Empty);

            if (serviceRequest.InvoiceDetails.Count > 0)
            {
                var detail = serviceRequest.InvoiceDetails.First();
                detail.RemoveDiscount();
                detail.Invoice.CalculateTotal();
                await context.SaveChangesAsync();
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ToggleNoShow(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var serviceRequest = await context.ServiceRequests.FindAsync(id);
            if (serviceRequest == null)
            {
                return HttpNotFound();
            }
            context.ToggleNoShow(id);

            serviceRequest.IsNoShow = !serviceRequest.IsNoShow;

            if (serviceRequest.InvoiceDetails.Count > 0)
            {
                var detail = serviceRequest.InvoiceDetails.First();
                if (serviceRequest.IsNoShow)
                {
                    var rates = context.GetServiceCatalogueRate(detail.Invoice.ServiceProviderGuid, detail.Invoice.CustomerGuid).First();
                    detail.ApplyDiscount(DiscountTypes.NoShow, rates.NoShowRate);
                }
                else
                {
                    detail.RemoveDiscount();
                }

                detail.Invoice.CalculateTotal();
                await context.SaveChangesAsync();
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        //[HttpPost]
        //[Authorize(Roles = "Staff, Super Admin")]
        //public async Task<ActionResult> GetAvailability([Bind(Include = "PhysicianId, AppointmentDate")] ServiceRequest serviceRequest)
        //{
        //    var ad = await context.AvailableDays
        //        .SingleOrDefaultAsync(c => c.PhysicianId == serviceRequest.PhysicianId && c.Day == serviceRequest.AppointmentDate);

        //    var p = await context.Physicians.SingleOrDefaultAsync(c => c.Id == serviceRequest.PhysicianId);

        //    var vm = new CreateViewModel();
        //    vm.ServiceRequest = serviceRequest;
        //    vm.IsAvailableDaySelected = true;

        //    if (ad != null)
        //    {
        //        vm.AvailableDay = ad;
        //        vm.IsAvailable = true;

        //        if (ad.LocationId != null)
        //        {
        //            vm.ServiceRequest.LocationId = ad.LocationId;
        //            vm.ServiceRequest.LocationName = ad.LocationName;
        //        }

        //        if (ad.IsPrebook)
        //        {
        //            vm.ServiceRequest.CompanyId = ad.CompanyId;
        //            vm.ServiceRequest.CompanyName = ad.CompanyName;
        //        }
        //    }
        //    else
        //    {
        //        vm.IsAvailable = false;

        //    }
        //    // this will be null if they are not available
        //    vm.ServiceRequest.PhysicianDisplayName = p.DisplayName;

        //    await GetCreateDropdownlistData(vm.AvailableDay);

        //    return View("Create", vm);
        //}

        #region Dropbox

        //[HttpPost]
        //[Authorize(Roles = "Case Coordinator, Super Admin")]
        //public async Task<ActionResult> CreateDropboxFolder(short id)
        //{
        //    var obj = await context.ServiceRequests.FindAsync(id);
        //    var client = await dropbox.GetServiceAccountClientAsync();

        //    if (obj.ServiceCategoryId == ServiceCategories.AddOn)
        //    {
        //        obj.DocumentFolderLink = string.Format("/cases/{0}/AddOns/{1}", obj.PhysicianUserName, obj.Title.Trim());
        //    }
        //    else
        //    {
        //        var month = obj.AppointmentDate.Value.ToString("yyyy-MM");
        //        obj.DocumentFolderLink = string.Format("/cases/{0}/{1}/{2}", obj.PhysicianUserName, month, obj.Title.Trim());
        //    }

        //    await context.SaveChangesAsync();

        //    // Get the destination folder name
        //    var destination = obj.DocumentFolderLink;

        //    // Check if the folder already exists)
        //    Metadata metadata = null;
        //    // Check if the folder already exists
        //    try
        //    {
        //        // Get the folder
        //        metadata = await client.Files.GetMetadataAsync(destination);
        //    }
        //    catch (ApiException<GetMetadataError> ex)
        //    {
        //        if (!ex.ErrorResponse.AsPath.Value.IsNotFound)
        //            throw;
        //    }

        //    if (metadata != null)
        //    {
        //        //TODO: Add some error messaging error
        //        throw new Exception("The dropbox folder already exists.");
        //    }

        //    // Copy the case template folder
        //    var folder = await client.Files.CopyAsync(new RelocationArg("/cases/_addonfoldertemplate", destination));

        //    return RedirectToAction("Details", new { id = id });
        //}

        //[HttpPost]
        //[Authorize(Roles = "Case Coordinator, Super Admin")]
        //public async Task<ActionResult> ShareDropboxFolder(int serviceRequestId, string folderId)
        //{
        //    var client = await dropbox.GetServiceAccountClientAsync();

        //    var metadata = await GetMetadata(folderId, client);

        //    // Share the folder
        //    await client.Sharing.ShareFolderAsync(new ShareFolderArg(metadata.AsFolder.PathLower, MemberPolicy.Team.Instance, AclUpdatePolicy.Editors.Instance));

        //    return RedirectToAction("Details", new { id = serviceRequestId });
        //}

        //[HttpPost]
        //[Authorize(Roles = "Case Coordinator, Super Admin")]
        //public async Task<ActionResult> UnshareDropboxFolder(int serviceRequestId, string folderId)
        //{
        //    var client = await dropbox.GetServiceAccountClientAsync();

        //    var metadata = await GetMetadata(folderId, client);

        //    // Unshare the folder
        //    await client.Sharing.UnshareFolderAsync(new UnshareFolderArg(metadata.AsFolder.SharingInfo.SharedFolderId, false));

        //    return RedirectToAction("Details", new { id = serviceRequestId });
        //}

        //[HttpPost]
        //[Authorize(Roles = "Case Coordinator, Super Admin")]
        //public async Task<ActionResult> ShareDropboxFolderToMember(string folderId, Guid userId, short serviceRequestId)
        //{
        //    var client = await dropbox.GetServiceAccountClientAsync();

        //    Metadata metadata = await GetMetadata(folderId, client);

        //    string sharedFolderId = string.Empty;
        //    if (metadata.AsFolder.SharingInfo == null)
        //    {
        //        throw new Exception("Folder is not shared out");
        //    }
        //    else
        //    {
        //        sharedFolderId = metadata.AsFolder.SharingInfo.SharedFolderId;
        //    }

        //    var user = await context.Users.SingleAsync(c => c.Id == userId);
        //    await DropboxAddMember(dropbox, client, user.Email, sharedFolderId);

        //    return RedirectToAction("Details", new { id = serviceRequestId });
        //}

        //[HttpPost]
        //[Authorize(Roles = "Case Coordinator, Super Admin")]
        //public async Task<ActionResult> UnshareDropboxFolderFromMember(string folderId, Guid userId, short serviceRequestId)
        //{
        //    var client = await dropbox.GetServiceAccountClientAsync();

        //    Metadata metadata = await GetMetadata(folderId, client);

        //    string sharedFolderId = string.Empty;
        //    if (metadata.AsFolder.SharingInfo == null)
        //    {
        //        throw new Exception("Folder is not shared out");
        //    }
        //    else
        //    {
        //        sharedFolderId = metadata.AsFolder.SharingInfo.SharedFolderId;
        //    }

        //    var user = await context.Users.SingleAsync(c => c.Id == userId);
        //    await DropboxRemoveMember(client, user.Email, sharedFolderId);

        //    return RedirectToAction("Details", new { id = serviceRequestId });
        //}

        //private static async Task<Metadata> GetMetadata(string folderId, DropboxClient client)
        //{
        //    Metadata metadata = null;
        //    // Check if the folder already exists
        //    try
        //    {
        //        // Get the folder
        //        metadata = await client.Files.GetMetadataAsync(folderId);
        //    }
        //    catch (ApiException<GetMetadataError> ex)
        //    {
        //        if (!ex.ErrorResponse.AsPath.Value.IsNotFound)
        //            throw;
        //    }

        //    if (metadata == null)
        //    {
        //        //TODO: Add some error messaging error
        //        throw new Exception("The dropbox folder does not exist.");
        //    }

        //    return metadata;
        //}

        //private static async Task<List<MembersGetInfoItem>> GetSharedFolderMembers(OrvosiDropbox dropbox, Dropbox.Api.DropboxClient client, string sharedFolderId)
        //{
        //    // Get the members for the shared folder
        //    var sharedMembers = await client.Sharing.ListFolderMembersAsync(sharedFolderId);

        //    // Get the full member entities of the shared members
        //    var args = new List<UserSelectorArg>();
        //    foreach (var m in sharedMembers.Users)
        //    {
        //        args.Add(new UserSelectorArg.TeamMemberId(m.User.TeamMemberId));
        //    }
        //    var members = await dropbox.TeamClient.Team.MembersGetInfoAsync(args);
        //    return members;
        //}

        //private static async Task DropboxAddMember(OrvosiDropbox dropbox, DropboxClient client, string email, string sharedFolderId)
        //{
        //    await client.Sharing.AddFolderMemberAsync(
        //        sharedFolderId,
        //        new List<AddMember>()
        //        {
        //                new AddMember(
        //                    new MemberSelector.Email(email)
        //                )
        //        },
        //        true
        //    );

        //    await client.Sharing.UpdateFolderMemberAsync(
        //        sharedFolderId,
        //        new MemberSelector.Email(email),
        //        AccessLevel.Editor.Instance
        //    );

        //    var cc = await dropbox.GetTeamMemberClientAsync(email);
        //    await cc.Sharing.MountFolderAsync(sharedFolderId);
        //}

        //private static async Task DropboxRemoveMember(DropboxClient client, string email, string sharedFolderId)
        //{
        //    // Add the case coordinator to the share
        //    await client.Sharing.RemoveFolderMemberAsync(
        //        sharedFolderId,
        //        new MemberSelector.Email(email),
        //        false
        //    );
        //}

        //private async Task ApplyMemberChangesToDropbox(ServiceRequest request, OrvosiDropbox dropbox, Dropbox.Api.DropboxClient client, string sharedFolderId)
        //{
        //    Guid?[] resources = new Guid?[3]
        //            {
        //                request.CaseCoordinatorId,
        //                request.DocumentReviewerId,
        //                request.IntakeAssistantId
        //            };

        //    var list = resources.Where(r => r.HasValue).Distinct().ToList();
        //    var users = context.Users.Where(c => list.Contains(c.Id)).ToList();

        //    var members = await GetSharedFolderMembers(dropbox, client, sharedFolderId);

        //    // Add members
        //    foreach (var user in users)
        //    {
        //        if (!members.Exists(c => c.AsMemberInfo.Value.Profile.Email == user.Email))
        //        {
        //            await DropboxAddMember(dropbox, client, user.Email, sharedFolderId);
        //        }
        //    }

        //    // remove members
        //    foreach (var member in members)
        //    {
        //        var memberEmail = member.AsMemberInfo.Value.Profile.Email;
        //        var user = context.Users.SingleOrDefault(c => c.Email == memberEmail);
        //        if (user.RoleId == Roles.CaseCoordinator || user.RoleId == Roles.DocumentReviewer || user.RoleId == Roles.IntakeAssistant)
        //        {
        //            if (!users.Exists(c => c.Email == memberEmail))
        //            {
        //                await DropboxRemoveMember(client, memberEmail, sharedFolderId);
        //            }
        //        }
        //    }
        //}

        #endregion


        #region Box

        [HttpPost]
        public ActionResult CreateBoxCaseFolder(int ServiceRequestId)
        {
            using (var db = new OrvosiDbContext(User.Identity.Name))
            {
                // Get the request
                var request = db.ServiceRequests.Single(sr => sr.Id == ServiceRequestId);

                // Get the request and assert they have a Box Folder Id
                var physician = db.AspNetUsers.Single(p => p.Id == request.PhysicianId);
                //var physicianBoxFolderId = "7027883033"; // This overrides to HanSolo box folder while developing. Comment out for production.
                var physicianBoxFolderId = physician.BoxFolderId;
                if (string.IsNullOrEmpty(physicianBoxFolderId))
                    return PartialView("Error", "Physician does not have a cases folder setup in Box.");

                // Create the case folder
                var box = new BoxManager();
                BoxFolder caseFolder;
                if (request.Service.ServiceCategoryId == ServiceCategories.AddOn)
                {
                    var province = db.GetCompanyProvince(request.CompanyId).FirstOrDefault();
                    if (province == null)
                    {
                        province = new GetCompanyProvinceReturnModel() { ProvinceID = 0, ProvinceName = "Ontario" };
                    }
                    caseFolder = box.CreateAddOnFolder(physicianBoxFolderId, province.ProvinceName, request.DueDate.Value, request.Title, physician.Physician.BoxAddOnTemplateFolderId);
                }
                else
                {
                    // Get the province which is used in the case folder path
                    var province = db.Provinces.Single(p => p.Id == request.Address.ProvinceId);
                    caseFolder = box.CreateCaseFolder(physicianBoxFolderId, province.ProvinceName, request.AppointmentDate.Value, request.Title, physician.Physician.BoxCaseTemplateFolderId);
                }

                // Persist the new case folder Id to the database.
                request.BoxCaseFolderId = caseFolder.Id;
                db.SaveChanges();

                // Redirect to display the Box Folder
                return RedirectToAction("Details", new { id = ServiceRequestId });
            }
        }

        [HttpPost]
        public ActionResult ShareBoxFolder(int ServiceRequestId, string FolderId, Guid UserId)
        {
            using (var db = new OrvosiDbContext())
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

                return RedirectToAction("Details", new { id = ServiceRequestId });
            }
        }

        public ActionResult UnshareBoxFolder(int ServiceRequestId, string CollaborationId)
        {
            using (var db = new OrvosiDbContext())
            {
                var box = new BoxManager();
                var success = box.RemoveCollaboration(CollaborationId);

                if (success)
                {
                    var collaboration = db.ServiceRequestBoxCollaborations.SingleOrDefault(b => b.BoxCollaborationId == CollaborationId);
                    db.ServiceRequestBoxCollaborations.Remove(collaboration);
                    db.SaveChanges();
                }
                return RedirectToAction("Details", new { id = ServiceRequestId });
            }
        }

        public ActionResult AcceptBoxFolder(int ServiceRequestId, Guid UserId, string CollaborationId)
        {
            string boxUserId;
            using (var db = new OrvosiDbContext())
            {
                var user = db.Profiles.Single(p => p.Id == UserId);
                boxUserId = user.BoxUserId;
            }
            var box = new BoxManager();
            var collaboration = box.AcceptCollaboration(CollaborationId, boxUserId);
            return RedirectToAction("Details", new { id = ServiceRequestId });
        }

        [HttpPost]
        public ActionResult UnsyncBoxFolder(int ServiceRequestId, string FolderId, string BoxUserId)
        {
            var box = new BoxManager();
            box.UpdateSyncState(FolderId, BoxUserId, BoxSyncStateType.not_synced);
            return RedirectToAction("Details", new { id = ServiceRequestId });
        }

        [HttpPost]
        public ActionResult SyncBoxFolder(int ServiceRequestId, string FolderId, string BoxUserId)
        {
            var box = new BoxManager();
            box.UpdateSyncState(FolderId, BoxUserId, BoxSyncStateType.synced);
            return RedirectToAction("Details", new { id = ServiceRequestId });
        }

        #endregion

        private async Task GetPhysicianDropDownData()
        {
            var physicians = await context.Physicians
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
            //var companies = db.Companies
            //    .Where(c => c.IsParent == false);

            //if (availableDay != null && availableDay.CompanyIsParent.Value)
            //{
            //    companies = companies.Where(c => c.ParentId == availableDay.CompanyId);
            //}

            ViewBag.Services = await context.Services
                .Where(c => c.ServicePortfolioId == ServicePortfolios.Physician)
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Group = new SelectListGroup() { Name = c.ServiceCategory.Name }
                }).ToListAsync();

            var slots = await context.AvailableSlotViews
                .Where(c => c.AvailableDayId == availableDay.Id).ToListAsync();

            ViewBag.AvailableSlots = slots.Select(c => new SelectListItem()
            {
                Text = c.StartTime.ToString(@"hh\:mm") + " - " + c.Title,
                Value = c.Id.ToString()
            })
            .OrderBy(c => c.Text)
            .ToList();

            ViewBag.Companies = await context.Companies
                .Where(c => c.IsParent == false)
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Group = new SelectListGroup() { Name = c.Parent.Name }
                }).ToListAsync();

            var l = await context.LocationViews.ToListAsync();
            ViewBag.Locations = l.Select(c => new SelectListItem()
            {
                Text = string.Format("{0} - {1}", c.LocationName, c.Name),
                Value = c.Id.ToString(),
                Group = new SelectListGroup() { Name = c.EntityDisplayName }
            });

            ViewBag.Staff = context.AspNetUsers
                .Where(u => u.GetRoleId() == AspNetRoles.CaseCoordinator || u.GetRoleId() == AspNetRoles.DocumentReviewer || u.GetRoleId() == AspNetRoles.IntakeAssistant || u.GetRoleId() == AspNetRoles.SuperAdmin)
                .AsEnumerable()
                .Select(c => new SelectListItem()
                {
                    Text = ValueConverters.GetDisplayName(c.Title, c.FirstName, c.LastName),
                    Value = c.Id.ToString()
                });
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
