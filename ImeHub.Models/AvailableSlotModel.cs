using LinqKit;
using ImeHub.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using ImeHub.Models.Extensions;

namespace ImeHub.Models
{
    public class AvailableSlotModel
    {
        public AvailableSlotModel()
        {
            ServiceRequests = new List<ServiceRequestModel>();
        }
        public Guid Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public short? Duration { get; set; }
        public IEnumerable<ServiceRequestModel> ServiceRequests { get; set; }
        public AvailableDayModel AvailableDay { get; set; }

        public IEnumerable<Guid> ServiceRequestIds { get
            {
                return ServiceRequests.Where(sr => sr.CancellationStatusId != Enums.CancellationStatus.Cancellation).Select(sr => sr.Id);
            }
        }
        public bool IsAvailable(IEnumerable<ServiceRequestModel> serviceRequests)
        {
            return !serviceRequests.Any() || serviceRequests.All(sr => sr.CancellationStatusId == Enums.CancellationStatus.Cancellation); // this includes cancelled and late cancelled
        }
        public string DisplayName(IEnumerable<ServiceRequestModel> serviceRequests, TimeSpan startTime)
        {
            string text = startTime.ToShortTimeSafe();
            if (IsAvailable(serviceRequests))
            {
                return text;
            }
            else
            {
                return text + " - " + serviceRequests.Where(sr => sr.CancellationStatusId != Enums.CancellationStatus.Cancellation).FirstOrDefault().ClaimantName + " - " + serviceRequests.FirstOrDefault().Id.ToString();
            }
        }

        public static Expression<Func<AvailableSlot, AvailableSlotModel>> FromAvailableSlot = e => e == null ? null : new AvailableSlotModel
        {
            Id = e.Id,
            StartTime = e.StartTime,
            EndTime = e.EndTime,
            Duration = e.Duration,
            ServiceRequests = e.ServiceRequests.AsQueryable().Select(ServiceRequestModel.FromServiceRequestForAvailability)
        };

        public static Expression<Func<AvailableSlot, AvailableSlotModel>> FromAvailableSlotForReschedule = e => e == null ? null : new AvailableSlotModel
        {
            Id = e.Id,
            StartTime = e.StartTime,
            EndTime = e.EndTime,
            Duration = e.Duration
            //ServiceRequests = e.ServiceRequests.Select(sr => new ServiceRequestDto
            //{
            //    Id = sr.Id,
            //    ClaimantName = sr.ClaimantName,
            //    CancelledDate = sr.CancelledDate,
            //    IsLateCancellation = sr.IsLateCancellation,
            //    IsNoShow = sr.IsNoShow,
            //    ServiceRequestStatusId = sr.ServiceRequestStatusId
            //})
        };

        public static Expression<Func<AvailableSlot, AvailableSlotModel>> FromAvailableSlotEntityForDayView = e => e == null ? null : new AvailableSlotModel
        {
            Id = e.Id,
            StartTime = e.StartTime,
            EndTime = e.EndTime,
            Duration = e.Duration,
            ServiceRequests = e.ServiceRequests.Select(sr => new ServiceRequestModel
            {
                Id = sr.Id,
                ClaimantName = sr.ClaimantName,
                CancellationStatusId = (Enums.CancellationStatus)sr.CancellationStatusId,
                StatusId = (Enums.ServiceRequestStatus)sr.StatusId
            })
        };

        // include AvailableDay
        public static Expression<Func<AvailableSlot, AvailableSlotModel>> FromAvailableSlotEntityForBooking = e => e == null ? null : new AvailableSlotModel
        {
            Id = e.Id,
            StartTime = e.StartTime,
            EndTime = e.EndTime,
            Duration = e.Duration,
            ServiceRequests = e.ServiceRequests.Select(sr => new ServiceRequestModel
            {
                Id = sr.Id,
                ClaimantName = sr.ClaimantName,
                CancellationStatusId = (Enums.CancellationStatus)sr.CancellationStatusId,
                StatusId = (Enums.ServiceRequestStatus)sr.StatusId
            }),
            AvailableDay = AvailableDayModel.FromAvailableDayEntityForBooking.Invoke(e.AvailableDay)
        };
    }
}