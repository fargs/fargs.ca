using System;
using System.Linq.Expressions;
using ImeHub.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Team.Views.TeamRole
{
    public class TeamRoleViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ColorCode { get; set; }
        public Guid PhysicianId { get; set; }
        public TeamRoleViewModel()
        {
        }
        public static Func<TeamRoleModel, TeamRoleViewModel> FromTeamRoleModel = c => new TeamRoleViewModel
        {
            Id = c.Id,
            Name = c.Name,
            Code = c.Code,
            ColorCode = c.ColorCode,
            PhysicianId = c.PhysicianId
        };
    }
}