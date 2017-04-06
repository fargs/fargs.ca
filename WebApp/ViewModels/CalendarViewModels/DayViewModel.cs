using LinqKit;
using Orvosi.Data;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WebApp.Library.Extensions;
using WebApp.Models;
using WebApp.ViewModels.DashboardViewModels;

namespace WebApp.ViewModels.CalendarViewModels
{
    public class DayViewModel
    {
        public DateTime Day { get; set; }
        public string DayName { get; set; }
        public IEnumerable<string> Addresses { get; set; }
        public IEnumerable<string> Companies { get; set; }
        public IEnumerable<CaseLinkViewModel> CaseLinks { get; set; }
        public IEnumerable<CaseViewModel> Cases { get; set; }
        public TaskStatusSummaryViewModel TaskStatusSummary { get; set; }

        public static Expression<Func<IGrouping<DateTime, ServiceRequestTask>, DayViewModel>> FromServiceRequestTaskEntityGrouping= dto => dto == null ? null : new DayViewModel
        {
            Day = dto.Key,
            TaskStatusSummary = TaskStatusSummaryViewModel.FromServiceRequestTaskEntityGrouping.Invoke(dto)
        };

        public static Expression<Func<IGrouping<DateTime, CaseViewModel>, DayViewModel>> FromServiceRequestDtoGroupingDtoForCases = dto => dto == null ? null : new DayViewModel
        {
            Day = dto.Key,
            DayName = dto.Key.ToOrvosiLongDateFormat(),
            Companies = dto.Select(sr => sr.Company == null ? "No company" : sr.Company.Name).Distinct(),
            Addresses = dto.Select(sr => sr.Address == null ? "No address" : sr.Address.City).Distinct().ToArray(),
            Cases = dto
        };

        public static Expression<Func<IGrouping<DateTime, CaseLinkViewModel>, DayViewModel>> FromServiceRequestDtoGroupingDtoForCaseLinks = dto => dto == null ? null : new DayViewModel
        {
            Day = dto.Key,
            DayName = dto.Key.ToOrvosiLongDateFormat(),
            Companies = dto.Select(sr => sr.Company == null ? "No company" : sr.Company.Name).Distinct(),
            Addresses = dto.Select(sr => sr.Address == null ? "No address" : sr.Address.City).Distinct().ToArray(),
            CaseLinks = dto
        };
    }
}