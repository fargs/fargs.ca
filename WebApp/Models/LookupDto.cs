using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class LookupDto<T>
    {
        public T Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ColorCode { get; set; }

        public static Expression<Func<ServiceRequestStatu, LookupDto<short>>> FromServiceRequestStatusEntity = e => e == null ? null : new LookupDto<short>
        {
            Id = e.Id,
            Name = e.Name,
            Code = e.Code,
            ColorCode = e.ColorCode
        };

        public static Expression<Func<Service, LookupDto<short>>> FromServiceEntity = e => e == null ? null : new LookupDto<short>
        {
            Id = e.Id,
            Name = e.Name,
            Code = e.Code,
            ColorCode = e.ColorCode
        };

        public static Expression<Func<Company, LookupDto<short>>> FromCompanyEntity = e => e == null ? null : new LookupDto<short>
        {
            Id = e.Id,
            Name = e.Name,
            Code = e.Code,
            ColorCode = string.Empty
        };

        public static Expression<Func<AspNetRole, LookupDto<Guid>>> FromAspNetRoleEntity = e => e == null ? null : new LookupDto<Guid>
        {
            Id = e.Id,
            Name = e.Name,
            Code = e.Name.Substring(0, 2),
            ColorCode = string.Empty
        };
    }
}