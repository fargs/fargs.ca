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
using WebApp.ViewModels.ServiceRequestViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class ServiceRequestController : Controller
    {
        private OrvosiEntities db = new OrvosiEntities();

        // GET: ServiceRequest
        public async Task<ActionResult> Index()
        {
            return View(await db.ServiceRequests.ToListAsync());
        }

        // GET: ServiceRequest/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var vm = new DetailsViewModel();

            vm.ServiceRequest = await db.ServiceRequests.FindAsync(id);
            vm.ServiceRequestTasks = db.ServiceRequestTasks.Where(sr => sr.ServiceRequestId == id).ToList();
            if (vm.ServiceRequest == null)
            {
                return HttpNotFound();
            }
            return View(vm);
        }

        // GET: ServiceRequest/Create
        public ActionResult Create(Nullable<byte> companyId, string physicianId = "")
        {
            return View();
        }

        // POST: ServiceRequest/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ObjectGuid,CompanyReferenceId,ServiceCatalogueId,HarvestProjectId,Title,Body,RequestedDate,RequestedBy,CancelledDate,AssignedTo,StatusId,DueDate,StartTime,EndTime,ServiceRequestPriceOverride,ModifiedDate,ModifiedUser,ServiceName,ServiceCategoryName,ServicePortfolioName,DefaultPrice,PhysicianDisplayName,CompanyName,ParentCompanyName,ServiceDefaultPrice,ServiceCataloguePriceOverride,EffectivePrice,AssignedToDisplayName,RequestedByDisplayName,StatusName")] ServiceRequest serviceRequest)
        {
            if (ModelState.IsValid)
            {
                db.ServiceRequests.Add(serviceRequest);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(serviceRequest);
        }

        // GET: ServiceRequest/Edit/5
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
            return View(serviceRequest);
        }

        // POST: ServiceRequest/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ObjectGuid,CompanyReferenceId,ServiceCatalogueId,HarvestProjectId,Title,Body,RequestedDate,RequestedBy,CancelledDate,AssignedTo,StatusId,DueDate,StartTime,EndTime,ServiceRequestPriceOverride,ModifiedDate,ModifiedUser,ServiceName,ServiceCategoryName,ServicePortfolioName,DefaultPrice,PhysicianDisplayName,CompanyName,ParentCompanyName,ServiceDefaultPrice,ServiceCataloguePriceOverride,EffectivePrice,AssignedToDisplayName,RequestedByDisplayName,StatusName")] ServiceRequest serviceRequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviceRequest).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(serviceRequest);
        }

        // GET: ServiceRequest/Delete/5
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
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ServiceRequest serviceRequest = await db.ServiceRequests.FindAsync(id);
            db.ServiceRequests.Remove(serviceRequest);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
