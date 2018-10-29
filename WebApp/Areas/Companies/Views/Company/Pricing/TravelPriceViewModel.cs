using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Areas.Companies.Views.Company
{
    public class TravelPriceViewModel
    {
        public TravelPriceViewModel(TravelPriceDto travelPrice)
        {
            Id = travelPrice.Id;
            CompanyServiceId = travelPrice.CompanyServiceId;
            CityId = travelPrice.CityId;
            Price = travelPrice.Price.ToString("C2");
        }
        public Guid Id { get; set; }
        public short CityId { get; set; }
        public Guid CompanyServiceId { get; set; }
        public string Price { get; set; }
    }
}