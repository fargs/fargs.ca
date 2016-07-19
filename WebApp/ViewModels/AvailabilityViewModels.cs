using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.ViewModels.AvailabilityViewModels
{
    public class IndexViewModel
    {
        public Orvosi.Data.AspNetUser CurrentUser { get; set; }
        public Orvosi.Data.AspNetUser SelectedUser { get; set; }
        public IEnumerable<Orvosi.Data.AvailableDay> AvailableDays { get; set; }
        public System.Globalization.Calendar Calendar { get; set; }
        public DateTime Today { get; internal set; }
        public Orvosi.Data.AvailableDay NewAvailableDay { get; set; }
        public FilterArgs FilterArgs { get; set; }
    }

    public class FilterArgs
    {
        public Guid? PhysicianId { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public DateTime FilterDate { get; set; }
    }
}