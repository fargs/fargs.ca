using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using threading = System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Orvosi.Data;

namespace WebApp.Controllers
{
    public class TaskController : Controller
    {
        private OrvosiDbContext db = new OrvosiDbContext();

        // GET: Task
        public async threading.Task<ActionResult> Index()
        {
            var tasks = 
                db.OTasks
                    .Include(t => t.AspNetRole)
                    .Include(t => t.TaskPhase)
                    .OrderBy(t => t.TaskPhase.Sequence)
                        .ThenBy(t => t.Sequence);
            return View(await tasks.ToListAsync());
        }

        // GET: Task/Details/5
        public async threading.Task<ActionResult> Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OTask task = await db.OTasks.FindAsync(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // GET: Task/Create
        public ActionResult Create()
        {
            ViewBag.ResponsibleRoleId = new SelectList(db.AspNetRoles, "Id", "Name");
            ViewBag.TaskPhaseId = new SelectList(db.TaskPhases, "Id", "Name");
            return View();
        }

        // POST: Task/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async threading.Task<ActionResult> Create([Bind(Include = "Id,ObjectGuid,ServiceCategoryId,ServiceId,Name,Guidance,TaskPhaseId,ResponsibleRoleId,IsBillable,HourlyRate,EstimatedHours,Sequence,IsMilestone,ModifiedDate,ModifiedUser,DependsOn,DueDateBase,DueDateDiff,ShortName,IsCriticalPath")] OTask task)
        {
            if (ModelState.IsValid)
            {
                db.OTasks.Add(task);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ResponsibleRoleId = new SelectList(db.AspNetRoles, "Id", "Name", task.ResponsibleRoleId);
            ViewBag.TaskPhaseId = new SelectList(db.TaskPhases, "Id", "Name", task.TaskPhaseId);
            return View(task);
        }

        // GET: Task/Edit/5
        public async threading.Task<ActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OTask task = await db.OTasks.FindAsync(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            ViewBag.ResponsibleRoleId = new SelectList(db.AspNetRoles, "Id", "Name", task.ResponsibleRoleId);
            ViewBag.TaskPhaseId = new SelectList(db.TaskPhases, "Id", "Name", task.TaskPhaseId);
            return View(task);
        }

        // POST: Task/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async threading.Task<ActionResult> Edit([Bind(Include = "Id,ObjectGuid,ServiceCategoryId,ServiceId,Name,Guidance,TaskPhaseId,ResponsibleRoleId,IsBillable,HourlyRate,EstimatedHours,Sequence,IsMilestone,ModifiedDate,ModifiedUser,DependsOn,DueDateBase,DueDateDiff,ShortName,IsCriticalPath")] OTask task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ResponsibleRoleId = new SelectList(db.AspNetRoles, "Id", "Name", task.ResponsibleRoleId);
            ViewBag.TaskPhaseId = new SelectList(db.TaskPhases, "Id", "Name", task.TaskPhaseId);
            return View(task);
        }

        // GET: Task/Delete/5
        public async threading.Task<ActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var task = await db.OTasks.FindAsync(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Task/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async threading.Task<ActionResult> DeleteConfirmed(short id)
        {
            var task = await db.OTasks.FindAsync(id);
            db.OTasks.Remove(task);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
