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
using Features = Orvosi.Shared.Enums.Features;

namespace WebApp.Controllers
{
    public class ServiceController : BaseController
    {
        private OrvosiDbContext db;

        public ServiceController(OrvosiDbContext db, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
        }
        [AuthorizeRole(Feature = Features.Services.Search)]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeRole(Feature = Features.Services.Search)]
        public ActionResult List()
        {
            var model = db.PhysicianServices
                .Where(pc => pc.PhysicianId == physicianId)
                .Select(pc => pc.Service)
                .Select(ServiceProjections.Search())
                .ToList();

            var viewModel = new ServiceProjections.ListViewModel
            {
                Services = model,
                ServiceCount = model.Count()
            };

            return PartialView("_List", viewModel);
        }

        [AuthorizeRole(Feature = Features.Services.Search)]
        public ActionResult Search(string searchTerm, int? page)
        {
                var data = db.Services
                    // WHERE User has a public profile needs to be added in
                    .Where(i => i.Name.Contains(searchTerm))
                    // except services that have already been added.
                    .Except(db.PhysicianServices.Where(ps => ps.PhysicianId == physicianId).Select(ps => ps.Service))
                    .Select(ServiceProjections.Search())
                    .ToList();

                return Json(new
                {
                    total_count = data.Count(),
                    items = data
                }, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeRole(Feature = Features.Services.Search)]
        public ActionResult Details()
        {
            return View();
        }
        [AuthorizeRole(Feature = Features.Services.Manage)]
        public ActionResult Create(short serviceId)
        {
            if (loggedInRoleId != AspNetRoles.Physician && !physicianId.HasValue)
            {
                return Json(new
                {
                    success = false,
                    errorMessage = "Set the User Context to a physician"
                });
            }

            var newRecord = new Orvosi.Data.PhysicianService
            {
                PhysicianId = physicianContext.Id,
                ServiceId = serviceId,
                CreatedDate = now,
                CreatedUser = physicianContext.Id.ToString(),
                ModifiedDate = now,
                ModifiedUser = physicianContext.Id.ToString()
            };
            db.PhysicianServices.Add(newRecord);
            db.SaveChanges();
            db.Entry(newRecord).Reload();

            return Json(new
            {
                success = true,
                data = newRecord
            });
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.Services.Manage)]
        public ActionResult Remove(short serviceId)
        {
            if (loggedInRoleId != AspNetRoles.Physician && !physicianId.HasValue)
            {
                return Json(new
                {
                    success = false,
                    errorMessage = "Set the User Context to a physician"
                });
            }
            var entity = db.PhysicianServices.Where(c => c.PhysicianId == physicianId && c.ServiceId == serviceId);
            db.PhysicianServices.RemoveRange(entity);
            db.SaveChanges();
            return Json(new { success = true });
        }
    }
}