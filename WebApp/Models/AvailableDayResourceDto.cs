using LinqKit;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class AvailableDayResourceDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public PersonDto Person { get; set; }
        public Guid? RoleId { get; set; }
        public LookupDto<Guid> Role { get; set; }
        public int AvailableDayId { get; set; }

        public static Expression<Func<AvailableDayResource, AvailableDayResourceDto>> FromAvailableDayResourceEntity = r => r == null ? null : new AvailableDayResourceDto
        {
            AvailableDayId = r.AvailableDayId,
            Id = r.Id,
            UserId = r.UserId,
            Person = PersonDto.FromAspNetUserEntityWithRole.Invoke(r.AspNetUser)
        };
    }
}