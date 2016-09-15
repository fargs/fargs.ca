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

namespace WebApp.Areas.Admin.Controllers
{
    public class AspNetRolesController : Controller
    {
        private OrvosiDbContext db = new OrvosiDbContext();

        // GET: Admin/AspNetRoles
        public async Task<ActionResult> Index()
        {
            var aspNetRoles = db.AspNetRoles.Include(a => a.RoleCategory);
            return View(await aspNetRoles.ToListAsync());
        }

        // GET: Admin/AspNetRoles/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRole aspNetRole = await db.AspNetRoles.FindAsync(id);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRole);
        }

        // GET: Admin/AspNetRoles/Create
        public ActionResult Create()
        {
            ViewBag.RoleCategoryId = new SelectList(db.RoleCategories, "Id", "Name");
            return View();
        }

        // POST: Admin/AspNetRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,RoleCategoryId,ModifiedDate,ModifiedUser")] AspNetRole aspNetRole)
        {
            if (ModelState.IsValid)
            {
                aspNetRole.Id = Guid.NewGuid();
                db.AspNetRoles.Add(aspNetRole);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.RoleCategoryId = new SelectList(db.RoleCategories, "Id", "Name", aspNetRole.RoleCategoryId);
            return View(aspNetRole);
        }

        // GET: Admin/AspNetRoles/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRole aspNetRole = await db.AspNetRoles.FindAsync(id);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleCategoryId = new SelectList(db.RoleCategories, "Id", "Name", aspNetRole.RoleCategoryId);
            return View(aspNetRole);
        }

        // POST: Admin/AspNetRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,RoleCategoryId,ModifiedDate,ModifiedUser")] AspNetRole aspNetRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetRole).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.RoleCategoryId = new SelectList(db.RoleCategories, "Id", "Name", aspNetRole.RoleCategoryId);
            return View(aspNetRole);
        }

        // GET: Admin/AspNetRoles/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRole aspNetRole = await db.AspNetRoles.FindAsync(id);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRole);
        }

        // POST: Admin/AspNetRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            AspNetRole aspNetRole = await db.AspNetRoles.FindAsync(id);
            db.AspNetRoles.Remove(aspNetRole);
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
