using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.ViewModels.AvailabilityViewModels
{
    public class IndexViewModel
    {
        public User CurrentUser { get; set; }
        public User SelectedUser { get; set; }
        public List<AvailableDay> AvailableDays { get; set; }
        public System.Globalization.Calendar Calendar { get; set; }
        public DateTime Today { get; internal set; }
        public AvailableDay NewAvailableDay { get; set; }
        public FilterArgs FilterArgs { get; set; }
    }

    public class FilterArgs
    {
        public string PhysicianId { get; set; }
        public int? Year{ get; set; }
        public int? Month{ get; set; }
    }
}