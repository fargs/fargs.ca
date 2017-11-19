using Orvosi.Data;
using Orvosi.Data.Filters;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.Library.Filters;
using Features = Orvosi.Shared.Enums.Features;

namespace WebApp.Controllers
{
    [AuthorizeRole(Feature = Features.Admin.ManageProcessTemplates)]
    public class ServiceRequestTemplateTaskController : BaseController
    {
        private OrvosiDbContext db;

        public ServiceRequestTemplateTaskController(OrvosiDbContext db, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
        }
        // GET: ServiceRequestTemplateTask
        public ViewResult Index(short ServiceRequestTemplateId)
        {
            var serviceRequestTemplate = db.ServiceRequestTemplates
                .Where(t => t.Id == ServiceRequestTemplateId)
                .Single();
            return View(serviceRequestTemplate);
        }

        // GET: ServiceRequestTemplateTask/Details/5
        public async Task<ViewResult> Details(Guid? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }
            ServiceRequestTemplateTask serviceRequestTemplateTask = await db.ServiceRequestTemplateTasks.FindAsync(id);
            if (serviceRequestTemplateTask == null)
            {
                return View("NotFound");
            }
            return View(serviceRequestTemplateTask);
        }

        // GET: ServiceRequestTemplateTask/Create
        public ViewResult Create(short ServiceRequestTemplateId)
        {
            var serviceRequestTemplate = db.ServiceRequestTemplates.Find(ServiceRequestTemplateId);
            ViewBag.TaskSelectList = db.ServiceRequestTemplateTasks.Include(t => t.OTask).AreNotDeleted().Where(t => t.ServiceRequestTemplateId == serviceRequestTemplate.Id).OrderBy(t => t.Sequence);
            ViewBag.ServiceRequestTemplateId = new SelectList(db.ServiceRequestTemplates, "Id", "Name", ServiceRequestTemplateId);
            ViewBag.TaskId = new SelectList(db.OTasks.Include(t => t.TaskPhase).OrderBy(t => t.TaskPhase.Sequence).ThenBy(t => t.Sequence), "Id", "Name", "TaskPhase.Name", new { });
            return View();
        }

        // POST: ServiceRequestTemplateTask/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Sequence,ServiceRequestTemplateId,TaskId,IsBaselineDate,DueDateDurationFromBaseline,EffectiveDateDurationFromBaseline,DueDateType,IsCriticalPath")] ServiceRequestTemplateTask serviceRequestTemplateTask)
        {
            serviceRequestTemplateTask.ModifiedDate = now;
            serviceRequestTemplateTask.ModifiedUser = loggedInUserId.ToString();
            if (ModelState.IsValid)
            {
                var children = (Request.Form.GetValues("Child") ?? new string[0]);
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

            ViewBag.TaskSelectList = db.ServiceRequestTemplateTasks.Include(t => t.OTask).AreNotDeleted().Where(t => t.ServiceRequestTemplateId == serviceRequestTemplateTask.ServiceRequestTemplateId).OrderBy(t => t.Sequence);
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
            ViewBag.TaskSelectList = db.ServiceRequestTemplateTasks.Include(t => t.OTask).AreNotDeleted().Where(t => t.ServiceRequestTemplateId == serviceRequestTemplateTask.ServiceRequestTemplateId).OrderBy(t => t.Sequence);
            ViewBag.ResponsibleRoleId = new SelectList(db.AspNetRoles, "Id", "Name", serviceRequestTemplateTask.ResponsibleRoleId);
            ViewBag.ServiceRequestTemplateId = new SelectList(db.ServiceRequestTemplates, "Id", "Name", serviceRequestTemplateTask.ServiceRequestTemplateId);
            ViewBag.TaskId = new SelectList(db.OTasks.Include(t => t.TaskPhase).OrderBy(t => t.TaskPhase.Sequence).ThenBy(t => t.Sequence), "Id", "Name", "TaskPhase.Name", serviceRequestTemplateTask.TaskId);
            return View(serviceRequestTemplateTask);
        }

        // POST: ServiceRequestTemplateTask/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Sequence,ServiceRequestTemplateId,TaskId,DueDateType,IsBaselineDate,DueDateDurationFromBaseline,EffectiveDateDurationFromBaseline,ResponsibleRoleId,IsCriticalPath")] ServiceRequestTemplateTask serviceRequestTemplateTask)
        {
            serviceRequestTemplateTask.ModifiedDate = now;
            serviceRequestTemplateTask.ModifiedUser = loggedInUserId.ToString();

            var c = Request.Form.GetValues("Child") == null ? new string[0] : Request.Form.GetValues("Child");
            foreach (var id in c)
            {
                var child = db.ServiceRequestTemplateTasks.Find(new Guid(id));
                serviceRequestTemplateTask.Child.Add(child);
            }

            if (ModelState.IsValid)
            {
                var model = db.ServiceRequestTemplateTasks.Single(srt => srt.Id == serviceRequestTemplateTask.Id);
                model.TaskId = serviceRequestTemplateTask.TaskId;
                model.Sequence = serviceRequestTemplateTask.Sequence;
                model.IsBaselineDate = serviceRequestTemplateTask.IsBaselineDate;
                model.DueDateDurationFromBaseline = serviceRequestTemplateTask.DueDateDurationFromBaseline;
                model.EffectiveDateDurationFromBaseline = serviceRequestTemplateTask.EffectiveDateDurationFromBaseline;
                model.DueDateType = serviceRequestTemplateTask.DueDateType;
                model.ResponsibleRoleId = serviceRequestTemplateTask.ResponsibleRoleId;
                model.IsCriticalPath = serviceRequestTemplateTask.IsCriticalPath;
                model.ModifiedDate = now;
                model.ModifiedUser = loggedInUserId.ToString();
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
            ViewBag.TaskSelectList = db.ServiceRequestTemplateTasks.Include(t => t.OTask).AreNotDeleted().Where(t => t.ServiceRequestTemplateId == serviceRequestTemplateTask.ServiceRequestTemplateId).OrderBy(t => t.Sequence);
            ViewBag.ResponsibleRoleId = new SelectList(db.AspNetRoles, "Id", "Name", serviceRequestTemplateTask.ResponsibleRoleId);
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
        public async Task<RedirectToRouteResult> DeleteConfirmed(Guid id)
        {
            ServiceRequestTemplateTask serviceRequestTemplateTask = await db.ServiceRequestTemplateTasks.FindAsync(id);
            serviceRequestTemplateTask.IsDeleted = true;

            await db.SaveChangesAsync();
            return RedirectToAction("Index", new { ServiceRequestTemplateId = serviceRequestTemplateTask.ServiceRequestTemplateId });
        }
    }
}
