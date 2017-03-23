using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace WebApp.ViewModels
{
    public class CalendarNavigationViewModel
    {
        public CalendarViewOptions ContentView { get; internal set; }
        public Dictionary<string, Uri> Links { get; set; }
        public DateTime SelectedDate { get; set; }
    }
}