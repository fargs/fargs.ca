using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model;
using Model.Enums;
using WebApp.ViewModels.ServiceRequestViewModels;
using System.Security.Claims;
using WebApp.Library;
using Dropbox.Api;
using Dropbox.Api.Files;
using Dropbox.Api.Sharing;
using Dropbox.Api.Team;
using Box.V2.Models;

namespace WebApp.Controllers
{
    public delegate void NoShowToggledHandler(object sender, EventArgs e);

    [Authorize]
    public class ServiceRequestController : Controller
    {
        private OrvosiEntities context = new OrvosiEntities();
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
            vm.User = context.Users.Single(u => u.UserName == User.Identity.Name);

            filterArgs.ShowAll = (filterArgs.ShowAll ?? true);
            filterArgs.Sort = (filterArgs.Sort ?? "Oldest");
            filterArgs.StatusId = (filterArgs.StatusId ?? ServiceRequestStatus.Open);

            var sr = context.ServiceRequests.AsQueryable();

            if (filterArgs.StatusId.HasValue)
            {
                sr = sr.Where(c => c.ServiceRequestStatusId == filterArgs.StatusId);
            }

            if (filterArgs.DateRange.HasValue)
            {
                var range = DateRanges.GetRange(filterArgs.DateRange.Value);
                var start = range[0];
                var end = range[1];
                sr = sr.Where(c => c.AppointmentDate >= start && c.AppointmentDate < end);
            }

            if (!string.IsNullOrEmpty(filterArgs.Ids))
            {
                var ids = Array.ConvertAll(filterArgs.Ids.Split(','), s => int.Parse(s));
                sr = sr.Where(c => ids.Contains(c.Id));
            }

            if (!string.IsNullOrEmpty(filterArgs.ClaimantName))
            {
                sr = sr.Where(c => c.ClaimantName.Contains(filterArgs.ClaimantName) || c.CompanyReferenceId.Contains(filterArgs.ClaimantName));
            }

            var userGuid = new Guid(vm.User.Id);

            // if the user is an administrator and the option showAll is true, then show all
            if (vm.User.RoleCategoryId != RoleCategory.Admin || (vm.User.RoleCategoryId != RoleCategory.Admin && filterArgs.ShowAll == false))
            {
                sr = sr.Where(c => c.CaseCoordinatorId == userGuid || c.IntakeAssistantId == userGuid || c.DocumentReviewerId == userGuid || c.PhysicianId == vm.User.Id);
            }

            if (vm.User.RoleCategoryId == RoleCategory.Admin || vm.User.RoleCategoryId == RoleCategory.Staff)
            {
                if (!string.IsNullOrEmpty(filterArgs.PhysicianId))
                {
                    sr = sr.Where(c => c.PhysicianId == filterArgs.PhysicianId);
                }
            }

            if (!string.IsNullOrEmpty(filterArgs.NextTask))
            {
                sr = sr.Where(c => c.NextTaskAssignedTo == filterArgs.NextTask);
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

            vm.FilterArgs = filterArgs;

            return View(vm);
        }

        public async Task<ActionResult> Dashboard()
        {
            var user = context.Users.Single(u => u.UserName == User.Identity.Name);

            var list = await context.ServiceRequests
                .Where(sr => sr.CaseCoordinatorId == new Guid(user.Id) || sr.IntakeAssistantId == new Guid(user.Id) || sr.DocumentReviewerId == new Guid(user.Id) || sr.PhysicianId == user.Id)
                .ToListAsync();

            return View(list);
        }

        // GET: ServiceRequest/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var context = new Orvosi.Data.OrvosiDbContext())
            {
                var vm = new DetailsViewModel();
                vm.ServiceRequest = await context.ServiceRequests.FindAsync(id);
                if (vm.ServiceRequest == null)
                {
                    return HttpNotFound();
                }

                vm.User = context.AspNetUsers.Single(u => u.UserName == User.Identity.Name);

                if (vm.User.AspNetUserRoles.First().AspNetRole.RoleCategoryId == RoleCategory.Admin)
                {
                    var dropbox = new OrvosiDropbox();
                    var client = await dropbox.GetServiceAccountClientAsync();

                    // Copy the case template folder
                    var destination = vm.ServiceRequest.DocumentFolderLink ?? "/qwertypoiu";

                    try
                    {
                        // Get the folder
                        vm.DropboxFolder = await client.Files.GetMetadataAsync(destination);
                    }
                    catch (Dropbox.Api.ApiException<Dropbox.Api.Files.GetMetadataError> ex)
                    {
                        if (!ex.ErrorResponse.AsPath.Value.IsNotFound)
                            throw;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    if (vm.DropboxFolder != null)
                    {
                        if (vm.DropboxFolder.AsFolder.SharingInfo != null)
                        {
                            // Get the shared folder members
                            vm.DropboxFolderMembers = await GetSharedFolderMembers(dropbox, client, vm.DropboxFolder.AsFolder.SharingInfo.SharedFolderId);
                        }
                    }
                }

                var ResourcesFromTasks = this.context.GetServiceRequestResources(id);
                var box = new BoxManager();
                if (!string.IsNullOrEmpty(vm.ServiceRequest.BoxCaseFolderId))
                {
                    vm.BoxFolder = box.GetFolder(vm.ServiceRequest.BoxCaseFolderId);
                    vm.BoxFolderCollaborations = box.GetCollaborations(vm.ServiceRequest.BoxCaseFolderId);
                    foreach (var item in ResourcesFromTasks)
                    {
                        var resource = new Resource() { ResourceFromTask = item };
                        resource.BoxFolder = await box.GetFolder(vm.ServiceRequest.BoxCaseFolderId, resource.ResourceFromTask.BoxUserId);
                        vm.Resources.Add(resource);
                    }
                }

                return View(vm);
            }
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
            sr.Duration = slot.Duration;
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
            var ad = await context.AvailableDays
                .SingleOrDefaultAsync(c => c.PhysicianId == form.PhysicianId && c.Day == form.AppointmentDate);

            var p = await context.Physicians.SingleOrDefaultAsync(c => c.Id == form.PhysicianId);

            if (ad == null)
            {
                this.ModelState.AddModelError("AppointmentDate", string.Format("{0} is not available on this day.", p.DisplayName));
            }

            if (ModelState.IsValid)
            {
                var vm = new CreateViewModel();
                vm.AvailableDay = ad;
                vm.Physician = p;
                vm.ServiceRequest.PhysicianId = p.Id;
                vm.ServiceRequest.PhysicianDisplayName = p.DisplayName;
                vm.ServiceRequest.AppointmentDate = ad.Day;

                return View("Create", vm);
            }
            return View(form);
        }

        // POST: Admin/ServiceRequest/Create
        [Authorize(Roles = "Case Coordinator, Super Admin")]
        [HttpPost]
        public async Task<ActionResult> Create(ServiceRequest sr)
        {
            var company = await context.Companies.SingleOrDefaultAsync(c => c.Id == sr.CompanyId);

            var location = await context.Locations.SingleOrDefaultAsync(c => c.Id == sr.LocationId);

            var serviceCatalogue = context.GetServiceCatalogueForCompany(sr.PhysicianId, sr.CompanyId).ToList();

            var service = serviceCatalogue.SingleOrDefault(c => c.LocationId == location.LocationId && c.ServiceId == sr.ServiceId);
            if (service == null || !service.Price.HasValue)
            {
                this.ModelState.AddModelError("ServiceId", "This service has not been offered to this company at this location.");
            }

            var rates = context.GetServiceCatalogueRate(new Guid(sr.PhysicianId), sr.CompanyGuid).First();
            if (rates == null || !rates.NoShowRate.HasValue || !rates.LateCancellationRate.HasValue)
            {
                this.ModelState.AddModelError("ServiceId", "No Show Rates or Late Cancellation Rates have not been set for this company.");
            }

            if (ModelState.IsValid)
            {
                var additionalErrors = new ModelErrorCollection();
                var obj = new ServiceRequest()
                {
                    ServiceCatalogueId = service.ServiceCatalogueId,
                    AppointmentDate = sr.AppointmentDate,
                    AvailableSlotId = sr.AvailableSlotId,
                    DueDate = sr.DueDate,
                    AddressId = location.Id,
                    CaseCoordinatorId = sr.CaseCoordinatorId,
                    IntakeAssistantId = sr.IntakeAssistantId,
                    DocumentReviewerId = sr.DocumentReviewerId,
                    ClaimantName = sr.ClaimantName,
                    CompanyReferenceId = sr.CompanyReferenceId,
                    RequestedBy = sr.RequestedBy,
                    RequestedDate = sr.RequestedDate,
                    DocumentFolderLink = sr.DocumentFolderLink,
                    CompanyId = sr.CompanyId,
                    PhysicianId = sr.PhysicianId,
                    ServiceId = service.ServiceId,
                    LocationId = (short?)service.LocationId,
                    ServiceCataloguePrice = service.Price,
                    NoShowRate = rates.NoShowRate,
                    LateCancellationRate = rates.LateCancellationRate,
                    ModifiedUser = User.Identity.Name,
                    ServiceName = string.Empty, // this should not be needed but edmx is making it non nullable
                    PhysicianUserName = string.Empty, // same as ServiceName
                    ModifiedDate = SystemTime.Now(),


                };

                context.ServiceRequests.Add(obj);
                await context.SaveChangesAsync();
                await context.Entry(obj).ReloadAsync();

                //CREATE THE INVOICE
                var serviceRequest = obj;

                var serviceProvider = await context.BillableEntities.SingleOrDefaultAsync(c => c.EntityGuid.ToString() == serviceRequest.PhysicianId);
                var customer = await context.BillableEntities.SingleOrDefaultAsync(c => c.EntityGuid == serviceRequest.CompanyGuid.Value);

                var invoiceNumber = context.GetNextInvoiceNumber().SingleOrDefault();
                var invoiceDate = serviceRequest.AppointmentDate.Value;

                var invoice = new Invoice();
                invoice.BuildInvoice(serviceProvider, customer, invoiceNumber, invoiceDate, User.Identity.Name);

                // Create or update the invoice detail for the service request
                InvoiceDetail invoiceDetail;

                invoiceDetail = context.InvoiceDetails.SingleOrDefault(c => c.ServiceRequestId == serviceRequest.Id);
                if (invoiceDetail == null)
                {
                    invoiceDetail = new InvoiceDetail();
                    invoiceDetail.BuildInvoiceDetailFromServiceRequest(serviceRequest, User.Identity.Name);
                    invoice.InvoiceDetails.Add(invoiceDetail);
                }
                else
                {
                    invoiceDetail.BuildInvoiceDetailFromServiceRequest(serviceRequest, User.Identity.Name);
                }

                invoice.CalculateTotal();
                context.Invoices.Add(invoice);

                var validationResults = context.GetValidationErrors();
                if (validationResults.Count() > 0)
                {
                    foreach (var validationResult in validationResults)
                    {
                        foreach (var error in validationResult.ValidationErrors)
                        {
                            additionalErrors.Add(new ModelError(error.ErrorMessage));
                        }
                    }
                }
                else
                {
                    await context.SaveChangesAsync();
                }

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
                //using (var db = new OrvosiEntities(User.Identity.Name))
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
                return RedirectToAction("Details", new { id = obj.Id });
            }

            var vm = new CreateViewModel();
            vm.AvailableDay = context.AvailableSlots.First(c => c.Id == sr.AvailableSlotId).AvailableDay;
            vm.Physician = context.Physicians.SingleOrDefault(c => c.Id == sr.PhysicianId);
            vm.ServiceRequest.AvailableSlotId = sr.AvailableSlotId;
            vm.ServiceRequest.PhysicianId = sr.PhysicianId;
            vm.ServiceRequest.PhysicianDisplayName = vm.Physician.DisplayName;
            vm.ServiceRequest.AppointmentDate = sr.AppointmentDate;

            return View(vm);
        }

        [HttpGet]
        public ActionResult CreateSuccess(CreateSuccessViewModel obj)
        {
            return View(obj);
        }

        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public ActionResult CreateAddOn() => View();

        [Authorize(Roles = "Case Coordinator, Super Admin")]
        [HttpPost]
        public async Task<ActionResult> CreateAddOn(ServiceRequest sr)
        {
            if (sr.ServiceId != Services.Addendum && sr.ServiceId != Services.PaperReview)
            {
                this.ModelState.AddModelError("ServiceId", "Service must be an AddOn.");
            }

            var company = await context.Companies.SingleOrDefaultAsync(c => c.Id == sr.CompanyId);

            var serviceCatalogue = context.GetServiceCatalogueForCompany(sr.PhysicianId, sr.CompanyId).ToList();

            var service = serviceCatalogue.SingleOrDefault(c => c.ServiceId == sr.ServiceId && c.LocationId == 0);
            if (service == null || !service.Price.HasValue)
            {
                this.ModelState.AddModelError("ServiceId", "This service has not been offered to this company at this location.");
            }

            var rates = context.GetServiceCatalogueRate(new Guid(sr.PhysicianId), sr.CompanyGuid).First();
            if (rates == null || !rates.NoShowRate.HasValue || !rates.LateCancellationRate.HasValue)
            {
                this.ModelState.AddModelError("ServiceId", "No Show Rates or Late Cancellation Rates have not been set for this company.");
            }

            if (ModelState.IsValid)
            {
                var additionalErrors = new ModelErrorCollection();
                var obj = new ServiceRequest()
                {
                    ServiceCatalogueId = service.ServiceCatalogueId,
                    DueDate = sr.DueDate,
                    CaseCoordinatorId = sr.CaseCoordinatorId,
                    ClaimantName = sr.ClaimantName,
                    CompanyReferenceId = sr.CompanyReferenceId,
                    RequestedDate = sr.RequestedDate,
                    CompanyId = sr.CompanyId,
                    PhysicianId = sr.PhysicianId,
                    ServiceId = service.ServiceId,
                    LocationId = (short?)service.LocationId,
                    ServiceCataloguePrice = service.Price,
                    NoShowRate = rates.NoShowRate,
                    LateCancellationRate = rates.LateCancellationRate,
                    ModifiedUser = User.Identity.Name,
                    ServiceName = string.Empty, // this should not be needed but edmx is making it non nullable
                    PhysicianUserName = string.Empty, // same as ServiceName
                    ModifiedDate = SystemTime.Now()
                };

                context.ServiceRequests.Add(obj);
                await context.SaveChangesAsync();

                return RedirectToAction("Details", new { id = obj.Id });
            }

            return View(sr);
        }

        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> ResourceAssignment(int? id)
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
        public async Task<ActionResult> ResourceAssignment(ServiceRequest sr)
        {
            if (ModelState.IsValid)
            {
                using (context = new OrvosiEntities(User.Identity.Name))
                {
                    // get the tracked object from the database
                    var obj = await context.ServiceRequests.SingleOrDefaultAsync(c => c.Id == sr.Id);

                    // update the resource assignments
                    obj.CaseCoordinatorId = sr.CaseCoordinatorId;
                    obj.DocumentReviewerId = sr.DocumentReviewerId;
                    obj.IntakeAssistantId = sr.IntakeAssistantId;

                    await context.SaveChangesAsync();

                    return RedirectToAction("Details", new { id = sr.Id });
                }
            }
            return View(sr);
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
                using (context = new OrvosiEntities(User.Identity.Name))
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
                context.ServiceRequest_ToggleCancellation(form.Id, form.CancelledDate, form.IsLate == "on" ? true : false, string.Concat(serviceRequest.Notes, '\n', form.Notes));

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
            context.ServiceRequest_ToggleCancellation(id, null, false, string.Empty);

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
        public async Task<ActionResult> MarkAsComplete(int? id, decimal? hours, string notes)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var serviceRequestTask = await context.ServiceRequestTasks.FindAsync(id);
            if (serviceRequestTask == null)
            {
                return HttpNotFound();
            }
            serviceRequestTask.CompletedDate = DateTime.UtcNow;
            serviceRequestTask.ActualHours = hours;
            serviceRequestTask.Notes = notes;
            serviceRequestTask.ModifiedDate = DateTime.UtcNow;
            serviceRequestTask.ModifiedUser = User.Identity.Name;
            await context.SaveChangesAsync();
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
            context.ServiceRequest_ToggleNoShow(id);

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

        [HttpPost]
        [Authorize(Roles = "Staff, Super Admin")]
        public async Task<ActionResult> GetAvailability([Bind(Include = "PhysicianId, AppointmentDate")] ServiceRequest serviceRequest)
        {
            var ad = await context.AvailableDays
                .SingleOrDefaultAsync(c => c.PhysicianId == serviceRequest.PhysicianId && c.Day == serviceRequest.AppointmentDate);

            var p = await context.Physicians.SingleOrDefaultAsync(c => c.Id == serviceRequest.PhysicianId);

            var vm = new CreateViewModel();
            vm.ServiceRequest = serviceRequest;
            vm.IsAvailableDaySelected = true;

            if (ad != null)
            {
                vm.AvailableDay = ad;
                vm.IsAvailable = true;

                if (ad.LocationId != null)
                {
                    vm.ServiceRequest.LocationId = ad.LocationId;
                    vm.ServiceRequest.LocationName = ad.LocationName;
                }

                if (ad.IsPrebook)
                {
                    vm.ServiceRequest.CompanyId = ad.CompanyId;
                    vm.ServiceRequest.CompanyName = ad.CompanyName;
                }
            }
            else
            {
                vm.IsAvailable = false;

            }
            // this will be null if they are not available
            vm.ServiceRequest.PhysicianDisplayName = p.DisplayName;

            await GetCreateDropdownlistData(vm.AvailableDay);

            return View("Create", vm);
        }

        #region Dropbox

        [HttpPost]
        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> CreateDropboxFolder(short id)
        {
            var obj = await context.ServiceRequests.FindAsync(id);
            var client = await dropbox.GetServiceAccountClientAsync();

            if (obj.ServiceCategoryId == ServiceCategories.AddOn)
            {
                obj.DocumentFolderLink = string.Format("/cases/{0}/AddOns/{1}", obj.PhysicianUserName, obj.Title.Trim());
            }
            else
            {
                var month = obj.AppointmentDate.Value.ToString("yyyy-MM");
                obj.DocumentFolderLink = string.Format("/cases/{0}/{1}/{2}", obj.PhysicianUserName, month, obj.Title.Trim());
            }

            await context.SaveChangesAsync();

            // Get the destination folder name
            var destination = obj.DocumentFolderLink;

            // Check if the folder already exists)
            Metadata metadata = null;
            // Check if the folder already exists
            try
            {
                // Get the folder
                metadata = await client.Files.GetMetadataAsync(destination);
            }
            catch (ApiException<GetMetadataError> ex)
            {
                if (!ex.ErrorResponse.AsPath.Value.IsNotFound)
                    throw;
            }

            if (metadata != null)
            {
                //TODO: Add some error messaging error
                throw new Exception("The dropbox folder already exists.");
            }

            // Copy the case template folder
            var folder = await client.Files.CopyAsync(new RelocationArg("/cases/_addonfoldertemplate", destination));

            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> ShareDropboxFolder(int serviceRequestId, string folderId)
        {
            var client = await dropbox.GetServiceAccountClientAsync();

            var metadata = await GetMetadata(folderId, client);

            // Share the folder
            await client.Sharing.ShareFolderAsync(new ShareFolderArg(metadata.AsFolder.PathLower, MemberPolicy.Team.Instance, AclUpdatePolicy.Editors.Instance));

            return RedirectToAction("Details", new { id = serviceRequestId });
        }

        [HttpPost]
        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> UnshareDropboxFolder(int serviceRequestId, string folderId)
        {
            var client = await dropbox.GetServiceAccountClientAsync();

            var metadata = await GetMetadata(folderId, client);

            // Unshare the folder
            await client.Sharing.UnshareFolderAsync(new UnshareFolderArg(metadata.AsFolder.SharingInfo.SharedFolderId, false));

            return RedirectToAction("Details", new { id = serviceRequestId });
        }

        [HttpPost]
        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> ShareDropboxFolderToMember(string folderId, string userId, short serviceRequestId)
        {
            var client = await dropbox.GetServiceAccountClientAsync();

            Metadata metadata = await GetMetadata(folderId, client);

            string sharedFolderId = string.Empty;
            if (metadata.AsFolder.SharingInfo == null)
            {
                throw new Exception("Folder is not shared out");
            }
            else
            {
                sharedFolderId = metadata.AsFolder.SharingInfo.SharedFolderId;
            }

            var user = await context.Users.SingleAsync(c => c.Id == userId);
            await DropboxAddMember(dropbox, client, user.Email, sharedFolderId);

            return RedirectToAction("Details", new { id = serviceRequestId });
        }

        [HttpPost]
        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> UnshareDropboxFolderFromMember(string folderId, string userId, short serviceRequestId)
        {
            var client = await dropbox.GetServiceAccountClientAsync();

            Metadata metadata = await GetMetadata(folderId, client);

            string sharedFolderId = string.Empty;
            if (metadata.AsFolder.SharingInfo == null)
            {
                throw new Exception("Folder is not shared out");
            }
            else
            {
                sharedFolderId = metadata.AsFolder.SharingInfo.SharedFolderId;
            }

            var user = await context.Users.SingleAsync(c => c.Id == userId);
            await DropboxRemoveMember(client, user.Email, sharedFolderId);

            return RedirectToAction("Details", new { id = serviceRequestId });
        }

        private static async Task<Metadata> GetMetadata(string folderId, DropboxClient client)
        {
            Metadata metadata = null;
            // Check if the folder already exists
            try
            {
                // Get the folder
                metadata = await client.Files.GetMetadataAsync(folderId);
            }
            catch (ApiException<GetMetadataError> ex)
            {
                if (!ex.ErrorResponse.AsPath.Value.IsNotFound)
                    throw;
            }

            if (metadata == null)
            {
                //TODO: Add some error messaging error
                throw new Exception("The dropbox folder does not exist.");
            }

            return metadata;
        }

        private static async Task<List<MembersGetInfoItem>> GetSharedFolderMembers(OrvosiDropbox dropbox, Dropbox.Api.DropboxClient client, string sharedFolderId)
        {
            // Get the members for the shared folder
            var sharedMembers = await client.Sharing.ListFolderMembersAsync(sharedFolderId);

            // Get the full member entities of the shared members
            var args = new List<UserSelectorArg>();
            foreach (var m in sharedMembers.Users)
            {
                args.Add(new UserSelectorArg.TeamMemberId(m.User.TeamMemberId));
            }
            var members = await dropbox.TeamClient.Team.MembersGetInfoAsync(args);
            return members;
        }

        private static async Task DropboxAddMember(OrvosiDropbox dropbox, DropboxClient client, string email, string sharedFolderId)
        {
            await client.Sharing.AddFolderMemberAsync(
                sharedFolderId,
                new List<AddMember>()
                {
                        new AddMember(
                            new MemberSelector.Email(email)
                        )
                },
                true
            );

            await client.Sharing.UpdateFolderMemberAsync(
                sharedFolderId,
                new MemberSelector.Email(email),
                AccessLevel.Editor.Instance
            );

            var cc = await dropbox.GetTeamMemberClientAsync(email);
            await cc.Sharing.MountFolderAsync(sharedFolderId);
        }

        private static async Task DropboxRemoveMember(DropboxClient client, string email, string sharedFolderId)
        {
            // Add the case coordinator to the share
            await client.Sharing.RemoveFolderMemberAsync(
                sharedFolderId,
                new MemberSelector.Email(email),
                false
            );
        }

        private async Task ApplyMemberChangesToDropbox(ServiceRequest request, OrvosiDropbox dropbox, Dropbox.Api.DropboxClient client, string sharedFolderId)
        {
            string[] resources = new string[3]
                    {
                        request.CaseCoordinatorId.ToString(),
                        request.DocumentReviewerId.ToString(),
                        request.IntakeAssistantId.ToString()
                    };

            var list = resources.Where(r => !string.IsNullOrEmpty(r)).Distinct().ToList();
            var users = context.Users.Where(c => list.Contains(c.Id)).ToList();

            var members = await GetSharedFolderMembers(dropbox, client, sharedFolderId);

            // Add members
            foreach (var user in users)
            {
                if (!members.Exists(c => c.AsMemberInfo.Value.Profile.Email == user.Email))
                {
                    await DropboxAddMember(dropbox, client, user.Email, sharedFolderId);
                }
            }

            // remove members
            foreach (var member in members)
            {
                var memberEmail = member.AsMemberInfo.Value.Profile.Email;
                var user = context.Users.SingleOrDefault(c => c.Email == memberEmail);
                if (user.RoleId == Roles.CaseCoordinator || user.RoleId == Roles.DocumentReviewer || user.RoleId == Roles.IntakeAssistant)
                {
                    if (!users.Exists(c => c.Email == memberEmail))
                    {
                        await DropboxRemoveMember(client, memberEmail, sharedFolderId);
                    }
                }
            }
        }

        #endregion


        #region Box

        [HttpPost]
        public ActionResult CreateBoxCaseFolder(int ServiceRequestId)
        {
            using (var db = new OrvosiEntities(User.Identity.Name))
            {
                // Get the request
                var request = db.ServiceRequests.Single(sr => sr.Id == ServiceRequestId);

                // Get the request and assert they have a Box Folder Id
                var physician = db.Physicians.Single(p => p.Id == request.PhysicianId);
                //var physicianBoxFolderId = "7027883033"; // This overrides to HanSolo box folder while developing. Comment out for production.
                var physicianBoxFolderId = physician.BoxFolderId;
                if (string.IsNullOrEmpty(physicianBoxFolderId))
                    return PartialView("Error", "Physician does not have a cases folder setup in Box.");

                // Create the case folder
                var box = new BoxManager();
                BoxFolder caseFolder;
                if (request.ServiceCategoryId == ServiceCategories.AddOn)
                {
                    var province = db.GetCompanyProvince(request.CompanyId).FirstOrDefault();
                    if (province == null)
                    {
                        province = new GetCompanyProvince_Result() { ProvinceID = 0, ProvinceName = "Ontario" };
                    }
                    caseFolder = box.CreateAddOnFolder(physicianBoxFolderId, province.ProvinceName, request.DueDate.Value, request.Title, physician.BoxAddOnTemplateFolderId);
                }
                else
                {
                    // Get the province which is used in the case folder path
                    var province = db.Provinces.Single(p => p.Id == request.ProvinceId);
                    caseFolder = box.CreateCaseFolder(physicianBoxFolderId, province.ProvinceName, request.AppointmentDate.Value, request.Title, physician.BoxCaseTemplateFolderId);
                }

                // Persist the new case folder Id to the database.
                request.BoxCaseFolderId = caseFolder.Id;
                db.SaveChanges();

                // Redirect to display the Box Folder
                return RedirectToAction("Details", new { id = ServiceRequestId });
            }
        }

        [HttpPost]
        public ActionResult ShareBoxFolder(int ServiceRequestId, string FolderId, string UserId)
        {
            using (var db = new OrvosiEntities())
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
            using (var db = new OrvosiEntities())
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

        public ActionResult AcceptBoxFolder(int ServiceRequestId, string UserId, string CollaborationId)
        {
            string boxUserId;
            using (var db = new OrvosiEntities())
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
            ViewBag.Physicians = await context.Physicians
                .Select(c => new SelectListItem()
                {
                    Text = c.DisplayName,
                    Value = c.Id.ToString(),
                    Group = new SelectListGroup() { Name = c.PrimarySpecialtyName }
                }).ToListAsync();
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
                .Where(c => c.ServicePortfolioId == Model.Enums.ServicePortfolios.Physician)
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Group = new SelectListGroup() { Name = c.ServiceCategoryName }
                }).ToListAsync();

            var slots = await context.AvailableSlots
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
                    Group = new SelectListGroup() { Name = c.ParentName }
                }).ToListAsync();

            var l = await context.Locations.ToListAsync();
            ViewBag.Locations = l.Select(c => new SelectListItem()
            {
                Text = string.Format("{0} - {1}", c.LocationName, c.Name),
                Value = c.Id.ToString(),
                Group = new SelectListGroup() { Name = c.EntityDisplayName }
            });

            ViewBag.Requestors = await context.Users
                .Where(u => u.RoleCategoryId == RoleCategory.Company)
                .Select(c => new SelectListItem()
                {
                    Text = c.DisplayName,
                    Value = c.Id.ToString()
                }).ToListAsync();

            ViewBag.Staff = await context.Users
                .Where(u => u.RoleCategoryId == RoleCategory.Staff || u.RoleCategoryId == RoleCategory.Admin)
                .Select(c => new SelectListItem()
                {
                    Text = c.DisplayName,
                    Value = c.Id.ToString()
                }).ToListAsync();
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
