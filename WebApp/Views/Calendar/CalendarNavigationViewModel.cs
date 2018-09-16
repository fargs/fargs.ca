using FluentDateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Library.Extensions;

namespace WebApp.Views.Calendar
{
    public class CalendarNavigationViewModel
    {
        public CalendarNavigationViewModel(DateTime? selectedDate, DateTime now, HttpRequestBase request, CalendarViewOptions viewOptions = CalendarViewOptions.Day)
        {
            Links = new Dictionary<string, Uri>();
            SelectedDate = selectedDate.GetValueOrDefault(now).Date;
            ViewOptions = viewOptions;

            Links.Add("Previous", request.Url.AddQuery("SelectedDate", GetPreviousDate(SelectedDate, ViewOptions)));
            Links.Add("Next", request.Url.AddQuery("SelectedDate", GetNextDate(SelectedDate, ViewOptions)));
            Links.Add("Now", request.Url.AddQuery("SelectedDate", now.ToOrvosiDateFormat()));

            Links.Add("Year", request.Url.AddQuery("ContentView", "Year"));
            Links.Add("Month", request.Url.AddQuery("ContentView", "Month"));
            Links.Add("Week", request.Url.AddQuery("ContentView", "Week"));
            Links.Add("Day", request.Url.AddQuery("ContentView", "Day"));
            Links.Add("Today", request.Url.AddQuery("ContentView", "Day"));

        }
        public CalendarViewOptions ViewOptions { get; internal set; }
        public Dictionary<string, Uri> Links { get; set; }
        public DateTime SelectedDate { get; private set; }


        private string GetPreviousDate(DateTime selectedDate, CalendarViewOptions contentView)
        {
            DateTime result;
            switch (contentView)
            {
                case CalendarViewOptions.Year:
                    result = selectedDate.PreviousYear();
                    break;
                case CalendarViewOptions.Month:
                    result = selectedDate.PreviousMonth();
                    break;
                case CalendarViewOptions.Week:
                    result = selectedDate.FirstDayOfWeek().Previous(DayOfWeek.Sunday);
                    break;
                case CalendarViewOptions.Day:
                    result = selectedDate.AddDays(-1);
                    break;
                default:
                    throw new NotSupportedException();
            }
            return result.ToOrvosiDateFormat();
        }

        private string GetNextDate(DateTime selectedDate, CalendarViewOptions contentView)
        {
            DateTime result;
            switch (contentView)
            {
                case CalendarViewOptions.Year:
                    result = selectedDate.NextYear();
                    break;
                case CalendarViewOptions.Month:
                    result = selectedDate.NextMonth();
                    break;
                case CalendarViewOptions.Week:
                    result = selectedDate.Next(DayOfWeek.Sunday);
                    break;
                case CalendarViewOptions.Day:
                    result = selectedDate.AddDays(1);
                    break;
                default:
                    throw new NotSupportedException();
            }
            return result.ToOrvosiDateFormat();
        }
    }
}