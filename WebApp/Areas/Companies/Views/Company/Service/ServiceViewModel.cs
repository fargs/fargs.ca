using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Areas.Companies.Views.Company
{
    public class ServiceViewModel
    {
        public ServiceViewModel(CompanyServiceDto service)
        {

            Id = service.Id;
            CompanyId = service.CompanyId;
            ServiceId = service.ServiceId;
            Service = service.ServiceId.HasValue ? new ServiceV2ViewModel(service.Service) : null;
            Name = service.Name;
            Price =  service.Price.ToString("C2");
            IsTravelRequired = service.IsTravelRequired;
            TravelPrices = service.TravelPrices.Select(tp => new TravelPriceViewModel(tp));
        }
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid? ServiceId { get; set; }
        public ServiceV2ViewModel Service { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public bool IsTravelRequired { get; set; }
        public IEnumerable<TravelPriceViewModel> TravelPrices { get; set; }
    }
}