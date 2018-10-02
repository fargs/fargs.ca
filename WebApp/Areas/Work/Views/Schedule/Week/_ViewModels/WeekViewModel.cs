using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using FluentDateTime;
using LinqKit;
using Orvosi.Data;
using Orvosi.Data.Filters;
using WebApp.Library.Extensions;
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Work.Views.Schedule
{
    public class WeekViewModel : ViewModelBase
    {
        public WeekViewModel(IEnumerable<ServiceRequestDto> serviceRequests) : base()
        {
        }
        public WeekViewModel(OrvosiDbContext db, IIdentity identity, DateTime now, DateTime firstDayOfWeek) : base(identity, now)
        {
            Id = firstDayOfWeek.Ticks;
            FirstDayOfWeek = firstDayOfWeek.ToOrvosiDateFormat();

            var serviceRequests = db.ServiceRequests
                .AsNoTracking()
                .AsExpandable()
                .HaveAppointment()
                .AreScheduledBetween(firstDayOfWeek, firstDayOfWeek.AddDays(7))
                .CanAccess(LoggedInUserId, PhysicianId, LoggedInRoleId)
                .AreNotClosed()
                .Select(ServiceRequestDto.FromServiceRequestEntityForSchedule(LoggedInUserId))
                .ToList();

            var appointments = serviceRequests
                .Select(AppointmentViewModel.FromServiceRequestDto);

            Days = appointments
                .OrderBy(a => a.AppointmentDate).ThenBy(a => a.StartTime)
                .GroupBy(c => c.AppointmentDate)
                .Select(DayViewModel.FromServiceRequestDtoGrouping);

        }
        public long Id { get; set; }
        public string FirstDayOfWeek { get; }
        public IEnumerable<DayViewModel> Days { get; set; }
        
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