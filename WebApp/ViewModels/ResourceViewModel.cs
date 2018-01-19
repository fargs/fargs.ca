using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class ResourceViewModel
    {
        public Guid Id { get; set; }
        public int ServiceRequestId { get; set; }
        public LookupViewModel<Guid> Person { get; set; }
        public LookupViewModel<Guid> Role { get; set; }

        public static Expression<Func<ResourceDto, ResourceViewModel>> FromResourceDto = dto => dto == null ? null : new ResourceViewModel
        {
            Id = dto.Id,
            ServiceRequestId = dto.ServiceRequestId,
            Person = LookupViewModel<Guid>.FromPersonDto.Invoke(dto.Person),
            Role = LookupViewModel<Guid>.FromLookupDto.Invoke(dto.Role)
        };
    }
}