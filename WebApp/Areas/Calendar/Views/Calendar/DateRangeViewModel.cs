using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Calendar.Views.Calendar
{
    public class DateRangeViewModel
    {
        public IEnumerable<DateTime> DateRange { get; set; }
        public DateTime FirstDayOfStartWeek { get; internal set; }
        public DateTime FirstDayOfEndWeek { get; internal set; }
        public DateTime LastDayOfEndWeek { get; internal set; }
    }
}