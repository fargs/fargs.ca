using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fargs.Web.Models
{
    public class Job
    {
        public DateTime StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public string Duration
        {
            get
            {
                Nullable<DateTime> endDate = EndDate;
                if (!EndDate.HasValue)
                {
                    endDate = DateTime.Today;
                }
                var totalDays = (endDate.Value - StartDate).Days;
                int years = totalDays / 365;
                int days = totalDays - (years * 365);
                string value = string.Empty;
                if (years == 0)
                {
                    value = string.Format("{0} days{1}", days, !EndDate.HasValue ? " and counting!" : "");
                }
                else
                {
                    value = string.Format("{0} year{1}, {2} days{3}", years, (years == 0) ? "" : "s", days, !EndDate.HasValue ? " and counting!" : "");
                }
                

                return value;
            }
        }
    }
}