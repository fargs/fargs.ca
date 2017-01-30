using Orvosi.Data;
using Orvosi.Shared.Enums;
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
using Features = Orvosi.Shared.Enums.Features;

namespace WebApp.Controllers
{
    public class ServiceController : Controller
    {
        OrvosiDbContext context = new OrvosiDbContext();
        DateTime now = SystemTime.Now();

        [AuthorizeRole(Feature = Features.Services.Search)]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeRole(Feature = Features.Services.Search)]
        public ActionResult List()
        {
            var userId = User.Identity.GetUserContext().Id;

            var model = context.PhysicianServices
                .Where(pc => pc.PhysicianId == userId)
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
            Guid userId = User.Identity.GetUserContext().Id;
            var now = SystemTime.Now();

            using (var context = new OrvosiDbContext())
            {
                var data = context.Services
                    // WHERE User has a public profile needs to be added in
                    .Where(i => i.Name.Contains(searchTerm))
                    // except services that have already been added.
                    .Except(context.PhysicianServices.Where(ps => ps.PhysicianId == userId).Select(ps => ps.Service))
                    .Select(ServiceProjections.Search())
                    .ToList();

                return Json(new
                {
                    total_count = data.Count(),
                    items = data
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [AuthorizeRole(Feature = Features.Services.Search)]
        public ActionResult Details()
        {
            return View();
        }
        [AuthorizeRole(Feature = Features.Services.Manage)]
        public ActionResult Create(short serviceId)
        {
            var now = SystemTime.Now();
            var userContext = User.Identity.GetUserContext();
            
            if (User.Identity.GetRoleId() != AspNetRoles.Physician && userContext.Id == User.Identity.GetGuidUserId())
            {
                return Json(new
                {
                    success = false,
                    errorMessage = "Set the User Context to a physician"
                });
            }

            var newRecord = new Orvosi.Data.PhysicianService
            {
                PhysicianId = userContext.Id,
                ServiceId = serviceId,
                CreatedDate = now,
                CreatedUser = userContext.Id.ToString(),
                ModifiedDate = now,
                ModifiedUser = userContext.Id.ToString()
            };
            context.PhysicianServices.Add(newRecord);
            context.SaveChanges();
            context.Entry(newRecord).Reload();

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
            var userId = User.Identity.GetUserContext().Id;
            var entity = context.PhysicianServices.Where(c => c.PhysicianId == userId && c.ServiceId == serviceId);
            context.PhysicianServices.RemoveRange(entity);
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