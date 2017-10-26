using LinqKit;
using Orvosi.Data;
using WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Library.Extensions;

namespace WebApp.ViewModels
{
    public class AvailableSlotViewModel
    {
        public AvailableSlotViewModel()
        {
            ServiceRequestIds = new List<int>();
        }
        public short Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public short? Duration { get; set; }
        public IEnumerable<int> ServiceRequestIds { get; set; }
        public IEnumerable<CaseViewModel> ServiceRequests { get; set; }

        public bool IsAvailable { get; set; }

        public string DisplayName { get; set; }

        public static Expression<Func<AvailableSlotDto, AvailableSlotViewModel>> FromAvailableSlotDto = e => e == null ? null : new AvailableSlotViewModel
        {
            Id = e.Id,
            StartTime = e.StartTime,
            EndTime = e.EndTime,
            Duration = e.Duration,
            IsAvailable = e.IsAvailable(e.ServiceRequests),
            DisplayName = e.DisplayName(e.ServiceRequests, e.StartTime),
            ServiceRequestIds = e.ServiceRequestIds,
            ServiceRequests = e.ServiceRequests.AsQueryable().Select(CaseViewModel.FromServiceRequestDto.Expand())
        };
        
    }
}