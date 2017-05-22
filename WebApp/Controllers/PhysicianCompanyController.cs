using Orvosi.Data;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
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
    public class PhysicianCompanyController : BaseController
    {
        private OrvosiDbContext db;

        public PhysicianCompanyController(OrvosiDbContext db, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
        }
        [AuthorizeRole(Feature = Features.PhysicianCompany.Search)]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeRole(Feature = Features.PhysicianCompany.Search)]
        public ActionResult List()
        {
            var model = db.PhysicianCompanies
                .Where(pc => pc.PhysicianId == physicianId)
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
                var data = db.Companies
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
        [AuthorizeRole(Feature = Features.PhysicianCompany.Search)]
        public ActionResult Details()
        {
            return View();
        }
        [AuthorizeRole(Feature = Features.PhysicianCompany.Create)]
        public ActionResult Create(short companyId)
        {
            if (loggedInRoleId != AspNetRoles.Physician && !physicianId.HasValue)
            {
                return Json(new
                {
                    success = false,
                    errorMessage = "Set the User Context to a physician"
                });
            }
            var newRecord = new PhysicianCompany
            {
                PhysicianId = physicianId.Value,
                CompanyId = companyId,
                StatusId = Orvosi.Shared.Enums.RelationshipStatuses.Active,
                ModifiedDate = now,
                ModifiedUser = loggedInUserId.ToString()
            };
            db.PhysicianCompanies.Add(newRecord);
            db.SaveChanges();
            db.Entry(newRecord).Reload();

            return Json(new
            {
                success = true,
                data = newRecord
            });
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.PhysicianCompany.Create)]
        public ActionResult Remove(short id)
        {
            if (loggedInRoleId != AspNetRoles.Physician && !physicianId.HasValue)
            {
                return Json(new
                {
                    success = false,
                    errorMessage = "Set the User Context to a physician"
                });
            }
            var entity = db.PhysicianCompanies.Where(c => c.Id == id);
            db.PhysicianCompanies.RemoveRange(entity);
            db.SaveChanges();
            return Json(new { success = true });
        }
    }
}