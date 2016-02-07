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
    }
}