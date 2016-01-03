using Model;
using Model.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    public class ServiceRequestController : Controller
    {
        private OrvosiEntities db = new OrvosiEntities();

        // GET: ServiceRequest
        public async Task<ActionResult> Index()
        {
            var list = await db.ServiceRequests
                .ToListAsync();
            return View(list);
        }

        // GET: Admin/ServiceRequest/Details/5
        public ActionResult Details(int id)
        {
            var obj = db.ServiceRequests.Single(c => c.Id == id);
            return View(obj);
        }

        // GET: Admin/ServiceRequest/Create
        public ActionResult Create()
        {
            ViewBag.Companies = db.Companies.Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() }).ToList();

            ViewBag.Requestors = db.Users
                .Where(u => u.RoleCategoryId == RoleCategory.Company)
                .Select(c => new SelectListItem() {
                    Text = c.DisplayName,
                    Value = c.Id.ToString()
                }).ToList();

            ViewBag.Staff = db.Users
                .Where(u => u.RoleCategoryId == RoleCategory.Staff)
                .Select(c => new SelectListItem()
                {
                    Text = c.DisplayName,
                    Value = c.Id.ToString()
                }).ToList();

            return View();
        }

        // POST: Admin/ServiceRequest/Create
        [HttpPost]
        public ActionResult Create(ServiceRequest sr)
        {
            try
            {
                var obj = new ServiceRequest()
                {
                    ServiceCatalogueId = sr.ServiceCatalogueId,
                    AppointmentDate = sr.AppointmentDate,
                    AvailableSlotId = sr.AvailableSlotId,
                    AddressId = sr.AddressId,
                    CaseCoordinatorId = sr.CaseCoordinatorId,
                    IntakeAssistantId = sr.IntakeAssistantId,
                    DocumentReviewerId = sr.DocumentReviewerId,
                    ClaimantName = sr.ClaimantName,
                    CompanyReferenceId = sr.CompanyReferenceId,
                    RequestedBy = sr.RequestedBy,
                    RequestedDate = sr.RequestedDate,
                    ModifiedUser = User.Identity.Name,
                    ServiceName = string.Empty // this should not be needed but edmx is making it non nullable
                };

                using (var db = new OrvosiEntities(User.Identity.Name))
                {
                    db.ServiceRequests.Add(obj);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Companies = db.Companies.Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() }).ToList();

                ViewBag.Requestors = db.Users
                    .Where(u => u.RoleCategoryId == RoleCategory.Company)
                    .Select(c => new SelectListItem()
                    {
                        Text = c.DisplayName,
                        Value = c.Id.ToString()
                    }).ToList();

                ViewBag.Staff = db.Users
                    .Where(u => u.RoleCategoryId == RoleCategory.Staff)
                    .Select(c => new SelectListItem()
                    {
                        Text = c.DisplayName,
                        Value = c.Id.ToString()
                    }).ToList();

                return View();
            }
        }

        // GET: Admin/ServiceRequest/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/ServiceRequest/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/ServiceRequest/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/ServiceRequest/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Services(string physicianId, short companyId, short addressId)
        {
            var selected = db.Physicians.Single(c => c.Id == physicianId);

            var locationId = db.Locations.Single(c => c.Id == addressId).LocationId;

            var list = db.GetServiceCatalogueForCompany(physicianId, companyId)
                .Where(c => c.ServiceCatalogueId != null && c.LocationId == locationId)
                .Select(c => new SelectListItem()
                {
                    Text = c.ServiceName,
                    Value = c.ServiceCatalogueId.ToString()
                })
                .Distinct().ToList();

            return Json(new
            {
                selected = selected,
                services = list
            });
        }

        [HttpPost]
        public ActionResult Physicians(short companyId)
        {
            var selected = db.Companies.Single(c => c.Id == companyId);

            var list = db.PhysicianCompanies
                .Where(c => c.CompanyId == companyId && (c.RelationshipStatusId == RelationshipStatuses.Active || c.RelationshipStatusId == RelationshipStatuses.InProgress))
                .Select(c => new SelectListItem()
                {
                    Text = c.PhysicianDisplayName,
                    Value = c.PhysicianId.ToString()
                })
                .Distinct().ToList();

            return Json(new
            {
                selected = selected,
                physicians = list
            });
        }

        [HttpPost]
        public ActionResult ServiceCatalogue(short serviceCatalogueId)
        {
            var obj = db.ServiceCatalogues
                .Single(c => c.Id == serviceCatalogueId);

            return Json(obj);
        }

        [HttpPost]
        public ActionResult Locations(short companyId, string physicianId)
        {
            // return locations owned by the selected company and physician.
            var list = db.Location_Select_PhysicianAndCompany(physicianId, companyId)
                .Select(c => new SelectListItem()
                {
                    Text = string.Format("{0} - {1} - {2}", c.LocationName, c.EntityDisplayName, c.Name),
                    Value = c.Id.ToString()
                }).ToList();
            return Json(list);
        }

        [HttpPost]
        public ActionResult Requestors(short companyId)
        {
            var list = db.Users
                .Where(c => c.CompanyId == companyId)
                .Select(c => new SelectListItem()
                {
                    Text = c.DisplayName,
                    Value = c.Id.ToString()
                });

            return Json(new
            {
                requestors = list
            });
        }

        [HttpPost]
        public ActionResult AvailableSlots(string physicianId, DateTime day)
        {
            var ad = db.AvailableDays
                .Single(c => c.PhysicianId == physicianId && c.Day == day)
                .AvailableSlots.Select(c => new {
                    Id = c.Id,
                    StartTime = c.StartTime.ToString(),
                    Title = c.Title
                })
                .ToList();
            return Json(new
            {
                //day = ad,
                slots = ad
            });
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
