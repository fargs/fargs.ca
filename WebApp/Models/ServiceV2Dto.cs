using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class ServiceV2Dto : LookupDto<Guid>
    {
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool IsTravelRequired { get; set; }

        public static new Expression<Func<ServiceV2, ServiceV2Dto>> FromServiceV2Entity = s => s == null ? null : new ServiceV2Dto
        {
            Id = s.Id,
            Name = s.Name,
            Code = s.Code,
            ColorCode = s.ColorCode,
            Price = s.Price,
            Description = s.Description,
            IsTravelRequired = s.IsTravelRequired
        };
    }
}