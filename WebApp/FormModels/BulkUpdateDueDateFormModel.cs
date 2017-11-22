using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.FormModels
{
    public class BulkUpdateDueDateFormModel
    {
        public int ServiceRequestTaskId { get; set; }
        public DateTime? NewDueDate { get; set; }
    }
}