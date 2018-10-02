using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Library.Extensions;
using WebApp.Views.Shared;
using FluentDateTime;

namespace WebApp.Areas.Work.Views.Schedule
{
    public class WeekSummaryViewModel
    {
        public WeekSummaryViewModel(DateTime firstDayOfWeek, DateTime now)
        {
            Id = firstDayOfWeek.Ticks;
            FirstDayOfWeek = firstDayOfWeek.ToOrvosiDateFormat();
            Css = GetStyle(now, firstDayOfWeek);
        }
        public long Id { get; set; }
        public string FirstDayOfWeek { get; set; }
        public string Css { get; set; }
        public int OpenCount { get; set; }
        public int ToDoCount { get; set; }
        public int WaitingCount { get; set; }
        public int OnHoldCount { get; set; }
        public int DoneCount { get; set; }

        private string GetStyle(DateTime now, DateTime firstDayOfWeek)
        {
            if (firstDayOfWeek < now.FirstDayOfWeek())
            {
                return "panel-danger";
            }
            else if (firstDayOfWeek > now.LastDayOfWeek())
            {
                return "panel-default";
            }
            else
            {
                return "panel-success";
            }
        }

    }
}