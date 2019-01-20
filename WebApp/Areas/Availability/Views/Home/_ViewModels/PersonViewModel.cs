using LinqKit;
using System;
using System.Linq.Expressions;
using WebApp.Views.Shared;
using ImeHub.Models;

namespace WebApp.Areas.Availability.Views.Home
{
    public class UserViewModel : LookupViewModel<Guid>
    {
        public LookupViewModel<Guid> Role { get; set; }

        public static Func<UserModel, UserViewModel> FromUserModel = e => e == null ? null : new UserViewModel
        {
            Id = e.Id,
            Name = e.DisplayName,
            Code = e.Initials,
            ColorCode = e.ColorCode,
            Role = new LookupViewModel<Guid>()
            {
                Id = e.RoleId,
                Name = e.Role.Name,
                Code = e.Role.Code,
                ColorCode = e.Role.ColorCode
            }
        };
    }
}