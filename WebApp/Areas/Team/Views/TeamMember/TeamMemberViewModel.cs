using System;
using System.Linq.Expressions;
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Team.Views.TeamMember
{
    public class TeamMemberViewModel : ContactViewModel
    {
        public Guid PhysicianId { get; set; }
        public LookupViewModel<Guid> Role { get; set; }
        public TeamMemberViewModel()
        {

        }
        public static Func<TeamMemberDto, TeamMemberViewModel> FromTeamMemberDto = c => new TeamMemberViewModel
        {
            Id = c.Id,
            Name = c.DisplayName,
            Code = c.Initials,
            ColorCode = c.ColorCode,
            Email = c.Email,
            PhysicianId = c.PhysicianId,
            Role = LookupViewModel<Guid>.FromLookupDto(c.Role)
        };
    }
}