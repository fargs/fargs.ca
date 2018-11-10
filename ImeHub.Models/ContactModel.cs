using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data = ImeHub.Data;

namespace ImeHub.Models
{
    public class ContactModel : PersonModel
    {
        public string Email { get; set; }
        public string Phone { get; set; }

        public static new Expression<Func<Data.User, ContactModel>> FromUser = a => a == null ? null : new ContactModel
        {
            Id = a.Id,
            FirstName = a.FirstName,
            LastName = a.LastName,
            Title = a.Title,
            ColorCode = a.ColorCode,
            Email = a.Email,
            Phone = a.PhoneNumber
        };
    }
}
