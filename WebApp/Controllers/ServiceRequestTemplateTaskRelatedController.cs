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

namespace WebApp.Controllers
{
    public class ServiceRequestTemplateTaskRelatedController : Controller
    {
        private OrvosiDbContext db = new OrvosiDbContext();

        // GET: ServiceRequestTemplateTaskRelated
        public async Task<ActionResult> Index(Guid ServiceRequestTemplateTaskId)
        {
            ViewBag.ServiceRequestTemplateTaskId = ServiceRequestTemplateTaskId;
            var serviceRequestTemplateTaskRelateds = 
                db.ServiceRequestTemplateTaskRelateds
                    .Include(s => s.RelatedTask)
                    .Include(s => s.ServiceRequestTemplateTask_ServiceRequestTemplateTaskId)
                    .Where(t => t.ServiceRequestTemplateTaskId == ServiceRequestTemplateTaskId);
            return View(await serviceRequestTemplateTaskRelateds.ToListAsync());
        }

        // GET: ServiceRequestTemplateTaskRelated/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceRequestTemplateTaskRelated serviceRequestTemplateTaskRelated = await db.ServiceRequestTemplateTaskRelateds.FindAsync(id);
            if (serviceRequestTemplateTaskRelated == null)
            {
                return HttpNotFound();
            }
            return View(serviceRequestTemplateTaskRelated);
        }

        // GET: ServiceRequestTemplateTaskRelated/Create
        public ActionResult Create(Guid ServiceRequestTemplateTaskId)
        {
            var serviceRequestTemplateId = db.ServiceRequestTemplateTasks.Find(ServiceRequestTemplateTaskId).ServiceRequestTemplateId;
            var tasks = db.ServiceRequestTemplateTasks.Where(t => t.ServiceRequestTemplateId == serviceRequestTemplateId);
            ViewBag.ServiceRequestTemplateTaskId = new SelectList(tasks, "Id", "Task.Name", ServiceRequestTemplateTaskId);
            ViewBag.RelatedTaskId = new SelectList(tasks, "Id", "Task.Name");
            return View();
        }

        // POST: ServiceRequestTemplateTaskRelated/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ServiceRequestTemplateTaskId,RelatedTaskId,Relationship")] ServiceRequestTemplateTaskRelated serviceRequestTemplateTaskRelated)
        {
            if (ModelState.IsValid)
            {
                db.ServiceRequestTemplateTaskRelateds.Add(serviceRequestTemplateTaskRelated);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { ServiceRequestTemplateTaskId = serviceRequestTemplateTaskRelated.ServiceRequestTemplateTaskId });
            }

            var serviceRequestTemplateId = db.ServiceRequestTemplateTasks.Find(serviceRequestTemplateTaskRelated.Id).ServiceRequestTemplateId;
            var tasks = db.ServiceRequestTemplateTasks.Where(t => t.ServiceRequestTemplateId == serviceRequestTemplateId);
            ViewBag.RelatedTaskId = new SelectList(tasks, "Id", "Task.Name", serviceRequestTemplateTaskRelated.RelatedTaskId);
            ViewBag.ServiceRequestTemplateTaskId = new SelectList(tasks, "Id", "Task.Name", serviceRequestTemplateTaskRelated.ServiceRequestTemplateTaskId);
            return View(serviceRequestTemplateTaskRelated);
        }

        // GET: ServiceRequestTemplateTaskRelated/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceRequestTemplateTaskRelated serviceRequestTemplateTaskRelated = await db.ServiceRequestTemplateTaskRelateds.FindAsync(id);
            if (serviceRequestTemplateTaskRelated == null)
            {
                return HttpNotFound();
            }

            var serviceRequestTemplateId = db.ServiceRequestTemplateTasks.Find(serviceRequestTemplateTaskRelated.Id).ServiceRequestTemplateId;
            var tasks = db.ServiceRequestTemplateTasks.Where(t => t.ServiceRequestTemplateId == serviceRequestTemplateId);
            ViewBag.RelatedTaskId = new SelectList(tasks, "Id", "Task.Name", serviceRequestTemplateTaskRelated.RelatedTaskId);
            ViewBag.ServiceRequestTemplateTaskId = new SelectList(tasks, "Id", "Task.Name", serviceRequestTemplateTaskRelated.ServiceRequestTemplateTaskId);
            return View(serviceRequestTemplateTaskRelated);
        }

        // POST: ServiceRequestTemplateTaskRelated/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ServiceRequestTemplateTaskId,RelatedTaskId,Relationship")] ServiceRequestTemplateTaskRelated serviceRequestTemplateTaskRelated)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviceRequestTemplateTaskRelated).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { ServiceRequestTemplateTaskId = serviceRequestTemplateTaskRelated.ServiceRequestTemplateTaskId });
            }
            var serviceRequestTemplateId = db.ServiceRequestTemplateTasks.Find(serviceRequestTemplateTaskRelated.Id).ServiceRequestTemplateId;
            var tasks = db.ServiceRequestTemplateTasks.Where(t => t.ServiceRequestTemplateId == serviceRequestTemplateId);
            ViewBag.RelatedTaskId = new SelectList(tasks, "Id", "Task.Name", serviceRequestTemplateTaskRelated.RelatedTaskId);
            ViewBag.ServiceRequestTemplateTaskId = new SelectList(tasks, "Id", "Task.Name", serviceRequestTemplateTaskRelated.ServiceRequestTemplateTaskId);
            return View(serviceRequestTemplateTaskRelated);
        }

        // GET: ServiceRequestTemplateTaskRelated/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceRequestTemplateTaskRelated serviceRequestTemplateTaskRelated = await db.ServiceRequestTemplateTaskRelateds.FindAsync(id);
            if (serviceRequestTemplateTaskRelated == null)
            {
                return HttpNotFound();
            }
            return View(serviceRequestTemplateTaskRelated);
        }

        // POST: ServiceRequestTemplateTaskRelated/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ServiceRequestTemplateTaskRelated serviceRequestTemplateTaskRelated = await db.ServiceRequestTemplateTaskRelateds.FindAsync(id);
            db.ServiceRequestTemplateTaskRelateds.Remove(serviceRequestTemplateTaskRelated);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", new { ServiceRequestTemplateTaskId = serviceRequestTemplateTaskRelated.ServiceRequestTemplateTaskId });
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
