using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.ViewModels.ServiceCatalogueViewModels
{
    public class IndexViewModel
    {
        public Model.User CurrentUser { get; set; }
        public Model.User SelectedUser { get; set; }
        public Company SelectedCompany { get; set; }
        public List<GetServiceCatalogue_Result> ServiceCatalogues { get; set; }
        public FilterArgs FilterArgs { get; set; }
    }

    public class FilterArgs
    {
        public string UserId { get; set; }
        public short? CompanyId { get; set; }
    }
}