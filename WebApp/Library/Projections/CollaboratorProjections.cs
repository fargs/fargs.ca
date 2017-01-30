using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Library.Projections
{
    public class CollaboratorProjections
    {
        public static Expression<Func<Orvosi.Data.Collaborator, Orvosi.Shared.Model.Person>> Basic()
        {
            return sr => new Orvosi.Shared.Model.Person
            {
                Id = sr.CollaboratorUserId,
                Title = sr.CollaboratorUser.Title,
                FirstName = sr.CollaboratorUser.FirstName,
                LastName = sr.CollaboratorUser.LastName,
                Email = sr.CollaboratorUser.Email,
                ColorCode = sr.CollaboratorUser.ColorCode,
                Role = sr.CollaboratorUser.AspNetUserRoles.Select(r => new Orvosi.Shared.Model.UserRole
                {
                    Id = r.RoleId,
                    Name = r.AspNetRole.Name
                }).FirstOrDefault()

            };
        }
    }
}