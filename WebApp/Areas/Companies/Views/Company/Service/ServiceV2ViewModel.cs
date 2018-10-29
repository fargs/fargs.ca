using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Areas.Companies.Views.Company
{
    public class ServiceV2ViewModel
    {
        public ServiceV2ViewModel(ServiceV2Dto service)
        {

            Id = service.Id;
            Name = service.Name;
            Price = service.Price.ToString("C2");
            IsTravelRequired = service.IsTravelRequired;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public bool IsTravelRequired { get; set; }
    }
}