using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Library.Extensions;
using WebApp.Views.Shared;
using FluentDateTime;
using Orvosi.Data;
using System.Security.Principal;
using Orvosi.Data.Filters;
using WebApp.Models;

namespace WebApp.Areas.Work.Views.Schedule
{
    public class IndexViewModel
    {
        public IndexViewModel(ScheduleViewModel weekList)
        {
            WeekList = weekList;
        }
        public ScheduleViewModel WeekList { get; set; }

    }
}