using ImeHub.Models;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WebApp.Views.Shared;
using Enums = ImeHub.Models.Enums;

namespace WebApp.Areas.Availability.Views.Home
{
    public class AvailableSlotViewModel
    {
        public AvailableSlotViewModel()
        {
            ServiceRequestIds = new List<Guid>();
        }
        public Guid Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public short? Duration { get; set; }
        public IEnumerable<Guid> ServiceRequestIds { get; set; }
        public IEnumerable<AppointmentViewModel> ServiceRequests { get; set; }

        public bool IsAvailable { get; set; }
        public AvailableDayViewModel AvailableDay { get; set; }

        public string DisplayName { get; set; }

        public static Expression<Func<AvailableSlotModel, AvailableSlotViewModel>> FromAvailableSlotModel = e => e == null ? null : new AvailableSlotViewModel
        {
            Id = e.Id,
            StartTime = e.StartTime,
            EndTime = e.EndTime,
            Duration = e.Duration,
            IsAvailable = e.IsAvailable(e.ServiceRequests),
            DisplayName = e.DisplayName(e.ServiceRequests, e.StartTime),
            ServiceRequestIds = e.ServiceRequestIds,
            ServiceRequests = e.ServiceRequests.Select(sr => new AppointmentViewModel
            {
                ServiceRequestId = sr.Id,
                ClaimantName = sr.ClaimantName,
                CancellationStatusId = sr.CancellationStatusId,
                CancellationStatus = new LookupViewModel<Enums.CancellationStatus>
                {
                    Id = sr.CancellationStatusId,
                    Name = sr.CancellationStatus.Name,
                    Code = sr.CancellationStatus.Code,
                    ColorCode = sr.CancellationStatus.ColorCode
                }
            })
        };

        public static Expression<Func<AvailableSlotModel, AvailableSlotViewModel>> FromAvailableSlotModelForBooking = e => e == null ? null : new AvailableSlotViewModel
        {
            Id = e.Id,
            StartTime = e.StartTime,
            EndTime = e.EndTime,
            Duration = e.Duration,
            AvailableDay = AvailableDayViewModel.FromAvailableDayModelForBooking.Invoke(e.AvailableDay)
        };
    }
}