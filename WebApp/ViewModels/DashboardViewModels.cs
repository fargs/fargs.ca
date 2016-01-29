using Model;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebApp.ViewModels.DashboardViewModels
{
    public class IndexViewModel
    {
        public List<ServiceRequest> Today { get; set; }
        public List<ServiceRequest> Tomorrow { get; set; }
        public List<ServiceRequest> RestOfWeek { get; set; }
        public List<ServiceRequest> NextWeek { get; set; }
    }
}