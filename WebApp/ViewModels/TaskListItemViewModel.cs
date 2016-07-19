using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.ViewModels
{
    public class TaskListItemViewModel
    {
        public ServiceRequestTask Task { get; set; }
        public byte Indentation { get; set; }
        public string RowId { get; set; }
        public DateTime Now { get; set; }
    }
}