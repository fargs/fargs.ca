using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Library.Projections
{
    public class AspNetUserProjections
    {
        public static Expression<Func<Orvosi.Data.AspNetUser, Orvosi.Shared.Model.Person>> Basic()
        {
            return sr => new Orvosi.Shared.Model.Person
            {
                Id = sr.Id,
                Title = sr.Title,
                FirstName = sr.FirstName,
                LastName = sr.LastName,
                Email = sr.Email,
                ColorCode = sr.ColorCode,
                Role = sr.AspNetUserRoles.Select(r => new Orvosi.Shared.Model.UserRole
                {
                    Id = r.RoleId,
                    Name = r.AspNetRole.Name
                }).FirstOrDefault()

            };
        }
    }
}