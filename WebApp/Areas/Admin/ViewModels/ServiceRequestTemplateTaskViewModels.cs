using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.ViewModels.ServiceRequestTemplateTaskViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<ServiceRequestTemplateTask> Tasks { get; set; }
    }
}