using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Orvosi.Data;
using Microsoft.AspNet.Identity;
using Orvosi.Shared.Enums;
using WebApp.Library.Filters;
using Features = Orvosi.Shared.Enums.Features;

namespace WebApp.Areas.Admin.Controllers
{
    public class ServiceController : BaseController
    {
        private OrvosiDbContext db = new OrvosiDbContext();


        [AuthorizeRole(Feature = Features.Admin.ManageServices)]
        public ActionResult Index(byte parentId)
        {
            var list = db.Services.Where(s => s.ServicePortfolioId == parentId).OrderBy(c => c.ServiceCategoryId).ToList();
            return View(list);
        }

        [AuthorizeRole(Feature = Features.Admin.ManageServices)]
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        [AuthorizeRole(Feature = Features.Admin.ManageServices)]
        public ActionResult Create()
        {
            ViewBag.ServiceCategories = db.ServiceCategories.Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() }).ToList();
            ViewBag.ServicePortfolios = db.ServicePortfolios.Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() }).ToList();
            return View();
        }

        [AuthorizeRole(Feature = Features.Admin.ManageServices)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Service service)
        {
            if (ModelState.IsValid)
            {
                service.ModifiedUser = User.Identity.GetUserId();
                db.Services.Add(service);
                db.SaveChanges();
                return RedirectToAction("Index", new { parentId = ServicePortfolios.Physician });
            }

            return View(service);
        }

        [AuthorizeRole(Feature = Features.Admin.ManageServices)]
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }

            ViewBag.ServiceCategories = db.ServiceCategories.Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() }).ToList();
            ViewBag.ServicePortfolios = db.ServicePortfolios.Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() }).ToList();

            return View(service);
        }

        [AuthorizeRole(Feature = Features.Admin.ManageServices)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Service service)
        {
            if (ModelState.IsValid)
            {
                service.ModifiedUser = User.Identity.GetUserId();
                db.Entry(service).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { parentId = ServicePortfolios.Physician });
            }
            return View(service);
        }

        [AuthorizeRole(Feature = Features.Admin.ManageServices)]
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        [AuthorizeRole(Feature = Features.Admin.ManageServices)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Service service = db.Services.Find(id);
            db.Services.Remove(service);
            db.SaveChanges();
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
