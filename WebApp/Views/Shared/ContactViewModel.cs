using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.Views.Shared
{
    public class ContactViewModel<T> : LookupViewModel<T>
    {
        public string Email { get; set; }

        public static Func<ContactDto, ContactViewModel<Guid>> FromContactDto = e => e == null ? null : new ContactViewModel<Guid>
        {
            Id = e.Id,
            Name = e.DisplayName,
            Code = e.Initials,
            ColorCode = e.ColorCode,
            Email = e.Email
        };
    }
}