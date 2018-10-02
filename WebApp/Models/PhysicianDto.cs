using LinqKit;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class PhysicianDto : PersonDto
    {
        public string Email { get; set; }
        public IEnumerable<PersonDto> TeamMembers { get; set; }

        public static Expression<Func<AspNetUser, PhysicianDto>> FromPhysicianEntity = a => a == null ? null : new PhysicianDto
        {
            Id = a.Id,
            FirstName = a.FirstName,
            LastName = a.LastName,
            ColorCode = a.ColorCode,
            Email = a.Email,
            Role = a.AspNetUserRoles.FirstOrDefault() == null ? null : LookupDto<Guid>.FromAspNetRoleEntity.Invoke(a.AspNetUserRoles.FirstOrDefault().AspNetRole),
            TeamMembers = a.User.AsQueryable().Select(cu => PersonDto.FromAspNetUserEntity.Invoke(cu.CollaboratorUser))
        };

        public new static Expression<Func<AspNetUser, PhysicianDto>> FromAspNetUserEntity = a => a == null ? null : new PhysicianDto
        {
            Id = a.Id,
            FirstName = a.FirstName,
            LastName = a.LastName,
            ColorCode = a.ColorCode,
            Email = a.Email
        };
    }
}