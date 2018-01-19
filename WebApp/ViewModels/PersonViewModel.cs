using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class PersonViewModel : LookupViewModel<Guid>
    {
        public LookupViewModel<Guid> Role { get; set; }

        public static new Expression<Func<PersonDto, PersonViewModel>> FromPersonDto = e => e == null ? null : new PersonViewModel
        {
            Id = e.Id,
            Name = e.DisplayName,
            Code = e.Initials,
            ColorCode = e.ColorCode,
            Role = LookupViewModel<Guid>.FromLookupDto.Invoke(e.Role)
        };
    }
}