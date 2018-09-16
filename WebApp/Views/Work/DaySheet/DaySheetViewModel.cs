using LinqKit;
using Orvosi.Data;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WebApp.Library.Extensions;
using WebApp.Models;
using Orvosi.Data.Filters;
using WebApp.Views.Shared;
using WebApp.Views.Work.DaySheet.ServiceRequest;

namespace WebApp.Views.Work.DaySheet
{
    public partial class DaySheetViewModel
    {
        private DaySheetViewModel() { }
        public DaySheetViewModel(DateTime selectedDate, OrvosiDbContext db, Guid loggedInUserId, Guid? physicianId, Guid loggedInRoleId)
        {
            var serviceRequests = db.ServiceRequests
                .AsExpandable()
                .AreScheduledThisDay(selectedDate)
                .AreNotCancellations()
                .CanAccess(loggedInUserId, physicianId, loggedInRoleId)
                .Select(ServiceRequestDto.FromServiceRequestEntityForDaySheet(loggedInUserId))
                .OrderBy(sr => sr.AppointmentDate).ThenBy(sr => sr.StartTime)
                .AsEnumerable();

            Day = selectedDate;
            DayName = selectedDate.ToOrvosiLongDateFormat();
            Companies = serviceRequests.Select(sr => sr.Company == null ? "No company" : sr.Company.Name).Distinct();
            Addresses = serviceRequests.Select(sr => sr.Address == null ? "No address" : sr.Address.City).Distinct().ToArray();
            ServiceRequests = serviceRequests.Select(ServiceRequestViewModel.FromServiceRequestDto);
        }

        public DateTime Day { get; set; }
        public string DayName { get; set; }
        public IEnumerable<string> Addresses { get; set; }
        public IEnumerable<string> Companies { get; set; }
        public IEnumerable<ServiceRequestViewModel> ServiceRequests { get; set; }

        public static Expression<Func<IGrouping<DateTime, ServiceRequestDto>, DaySheetViewModel>> FromServiceRequestDtoGroupingForDaySheet = dto => dto == null ? null : new DaySheetViewModel
        {
            Day = dto.Key,
            DayName = dto.Key.ToOrvosiLongDateFormat(),
            Companies = dto.Select(sr => sr.Company == null ? "No company" : sr.Company.Name).Distinct(),
            Addresses = dto.Select(sr => sr.Address == null ? "No address" : sr.Address.City).Distinct().ToArray(),
            ServiceRequests = dto.Select(ServiceRequestViewModel.FromServiceRequestDto)
        };
    }
}