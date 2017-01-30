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
using Features = Orvosi.Shared.Enums.Features;
using WebApp.Library.Filters;

namespace WebApp.Areas.Admin.Controllers
{
    [AuthorizeRole(Feature = Features.SecurityAdmin.FeatureManagement)]
    public class FeatureController : Controller
    {
        private OrvosiDbContext db = new OrvosiDbContext();

        // GET: Admin/Feature
        public async Task<ActionResult> Index()
        {
            return View(await db.Features.ToListAsync());
        }

        // GET: Admin/Feature/Details/5
        public async Task<ActionResult> Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feature feature = await db.Features.FindAsync(id);
            if (feature == null)
            {
                return HttpNotFound();
            }
            return View(feature);
        }

        // GET: Admin/Feature/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Feature/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Description,IsActive")] Feature feature)
        {
            if (ModelState.IsValid)
            {
                feature.ModifiedBy = User.Identity.GetGuidUserId().ToString();
                db.Features.Add(feature);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(feature);
        }

        // GET: Admin/Feature/Edit/5
        public async Task<ActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feature feature = await db.Features.FindAsync(id);
            if (feature == null)
            {
                return HttpNotFound();
            }
            return View(feature);
        }

        // POST: Admin/Feature/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Description,IsActive")] Feature feature)
        {
            if (ModelState.IsValid)
            {
                feature.ModifiedBy = User.Identity.GetGuidUserId().ToString();
                db.Entry(feature).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(feature);
        }

        // GET: Admin/Feature/Delete/5
        public async Task<ActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feature feature = await db.Features.FindAsync(id);
            if (feature == null)
            {
                return HttpNotFound();
            }
            return View(feature);
        }

        // POST: Admin/Feature/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(short id)
        {
            Feature feature = await db.Features.FindAsync(id);
            db.Features.Remove(feature);
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
