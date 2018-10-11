using LinqKit;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Library.Extensions;
using WebApp.Models;
using WebApp.Views.Cancellation;
using WebApp.Views.Shared;

namespace WebApp.Areas.Work.Views.DaySheet.ServiceRequest
{
    //TODO: Move the CanBeCancelled, CanBeUncancelled, CanBeNoShow, CanNoShowBeUndone properties from CaseViewModel to here
    public class SummaryViewModel
    {
        public SummaryViewModel()
        {

        }
        public SummaryViewModel(int serviceRequestId, OrvosiDbContext db)
        {
            var entity = db.ServiceRequests.Single(sr => sr.Id == serviceRequestId);
            var serviceRequest = ServiceRequestDto.FromServiceRequestEntityForDaySheetSummary.Invoke(entity);

            ServiceRequestId = serviceRequest.Id;
            ClaimantName = serviceRequest.ClaimantName;
            StartTime = serviceRequest.StartTime.ToShortTimeSafe();
            SourceCompany = serviceRequest.SourceCompany;
            MedicolegalTypeId = serviceRequest.MedicolegalTypeId;

            Service = LookupViewModel<short>.FromLookupDto.Invoke(serviceRequest.Service);
            Company = LookupViewModel<short>.FromLookupDto.Invoke(serviceRequest.Company);
            MedicolegalType = LookupViewModel<byte>.FromLookupDto.Invoke(serviceRequest.MedicolegalType);
            ServiceRequestStatus = LookupViewModel<short>.FromLookupDto.Invoke(serviceRequest.ServiceRequestStatus);
            CancellationStatus = CancellationStatusViewModel.FromServiceRequestDto.Invoke(serviceRequest);
        }
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