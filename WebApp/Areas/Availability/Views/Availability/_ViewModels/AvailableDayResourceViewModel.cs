using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.Areas.Availability.Views.Home
{
    public class AvailableDayResourceViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public PersonViewModel Person { get; set; }
        public Guid? RoleId { get; set; }
        public LookupDto<Guid> Role { get; set; }
        public int AvailableDayId { get; set; }

        public static Expression<Func<AvailableDayResourceDto, AvailableDayResourceViewModel>> FromAvailableDayResourceDto = r => r == null ? null : new AvailableDayResourceViewModel
        {
            AvailableDayId = r.AvailableDayId,
            Id = r.Id,
            UserId = r.UserId,
            Person = PersonViewModel.FromPersonDto.Invoke(r.Person)
        };
    }
}