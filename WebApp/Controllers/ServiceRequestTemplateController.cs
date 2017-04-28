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
using WebApp.Library.Extensions;
using WebApp.Library.Filters;
using Features = Orvosi.Shared.Enums.Features;
using Orvosi.Data.Filters;

namespace WebApp.Controllers
{
    [AuthorizeRole(Feature = Features.Admin.ManageProcessTemplates)]
    public class ServiceRequestTemplateController : Controller
    {
        private OrvosiDbContext db = new OrvosiDbContext();

        // GET: ServiceRequestTemplates
        public async Task<ActionResult> Index()
        {
            return View(await db.ServiceRequestTemplates.ToListAsync());
        }

        // GET: ServiceRequestTemplates/Details/5
        public async Task<ActionResult> Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceRequestTemplate serviceRequestTemplate = await db.ServiceRequestTemplates.FindAsync(id);
            if (serviceRequestTemplate == null)
            {
                return HttpNotFound();
            }
            return View(serviceRequestTemplate);
        }

        // GET: ServiceRequestTemplates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServiceRequestTemplates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(short processTemplateId, ServiceRequestTemplate serviceRequestTemplate)
        {
            serviceRequestTemplate.ModifiedDate = SystemTime.UtcNow();
            serviceRequestTemplate.ModifiedUser = User.Identity.GetGuidUserId().ToString();
            if (ModelState.IsValid)
            {
                db.ServiceRequestTemplates.Add(serviceRequestTemplate);
                await db.SaveChangesAsync();
                await db.Entry(serviceRequestTemplate).ReloadAsync();

                // get the task from the default template
                var tasks = db.ServiceRequestTemplateTasks.AreNotDeleted().Where(t => t.ServiceRequestTemplateId == processTemplateId);
                foreach (var template in tasks)
                {
                    var st = new Orvosi.Data.ServiceRequestTemplateTask();
                    st.Id = Guid.NewGuid();
                    st.ResponsibleRoleId = template.ResponsibleRoleId;
                    st.Sequence = template.Sequence;
                    st.TaskId = template.OTask.Id;
                    st.DueDateType = template.DueDateType;
                    st.ModifiedDate = SystemTime.Now();
                    st.ModifiedUser = User.Identity.Name;

                    serviceRequestTemplate.ServiceRequestTemplateTasks.Add(st);
                }
                
                await db.SaveChangesAsync();

                // Clone the related tasks
                foreach (var taskTemplate in tasks.AreNotDeleted())
                {
                    foreach (var dependentTemplate in taskTemplate.Child.AsQueryable().AreNotDeleted())
                    {
                        var task = serviceRequestTemplate.ServiceRequestTemplateTasks.First(srt => srt.TaskId == taskTemplate.TaskId);
                        var dependent = serviceRequestTemplate.ServiceRequestTemplateTasks.First(srt => srt.TaskId == dependentTemplate.TaskId);
                        task.Child.Add(dependent);
                    }
                }

                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(serviceRequestTemplate);
        }

        // GET: ServiceRequestTemplates/Edit/5
        public async Task<ActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceRequestTemplate serviceRequestTemplate = await db.ServiceRequestTemplates.FindAsync(id);
            if (serviceRequestTemplate == null)
            {
                return HttpNotFound();
            }
            return View(serviceRequestTemplate);
        }

        // POST: ServiceRequestTemplates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,PhysicianId")] ServiceRequestTemplate serviceRequestTemplate)
        {
            serviceRequestTemplate.ModifiedDate = SystemTime.UtcNow();
            serviceRequestTemplate.ModifiedUser = User.Identity.GetGuidUserId().ToString();
            if (ModelState.IsValid)
            {
                db.Entry(serviceRequestTemplate).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(serviceRequestTemplate);
        }

        // GET: ServiceRequestTemplates/Delete/5
        public async Task<ActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceRequestTemplate serviceRequestTemplate = await db.ServiceRequestTemplates.FindAsync(id);
            if (serviceRequestTemplate == null)
            {
                return HttpNotFound();
            }
            return View(serviceRequestTemplate);
        }

        // POST: ServiceRequestTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(short id)
        {
            ServiceRequestTemplate serviceRequestTemplate = await db.ServiceRequestTemplates.FindAsync(id);
            db.ServiceRequestTemplates.Remove(serviceRequestTemplate);
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
