using LinqKit;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class CaseLinkViewModel
    { 
        public CaseLinkViewModel()
        {
            //Comments = new List<CommentViewModel>();
        }
        public int ServiceRequestId { get; set; }
        public string ClaimantName { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public DateTime? DueDate { get; set; }
        public string Notes { get; set; }
        public bool HasAppointment { get; set; }
        public bool HasReportDeliverable { get; set; }
        public CancellationViewModel CancellationViewModel { get; set; }
        public short ServiceRequestStatusId { get; set; }
        public LookupViewModel<short> ServiceRequestStatus { get; set; }
        public LookupViewModel<short> NextTaskStatusForUser { get; set; }
        public LookupViewModel<Guid> Physician { get; set; }
        public LookupViewModel<short> Service { get; set; }
        public LookupViewModel<short> Company { get; set; }
        public AddressViewModel Address { get; set; }
        public LookupViewModel<Guid> CaseCoordinator { get; set; }
        public LookupViewModel<Guid> DocumentReviewer { get; set; }
        public LookupViewModel<Guid> IntakeAssistant { get; set; }

        public static Expression<Func<ServiceRequestDto, CaseLinkViewModel>> FromServiceRequestDto = dto => new CaseLinkViewModel
        {
            ServiceRequestId = dto.Id,
            ClaimantName = dto.ClaimantName,
            AppointmentDate = dto.AppointmentDate,
            StartTime = dto.StartTime,
            DueDate = dto.DueDate,
            Notes = dto.Notes,
            HasAppointment = dto.HasAppointment,
            HasReportDeliverable = dto.HasReportDeliverable,

            ServiceRequestStatus = LookupViewModel<short>.FromServiceRequestStatusDto.Invoke(dto.ServiceRequestStatus),
            NextTaskStatusForUser = LookupViewModel<short>.FromNextTaskStatusForUserDto.Invoke(dto.NextTaskStatusForUser),
            CancellationViewModel = CancellationViewModel.FromServiceRequestDto.Invoke(dto),
            Service = LookupViewModel<short>.FromLookupDto.Invoke(dto.Service),
            Company = LookupViewModel<short>.FromLookupDto.Invoke(dto.Company),
            Address = AddressViewModel.FromAddressDto.Invoke(dto.Address),
            Physician = LookupViewModel<Guid>.FromPersonDto.Invoke(dto.Physician),
            CaseCoordinator = LookupViewModel<Guid>.FromPersonDto.Invoke(dto.CaseCoordinator),
            DocumentReviewer = LookupViewModel<Guid>.FromPersonDto.Invoke(dto.DocumentReviewer),
            IntakeAssistant = LookupViewModel<Guid>.FromPersonDto.Invoke(dto.IntakeAssistant)
        };
    }
}