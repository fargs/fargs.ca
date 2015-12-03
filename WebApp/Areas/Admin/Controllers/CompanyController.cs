using Model;
using Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Admin.ViewModels.CompanyViewModels;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Data.Entity;

namespace WebApp.Areas.Admin.Controllers
{
    public class CompanyController : BaseController
    {
        OrvosiEntities db = new OrvosiEntities();
        // GET: Admin/Company
        public ActionResult Index(Nullable<byte> parentId)
        {
            var companies = db.Companies.Where(c => c.ParentId == parentId && c.IsParent == false).ToList(); // exclude examworks and scm
            var companyContacts = db.Users.Where(c => c.RoleId == Roles.Company).ToList();

            var vm = new IndexViewModel()
            {
                Companies = companies,
                CompanyContacts = companyContacts
            };

            return View(vm);
        }

        // GET: Admin/company/Create
        public ActionResult Create()
        {
            ViewBag.ParentCompanies = db.Companies
                .Where(c => c.IsParent == true)
                .Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() })
                .ToList();
            return View();
        }

        // POST: Admin/company/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "Id,ObjectGuid,ModifiedDate,ModifiedUser")]Company company)
        {
            if (ModelState.IsValid)
            {
                company.ModifiedUser = User.Identity.GetUserId();
                db.Companies.Add(company);
                db.SaveChanges();
                return RedirectToAction("Index", new { parentId = company.ParentId });
            }

            return View(company);
        }

        // GET: Admin/company/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }

            ViewBag.ParentCompanies = db.Companies
                .Where(c => c.IsParent == true)
                .Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() })
                .ToList();

            return View(company);
        }

        // POST: Admin/company/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                company.ModifiedUser = User.Identity.GetUserId();
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(company);
        }
    }
}