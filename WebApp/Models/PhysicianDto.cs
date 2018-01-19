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

        public static Expression<Func<AspNetUser, PersonDto>> FromPhysicianEntity = a => a == null ? null : new PersonDto
        {
            Id = a.Id,
            FirstName = a.FirstName,
            LastName = a.LastName,
            ColorCode = a.ColorCode,
            Role = a.AspNetUserRoles.FirstOrDefault() == null ? null : LookupDto<Guid>.FromAspNetRoleEntity.Invoke(a.AspNetUserRoles.FirstOrDefault().AspNetRole)
        };
    }
}