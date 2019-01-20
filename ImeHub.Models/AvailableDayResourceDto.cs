using LinqKit;
using ImeHub.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ImeHub.Models
{
    public class AvailableDayResourceModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public UserModel User { get; set; }
        public Guid? RoleId { get; set; }
        public LookupModel<Guid> Role { get; set; }
        public Guid AvailableDayId { get; set; }

        public static Expression<Func<AvailableDayResource, AvailableDayResourceModel>> FromAvailableDayResource = r => r == null ? null : new AvailableDayResourceModel
        {
            AvailableDayId = r.Id,
            Id = r.Id,
            UserId = r.UserId
            //User = UserModel.FromUser.Invoke(r.User)
        };
    }
}