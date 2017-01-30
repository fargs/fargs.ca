using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp.Library;
using WebApp.Library.Extensions;
using WebApp.Library.Filters;
using WebApp.Library.Projections;
using WebApp.ViewModels.UIElements;
using static WebApp.Library.Projections.CompanyProjections;
using Features = Orvosi.Shared.Enums.Features;

namespace WebApp.Controllers
{
    public class PhysicianCompanyController : Controller
    {
        OrvosiDbContext context = new OrvosiDbContext();
        DateTime now = SystemTime.Now();

        [AuthorizeRole(Feature = Features.PhysicianCompany.Search)]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeRole(Feature = Features.PhysicianCompany.Search)]
        public ActionResult List()
        {
            var userId = User.Identity.GetUserContext().Id;

            var model = context.PhysicianCompanies
                .Where(pc => pc.PhysicianId == userId)
                .Select(PhysicianCompanyProjections.Basic())
                .ToList();

            var viewModel = new WebApp.ViewModels.PhysicianCompanyViewModels.IndexViewModel
            {
                Companies = model,
                CompanyCount = model.Count()
            };

            return PartialView("_List", viewModel);
        }

        [AuthorizeRole(Feature = Features.PhysicianCompany.Search)]
        public ActionResult Search(string searchTerm, int? page)
        {
            Guid userId = User.Identity.GetUserContext().Id;
            var now = SystemTime.Now();

            using (var context = new OrvosiDbContext())
            {
                var data = context.Companies
                    // WHERE User has a public profile needs to be added in
                    .Where(i => i.Name.Contains(searchTerm))
                    .Select(CompanyProjections.Search())
                    .ToList();

                return Json(new
                {
                    total_count = data.Count(),
                    items = data
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [AuthorizeRole(Feature = Features.PhysicianCompany.Search)]
        public ActionResult Details()
        {
            return View();
        }
        [AuthorizeRole(Feature = Features.PhysicianCompany.Create)]
        public ActionResult Create(short companyId)
        {
            var now = SystemTime.Now();
            var userContext = User.Identity.GetUserContext();
            var newRecord = new PhysicianCompany
            {
                PhysicianId = userContext.Id,
                CompanyId = companyId,
                StatusId = Orvosi.Shared.Enums.RelationshipStatuses.Active,
                ModifiedDate = now,
                ModifiedUser = userContext.Id.ToString()
            };
            context.PhysicianCompanies.Add(newRecord);
            context.SaveChanges();
            context.Entry(newRecord).Reload();

            return Json(new
            {
                data = newRecord
            });
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.PhysicianCompany.Create)]
        public ActionResult Remove(short id)
        {
            var userId = User.Identity.GetUserContext().Id;
            var entity = context.PhysicianCompanies.Where(c => c.Id == id).Single();
            context.PhysicianCompanies.Remove(entity);
            context.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}