using System.Collections.Generic;
using WebApp.Views.Shared;
using System.Web.Mvc;
using Orvosi.Data;
using System.Security.Principal;
using System;
using System.Linq;
using WebApp.Models;
using LinqKit;

namespace WebApp.Areas.Team.Views.TeamMember
{
    public class ListViewModel : ViewModelBase
    {
        public ListViewModel(Guid? teamMemberId, OrvosiDbContext db, IIdentity identity, DateTime now) : base(identity, now)
        {
            var teamMembers = db.TeamMembers
                .Where(pc => pc.PhysicianId == PhysicianId)
                .Select(TeamMemberDto.FromTeamMemberEntity.Expand())
                .ToList();

            TeamMembers = teamMembers.Select(TeamMemberViewModel.FromTeamMemberDto);
            if (teamMemberId.HasValue)
            {
                SelectedTeamMemberId = teamMemberId.Value;
                SelectedTeamMember = TeamMembers.Single(c => c.Id == teamMemberId.Value);
            }
        }
        public IEnumerable<TeamMemberViewModel> TeamMembers { get; set; }
        public Guid? SelectedTeamMemberId { get; private set; }
        public TeamMemberViewModel SelectedTeamMember { get; private set; }
    }
}