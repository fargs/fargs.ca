using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApp.Models.ServiceRequestModels2;

namespace WebApp.ViewModels.DashboardViewModels
{
    public class DueDatesViewModel
    {
        public DueDatesViewModel()
        {
        }
        public IEnumerable<DueDateViewModel> DueDates { get; set; }
        public TaskFilterViewModel TaskFilterViewModel { get; set; }
    }
}