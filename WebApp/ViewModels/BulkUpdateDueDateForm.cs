using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.ViewModels.ServiceRequestTaskViewModels;

namespace WebApp.ViewModels
{
    public class BulkUpdateDueDateForm
    {
        public BulkUpdateDueDateForm()
        {
            Tasks = new List<BulkUpdateDueDateViewModel>();
        }
        public int ServiceRequestId { get; set; }
        public List<BulkUpdateDueDateViewModel> Tasks { get; set; }
    }
}