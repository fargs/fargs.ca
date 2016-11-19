using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebApp.ViewModels.DashboardViewModels
{
    public class ScheduleViewModel
    {
        public ScheduleViewModel()
        {
        }
        public IEnumerable<Orvosi.Shared.Model.WeekFolder> WeekFolders { get; set; }
        public Guid SelectedUserId { get; set; }
        public List<SelectListItem> UserSelectList { get; set; }
        public ServiceRequestMessageJSViewModel ServiceRequestMessageJSViewModel { get; set; }
        public TaskFilterViewModel TaskFilterViewModel { get; set; }
    }
}