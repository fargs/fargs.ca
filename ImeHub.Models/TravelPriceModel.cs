using ImeHub.Data;
using System;
using System.Linq.Expressions;

namespace ImeHub.Models
{
    public class TravelPriceModel
    {
        public Guid Id { get; set; }
        public Guid CityId { get; set; }
        public Guid ServiceId { get; set; }
        public decimal Price { get; set; }
        
        public static Expression<Func<TravelPrice, TravelPriceModel>> FromTravelPrice = c => new TravelPriceModel
        {
            Id = c.Id,
            CityId = c.CityId,
            ServiceId = c.ServiceId,
            Price = c.Price
        };
    }
}