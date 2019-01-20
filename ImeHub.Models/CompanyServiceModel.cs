using LinqKit;
using ImeHub.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ImeHub.Models
{
    public class CompanyServiceModel
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid? ServiceId { get; set; }
        public ServiceModel Service { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsTravelRequired { get; set; }
        //public IEnumerable<TravelPriceModel> TravelPrices { get; set; }

        public static Expression<Func<CompanyService, CompanyServiceModel>> FromCompanyServiceEntity = c => new CompanyServiceModel
        {
            Id = c.Id,
            CompanyId = c.CompanyId,
            ServiceId = c.ServiceId,
            Service = ServiceModel.FromServiceEntity.Invoke(c.Service),
            Name = c.Name,
            Price = c.Price.Value,
            IsTravelRequired = c.IsTravelRequired,
            //TravelPrices = c.TravelPrices.AsQueryable().Select(TravelPriceModel.FromTravelPriceEntity.Expand()),
        };
    }
}