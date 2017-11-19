using Orvosi.Data;
using Orvosi.Shared.Enums;
using System;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using WebApp.Library.Filters;
using WebApp.Library.Projections;
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
        public ViewResult Index()
        {
            return View();
        }

        [AuthorizeRole(Feature = Features.PhysicianCompany.Search)]
        public PartialViewResult List()
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
        public JsonResult Search(string searchTerm, int? page)
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
        public ViewResult Details()
        {
            return View();
        }
        [AuthorizeRole(Feature = Features.PhysicianCompany.Create)]
        public JsonResult Create(short companyId)
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
        public JsonResult Remove(short id)
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