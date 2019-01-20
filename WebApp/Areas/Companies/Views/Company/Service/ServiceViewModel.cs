using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImeHub.Models;

namespace WebApp.Areas.Companies.Views.Company
{
    public class ServiceViewModel
    {
        public ServiceViewModel(ServiceModel service)
        {

            Id = service.Id;
            CompanyId = service.CompanyId;
            Name = service.Name;
            Price =  service.Price.ToString("C2");
            IsTravelRequired = service.IsTravelRequired;
            TravelPrices = service.TravelPrices.Select(tp => new TravelPriceViewModel(tp));
        }
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public bool IsTravelRequired { get; set; }
        public IEnumerable<TravelPriceViewModel> TravelPrices { get; set; }
    }
}