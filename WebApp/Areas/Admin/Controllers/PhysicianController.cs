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
    [Authorize(Roles="Super Admin")]
    public class PhysicianController : Controller
    {
        private OrvosiDbContext db = new OrvosiDbContext();

        
        // GET: Admin/Physician/Edit/5
        public async Task<ActionResult> Edit(Guid? userId)
        {
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Physician physician = await db.Physicians.FindAsync(userId);
            if (physician == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", physician.Id);
            ViewBag.SpecialtyId = new SelectList(db.PhysicianSpecialities, "Id", "Name", physician.SpecialtyId);
            return View(physician);
        }

        // POST: Admin/Physician/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Designations,SpecialtyId,OtherSpecialties,Pediatrics,Adolescents,Adults,Geriatrics,PrimaryAddressId,ModifiedDate,ModifiedUser,BoxCaseTemplateFolderId,BoxAddOnTemplateFolderId")] Physician physician)
        {
            if (ModelState.IsValid)
            {
                db.Entry(physician).State = EntityState.Modified;
                await db.SaveChangesAsync();

                var user = db.AspNetUsers.Find(physician.Id);
                var roleId = user.GetRoleId();
                var roleCategoryId = db.AspNetRoles.FirstOrDefault(r => r.Id == roleId).RoleCategoryId;
                return RedirectToAction("Index", "User", new { parentId = roleCategoryId });
            }
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", physician.Id);
            ViewBag.SpecialtyId = new SelectList(db.PhysicianSpecialities, "Id", "Name", physician.SpecialtyId);

            return View(physician);
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
