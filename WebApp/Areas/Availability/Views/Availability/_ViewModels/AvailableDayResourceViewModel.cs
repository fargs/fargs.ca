using ImeHub.Models;
using System;
using System.Linq.Expressions;
using WebApp.Views.Shared;

namespace WebApp.Areas.Availability.Views.Home
{
    public class AvailableDayResourceViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public UserViewModel User { get; set; }
        public Guid? RoleId { get; set; }
        public LookupViewModel<Guid> Role { get; set; }
        public Guid AvailableDayId { get; set; }

        public static Expression<Func<AvailableDayResourceModel, AvailableDayResourceViewModel>> FromAvailableDayResourceModel = r => r == null ? null : new AvailableDayResourceViewModel
        {
            AvailableDayId = r.AvailableDayId,
            Id = r.Id,
            UserId = r.UserId,
            User = UserViewModel.FromUserModel(r.User)
        };
    }
}