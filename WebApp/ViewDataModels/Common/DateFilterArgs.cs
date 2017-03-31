using FluentDateTime;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.ViewDataModels
{
    public class DateFilterArgs
    {
        public DateTime StartDate { get; set; }
        public DateTime? EndDate {
            get
            {
                switch (FilterType)
                {
                    case DateFilterType.On:
                        return StartDate;
                    case DateFilterType.Between:
                        return EndDate;
                    case DateFilterType.Week:
                        return StartDate.LastDayOfWeek();
                    case DateFilterType.Month:
                        return StartDate.LastDayOfMonth();
                    default:
                        return null;
                }
            }
        }
        public DateFilterType FilterType { get; set; } = DateFilterType.On;
        
    }

    public enum DateFilterType
    {
        On, Before, OnOrBefore, After, OnOrAfter, Between, Week, Month
    }
}