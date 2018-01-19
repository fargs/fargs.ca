using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentDateTime;
using WebApp.ViewModels;

namespace WebApp.Areas.Calendar.Views.Calendar
{
    public class Index
    {
        public AvailabilityDateRange AvailabilityDateRange { get; set; }
        public Index(AvailabilityDateRange dateRange)
        {
            AvailabilityDateRange = dateRange;
        }
    }
}