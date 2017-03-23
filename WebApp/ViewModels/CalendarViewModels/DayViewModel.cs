using LinqKit;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WebApp.Library.Extensions;
using WebApp.Models;

namespace WebApp.ViewModels.CalendarViewModels
{
    public class DayViewModel
    {
        public DateTime Day { get; set; }
        public string DayName { get; set; }
        public IEnumerable<string> Addresses { get; set; }
        public IEnumerable<string> Companies { get; set; }
        public IEnumerable<CaseViewModel> Cases { get; set; }

        public static Expression<Func<IGrouping<DateTime, CaseViewModel>, DayViewModel>> FromServiceRequestGroupingDto = dto => dto == null ? null : new DayViewModel
        {
            Day = dto.Key,
            DayName = dto.Key.ToOrvosiLongDateFormat(),
            Companies = dto.Select(sr => sr.Company == null ? "No company" : sr.Company.Name).Distinct(),
            Addresses = dto.Select(sr => sr.Address == null ? "No address" : sr.Address.City).Distinct().ToArray(),
            Cases = dto
        };
    }
}