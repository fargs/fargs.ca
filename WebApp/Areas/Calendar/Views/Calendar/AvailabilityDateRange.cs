using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentDateTime;
using System.Globalization;
using WebApp.Library.Extensions;
using WebApp.ViewModels;
using MoreLinq;
using WebApp.Models;
using Orvosi.Data;
using LinqKit;

namespace WebApp.Areas.Calendar.Views.Calendar
{
    public class AvailabilityDateRange
    {
        private IEnumerable<IGrouping<DateTime, IGrouping<DateTime, AvailabilityDate>>> _weeks;
        private DateRangeViewModel _range;

        public AvailabilityDateRange(IEnumerable<IGrouping<DateTime, IGrouping<DateTime, AvailabilityDate>>> weeks,
            DateRangeViewModel dateRange)
        {
            _range = dateRange;
            _weeks = weeks;
        }
        
        public IEnumerable<IGrouping<DateTime, IGrouping<DateTime, AvailabilityDate>>> Weeks => _weeks;
        public DateRangeViewModel DateRange => _range;
    }
}