using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.ViewModels.ServiceRequestTaskViewModels
{
    public class IndexViewModel
    {
        public User User { get; set; }
        public List<ServiceRequestTask> Tasks { get; set; }
        public FilterArgs FilterArgs { get; set; }
    }

    public class FilterArgs
    {
        public string Sort { get; set; }
        public byte? DateRange { get; set; }
        public byte? StatusId { get; set; }
        public string Ids { get; set; }
        public string ClaimantName { get; set; }
        public string PhysicianId { get; set; }
        public bool? ShowAll { get; set; }
    }
}