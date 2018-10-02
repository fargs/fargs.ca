using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using FluentDateTime;
using LinqKit;
using Orvosi.Data;
using Orvosi.Data.Filters;
using Orvosi.Shared.Enums;
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Work.Views.Schedule
{
    public class ScheduleViewModel : ViewModelBase
    {
        public ScheduleViewModel(OrvosiDbContext db, IIdentity identity, DateTime now) : base(identity, now)
        {
            var serviceRequests = db.ServiceRequests
                .AsExpandable()
                .CanAccess(LoggedInUserId, PhysicianId, LoggedInRoleId)
                .AreNotClosed()
                .HaveAppointment()
                .Select(ServiceRequestDto.FromServiceRequestEntityForSchedule(LoggedInUserId))
                .ToList();

            var days = serviceRequests
                .GroupBy(srt => srt.AppointmentDate.Value)
                .Select(c => new 
                {
                    Id = c.Key.Ticks,
                    Day = c.Key,
                    OpenCount = c.Count(),
                    ToDoCount = c.Count(sr => sr.NextTaskStatusForUser == null ? false : sr.NextTaskStatusForUser.Id == TaskStatuses.ToDo),
                    WaitingCount = c.Count(sr => sr.NextTaskStatusForUser == null ? false : sr.NextTaskStatusForUser.Id == TaskStatuses.Waiting),
                    OnHoldCount = c.Count(sr => sr.NextTaskStatusForUser == null ? false : sr.NextTaskStatusForUser.Id == TaskStatuses.OnHold),
                    DoneCount = c.Count(sr => sr.NextTaskStatusForUser == null ? false : sr.NextTaskStatusForUser.Id == TaskStatuses.Done)  
                });

            WeekSummaries = days
                .GroupBy(d => d.Day.FirstDayOfWeek())
                .Select(weekGrp => new WeekSummaryViewModel(weekGrp.Key, now)
                {
                    OpenCount = weekGrp.Sum(day => day.OpenCount),
                    ToDoCount = weekGrp.Sum(day => day.ToDoCount),
                    WaitingCount = weekGrp.Sum(day => day.WaitingCount),
                    OnHoldCount = weekGrp.Sum(day => day.OnHoldCount),
                    DoneCount = weekGrp.Sum(day => day.DoneCount)
                });

        }
        public IEnumerable<WeekSummaryViewModel> WeekSummaries { get; set; }
    }
}