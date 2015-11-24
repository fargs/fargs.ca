using System.Collections.Generic;
using System.Web.Mvc;

namespace WebApp.ViewModels
{
    public class DashboardViewModel
    {
        public string UserDisplayName { get; set; }
        public string UserCompanyDisplayName  { get; set; }
        public string UserRoleName { get; set; }
        public string UserCompanyLogoCssClass { get; set; }
        public string SchedulingProcess { get; set; }
        public string BookingPageName { get; set; }
        public string Instructions { get; set; }
        public SelectList Physicians { get; set; }
        public SelectListItem Physician { get; set; }
        public SelectList Services { get; set; }
        public SelectListItem Service { get; set; }
        public string UserEmail { get; set; }
        public byte ActionState { get; set; }

        public DashboardViewModel()
        {
        }
    }

    public class Physician
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public IEnumerable<Service> ServiceCatalogue { get; set; }
        public Physician()
        {
            ServiceCatalogue = new List<Service>();
        }
    }

    public class Service
    {
        public short Id { get; set; }
        public string Name { get; set; }
    }
}