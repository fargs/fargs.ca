using ImeHub.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImeHub.Models
{
    public class TeamRoleModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ColorCode { get; set; }
        public Guid PhysicianId { get; set; }
        public TeamRoleModel()
        {
        }
        public static Func<TeamRole, TeamRoleModel> FromTeamRole = c => new TeamRoleModel
        {
            Id = c.Id,
            Name = c.Name,
            Code = c.Code,
            ColorCode = c.ColorCode,
            PhysicianId = c.PhysicianId
        };
    }
}
