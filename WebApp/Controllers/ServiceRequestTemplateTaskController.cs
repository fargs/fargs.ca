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
                .Include(s => s.OTask)
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
            var serviceRequestTemplate = db.ServiceRequestTemplates.Find(ServiceRequestTemplateId);
            ViewBag.TaskSelectList = db.ServiceRequestTemplateTasks.Include(t => t.OTask).Where(t => t.ServiceRequestTemplateId == serviceRequestTemplate.Id).OrderBy(t => t.Sequence);
            ViewBag.ServiceRequestTemplateId = new SelectList(db.ServiceRequestTemplates, "Id", "Name", ServiceRequestTemplateId);
            ViewBag.TaskId = new SelectList(db.OTasks.Include(t => t.TaskPhase).OrderBy(t => t.TaskPhase.Sequence).ThenBy(t => t.Sequence), "Id", "Name", "TaskPhase.Name", new { });
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
                var children = Request.Form.GetValues("Child");
                foreach (var id in children)
                {
                    var child = db.ServiceRequestTemplateTasks.Find(new Guid(id));
                    serviceRequestTemplateTask.Child.Add(child);
                }
                serviceRequestTemplateTask.Id = Guid.NewGuid();
                db.ServiceRequestTemplateTasks.Add(serviceRequestTemplateTask);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { ServiceRequestTemplateId = serviceRequestTemplateTask.ServiceRequestTemplateId });
            }

            ViewBag.TaskSelectList = db.ServiceRequestTemplateTasks.Include(t => t.OTask).Where(t => t.ServiceRequestTemplateId == serviceRequestTemplateTask.ServiceRequestTemplateId).OrderBy(t => t.Sequence);
            ViewBag.ServiceRequestTemplateId = new SelectList(db.ServiceRequestTemplates, "Id", "Name", serviceRequestTemplateTask.ServiceRequestTemplateId);
            ViewBag.TaskId = new SelectList(db.OTasks.Include(t => t.TaskPhase).OrderBy(t => t.TaskPhase.Sequence).ThenBy(t => t.Sequence), "Id", "Name", serviceRequestTemplateTask.TaskId);
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
            ViewBag.TaskSelectList = db.ServiceRequestTemplateTasks.Include(t => t.OTask).Where(t => t.ServiceRequestTemplateId == serviceRequestTemplateTask.ServiceRequestTemplateId).OrderBy(t => t.Sequence);
            ViewBag.ServiceRequestTemplateId = new SelectList(db.ServiceRequestTemplates, "Id", "Name", serviceRequestTemplateTask.ServiceRequestTemplateId);
            ViewBag.TaskId = new SelectList(db.OTasks.Include(t => t.TaskPhase).OrderBy(t => t.TaskPhase.Sequence).ThenBy(t => t.Sequence), "Id", "Name", "TaskPhase.Name", serviceRequestTemplateTask.TaskId);
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
                var model = db.ServiceRequestTemplateTasks.Find(serviceRequestTemplateTask.Id);
                model.TaskId = serviceRequestTemplateTask.TaskId;
                model.Sequence = serviceRequestTemplateTask.Sequence;
                model.ModifiedDate = SystemTime.Now();
                model.ModifiedUser = User.Identity.Name;
                model.Child.Clear();
                var children = Request.Form.GetValues("Child") == null ? new string[0] : Request.Form.GetValues("Child");
                foreach (var id in children)
                {
                    var child = db.ServiceRequestTemplateTasks.Find(new Guid(id));
                    model.Child.Add(child);
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { ServiceRequestTemplateId = serviceRequestTemplateTask.ServiceRequestTemplateId });
            }
            ViewBag.TaskSelectList = db.ServiceRequestTemplateTasks.Include(t => t.OTask).Where(t => t.ServiceRequestTemplateId == serviceRequestTemplateTask.ServiceRequestTemplateId).OrderBy(t => t.Sequence);
            ViewBag.ServiceRequestTemplateId = new SelectList(db.ServiceRequestTemplates, "Id", "Name", serviceRequestTemplateTask.ServiceRequestTemplateId);
            ViewBag.TaskId = new SelectList(db.OTasks.Include(t => t.TaskPhase).OrderBy(t => t.TaskPhase.Sequence).ThenBy(t => t.Sequence), "Id", "Name", "TaskPhase.Name", serviceRequestTemplateTask.TaskId);
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
