using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Views.Calendar;

namespace WebApp.Areas.Work.Views.DaySheet
{
    public class IndexViewModel
    {
        public IndexViewModel(CalendarNavigationViewModel calendarNavigation, DaySheetViewModel daySheet)
        {
            CalendarNavigation = calendarNavigation;
            DaySheet = daySheet;
        }

        public CalendarNavigationViewModel CalendarNavigation { get; private set; }
        public DaySheetViewModel DaySheet { get; private set; }
    }
}