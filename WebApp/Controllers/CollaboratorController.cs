using Orvosi.Data;
using Orvosi.Shared.Enums;
using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.Library.Filters;
using WebApp.Library.Projections;
using Features = Orvosi.Shared.Enums.Features;

namespace WebApp.Controllers
{

    public class CollaboratorController : BaseController
    {
        private OrvosiDbContext db;

        public CollaboratorController(OrvosiDbContext db, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
        }

        [AuthorizeRole(Feature = Features.Collaborator.Search)]
        public ViewResult Index()
        {
            return View();
        }

        [AuthorizeRole(Feature = Features.Collaborator.Search)]
        public PartialViewResult List()
        {
            var model = db.Collaborators
                .Where(ww => ww.UserId == physicianId)
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
        public JsonResult Search(string searchTerm, int? page)
        {
            var data = db.AspNetUsers
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

        [AuthorizeRole(Feature = Features.Collaborator.Search)]
        public ViewResult Details()
        {
            return View();
        }

        [AuthorizeRole(Feature = Features.Collaborator.Create)]
        public JsonResult Create(Guid collaboratorUserId)
        {
            if (loggedInRoleId != AspNetRoles.Physician && !physicianId.HasValue)
            {
                return Json(new
                {
                    success = false,
                    errorMessage = "Set the User Context to a physician"
                });
            }
            var newRecord = new Collaborator
            {
                UserId = physicianId.Value,
                CollaboratorUserId = collaboratorUserId,
                CreatedDate = now,
                CreatedUser = loggedInUserId.ToString(),
                ModifiedDate = now,
                ModifiedUser = loggedInUserId.ToString()
            };
            db.Collaborators.Add(newRecord);
            db.SaveChanges();
            db.Entry(newRecord).Reload();

            return Json(new
            {
                success = true,
                data = newRecord
            });
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.Collaborator.Create)]
        public async Task<JsonResult> Remove(Guid collaboratorUserId)
        {
            if (loggedInRoleId != AspNetRoles.Physician && !physicianId.HasValue)
            {
                return Json(new
                {
                    success = false,
                    errorMessage = "Set the User Context to a physician"
                });
            }
            var entity = await db.Collaborators.Where(c => c.UserId == physicianId && c.CollaboratorUserId == collaboratorUserId).ToListAsync();
            db.Collaborators.RemoveRange(entity);
            db.SaveChanges();
            return Json(new { success = true });
        }
    }
}