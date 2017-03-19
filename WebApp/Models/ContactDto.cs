using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class ContactDto : PersonDto
    {
        public string Email { get; set; }

        public static new Expression<Func<AspNetUser, ContactDto>> FromAspNetUserEntity = a => new ContactDto
        {
            Id = a.Id,
            FirstName = a.FirstName,
            LastName = a.LastName,
            Title = a.Title,
            ColorCode = a.ColorCode,
            Email = a.Email
        };
        
    }
}