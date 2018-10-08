using FluentDateTime;
using System;
using System.Collections.Generic;
using System.Web;
using WebApp.Library.Extensions;

namespace WebApp.Areas.Availability.Views.Home
{
    public class CalendarNavigationViewModel
    {
        public CalendarNavigationViewModel(DateTime selectedDate, HttpRequestBase request, DateTime now)
        {
            SelectedDate = selectedDate.ToOrvosiDateFormat();
            SelectedMonth = selectedDate.ToString("MMMM yyyy");
            ThisMonth = now.ToString("MMMM yyyy");
            PreviousMonth = selectedDate.PreviousMonth().ToString("MMMM yyyy");
            NextMonth = selectedDate.NextMonth().ToString("MMMM yyyy");

            Links = new Dictionary<string, Uri>();
            Links.Add("Previous", request.Url.AddQuery("SelectedDate", PreviousMonth));
            Links.Add("Next", request.Url.AddQuery("SelectedDate", NextMonth));
            Links.Add("ThisMonth", request.Url.AddQuery("SelectedDate", ThisMonth));
        }
        public string SelectedMonth { get; private set; }
        public string ThisMonth { get; private set; }
        public string PreviousMonth { get; private set; }
        public string NextMonth { get; private set; }
        public Dictionary<string, Uri> Links { get; private set; }
        public string SelectedDate { get; private set; }
    }
}