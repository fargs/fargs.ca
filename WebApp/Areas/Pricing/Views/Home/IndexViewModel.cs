using LinqKit;
using Orvosi.Data;
using Orvosi.Data.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Pricing.Views.Home
{
    public class IndexViewModel
    {
        public IndexViewModel(ServiceCatalogueViewModel serviceCatalogue)
        {
            ServiceCatalogue = serviceCatalogue;
        }

        public ServiceCatalogueViewModel ServiceCatalogue { get; private set; }
    }
}