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
using Dropbox.Api.Files;
using Dropbox.Api.Sharing;
using Dropbox.Api.Team;

namespace WebApp.Controllers
{
    public delegate void NoShowToggledHandler(object sender, EventArgs e);

    [Authorize]
    public class ServiceRequestController : Controller
    {
        private OrvosiEntities db = new OrvosiEntities();

        public event NoShowToggledHandler NoShowToggledEvent;
        

        public async Task<ActionResult> Index(FilterArgs filterArgs)
        {
            var vm = new IndexViewModel();
            // get the user
            vm.User = db.Users.Single(u => u.UserName == User.Identity.Name);

            filterArgs.ShowAll = (filterArgs.ShowAll ?? true);
            filterArgs.Sort = (filterArgs.Sort ?? "Oldest");
            filterArgs.StatusId = (filterArgs.StatusId ?? ServiceRequestStatus.Open);

            var sr = db.ServiceRequests.AsQueryable();

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
                sr = sr.OrderByDescending(c => c.AppointmentDate.Value).ThenBy(c => c.StartTime.Value);
            }
            else
            {
                sr = sr.OrderBy(c => c.AppointmentDate.Value).ThenBy(c => c.StartTime.Value);
            }

            // order the requests from oldest to newest
            vm.ServiceRequests = await sr.ToListAsync();

            vm.FilterArgs = filterArgs;

            return View(vm);
        }

        public async Task<ActionResult> Dashboard()
        {
            var user = db.Users.Single(u => u.UserName == User.Identity.Name);

            var list = await db.ServiceRequests
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

            var vm = new DetailsViewModel();

            vm.User = db.Users.Single(u => u.UserName == User.Identity.Name);
            vm.ServiceRequest = await db.ServiceRequests.FindAsync(id);
            vm.ServiceRequestTasks = db.ServiceRequestTasks.Where(sr => sr.ServiceRequestId == id).OrderBy(c => c.Sequence).ToList();
            vm.ServiceRequestCostRollUps = db.ServiceRequestCostRollUps.Where(sr => sr.ServiceRequestId == id).OrderBy(c => c.Id).ToList();
            vm.Invoices = db.Invoices.Join(db.InvoiceDetails.Where(ids => ids.ServiceRequestId == id),
                i => i.Id,
                ids => ids.InvoiceId,
                (i, ids) => i);

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

            if (vm.DropboxFolder != null)
            {
                if (vm.DropboxFolder.AsFolder.SharingInfo != null)
                {
                    // Get the shared folder members
                    vm.DropboxFolderMembers = await GetSharedFolderMembers(dropbox, client, vm.DropboxFolder.AsFolder.SharingInfo.SharedFolderId);
                }
            }

            if (vm.ServiceRequest == null)
            {
                return HttpNotFound();
            }
            return View(vm);
        }

        [Authorize(Roles = "Case Coordinator, Super Admin")]
        // GET: Admin/ServiceRequest/Create
        public ActionResult Availability() => View();

        [HttpPost]
        [Authorize(Roles = "Case Coordinator, Super Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Availability(AvailabilityForm form)
        {
            var ad = await db.AvailableDays
                .SingleOrDefaultAsync(c => c.PhysicianId == form.PhysicianId && c.Day == form.AppointmentDate);

            var p = await db.Physicians.SingleOrDefaultAsync(c => c.Id == form.PhysicianId);

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
            var company = await db.Companies.SingleOrDefaultAsync(c => c.Id == sr.CompanyId);

            var location = await db.Locations.SingleOrDefaultAsync(c => c.Id == sr.LocationId);

            var serviceCatalogue = db.GetServiceCatalogueForCompany(sr.PhysicianId, sr.CompanyId).ToList();

            var service = serviceCatalogue.SingleOrDefault(c => c.LocationId == location.LocationId && c.ServiceId == sr.ServiceId);
            if (service == null || !service.Price.HasValue)
            {
                this.ModelState.AddModelError("ServiceId", "This service has not been offered to this company at this location.");
            }

            var rates = db.GetServiceCatalogueRate(new Guid(sr.PhysicianId), sr.CompanyGuid).First();
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
                    LocationId = service.LocationId,
                    ServiceCataloguePrice = service.Price,
                    NoShowRate = rates.NoShowRate,
                    LateCancellationRate = rates.LateCancellationRate,
                    ModifiedUser = User.Identity.Name,
                    ServiceName = string.Empty, // this should not be needed but edmx is making it non nullable
                    PhysicianUserName = string.Empty, // same as ServiceName
                    ModifiedDate = SystemTime.Now()

                };

                db.ServiceRequests.Add(obj);
                await db.SaveChangesAsync();
                await db.Entry(obj).ReloadAsync();

                var month = obj.AppointmentDate.Value.ToString("yyyy-MM");
                obj.DocumentFolderLink = string.Format("/cases/{0}/{1}/{2}", obj.PhysicianUserName, month, obj.Title.Trim());


                //CREATE THE INVOICE
                var serviceRequest = obj;

                var serviceProvider = await db.BillableEntities.SingleOrDefaultAsync(c => c.EntityGuid.ToString() == serviceRequest.PhysicianId);
                var customer = await db.BillableEntities.SingleOrDefaultAsync(c => c.EntityGuid == serviceRequest.CompanyGuid.Value);

                var invoiceNumber = db.GetNextInvoiceNumber().SingleOrDefault();
                var invoiceDate = serviceRequest.AppointmentDate.Value;

                var invoice = new Invoice();
                invoice.BuildInvoice(serviceProvider, customer, invoiceNumber, invoiceDate, User.Identity.Name);

                // Create or update the invoice detail for the service request
                InvoiceDetail invoiceDetail;

                invoiceDetail = db.InvoiceDetails.SingleOrDefault(c => c.ServiceRequestId == serviceRequest.Id);
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
                db.Invoices.Add(invoice);

                var validationResults = db.GetValidationErrors();
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
                    await db.SaveChangesAsync();
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

                /***********************************
                Dropbox
                ***********************************/
                var dropbox = new OrvosiDropbox();
                var client = await dropbox.GetServiceAccountClientAsync();
                Metadata folder = null;
                List<MembersGetInfoItem> members = new List<MembersGetInfoItem>();
                try
                {

                    // Copy the case template folder
                    var destination = obj.DocumentFolderLink;
                    folder = await client.Files.CopyAsync(new RelocationArg("/cases/_casefoldertemplate", destination));
                    // Share the folder
                    var caseCoordinator = await db.Users.SingleAsync(c => c.Id == sr.CaseCoordinatorId.Value.ToString());
                    var share = await client.Sharing.ShareFolderAsync(new ShareFolderArg(folder.PathLower, MemberPolicy.Team.Instance, AclUpdatePolicy.Editors.Instance));
                    var sharedFolderId = share.AsComplete.Value.SharedFolderId;

                    var physician = await db.Users.SingleAsync(c => c.Id == obj.PhysicianId);
                    await DropboxAddMember(dropbox, client, caseCoordinator.Email, sharedFolderId);
                    await DropboxAddMember(dropbox, client, physician.Email, sharedFolderId);

                    // Get the folder
                    folder = await client.Files.GetMetadataAsync(folder.AsFolder.PathLower);

                    // Get the shared folder members
                    members = await GetSharedFolderMembers(dropbox, client, sharedFolderId);

                }
                catch (Exception ex)
                {
                    additionalErrors.Add(new ModelError(ex.Message));
                }
                // Create the calendar event

                // mark the first task as complete
                using (var db = new OrvosiEntities(User.Identity.Name))
                {
                    var serviceRequestTask = await db.ServiceRequestTasks.SingleOrDefaultAsync(c => c.ServiceRequestId == obj.Id && c.TaskId == Tasks.CreateCaseFolder);
                    serviceRequestTask.CompletedDate = SystemTime.Now();
                    await db.SaveChangesAsync();
                }

                var model = new CreateSuccessViewModel()
                {
                    ServiceRequest = obj,
                    Folder = folder,
                    Members = members,
                    Invoice = invoice,
                    Errors = additionalErrors
                };
                return View("CreateSuccess", model);
            }

            var vm = new CreateViewModel();
            vm.AvailableDay = db.AvailableSlots.First(c => c.Id == sr.AvailableSlotId).AvailableDay;
            vm.Physician = db.Physicians.SingleOrDefault(c => c.Id == sr.PhysicianId);
            vm.ServiceRequest.AvailableSlotId = sr.AvailableSlotId;
            vm.ServiceRequest.PhysicianId = sr.PhysicianId;
            vm.ServiceRequest.PhysicianDisplayName = vm.Physician.DisplayName;
            vm.ServiceRequest.AppointmentDate = sr.AppointmentDate;

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> CreateDropboxFolder(short id)
        {
            var obj = await db.ServiceRequests.FindAsync(id);
            var dropbox = new OrvosiDropbox();
            var client = await dropbox.GetServiceAccountClientAsync();

            var month = obj.AppointmentDate.Value.ToString("yyyy-MM");
            obj.DocumentFolderLink = string.Format("/cases/{0}/{1}/{2}", obj.PhysicianUserName, month, obj.Title.Trim());
            await db.SaveChangesAsync();

            // Get the destination folder name
            var destination = obj.DocumentFolderLink;

            Metadata metadata = null;
            // Check if the folder already exists
            try
            {
                // Get the folder
                metadata = await client.Files.GetMetadataAsync(destination);

                if (metadata != null)
                {
                    return RedirectToAction("Details", new { id = id });
                }

                // Copy the case template folder
                var folder = await client.Files.CopyAsync(new RelocationArg("/cases/_casefoldertemplate", destination));

                // Share the folder
                var share = await client.Sharing.ShareFolderAsync(new ShareFolderArg(folder.PathLower, MemberPolicy.Team.Instance, AclUpdatePolicy.Editors.Instance));
                var sharedFolderId = share.AsComplete.Value.SharedFolderId;

                var physician = await db.Users.SingleOrDefaultAsync(c => c.Id == obj.PhysicianId);
                if (physician != null) { await DropboxAddMember(dropbox, client, physician.Email, sharedFolderId); }

                await ApplyMemberChangesToDropbox(obj, dropbox, client, sharedFolderId);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex);
            }
            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> ShareFolderToPhysician(short id)
        {
            var obj = await db.ServiceRequests.FindAsync(id);
            var dropbox = new OrvosiDropbox();
            var client = await dropbox.GetServiceAccountClientAsync();

            // Get the destination folder name
            var destination = obj.DocumentFolderLink;

            Metadata metadata = null;
            // Check if the folder already exists
            try
            {
                // Get the folder
                metadata = await client.Files.GetMetadataAsync(destination);
            }
            catch (Dropbox.Api.ApiException<Dropbox.Api.Files.GetMetadataError> ex)
            {
                if (!ex.ErrorResponse.AsPath.Value.IsNotFound)
                    throw;
            }

            if (metadata == null)
            {
                //TODO: Add some error messaging error
                return RedirectToAction("Details", new { id = id });
            }

            string sharedFolderId = string.Empty;
            // Share the folder
            if (metadata.AsFolder.SharingInfo == null)
            {
                var share = await client.Sharing.ShareFolderAsync(new ShareFolderArg(metadata.AsFolder.PathLower, MemberPolicy.Team.Instance, AclUpdatePolicy.Editors.Instance));
                sharedFolderId = share.AsComplete.Value.SharedFolderId;
            }
            else
            {
                sharedFolderId = metadata.AsFolder.SharingInfo.SharedFolderId;
            }

            var physician = await db.Users.SingleOrDefaultAsync(c => c.Id == obj.PhysicianId);
            if (physician != null) { await DropboxAddMember(dropbox, client, physician.Email, sharedFolderId); }

            return RedirectToAction("Details", new { id = id });
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

        private static async Task DropboxAddMember(OrvosiDropbox dropbox, Dropbox.Api.DropboxClient client, string email, string sharedFolderId)
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

        private static async Task DropboxRemoveMember(Dropbox.Api.DropboxClient client, string email, string sharedFolderId)
        {
            // Add the case coordinator to the share
            await client.Sharing.RemoveFolderMemberAsync(
                sharedFolderId,
                new MemberSelector.Email(email),
                false
            );
        }

        [HttpGet]
        public ActionResult CreateSuccess(CreateSuccessViewModel obj)
        {
            return View(obj);
        }

        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> ResourceAssignment(int? id)
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
                using (db = new OrvosiEntities(User.Identity.Name))
                {
                    // get the tracked object from the database
                    var obj = await db.ServiceRequests.SingleOrDefaultAsync(c => c.Id == sr.Id);

                    // update the resource assignments
                    obj.CaseCoordinatorId = sr.CaseCoordinatorId;
                    obj.DocumentReviewerId = sr.DocumentReviewerId;
                    obj.IntakeAssistantId = sr.IntakeAssistantId;

                    await db.SaveChangesAsync();

                    // Dropbox Integration
                    var dropbox = new OrvosiDropbox();
                    var client = await dropbox.GetServiceAccountClientAsync();
                    var folder = await client.Files.GetMetadataAsync(obj.DocumentFolderLink);
                    var sharedFolderId = folder.AsFolder.SharingInfo.SharedFolderId;

                    await ApplyMemberChangesToDropbox(obj, dropbox, client, sharedFolderId);

                    return RedirectToAction("Index");
                }
            }
            return View(sr);
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
            var users = db.Users.Where(c => list.Contains(c.Id)).ToList();

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
                var user = db.Users.SingleOrDefault(c => c.Email == memberEmail);
                if (user.RoleId == Roles.CaseCoordinator || user.RoleId == Roles.DocumentReviewer || user.RoleId == Roles.IntakeAssistant)
                {
                    if (!users.Exists(c => c.Email == memberEmail))
                    {
                        await DropboxRemoveMember(client, memberEmail, sharedFolderId);
                    }
                }
            }
        }

        [Authorize(Roles = "Case Coordinator, Super Admin")]
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
                using (db = new OrvosiEntities(User.Identity.Name))
                {
                    // get the tracked object from the database
                    var obj = await db.ServiceRequests.SingleOrDefaultAsync(c => c.Id == sr.Id);

                    // update the resource assignments
                    obj.ClaimantName = sr.ClaimantName;
                    obj.CompanyReferenceId = sr.CompanyReferenceId;
                    obj.DueDate = sr.DueDate;
                    obj.Notes = sr.Notes;

                    await db.SaveChangesAsync();

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
        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ServiceRequest serviceRequest = await db.ServiceRequests.FindAsync(id);
            db.ServiceRequests.Remove(serviceRequest);
            await db.SaveChangesAsync();

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

            ServiceRequest serviceRequest = await db.ServiceRequests.FindAsync(id);

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
            var serviceRequest = await db.ServiceRequests.FindAsync(form.Id);
            if (serviceRequest == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                db.ServiceRequest_ToggleCancellation(form.Id, form.CancelledDate, form.IsLate == "on" ? true : false, string.Concat(serviceRequest.Notes, '\n', form.Notes));

                serviceRequest.IsLateCancellation = form.IsLate == "on" ? true : false;

                if (serviceRequest.InvoiceDetails.Count > 0)
                {
                    var detail = serviceRequest.InvoiceDetails.First();
                    if (serviceRequest.IsLateCancellation)
                    {
                        var rates = db.GetServiceCatalogueRate(detail.Invoice.ServiceProviderGuid, detail.Invoice.CustomerGuid).First();
                        detail.ApplyDiscount(DiscountTypes.LateCancellation, rates.LateCancellationRate);
                    }
                    else
                    {
                        detail.RemoveDiscount();
                    }
                    detail.Invoice.CalculateTotal();
                    await db.SaveChangesAsync();
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
            var serviceRequest = await db.ServiceRequests.FindAsync(id);
            if (serviceRequest == null)
            {
                return HttpNotFound();
            }
            db.ServiceRequest_ToggleCancellation(id, null, false, string.Empty);

            if (serviceRequest.InvoiceDetails.Count > 0)
            {
                var detail = serviceRequest.InvoiceDetails.First();
                detail.RemoveDiscount();
                detail.Invoice.CalculateTotal();
                await db.SaveChangesAsync();
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
            var serviceRequestTask = await db.ServiceRequestTasks.FindAsync(id);
            if (serviceRequestTask == null)
            {
                return HttpNotFound();
            }
            serviceRequestTask.CompletedDate = DateTime.UtcNow;
            serviceRequestTask.ActualHours = hours;
            serviceRequestTask.Notes = notes;
            serviceRequestTask.ModifiedDate = DateTime.UtcNow;
            serviceRequestTask.ModifiedUser = User.Identity.Name;
            await db.SaveChangesAsync();
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
            var serviceRequest = await db.ServiceRequests.FindAsync(id);
            if (serviceRequest == null)
            {
                return HttpNotFound();
            }
            db.ServiceRequest_ToggleNoShow(id);

            serviceRequest.IsNoShow = !serviceRequest.IsNoShow;

            if (serviceRequest.InvoiceDetails.Count > 0)
            {
                var detail = serviceRequest.InvoiceDetails.First();
                if (serviceRequest.IsNoShow)
                {
                    var rates = db.GetServiceCatalogueRate(detail.Invoice.ServiceProviderGuid, detail.Invoice.CustomerGuid).First();
                    detail.ApplyDiscount(DiscountTypes.NoShow, rates.NoShowRate);
                }
                else
                {
                    detail.RemoveDiscount();
                }
                detail.Invoice.CalculateTotal();
                await db.SaveChangesAsync();
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [Authorize(Roles = "Staff, Super Admin")]
        public async Task<ActionResult> GetAvailability([Bind(Include = "PhysicianId, AppointmentDate")] ServiceRequest serviceRequest)
        {
            var ad = await db.AvailableDays
                .SingleOrDefaultAsync(c => c.PhysicianId == serviceRequest.PhysicianId && c.Day == serviceRequest.AppointmentDate);

            var p = await db.Physicians.SingleOrDefaultAsync(c => c.Id == serviceRequest.PhysicianId);

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

        private async Task GetPhysicianDropDownData()
        {
            ViewBag.Physicians = await db.Physicians
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

            ViewBag.Services = await db.Services
                .Where(c => c.ServicePortfolioId == Model.Enums.ServicePortfolios.Physician)
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Group = new SelectListGroup() { Name = c.ServiceCategoryName }
                }).ToListAsync();

            var slots = await db.AvailableSlots
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
                    Group = new SelectListGroup() { Name = c.ParentName }
                }).ToListAsync();

            var l = await db.Locations.ToListAsync();
            ViewBag.Locations = l.Select(c => new SelectListItem()
            {
                Text = string.Format("{0} - {1}", c.LocationName, c.Name),
                Value = c.Id.ToString(),
                Group = new SelectListGroup() { Name = c.EntityDisplayName }
            });

            ViewBag.Requestors = await db.Users
                .Where(u => u.RoleCategoryId == RoleCategory.Company)
                .Select(c => new SelectListItem()
                {
                    Text = c.DisplayName,
                    Value = c.Id.ToString()
                }).ToListAsync();

            ViewBag.Staff = await db.Users
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
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
