using LinqKit;
using Orvosi.Data;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class TeamMemberDto : ContactDto
    {
        public Guid PhysicianId { get; set; }

        public static Expression<Func<TeamMember, TeamMemberDto>> FromTeamMemberEntity = c => new TeamMemberDto
        {
            Id = c.Id,
            FirstName = c.AspNetUser.FirstName,
            LastName = c.AspNetUser.LastName,
            ColorCode = c.AspNetUser.ColorCode,
            Email = c.AspNetUser.Email,
            PhysicianId = c.PhysicianId,
            Role = LookupDto<Guid>.FromAspNetRoleEntity.Invoke(c.AspNetRole)
        };
    }
}