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

namespace WebApp.Areas.Admin.Controllers
{
    [AuthorizeRole(Feature = Features.SecurityAdmin.RoleManagement)]
    public class AspNetRolesFeatureController : Controller
    {
        private OrvosiDbContext db = new OrvosiDbContext();

        // GET: Admin/AspNetRolesFeature
        public async Task<ActionResult> Index()
        {
            var aspNetRolesFeatures = db.AspNetRolesFeatures.Include(a => a.AspNetRole).Include(a => a.Feature);
            return View(await aspNetRolesFeatures.ToListAsync());
        }

        // GET: Admin/AspNetRolesFeature/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRolesFeature aspNetRolesFeature = await db.AspNetRolesFeatures.FindAsync(id);
            if (aspNetRolesFeature == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRolesFeature);
        }

        // GET: Admin/AspNetRolesFeature/Create
        public ActionResult Create()
        {
            ViewBag.AspNetRolesId = new SelectList(db.AspNetRoles, "Id", "Name");
            ViewBag.FeatureId = new SelectList(db.Features, "Id", "Name");
            return View();
        }

        // POST: Admin/AspNetRolesFeature/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,AspNetRolesId,FeatureId,IsActive")] AspNetRolesFeature aspNetRolesFeature)
        {
            if (ModelState.IsValid)
            {
                aspNetRolesFeature.ModifiedBy = User.Identity.GetGuidUserId().ToString();
                db.AspNetRolesFeatures.Add(aspNetRolesFeature);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AspNetRolesId = new SelectList(db.AspNetRoles, "Id", "Name", aspNetRolesFeature.AspNetRolesId);
            ViewBag.FeatureId = new SelectList(db.Features, "Id", "Name", aspNetRolesFeature.FeatureId);
            return View(aspNetRolesFeature);
        }

        // GET: Admin/AspNetRolesFeature/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRolesFeature aspNetRolesFeature = await db.AspNetRolesFeatures.FindAsync(id);
            if (aspNetRolesFeature == null)
            {
                return HttpNotFound();
            }
            ViewBag.AspNetRolesId = new SelectList(db.AspNetRoles, "Id", "Name", aspNetRolesFeature.AspNetRolesId);
            ViewBag.FeatureId = new SelectList(db.Features, "Id", "Name", aspNetRolesFeature.FeatureId);
            return View(aspNetRolesFeature);
        }

        // POST: Admin/AspNetRolesFeature/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,AspNetRolesId,FeatureId,IsActive")] AspNetRolesFeature aspNetRolesFeature)
        {
            if (ModelState.IsValid)
            {
                aspNetRolesFeature.ModifiedBy = User.Identity.GetGuidUserId().ToString();
                db.Entry(aspNetRolesFeature).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AspNetRolesId = new SelectList(db.AspNetRoles, "Id", "Name", aspNetRolesFeature.AspNetRolesId);
            ViewBag.FeatureId = new SelectList(db.Features, "Id", "Name", aspNetRolesFeature.FeatureId);
            return View(aspNetRolesFeature);
        }

        // GET: Admin/AspNetRolesFeature/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRolesFeature aspNetRolesFeature = await db.AspNetRolesFeatures.FindAsync(id);
            if (aspNetRolesFeature == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRolesFeature);
        }

        // POST: Admin/AspNetRolesFeature/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            AspNetRolesFeature aspNetRolesFeature = await db.AspNetRolesFeatures.FindAsync(id);
            db.AspNetRolesFeatures.Remove(aspNetRolesFeature);
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
