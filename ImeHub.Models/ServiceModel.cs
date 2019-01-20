using ImeHub.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ImeHub.Models
{
    public class ServiceModel : LookupModel<Guid>
    {
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool IsTravelRequired { get; set; }
        public Guid CompanyId { get; set; }
        public IEnumerable<TravelPriceModel> TravelPrices { get; set; }

        public static Expression<Func<Service, ServiceModel>> FromServiceEntity = s => s == null ? null : new ServiceModel
        {
            Id = s.Id,
            Name = s.Name,
            Code = s.Code,
            ColorCode = s.ColorCode,
            Price = s.Price,
            Description = s.Description,
            IsTravelRequired = s.IsTravelRequired,
            CompanyId = s.CompanyId,
            TravelPrices = s.TravelPrices.AsQueryable().Select(TravelPriceModel.FromTravelPrice)
        };
    }
}