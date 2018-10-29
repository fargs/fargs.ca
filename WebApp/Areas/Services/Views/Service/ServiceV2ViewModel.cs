using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Services.Views.Service
{
    public class ServiceV2ViewModel : LookupViewModel<Guid>
    {
        public ServiceV2ViewModel(ServiceV2Dto service)
        {
            Id = service.Id;
            Name = service.Name;
            Code = service.Code;
            ColorCode = service.ColorCode;
            Price = String.Format(service.Price.ToString(), "{0}:C2");
            Description = service.Description;
        }
        public string Price { get; set; }
        public string Description { get; set; }
    }
}