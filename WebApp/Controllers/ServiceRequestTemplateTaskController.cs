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
    public class ServiceRequestTemplateTaskController : Controller
    {
        private OrvosiDbContext db = new OrvosiDbContext();

        // GET: ServiceRequestTemplateTask
        public async Task<ActionResult> Index(short ServiceRequestTemplateId)
        {
            var serviceRequestTemplateTasks = db.ServiceRequestTemplateTasks
                .Include(s => s.ServiceRequestTemplate)
                .Include(s => s.Task)
                .Where(t => t.ServiceRequestTemplateId == ServiceRequestTemplateId)
                .OrderBy(t => t.Sequence);
            return View(await serviceRequestTemplateTasks.ToListAsync());
        }

        // GET: ServiceRequestTemplateTask/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceRequestTemplateTask serviceRequestTemplateTask = await db.ServiceRequestTemplateTasks.FindAsync(id);
            if (serviceRequestTemplateTask == null)
            {
                return HttpNotFound();
            }
            return View(serviceRequestTemplateTask);
        }

        // GET: ServiceRequestTemplateTask/Create
        public ActionResult Create(short ServiceRequestTemplateId)
        {
            ViewBag.ServiceRequestTemplateId = new SelectList(db.ServiceRequestTemplates, "Id", "Name", ServiceRequestTemplateId);
            ViewBag.TaskId = new SelectList(db.Tasks.Include(t => t.TaskPhase).OrderBy(t => t.TaskPhase.Sequence).ThenBy(t => t.Sequence), "Id", "Name", "TaskPhase.Name", new { });
            return View();
        }

        // POST: ServiceRequestTemplateTask/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Sequence,ServiceRequestTemplateId,TaskId,ModifiedDate,ModifiedUser")] ServiceRequestTemplateTask serviceRequestTemplateTask)
        {
            if (ModelState.IsValid)
            {
                serviceRequestTemplateTask.Id = Guid.NewGuid();
                db.ServiceRequestTemplateTasks.Add(serviceRequestTemplateTask);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { ServiceRequestTemplateId = serviceRequestTemplateTask.ServiceRequestTemplateId });
            }

            ViewBag.ServiceRequestTemplateId = new SelectList(db.ServiceRequestTemplates, "Id", "Name", serviceRequestTemplateTask.ServiceRequestTemplateId);
            ViewBag.TaskId = new SelectList(db.Tasks.Include(t => t.TaskPhase).OrderBy(t => t.TaskPhase.Sequence).ThenBy(t => t.Sequence), "Id", "Name", serviceRequestTemplateTask.TaskId);
            return View(serviceRequestTemplateTask);
        }

        // GET: ServiceRequestTemplateTask/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceRequestTemplateTask serviceRequestTemplateTask = await db.ServiceRequestTemplateTasks.FindAsync(id);
            if (serviceRequestTemplateTask == null)
            {
                return HttpNotFound();
            }
            ViewBag.ServiceRequestTemplateId = new SelectList(db.ServiceRequestTemplates, "Id", "Name", serviceRequestTemplateTask.ServiceRequestTemplateId);
            ViewBag.TaskId = new SelectList(db.Tasks.Include(t => t.TaskPhase).OrderBy(t => t.TaskPhase.Sequence).ThenBy(t => t.Sequence), "Id", "Name", "TaskPhase.Name", serviceRequestTemplateTask.TaskId);
            return View(serviceRequestTemplateTask);
        }

        // POST: ServiceRequestTemplateTask/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Sequence,ServiceRequestTemplateId,TaskId,ModifiedDate,ModifiedUser")] ServiceRequestTemplateTask serviceRequestTemplateTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviceRequestTemplateTask).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { ServiceRequestTemplateId = serviceRequestTemplateTask.ServiceRequestTemplateId });
            }
            ViewBag.ServiceRequestTemplateId = new SelectList(db.ServiceRequestTemplates, "Id", "Name", serviceRequestTemplateTask.ServiceRequestTemplateId);
            ViewBag.TaskId = new SelectList(db.Tasks.Include(t => t.TaskPhase).OrderBy(t => t.TaskPhase.Sequence).ThenBy(t => t.Sequence), "Id", "Name", "TaskPhase.Name", serviceRequestTemplateTask.TaskId);
            return View(serviceRequestTemplateTask);
        }

        // GET: ServiceRequestTemplateTask/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceRequestTemplateTask serviceRequestTemplateTask = await db.ServiceRequestTemplateTasks.FindAsync(id);
            if (serviceRequestTemplateTask == null)
            {
                return HttpNotFound();
            }
            return View(serviceRequestTemplateTask);
        }

        // POST: ServiceRequestTemplateTask/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ServiceRequestTemplateTask serviceRequestTemplateTask = await db.ServiceRequestTemplateTasks.FindAsync(id);
            db.ServiceRequestTemplateTasks.Remove(serviceRequestTemplateTask);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", new { ServiceRequestTemplateId = serviceRequestTemplateTask.ServiceRequestTemplateId });
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
