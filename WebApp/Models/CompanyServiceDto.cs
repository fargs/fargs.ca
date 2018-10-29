using LinqKit;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class CompanyServiceDto
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid? ServiceId { get; set; }
        public ServiceV2Dto Service { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsTravelRequired { get; set; }
        public IEnumerable<TravelPriceDto> TravelPrices { get; set; }

        public static Expression<Func<CompanyService, CompanyServiceDto>> FromCompanyServiceEntity = c => new CompanyServiceDto
        {
            Id = c.Id,
            CompanyId = c.CompanyId,
            ServiceId = c.ServiceId,
            Service = ServiceV2Dto.FromServiceV2Entity.Invoke(c.ServiceV2),
            Name = c.Name,
            Price = c.Price.Value,
            IsTravelRequired = c.IsTravelRequired,
            TravelPrices = c.TravelPrices.AsQueryable().Select(TravelPriceDto.FromTravelPriceEntity.Expand()),
        };
    }
}