using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Orvosi.Data;
using System;
using System.Linq;
using Orvosi.Shared.Enums;
using WebApp.Areas.Admin.ViewModels;
using WebApp.Library;
using WebApp.Library.Filters;
using Features = Orvosi.Shared.Enums.Features;

namespace WebApp.Areas.Admin.Controllers
{
    public class LocationController : BaseController
    {
        private OrvosiDbContext db = new OrvosiDbContext();

        [AuthorizeRole(Feature = Features.Admin.ManageAddresses)]
        public async Task<ActionResult> Index()
        {
            // retrieve all the PHYSICIANS from the database
            var result = new DataHelper().LoadAddressesWithOwner(new OrvosiDbContext());
            return View(result);
        }

        [AuthorizeRole(Feature = Features.Admin.ManageAddresses)]
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

        [AuthorizeRole(Feature = Features.Admin.ManageAddresses)]
        public ActionResult Create() => View();

        [AuthorizeRole(Feature = Features.Admin.ManageAddresses)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,OwnerGuid,AddressTypeID,Name,Attention,Address1,Address2,CityId,PostalCode,CountryID,ProvinceID,ModifiedUser,LocationId,TimeZoneId")] Address location)
        {
            if (ModelState.IsValid)
            {
                location.City = location.CityId.ToString(); // to maintain backwards compatibility
                location.ModifiedDate = SystemTime.Now();
                location.ModifiedUser = User.Identity.Name;
                db.Addresses.Add(location);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(location);
        }

        [AuthorizeRole(Feature = Features.Admin.ManageAddresses)]
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

        [AuthorizeRole(Feature = Features.Admin.ManageAddresses)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,OwnerGuid,AddressTypeID,Name,Attention,Address1,Address2,CityId,PostalCode,CountryID,ProvinceID,ModifiedUser,LocationId,TimeZoneId")] Address location)
        {
            if (ModelState.IsValid)
            {
                location.City = location.CityId.ToString(); // to maintain backwards compatibility
                location.ModifiedDate = SystemTime.Now();
                location.ModifiedUser = User.Identity.Name;
                db.Entry(location).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(location);
        }

        [AuthorizeRole(Feature = Features.Admin.ManageAddresses)]
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

        [AuthorizeRole(Feature = Features.Admin.ManageAddresses)]
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
