using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model;
using Model.Enums;

namespace WebApp.Areas.Admin.Controllers
{
    public class LocationController : Controller
    {
        private OrvosiEntities db = new OrvosiEntities();

        // GET: Admin/Location
        public async Task<ActionResult> Index()
        {
            return View(await db.Locations.ToListAsync());
        }

        // GET: Admin/Location/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = await db.Locations.FindAsync(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // GET: Admin/Location/Create
        public ActionResult Create()
        {
            ViewBag.Owners = db.Entities
                .Where(c => c.EntityType == EntityTypes.Company || c.EntityType == EntityTypes.Physician)
                .Select(c => new SelectListItem()
                {
                    Text = c.DisplayName,
                    Value = c.EntityId.ToString()
                }).ToList();

            ViewBag.Locations = db.LookupItems
                .Where(c => c.LookupId == Lookups.LocationAreas)
                .Select(c => new SelectListItem()
                {
                    Text = c.ItemText,
                    Value = c.ItemId.ToString()
                }).ToList();

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
        public async Task<ActionResult> Create([Bind(Include = "Id,ObjectGuid,AddressTypeID,AddressTypeName,Name,Attention,Address1,Address2,City,PostalCode,CountryID,ProvinceID,ModifiedDate,ModifiedUser,CountryName,CountryCode,ProvinceName,ProvinceCode,LocationId,LocationName")] Location location)
        {
            if (ModelState.IsValid)
            {
                db.Locations.Add(location);
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
            Location location = await db.Locations.FindAsync(id);
            if (location == null)
            {
                return HttpNotFound();
            }

            ViewBag.Owners = db.Entities
                .Where(c => c.EntityType == EntityTypes.Company || c.EntityType == EntityTypes.Physician)
                .Select(c => new SelectListItem() {
                    Text = c.DisplayName,
                    Value = c.EntityId.ToString()
                }).ToList();

            ViewBag.Locations = db.LookupItems
                .Where(c => c.LookupId == Lookups.LocationAreas)
                .Select(c => new SelectListItem()
                {
                    Text = c.ItemText,
                    Value = c.ItemId.ToString()
                }).ToList();

            ViewBag.Provinces = db.Provinces
                .Where(c => c.CountryId == 124) // Canada
                .Select(c => new SelectListItem()
                {
                    Text = c.ProvinceName,
                    Value = c.Id.ToString()
                }).ToList();
            return View(location);
        }

        // POST: Admin/Location/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ObjectGuid,AddressTypeID,AddressTypeName,Name,Attention,Address1,Address2,City,PostalCode,CountryID,ProvinceID,ModifiedDate,ModifiedUser,CountryName,CountryCode,ProvinceName,ProvinceCode,LocationId,LocationName")] Location location)
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
            Location location = await db.Locations.FindAsync(id);
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
            Location location = await db.Locations.FindAsync(id);
            db.Locations.Remove(location);
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
