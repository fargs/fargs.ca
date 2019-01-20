using System;
using System.Linq.Expressions;
using ImeHub.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Team.Views.TeamMember
{
    public class TeamMemberViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid PhysicianId { get; set; }
        public LookupViewModel<Guid> Role { get; set; }
        public TeamMemberViewModel()
        {
        }
        public static Func<TeamMemberModel, TeamMemberViewModel> FromTeamMember = c => new TeamMemberViewModel
        {
            Id = c.Id,
            Name = c.User.DisplayName,
            PhysicianId = c.PhysicianId,
            Role = LookupViewModel<Guid>.FromLookupModel(c.Role)
        };
    }
}