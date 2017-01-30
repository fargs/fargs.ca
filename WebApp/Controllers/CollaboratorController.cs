using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using WebApp.Library.Extensions;
using WebApp.Library.Filters;
using WebApp.Library.Projections;
using Features = Orvosi.Shared.Enums.Features;
using Orvosi.Shared.Enums;
using WebApp.ViewModels.UIElements;
using WebApp.Library;

namespace WebApp.Controllers
{
    
    public class CollaboratorController : Controller
    {
        OrvosiDbContext context = new OrvosiDbContext();
        DateTime now = SystemTime.Now();

        [AuthorizeRole(Feature = Features.Collaborator.Search)]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeRole(Feature = Features.Collaborator.Search)]
        public ActionResult List()
        {
            var userId = User.Identity.GetUserContext().Id;

            var model = context.Collaborators
                .Where(ww => ww.UserId == userId)
                .Select(CollaboratorProjections.Basic())
                .ToList();

            // Group the results by Role
            var groupedModel = model
                .GroupBy(m => new { m.Role.Id, m.Role.Name })
                .Select(m => new WebApp.ViewModels.CollaboratorViewModel.Role
                {
                    Id = m.Key.Id,
                    Name = m.Key.Name,
                    People = m.OrderBy(p => p.LastName)
                })
                .ToList();

            var viewModel = new WebApp.ViewModels.CollaboratorViewModel.IndexViewModel
            {
                Roles = groupedModel,
                Total = model.Count()
            };

            return PartialView("_List", viewModel);
        }

        [AuthorizeRole(Feature = Features.Collaborator.Search)]
        public ActionResult Search(string searchTerm, int? page)
        {
            Guid userId = User.Identity.GetUserContext().Id;
            var now = SystemTime.Now();

            using (var context = new OrvosiDbContext())
            {
                var data = context.AspNetUsers
                    // WHERE User has a public profile needs to be added in
                    .Where(i => i.FirstName.Contains(searchTerm)
                        || i.LastName.Contains(searchTerm)
                        || i.Title.Contains(searchTerm))
                    .Select(AspNetUserProjections.Basic())
                    .ToList();

                var result = data.Select(d => new
                {
                    id = d.Id,
                    DisplayName = d.DisplayName,
                    Initials = d.Initials,
                    ColorCode = d.ColorCode,
                    Role = d.Role
                });

                return Json(new
                {
                    total_count = result.Count(),
                    items = result
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRole(Feature = Features.Collaborator.Search)]
        public ActionResult Details()
        {
            return View();
        }

        [AuthorizeRole(Feature = Features.Collaborator.Create)]
        public ActionResult Create(Guid collaboratorUserId)
        {
            var now = SystemTime.Now();
            var userContext = User.Identity.GetUserContext();
            var newRecord = new Collaborator
            {
                UserId = userContext.Id,
                CollaboratorUserId = collaboratorUserId,
                CreatedDate = now,
                CreatedUser = userContext.Id.ToString(),
                ModifiedDate = now,
                ModifiedUser = userContext.Id.ToString()
            };
            context.Collaborators.Add(newRecord);
            context.SaveChanges();
            context.Entry(newRecord).Reload();

            return Json(new
            {
                data = newRecord
            });
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.Collaborator.Create)]
        public ActionResult Remove(Guid collaboratorUserId)
        {
            var userId = User.Identity.GetUserContext().Id;
            var entity = context.Collaborators.Where(c => c.UserId == userId && c.CollaboratorUserId == collaboratorUserId).Single();
            context.Collaborators.Remove(entity);
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