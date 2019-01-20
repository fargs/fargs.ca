using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data = ImeHub.Data;

namespace ImeHub.Models
{
    public class TeamMemberModel
    {
        public Guid Id { get; set; }
        public Guid PhysicianId { get; set; }
        public PhysicianModel Physician { get; set; }
        public Guid RoleId { get; set; }
        public LookupModel<Guid> Role { get; set; }
        public Guid UserId { get; set; }
        public ContactModel User { get; set; }

        public static Expression<Func<Data.TeamMember, TeamMemberModel>> FromTeamMember = a => a == null ? null : new TeamMemberModel
        {
            Id = a.Id,
            PhysicianId = a.PhysicianId,
            Physician = PhysicianModel.FromPhysician.Invoke(a.Physician),
            RoleId = a.RoleId,
            Role = LookupModel<Guid>.FromTeamRole.Invoke(a.TeamRole),
            UserId = a.UserId,
            User = ContactModel.FromUser.Invoke(a.User)
        };
    }
}
