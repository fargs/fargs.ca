using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using LinqKit;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class TravelPriceDto
    {
        public Guid Id { get; set; }
        public short CityId { get; set; }
        public Guid CompanyServiceId { get; set; }
        public decimal Price { get; set; }
        
        public static Expression<Func<TravelPrice, TravelPriceDto>> FromTravelPriceEntity = c => new TravelPriceDto
        {
            Id = c.Id,
            CityId = c.CityId,
            CompanyServiceId = c.CompanyServiceId,
            Price = c.Price
        };
    }
}