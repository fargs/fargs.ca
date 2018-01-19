using LinqKit;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class ResourceDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public ContactDto Person { get; set; }
        public Guid? RoleId { get; set; }
        public LookupDto<Guid> Role { get; set; }
        public int ServiceRequestId { get; set; }

        public static Expression<Func<ServiceRequestResource, ResourceDto>> FromServiceRequestResourceEntity = r => r == null ? null : new ResourceDto
        {
            ServiceRequestId = r.ServiceRequestId,
            Id = r.Id,
            RoleId = r.RoleId,
            Role = LookupDto<Guid>.FromAspNetRoleEntity.Invoke(r.AspNetRole),
            UserId = r.UserId,
            Person = ContactDto.FromAspNetUserEntity.Invoke(r.AspNetUser)
        };
    }
}