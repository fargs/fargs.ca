using Microsoft.AspNet.Identity;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Admin.ViewModels.PhysicianInsuranceViewModels;
using System.IO;
using System.Net;
using System.Data.Entity;

namespace WebApp.Areas.Admin.Controllers
{
    public class PhysicianInsuranceController : Controller
    {
        OrvosiEntities db = new OrvosiEntities();

        // GET: Admin/PhysicianLicence
        public ActionResult Index(string userId)
        {
            var user = db.Users.Single(u => u.Id == userId);
            if (user == null)
            {
                throw new Exception("User does not exist");
            }

            // drop down lists
            var insurances = db.PhysicianInsurances.Where(p => p.PhysicianId == userId).ToList();

            var vm = new IndexViewModel()
            {
                User = user,
                Insurance = insurances
            };

            return View(vm);
        }

        public ActionResult Create(string id)
        {
            var user = db.Users.Single(u => u.Id == id);
            if (user == null)
            {
                throw new Exception("User does not exist");
            }

            ViewBag.User = user;

            return View();
        }

        // POST: Admin/Location/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="PhysicianId, Insurer, PolicyNumber, ExpiryDate")]PhysicianInsurance insurance, HttpPostedFileBase upload)
        {
            var user = db.Users.Single(u => u.Id == insurance.PhysicianId);
            if (user == null)
            {
                throw new Exception("User does not exist");
            }

            ViewBag.User = user;

            if (ModelState.IsValid)
            {
                // handle the uploaded document
                if (upload != null && upload.ContentLength > 0)
                {
                    var doc = new Document
                    {
                        OwnedByObjectGuid = new Guid(user.Id),
                        ModifiedUser = User.Identity.GetUserId(),
                        Name = System.IO.Path.GetFileName(upload.FileName),
                        ContentType = upload.ContentType,
                        DocumentTemplateId = 2
                        //ContentType = upload.ContentType
                    };
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        doc.Content = reader.ReadBytes(upload.ContentLength);
                    }
                    db.Documents.Add(doc);
                    await db.SaveChangesAsync();
                    await db.Entry(doc).ReloadAsync();
                    insurance.DocumentId = doc.Id;
                }
                
                insurance.ModifiedUser = User.Identity.GetUserId();
                db.PhysicianInsurances.Add(insurance);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { userId = user.Id } );
            }

            return View(insurance);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var insurance = db.PhysicianInsurances.SingleOrDefault(c => c.Id == id);
            if (insurance == null)
            {
                return HttpNotFound();
            }

            var user = db.Users.Single(u => u.Id == insurance.PhysicianId);
            if (user == null)
            {
                throw new Exception("User does not exist");
            }

            ViewBag.User = user;

            return View(insurance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(short? id, HttpPostedFileBase upload)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var insuranceToUpdate = await db.PhysicianInsurances.FindAsync(id);
            if (TryUpdateModel(insuranceToUpdate, "",
                new string[] { "Insurer", "PolicyNumber", "ExpiryDate" }))
            {
                // handle the uploaded document
                if (upload != null && upload.ContentLength > 0)
                {
                    if (insuranceToUpdate.DocumentId != null)
                    {
                        var docToUpdate = await db.Documents.FindAsync(insuranceToUpdate.DocumentId);
                        db.Documents.Remove(docToUpdate);
                        insuranceToUpdate.DocumentId = null;
                    }
                    var doc = new Document
                    {
                        OwnedByObjectGuid = new Guid(insuranceToUpdate.PhysicianId),
                        ModifiedUser = User.Identity.GetUserId(),
                        Name = System.IO.Path.GetFileName(upload.FileName),
                        ContentType = upload.ContentType,
                        DocumentTemplateId = 2
                        //ContentType = upload.ContentType
                    };
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        doc.Content = reader.ReadBytes(upload.ContentLength);
                    }
                    db.Documents.Add(doc);
                    await db.SaveChangesAsync();
                    await db.Entry(doc).ReloadAsync();
                    insuranceToUpdate.DocumentId = doc.Id;
                }

                insuranceToUpdate.ModifiedUser = User.Identity.GetUserId();
                db.Entry(insuranceToUpdate).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { userId = insuranceToUpdate.PhysicianId });
            }

            return View(insuranceToUpdate);
        }

        // GET: Admin/Document/Delete/5
        public async Task<ActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var insurance = await db.PhysicianInsurances.FindAsync(id);
            if (insurance == null)
            {
                return HttpNotFound();
            }

            var user = db.Users.Single(u => u.Id == insurance.PhysicianId);
            if (user == null)
            {
                throw new Exception("User does not exist");
            }

            ViewBag.User = user;

            return View(insurance);
        }

        // POST: Admin/Document/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(short id)
        {
            var insurance = await db.PhysicianInsurances.FindAsync(id);
            var document = await db.Documents.FindAsync(insurance.DocumentId);
            db.PhysicianInsurances.Remove(insurance);
            if (document != null)
            {
                db.Documents.Remove(document);
            }
            await db.SaveChangesAsync();
            return RedirectToAction("Index", new { userId = insurance.PhysicianId });
        }
    }
}