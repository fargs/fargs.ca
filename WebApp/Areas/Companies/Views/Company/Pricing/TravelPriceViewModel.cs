using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImeHub.Models;

namespace WebApp.Areas.Companies.Views.Company
{
    public class TravelPriceViewModel
    {
        public TravelPriceViewModel(TravelPriceModel travelPrice)
        {
            Id = travelPrice.Id;
            CompanyServiceId = travelPrice.ServiceId;
            CityId = travelPrice.CityId;
            Price = travelPrice.Price.ToString("C2");
        }
        public Guid Id { get; set; }
        public Guid CityId { get; set; }
        public Guid CompanyServiceId { get; set; }
        public string Price { get; set; }
    }
}