using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebApp.ViewModels.CalendarViewModels;

namespace WebApp.ViewModels.WorkViewModels
{
    public class ScheduleViewModel
    {
        public ScheduleViewModel()
        {
        }
        public IEnumerable<DayViewModel> WeekFolders { get; set; }
        public Guid SelectedUserId { get; set; }
        public List<SelectListItem> UserSelectList { get; set; }
        public ServiceRequestMessageJSViewModel ServiceRequestMessageJSViewModel { get; set; }
        public TaskFilterViewModel TaskFilterViewModel { get; set; }
    }
}