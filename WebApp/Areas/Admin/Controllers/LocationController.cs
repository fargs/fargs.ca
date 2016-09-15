using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Orvosi.Data;
using System;

namespace WebApp.Areas.Admin.Controllers
{
    public class LocationController : BaseController
    {
        private OrvosiDbContext db = new OrvosiDbContext();

        // GET: Admin/Location
        public async Task<ActionResult> Index()
        {
            return View(await db.Addresses.ToListAsync());
        }

        // GET: Admin/Location/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = await db.Addresses.FindAsync(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return View(address);
        }

        // GET: Admin/Location/Create
        public ActionResult Create() => View();

        // POST: Admin/Location/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,OwnerGuid,AddressTypeID,Name,Attention,Address1,Address2,City,PostalCode,CountryID,ProvinceID,ModifiedUser,LocationId")] Address location)
        {
            if (ModelState.IsValid)
            {
                location.ModifiedDate = SystemTime.Now();
                location.ModifiedUser = User.Identity.Name;
                db.Addresses.Add(location);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(location);
        }

        // GET: Admin/Location/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address location = await db.Addresses.FindAsync(id);
            if (location == null)
            {
                return HttpNotFound();
            }

            return View(location);
        }

        // POST: Admin/Location/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,OwnerGuid,AddressTypeID,Name,Attention,Address1,Address2,City,PostalCode,CountryID,ProvinceID,ModifiedUser,LocationId")] Address location)
        {
            if (ModelState.IsValid)
            {
                db.Entry(location).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(location);
        }

        // GET: Admin/Location/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
         
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address location = await db.Addresses.FindAsync(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: Admin/Location/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Address location = await db.Addresses.FindAsync(id);
            db.Addresses.Remove(location);
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
