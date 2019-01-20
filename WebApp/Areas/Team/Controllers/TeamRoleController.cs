using ImeHub.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Shared;
using WebApp.Library.Filters;
using Features = ImeHub.Models.Enums.Features.PhysicianPortal;
using WebApp.Areas.Team.Views.TeamRole;

namespace WebApp.Areas.Team.Controllers
{
    public class TeamRoleController : BaseController
    {
        private ImeHubDbContext db;

        public TeamRoleController(ImeHubDbContext db, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
        }
        [AuthorizeRole(Feature = Features.Team.Search)]
        public ViewResult Index(Guid? selectedTeamMemberId)
        {
            var list = new ListViewModel(selectedTeamMemberId, db, identity, now);

            var viewModel = new IndexViewModel(list, identity, now);

            return View(viewModel);
        }

        #region Views


        public PartialViewResult ShowTeamRoleForm()
        {
            var formModel = new TeamRoleForm(physicianId.Value);

            return PartialView("TeamRoleForm", formModel);
        }

        public async System.Threading.Tasks.Task<ActionResult> SaveTeamRoleForm(TeamRoleForm form)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("TeamRole/TeamRoleForm", form);
            }

            var teamRole = new TeamRole
            {
                Id = Guid.NewGuid(),
                Name = form.Name,
                PhysicianId = form.PhysicianId
            };

            db.TeamRoles.Add(teamRole);
            await db.SaveChangesAsync();

            return Json(new
            {
                id = teamRole.Id
            });
        }
        #endregion
    }
}