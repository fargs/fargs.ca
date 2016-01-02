using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

namespace WebApp.Areas.Admin.ViewModels.AvailabilityViewModels
{
    public class IndexViewModel
    {
        public Physician Physician { get; set; }
        public List<FullCalendarEvent> AvailableDays { get; set; }
    }

    public class FullCalendarEvent
    {
        public string title { get; set; }
        public string start { get; set; }
    }
}