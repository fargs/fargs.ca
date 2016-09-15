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
    public class AspNetUsersController : Controller
    {
        private OrvosiDbContext db = new OrvosiDbContext();

        // GET: Admin/AspNetUsers
        public async Task<ActionResult> Index()
        {
            var aspNetUsers = db.AspNetUsers.Include(a => a.Company).Include(a => a.Physician).OrderBy(a => a.LastName).ThenBy(a => a.FirstName);
            return View(await aspNetUsers.ToListAsync());
        }

        // GET: Admin/AspNetUsers/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // GET: Admin/AspNetUsers/Create
        public ActionResult Create()
        {
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name");
            ViewBag.Id = new SelectList(db.Physicians, "Id", "Designations");
            return View();
        }

        // POST: Admin/AspNetUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Title,FirstName,LastName,EmployeeId,CompanyId,CompanyName,ModifiedDate,ModifiedUser,LastActivationDate,IsTestRecord,RoleLevelId,HourlyRate,LogoCssClass,ColorCode,BoxFolderId,BoxUserId,BoxAccessToken,BoxRefreshToken")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                aspNetUser.Id = Guid.NewGuid();
                db.AspNetUsers.Add(aspNetUser);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", aspNetUser.CompanyId);
            ViewBag.Id = new SelectList(db.Physicians, "Id", "Designations", aspNetUser.Id);
            return View(aspNetUser);
        }

        // GET: Admin/AspNetUsers/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", aspNetUser.CompanyId);
            ViewBag.Id = new SelectList(db.Physicians, "Id", "Designations", aspNetUser.Id);
            ViewBag.RoleSelectList = db.AspNetRoles.OrderBy(t => t.Name);
            return View(aspNetUser);
        }

        // POST: Admin/AspNetUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Title,FirstName,LastName,EmployeeId,CompanyId,CompanyName,ModifiedDate,ModifiedUser,LastActivationDate,IsTestRecord,RoleLevelId,HourlyRate,LogoCssClass,ColorCode,BoxFolderId,BoxUserId,BoxAccessToken,BoxRefreshToken")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetUser).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", aspNetUser.CompanyId);
            ViewBag.Id = new SelectList(db.Physicians, "Id", "Designations", aspNetUser.Id);
            return View(aspNetUser);
        }

        // GET: Admin/AspNetUsers/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: Admin/AspNetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            db.AspNetUsers.Remove(aspNetUser);
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
