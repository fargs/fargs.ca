using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;
using WebApp.Library.Extensions;
using WebApp.Models;
using WebApp.Views.Address;
using WebApp.Views.Cancellation;
using WebApp.Views.Resources;
using WebApp.Views.Shared;
using Orvosi.Shared.Enums;

namespace WebApp.Areas.Work.Views.Schedule
{
    public class AppointmentViewModel
    { 
        public AppointmentViewModel()
        {
        }
        public int ServiceRequestId { get; set; }
        public string ClaimantName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentDateFormatted { get; set; }
        public string StartTime { get; set; }
        public string DueDate { get; set; }
        public bool HasNotes { get; set; }
        public bool HasAppointment { get; set; }
        public bool HasReportDeliverable { get; set; }
        public CancellationStatusViewModel CancellationStatus { get; set; }
        public short ServiceRequestStatusId { get; set; }
        public LookupViewModel<short> ServiceRequestStatus { get; set; }
        public LookupViewModel<short> NextTaskStatusForUser { get; set; }
        public LookupViewModel<Guid> Physician { get; set; }
        public LookupViewModel<short> Service { get; set; }
        public LookupViewModel<short> Company { get; set; }
        public AddressViewModel Address { get; set; }
        public IEnumerable<ResourceViewModel> Resources { get; set; }
        
        public static Func<ServiceRequestDto, AppointmentViewModel> FromServiceRequestDto = dto => new AppointmentViewModel
        {
            ServiceRequestId = dto.Id,
            ClaimantName = dto.ClaimantName,
            AppointmentDate = dto.AppointmentDate.Value,
            AppointmentDateFormatted = dto.AppointmentDate.ToOrvosiDateFormat(),
            StartTime = dto.StartTime.ToShortTimeSafe(),
            DueDate = dto.DueDate.ToOrvosiDateFormat(),
            HasNotes = dto.Comments.Any(),
            HasAppointment = dto.HasAppointment,
            HasReportDeliverable = dto.HasReportDeliverable,

            ServiceRequestStatus = LookupViewModel<short>.FromServiceRequestStatusDto.Invoke(dto.ServiceRequestStatus),
            NextTaskStatusForUser = LookupViewModel<short>.FromNextTaskStatusForUserDto.Invoke(dto.NextTaskStatusForUser),
            CancellationStatus = CancellationStatusViewModel.FromServiceRequestDto.Invoke(dto),
            Service = LookupViewModel<short>.FromLookupDto.Invoke(dto.Service),
            Company = LookupViewModel<short>.FromLookupDto.Invoke(dto.Company),
            Address = AddressViewModel.FromAddressDtoExpr.Invoke(dto.Address),
            Physician = LookupViewModel<Guid>.FromPersonDto.Invoke(dto.Physician),
            Resources = dto.Resources.Where(r => r.RoleId != AspNetRoles.CaseCoordinator)
                .Select(ResourceViewModel.FromResourceDto)
        };
    }
}