using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.ViewModels.ServiceCatalogueViewModels
{
    public class IndexViewModel
    {
        public Physician Physician { get; set; }
        public List<Service> Services { get; set; }
        public List<LocationArea> LocationAreas { get; set; }
        public List<GetServiceCatalogue_Result> ServiceCatalogues { get; set; }
    }
}