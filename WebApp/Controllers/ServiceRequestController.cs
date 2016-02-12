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

namespace WebApp.Controllers
{
    [Authorize]
    public class ServiceRequestController : Controller
    {
        private OrvosiEntities db = new OrvosiEntities();

        public async Task<ActionResult> Index(FilterArgs filterArgs)
        {
            filterArgs.ShowAll = (filterArgs.ShowAll ?? true);
            filterArgs.Sort = (filterArgs.Sort ?? "Oldest");

            var vm = new IndexViewModel();

            // get the user
            vm.User = db.Users.Single(u => u.UserName == User.Identity.Name);

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

            ViewBag.ServiceRequestStatuses = db.LookupItems
                .Where(l => l.LookupId == Lookups.ServiceRequestStatus)
                .Select(c => new SelectListItem()
                {
                    Text = c.ItemText,
                    Value = c.ItemId.ToString()
                })
                .ToList();

            ViewBag.Physicians = db.Physicians
                .Select(c => new SelectListItem()
                {
                    Text = c.DisplayName,
                    Value = c.Id.ToString()
                }).ToList();

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

            if (vm.ServiceRequest == null)
            {
                return HttpNotFound();
            }
            return View(vm);
        }

        [Authorize(Roles = "Case Coordinator, Super Admin")]
        // GET: Admin/ServiceRequest/Create
        public async Task<ActionResult> Availability()
        {
            await GetPhysicianDropDownData();

            return View();
        }

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

                await GetPhysicianDropDownData();

                return View();
            }

            var vm = new CreateViewModel();
            vm.AvailableDay = ad;
            vm.Physician = p;
            vm.ServiceRequest.PhysicianId = p.Id;
            vm.ServiceRequest.PhysicianDisplayName = p.DisplayName;
            vm.ServiceRequest.AppointmentDate = ad.Day;

            await GetCreateDropdownlistData(vm.AvailableDay);

            return View("Create", vm);
        }

        // POST: Admin/ServiceRequest/Create
        [Authorize(Roles = "Case Coordinator, Super Admin")]
        [HttpPost]
        public async Task<ActionResult> Create(ServiceRequest sr)
        {
            var company = await db.Companies.SingleOrDefaultAsync(c => c.Id == sr.CompanyId);

            var location = await db.Locations.SingleOrDefaultAsync(c => c.Id == sr.LocationId);

            var serviceCatalogue = await db.ServiceCatalogues
                .SingleOrDefaultAsync(c => c.PhysicianId == sr.PhysicianId
                                && c.CompanyId != null // parent id can be null and if these are included, more than one row can be returned.
                                && (c.CompanyId == sr.CompanyId || c.CompanyId == company.ParentId)
                                && c.LocationId == location.LocationId
                                && c.ServiceId == sr.ServiceId);

            if (serviceCatalogue == null)
            {
                this.ModelState.AddModelError("ServiceId", "This service has not been offered to this company at this location.");
            }

            var obj = new ServiceRequest()
            {
                ServiceCatalogueId = serviceCatalogue.Id,
                AppointmentDate = sr.AppointmentDate,
                AvailableSlotId = sr.AvailableSlotId,
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
                ModifiedUser = User.Identity.Name,
                ServiceName = string.Empty // this should not be needed but edmx is making it non nullable
            };

            using (var db = new OrvosiEntities(User.Identity.Name))
            {
                db.ServiceRequests.Add(obj);
                await db.SaveChangesAsync();
                await db.Entry(obj).ReloadAsync();
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

            // Copy the case template folder
            var month = obj.AppointmentDate.Value.ToString("yyyy-MM");
            var destination = string.Format("/cases/{0}/{1}/{2}", obj.PhysicianUserName, month, obj.Title);
            var folder = await client.Files.CopyAsync(new RelocationArg("cases/_CaseFolderTemplate", destination));

            var model = new CreateSuccessViewModel()
            {
                ServiceRequest = obj,
                Folder = folder
            };
            return RedirectToAction("CreateSuccess", model);
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult CreateSuccess(CreateSuccessViewModel obj)
        {
            return View(obj);
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

            ViewBag.Staff = await db.Users
                .Where(u => u.RoleCategoryId == RoleCategory.Staff || u.RoleCategoryId == RoleCategory.Admin)
                .Select(c => new SelectListItem()
                {
                    Text = c.DisplayName,
                    Value = c.Id.ToString()
                }).ToListAsync();

            // TODO: Update the calendar and dropbox folder if appropriate.

            return View(serviceRequest);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Case Coordinator, Super Admin")]
        public async Task<ActionResult> Edit(ServiceRequest serviceRequest)
        {
            if (ModelState.IsValid)
            {
                using (db = new OrvosiEntities(User.Identity.Name))
                {
                    db.Entry(serviceRequest).State = EntityState.Modified;
                    serviceRequest.ServiceName = string.Empty; // this should not be needed but edmx is making it non nullable

                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return View(serviceRequest);
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
            return RedirectToAction("Index");
        }

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
                return RedirectToAction("Index");
            }
            return View(serviceRequest);
        }

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
            }).ToList();

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
