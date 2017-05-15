using Orvosi.Data;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Admin.ViewModels.CompanyViewModels;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Data.Entity;
using WebApp.Library.Filters;
using Features = Orvosi.Shared.Enums.Features;

namespace WebApp.Areas.Admin.Controllers
{
    public class CompanyController : BaseController
    {
        OrvosiDbContext db = new OrvosiDbContext();

        [AuthorizeRole(Feature = Features.Admin.ManageCompanies)]
        public ActionResult Index(Nullable<byte> parentId)
        {
            var companies = db.Companies.Where(c => c.ParentId == parentId && c.IsParent == false).ToList(); // exclude examworks and scm
            var companyContacts = db.AspNetUsers.Where(c => c.AspNetUserRoles.FirstOrDefault().RoleId == AspNetRoles.Company).ToList();

            var vm = new IndexViewModel()
            {
                Companies = companies,
                CompanyContacts = companyContacts
            };

            return View(vm);
        }

        [AuthorizeRole(Feature = Features.Admin.ManageCompanies)]
        public ActionResult Create()
        {
            ViewBag.ParentCompanies = db.Companies
                .Where(c => c.IsParent == true)
                .Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() })
                .ToList();
            return View();
        }

        [AuthorizeRole(Feature = Features.Admin.ManageCompanies)]
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

        [AuthorizeRole(Feature = Features.Admin.ManageCompanies)]
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

        [AuthorizeRole(Feature = Features.Admin.ManageCompanies)]
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