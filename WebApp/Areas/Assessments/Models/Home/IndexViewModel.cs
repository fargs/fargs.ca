using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

namespace WebApp.Areas.Assessments.Models.Home
{
    public class IndexViewModel
    {
        public IQueryable<fn_Weekdays_Result> Weekdays { get; set; }
        public IQueryable<fn_Timeframe_Result> Years { get; internal set; }
    }
}