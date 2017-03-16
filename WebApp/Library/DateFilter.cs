using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Library
{
    public class DateFilter
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateFilterType FilterType { get; set; }
        
    }

    public enum DateFilterType
    {
        On, Before, OnOrBefore, After, OnOrAfter, Between
    }
}