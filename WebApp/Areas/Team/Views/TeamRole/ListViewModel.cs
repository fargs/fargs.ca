using System.Collections.Generic;
using WebApp.Views.Shared;
using System.Web.Mvc;
using ImeHub.Data;
using ImeHub.Models;
using System.Security.Principal;
using System;
using System.Linq;
using Enums = ImeHub.Models.Enums;
using LinqKit;

namespace WebApp.Areas.Team.Views.TeamRole
{
    public class ListViewModel : ViewModelBase
    {
        public ListViewModel(Guid? selectedTeamRoleId, ImeHubDbContext db, IIdentity identity, DateTime now) : base(identity, now)
        {
            var teamRole = db.TeamRoles
                .AsNoTracking()
                .AsExpandable()
                .Where(t => t.PhysicianId == PhysicianId)
                .Select(TeamRoleModel.FromTeamRole)
                .ToList();
            TeamRoles = teamRole.Select(TeamRoleViewModel.FromTeamRoleModel);
            
            if (selectedTeamRoleId.HasValue)
            {
                SelectedTeamRoleId = selectedTeamRoleId.Value;
                SelectedTeamRole = TeamRoles.Single(c => c.Id == selectedTeamRoleId.Value);
            }
        }
        public IEnumerable<TeamRoleViewModel> TeamRoles { get; set; }
        public Guid? SelectedTeamRoleId { get; private set; }
        public TeamRoleViewModel SelectedTeamRole { get; private set; }
    }
}