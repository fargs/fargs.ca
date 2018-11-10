using Microsoft.AspNet.Identity;
using Orvosi.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Admin.ViewModels.PhysicianLicenceViewModels;
using System.Net;
using System.Data.Entity;

namespace WebApp.Areas.Admin.Controllers
{
    public class PhysicianLicenceController : Controller
    {
        OrvosiDbContext db = new OrvosiDbContext();

        // GET: Admin/PhysicianLicence
        public ActionResult Index(Guid userId)
        {
            var user = db.AspNetUsers.Single(u => u.Id == userId);
            if (user == null)
            {
                throw new Exception("User does not exist");
            }

            var licences = db.PhysicianLicenses
                    .Include(pl => pl.Document)
                .Where(p => p.PhysicianId == userId).ToList();

            var vm = new IndexViewModel()
            {
                User = user,
                Licences = licences
            };

            return View(vm);
        }

        public ActionResult Create(Guid id)
        {
            var user = db.AspNetUsers.Single(u => u.Id == id);
            if (user == null)
            {
                throw new Exception("User does not exist");
            }

            ViewBag.User = user;

            ViewBag.Provinces = db.Provinces
                .Where(c => c.CountryId == 124) // Canada
                .Select(c => new SelectListItem()
                {
                    Text = c.ProvinceName,
                    Value = c.Id.ToString()
                }).ToList();

            return View();
        }

        // POST: Admin/Location/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PhysicianId, ProvinceId, MemberName, CollegeName, CertificateClass, ExpiryDate")]PhysicianLicense licence, HttpPostedFileBase upload)
        {
            var user = db.AspNetUsers.Single(u => u.Id == licence.PhysicianId);
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
                        OwnedByObjectGuid = user.Id,
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
                    licence.DocumentId = doc.Id;
                }

                licence.ModifiedUser = User.Identity.GetUserId();
                db.PhysicianLicenses.Add(licence);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { userId = user.Id });
            }

            return View(licence);
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
            var licence = db.PhysicianLicenses.SingleOrDefault(c => c.Id == id);
            if (licence == null)
            {
                return HttpNotFound();
            }

            var user = db.AspNetUsers.Single(u => u.Id == licence.PhysicianId);
            if (user == null)
            {
                throw new Exception("User does not exist");
            }

            ViewBag.User = user;

            ViewBag.Provinces = db.Provinces
                .Where(c => c.CountryId == 124) // Canada
                .Select(c => new SelectListItem()
                {
                    Text = c.ProvinceName,
                    Value = c.Id.ToString()
                }).ToList();

            return View(licence);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(short? id, HttpPostedFileBase upload)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var licenceToUpdate = await db.PhysicianLicenses.FindAsync(id);
            if (TryUpdateModel(licenceToUpdate, "",
                new string[] { "ProvinceId", "MemberName", "CollegeName", "CertificateClass", "ExpiryDate" }))
            {
                // handle the uploaded document
                if (upload != null && upload.ContentLength > 0)
                {
                    if (licenceToUpdate.DocumentId != null)
                    {
                        var docToUpdate = await db.Documents.FindAsync(licenceToUpdate.DocumentId);
                        db.Documents.Remove(docToUpdate);
                        licenceToUpdate.DocumentId = null;
                    }
                    var doc = new Document
                    {
                        OwnedByObjectGuid = licenceToUpdate.PhysicianId,
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
                    licenceToUpdate.DocumentId = doc.Id;
                }

                licenceToUpdate.ModifiedUser = User.Identity.GetUserId();
                db.Entry(licenceToUpdate).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { userId = licenceToUpdate.PhysicianId });
            }

            return View(licenceToUpdate);
        }

        // GET: Admin/Document/Delete/5
        public async Task<ActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var licence = await db.PhysicianLicenses.FindAsync(id);
            if (licence == null)
            {
                return HttpNotFound();
            }

            var user = db.AspNetUsers.Single(u => u.Id == licence.PhysicianId);
            if (user == null)
            {
                throw new Exception("User does not exist");
            }

            ViewBag.User = user;

            return View(licence);
        }

        // POST: Admin/Document/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(short id)
        {
            var licence = await db.PhysicianLicenses.FindAsync(id);
            var document = await db.Documents.FindAsync(licence.DocumentId);
            db.PhysicianLicenses.Remove(licence);
            if (document != null)
            {
                db.Documents.Remove(document);
            }
            await db.SaveChangesAsync();
            return RedirectToAction("Index", new { userId = licence.PhysicianId });
        }
    }
}