using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model;
using Microsoft.AspNet.Identity;
using Model.Enums;

namespace WebApp.Areas.Admin.Controllers
{
    public class ServiceController : BaseController
    {
        private OrvosiEntities db = new OrvosiEntities();

        // GET: Admin/Service
        public ActionResult Index(byte parentId)
        {
            var list = db.Services.Where(s => s.ServicePortfolioId == parentId).OrderBy(c => c.ServiceCategoryId).ToList();
            return View(list);
        }

        // GET: Admin/Service/Details/5
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

        // GET: Admin/Service/Create
        public ActionResult Create()
        {
            ViewBag.ServiceCategories = db.ServiceCategories.Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() }).ToList();
            ViewBag.ServicePortfolios = db.ServicePortfolios.Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() }).ToList();
            return View();
        }

        // POST: Admin/Service/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Admin/Service/Edit/5
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

        // POST: Admin/Service/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Admin/Service/Delete/5
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

        // POST: Admin/Service/Delete/5
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
