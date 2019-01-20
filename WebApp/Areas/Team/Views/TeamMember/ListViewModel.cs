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

namespace WebApp.Areas.Team.Views.TeamMember
{
    public class ListViewModel : ViewModelBase
    {
        public ListViewModel(Guid? selectedTeamMemberId, ImeHubDbContext db, IIdentity identity, DateTime now) : base(identity, now)
        {
            var teamMemberInvites = db.TeamMemberInvites
                .AsNoTracking()
                .AsExpandable()
                .Where(t => t.PhysicianId == PhysicianId)
                .Where(t => t.InviteStatusId == (byte)Enums.InviteStatus.NotSent || t.InviteStatusId == (byte)Enums.InviteStatus.NotResponded)
                .Select(TeamMemberInviteModel.FromTeamMemberInvite)
                .ToList();
            TeamMemberInvites = teamMemberInvites.Select(TeamMemberInviteViewModel.FromTeamMemberInviteModel);

            var teamMembers = db.TeamMembers
                .Where(pc => pc.PhysicianId == PhysicianId)
                .Select(TeamMemberModel.FromTeamMember.Expand())
                .ToList();

            TeamMembers = teamMembers.Select(TeamMemberViewModel.FromTeamMember);
            if (selectedTeamMemberId.HasValue)
            {
                SelectedTeamMemberId = selectedTeamMemberId.Value;
                SelectedTeamMember = TeamMembers.Single(c => c.Id == selectedTeamMemberId.Value);
            }
        }
        public IEnumerable<TeamMemberInviteViewModel> TeamMemberInvites { get; set; }
        public IEnumerable<TeamMemberViewModel> TeamMembers { get; set; }
        public Guid? SelectedTeamMemberId { get; private set; }
        public TeamMemberViewModel SelectedTeamMember { get; private set; }
    }
}