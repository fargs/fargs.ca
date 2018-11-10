using LinqKit;
using Data = ImeHub.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ImeHub.Models
{
    public class PersonModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ColorCode { get; set; }

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
        
        // projections
        public static Expression<Func<Data.User, PersonModel>> FromUser = a => a == null ? null : new PersonModel
        {
            Id = a.Id,
            FirstName = a.FirstName,
            LastName = a.LastName,
            ColorCode = a.ColorCode
        };

        public static Expression<Func<Data.TeamMember, PersonModel>> FromTeamMember = a => a == null ? null : new PersonModel
        {
            Id = a.User.Id,
            FirstName = a.User.FirstName,
            LastName = a.User.LastName,
            ColorCode = a.User.ColorCode
        };
    }
}