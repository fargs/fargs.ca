using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Library.Extensions;
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Views.Work.DaySheet.ServiceRequest
{
    //TODO: Move the CanBeCancelled, CanBeUncancelled, CanBeNoShow, CanNoShowBeUndone properties from CaseViewModel to here
    public class SummaryViewModel
    {
        public int ServiceRequestId { get; set; }
        public string ClaimantName { get; set; }
        public object StartTime { get; set; }
        public string SourceCompany { get; set; }
        public byte? MedicolegalTypeId { get; set; }


        public LookupViewModel<short> Service { get; set; }
        public LookupViewModel<short> Company { get; set; }
        public LookupViewModel<byte> MedicolegalType { get; set; }
        public LookupViewModel<short> ServiceRequestStatus { get; set; }
        public CancellationStatusViewModel CancellationStatus { get; set; }


        public static Func<ServiceRequestDto, SummaryViewModel> FromServiceRequestDto = dto => dto == null ? null : new SummaryViewModel
        {
            ServiceRequestId = dto.Id,
            ClaimantName = dto.ClaimantName,
            StartTime = dto.StartTime.ToShortTimeSafe(),
            SourceCompany = dto.SourceCompany,
            MedicolegalTypeId = dto.MedicolegalTypeId,

            Service = LookupViewModel<short>.FromLookupDto.Invoke(dto.Service),
            Company = LookupViewModel<short>.FromLookupDto.Invoke(dto.Company),
            MedicolegalType = LookupViewModel<byte>.FromLookupDto.Invoke(dto.MedicolegalType),
            ServiceRequestStatus = LookupViewModel<short>.FromLookupDto.Invoke(dto.ServiceRequestStatus),
            CancellationStatus = CancellationStatusViewModel.FromServiceRequestDto.Invoke(dto),
        };
    }
}