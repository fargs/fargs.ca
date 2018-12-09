using ImeHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.Views.Shared
{
    public class ContactViewModel : PersonViewModel
    {
        public ContactViewModel()
        {
        }
        public ContactViewModel(ImeHub.Models.ContactModel model) : base(model)
        {
            Email = model.Email;
        }
        public string Email { get; set; }

        public static Func<ContactDto, ContactViewModel> FromContactDto = e => e == null ? null : new ContactViewModel
        {
            Id = e.Id,
            Name = e.DisplayName,
            Code = e.Initials,
            ColorCode = e.ColorCode,
            Email = e.Email
        };
        public static Func<ContactModel, ContactViewModel> FromContactModel = e => e == null ? null : new ContactViewModel
        {
            Id = e.Id,
            Name = e.DisplayName,
            Code = e.Initials,
            ColorCode = e.ColorCode,
            Email = e.Email
        };
    }
}