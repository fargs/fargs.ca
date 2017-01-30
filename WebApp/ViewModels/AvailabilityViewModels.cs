using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.ViewModels.AvailabilityViewModels
{
    public class IndexViewModel
    {

        public List<MonthGroup> Months { get; set; }
        public System.Globalization.Calendar Calendar { get; set; }
        public DateTime Today { get; internal set; }
        public Orvosi.Data.AvailableDay NewAvailableDay { get; set; }
        public FilterArgs FilterArgs { get; set; }
    }

    public class FilterArgs
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
    }

    public class MonthGroup    {
        public DateTime Month { get; set; }
        public IEnumerable<Orvosi.Data.AvailableDay> AvailableDays { get; set; }
    }
}