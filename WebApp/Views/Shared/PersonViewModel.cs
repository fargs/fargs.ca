using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using ImeHub.Models;

namespace WebApp.Views.Shared
{
    public class PersonViewModel : LookupViewModel<Guid>
    {
        public LookupViewModel<Guid> Role { get; set; }

        public PersonViewModel()
        {
        }
        public PersonViewModel(ImeHub.Models.PersonModel model)
        {
            Id = model.Id;
            Name = model.DisplayName;
            Code = model.Initials;
            ColorCode = model.ColorCode;
        }

        public static Expression<Func<PersonModel, PersonViewModel>> FromPersonModel = e => e == null ? null : new PersonViewModel
        {
            Id = e.Id,
            Name = e.DisplayName,
            Code = e.Initials,
            ColorCode = e.ColorCode
        };
    }
}