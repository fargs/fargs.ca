using System;
using System.Linq.Expressions;
using ImeHub.Models;
using WebApp.Views.Shared;
using Enums = ImeHub.Models.Enums;

namespace WebApp.Areas.Team.Views.TeamMember
{
    public class TeamMemberInviteViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public LookupViewModel<Guid> Role { get; set; }
        public Enums.InviteStatus InviteStatusId { get; set; }
        public LookupViewModel<byte> InviteStatus { get; set; }
        public TeamMemberInviteViewModel()
        {
        }
        public static Func<TeamMemberInviteModel, TeamMemberInviteViewModel> FromTeamMemberInviteModel = c => new TeamMemberInviteViewModel
        {
            Id = c.Id,
            Name = c.DisplayName,
            Role = LookupViewModel<Guid>.FromLookupModel(c.Role),
            InviteStatusId = c.InviteStatusId,
            InviteStatus = LookupViewModel<byte>.FromLookupModel(c.InviteStatus)
        };
    }
}