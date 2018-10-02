using System;
using System.Linq.Expressions;
using LinqKit;
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Views.Resources
{
    public class ResourceViewModel
    {
        public Guid Id { get; set; }
        public int ServiceRequestId { get; set; }
        public LookupViewModel<Guid> Person { get; set; }
        public LookupViewModel<Guid> Role { get; set; }

        public static Func<ResourceDto, ResourceViewModel> FromResourceDto = dto => new ResourceViewModel
        {
            Id = dto.Id,
            ServiceRequestId = dto.ServiceRequestId,
            Person = LookupViewModel<Guid>.FromPersonDtoExpr.Invoke(dto.Person),
            Role = LookupViewModel<Guid>.FromLookupDto.Invoke(dto.Role)
        };

        public static Expression<Func<ResourceDto, ResourceViewModel>> FromResourceDtoExpr = dto => dto == null ? null : FromResourceDto(dto);
    }
}