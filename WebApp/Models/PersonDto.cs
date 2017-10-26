﻿using LinqKit;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class PersonDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ColorCode { get; set; }
        public LookupDto<Guid> Role { get; set; }
        // computeds
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(FirstName))
                    return "Unassigned";
                else
                    return $"{(!string.IsNullOrEmpty(Title) ? Title + " " : "")}{FirstName} {LastName}";
            }
        }
        public string Initials
        {
            get
            {
                if (string.IsNullOrEmpty(FirstName))
                    return "Unassigned";
                else
                    return $"{FirstName.ToUpper().First()}{LastName.ToUpper().First()}";
            }
        }

        public static Expression<Func<AspNetUser, PersonDto>> FromAspNetUserEntity = a => a == null ? null : new PersonDto
        {
            Id = a.Id,
            FirstName = a.FirstName,
            LastName = a.LastName,
            ColorCode = a.ColorCode
        };

        public static Expression<Func<AspNetUser, PersonDto>> FromAspNetUserEntityWithRole = a => a == null ? null : new PersonDto
        {
            Id = a.Id,
            FirstName = a.FirstName,
            LastName = a.LastName,
            ColorCode = a.ColorCode,
            Role = a.AspNetUserRoles.FirstOrDefault() == null ? null : LookupDto<Guid>.FromAspNetRoleEntity.Invoke(a.AspNetUserRoles.FirstOrDefault().AspNetRole)
        };

        public static Expression<Func<Collaborator, PersonDto>> FromCollaboratorEntity = a => a == null ? null : new PersonDto
        {
            Id = a.CollaboratorUser.Id,
            FirstName = a.CollaboratorUser.FirstName,
            LastName = a.CollaboratorUser.LastName,
            ColorCode = a.CollaboratorUser.ColorCode
        };

        public static Expression<Func<ServiceRequestResource, PersonDto>> FromServiceRequestResourceEntity = a => a == null ? null : new PersonDto
        {
            Id = a.AspNetUser.Id,
            Title = a.AspNetUser.Title,
            FirstName = a.AspNetUser.FirstName,
            LastName = a.AspNetUser.LastName,
            ColorCode = a.AspNetUser.ColorCode
        };
    }
}