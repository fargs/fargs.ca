using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.ViewModels.ServiceRequestViewModels
{
    public class DetailsViewModel
    {
        public ServiceRequest ServiceRequest { get; set; }
        public List<ServiceRequestTask> ServiceRequestTasks{ get; set; }
    }
}