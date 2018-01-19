using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.ViewModels;

namespace WebApp.Areas.Calendar.Views.Calendar
{
    public class AvailabilityDate
    {
        public DateTime Day { get; set; }
        public AvailableDayViewModel AvailableDay { get; set; }
    }
}