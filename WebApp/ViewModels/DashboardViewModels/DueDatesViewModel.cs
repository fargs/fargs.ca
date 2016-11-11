using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebApp.Models.ServiceRequestModels2;

namespace WebApp.ViewModels.DashboardViewModels
{
    public class DueDatesViewModel
    {
        public DueDatesViewModel()
        {
        }
        public IEnumerable<DueDateDayFolder> DueDates { get; set; }
        public Guid SelectedUserId { get; set; }
        public List<SelectListItem> UserSelectList { get; internal set; }
    }
}