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
        public IEnumerable<Orvosi.Shared.Model.DueDateDayFolder> DueDates { get; set; }
        public Guid SelectedUserId { get; set; }
        public List<SelectListItem> UserSelectList { get; set; }
        public ServiceRequestMessageJSViewModel ServiceRequestMessageJSViewModel { get; set; }
        public TaskFilterViewModel TaskFilterViewModel { get; set; }
    }
}