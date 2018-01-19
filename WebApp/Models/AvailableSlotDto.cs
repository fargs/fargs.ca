using LinqKit;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Library.Extensions;

namespace WebApp.Models
{
    public class AvailableSlotDto
    {
        public AvailableSlotDto()
        {
            ServiceRequestIds = new List<int>();
        }
        public short Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public short? Duration { get; set; }
        public IEnumerable<int> ServiceRequestIds { get; set; }
        public IEnumerable<ServiceRequestDto> ServiceRequests { get; set; }
        public AvailableDayDto AvailableDay { get; set; }

        public bool IsAvailable(IEnumerable<ServiceRequestDto> serviceRequests)
        {
            return !serviceRequests.Any() || serviceRequests.All(sr => sr.CancelledDate.HasValue); // this includes cancelled and late cancelled
        }
        public string DisplayName(IEnumerable<ServiceRequestDto> serviceRequests, TimeSpan startTime)
        {
            string text = startTime.ToShortTimeSafe();
            if (IsAvailable(serviceRequests))
            {
                return text;
            }
            else
            {
                return text + " - " + serviceRequests.Where(sr => !sr.CancelledDate.HasValue).FirstOrDefault().ClaimantName + " - " + serviceRequests.FirstOrDefault().Id.ToString();
            }
        }

        public static Expression<Func<AvailableSlot, AvailableSlotDto>> FromAvailableSlotEntity = e => e == null ? null : new AvailableSlotDto
        {
            Id = e.Id,
            StartTime = e.StartTime,
            EndTime = e.EndTime,
            Duration = e.Duration,
            ServiceRequestIds = e.ServiceRequests.AsQueryable().Where(sr => !sr.CancelledDate.HasValue).Select(sr => sr.Id),
            ServiceRequests = e.ServiceRequests.AsQueryable().Select(ServiceRequestDto.FromEntityForAvailability.Expand())
        };

        public static Expression<Func<AvailableSlot, AvailableSlotDto>> FromAvailableSlotEntityForReschedule = e => e == null ? null : new AvailableSlotDto
        {
            Id = e.Id,
            StartTime = e.StartTime,
            EndTime = e.EndTime,
            Duration = e.Duration,
            ServiceRequestIds = e.ServiceRequests.AsQueryable().Where(sr => !sr.CancelledDate.HasValue).Select(sr => sr.Id),
            ServiceRequests = e.ServiceRequests.Select(sr => new ServiceRequestDto
            {
                Id = sr.Id,
                ClaimantName = sr.ClaimantName,
                CancelledDate = sr.CancelledDate,
                IsLateCancellation = sr.IsLateCancellation,
                IsNoShow = sr.IsNoShow,
                ServiceRequestStatusId = sr.ServiceRequestStatusId
            })
        };

        public static Expression<Func<AvailableSlot, AvailableSlotDto>> FromAvailableSlotEntityForDayView = e => e == null ? null : new AvailableSlotDto
        {
            Id = e.Id,
            StartTime = e.StartTime,
            EndTime = e.EndTime,
            Duration = e.Duration,
            ServiceRequestIds = e.ServiceRequests.AsQueryable().Where(sr => !sr.CancelledDate.HasValue).Select(sr => sr.Id),
            ServiceRequests = e.ServiceRequests.Select(sr => new ServiceRequestDto
            {
                Id = sr.Id,
                ClaimantName = sr.ClaimantName,
                CancelledDate = sr.CancelledDate,
                IsLateCancellation = sr.IsLateCancellation,
                IsNoShow = sr.IsNoShow,
                ServiceRequestStatusId = sr.ServiceRequestStatusId
            })
        };

        // include AvailableDay
        public static Expression<Func<AvailableSlot, AvailableSlotDto>> FromAvailableSlotEntityForBooking = e => e == null ? null : new AvailableSlotDto
        {
            Id = e.Id,
            StartTime = e.StartTime,
            EndTime = e.EndTime,
            Duration = e.Duration,
            ServiceRequestIds = e.ServiceRequests.AsQueryable().Where(sr => !sr.CancelledDate.HasValue).Select(sr => sr.Id),
            AvailableDay = AvailableDayDto.FromAvailableDayEntityForBooking.Invoke(e.AvailableDay)
        };
    }
}