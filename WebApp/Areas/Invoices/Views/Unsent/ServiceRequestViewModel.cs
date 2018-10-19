using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;
using WebApp.Views.Address;
using WebApp.Views.Cancellation;
using WebApp.Views.Comment;
using WebApp.Views.Shared;

namespace WebApp.Areas.Invoices.Views.Unsent
{
    public class ServiceRequestViewModel
    {
        public ServiceRequestViewModel(ServiceRequestDto serviceRequest)
        {
            ServiceRequestId = serviceRequest.Id;
            ExpectedInvoiceDate = serviceRequest.ExpectedInvoiceDate;
            ClaimantName = serviceRequest.ClaimantName;
            StartTime = serviceRequest.StartTime;
            SourceCompany = serviceRequest.SourceCompany;
            MedicolegalTypeId = serviceRequest.MedicolegalTypeId;

            Service = LookupViewModel<short>.FromLookupDto(serviceRequest.Service);
            Company = LookupViewModel<short>.FromLookupDto(serviceRequest.Company);
            Address = AddressViewModel.FromAddressDto(serviceRequest.Address);
            MedicolegalType = LookupViewModel<byte>.FromLookupDto(serviceRequest.MedicolegalType);
            ServiceRequestStatus = LookupViewModel<short>.FromLookupDto(serviceRequest.ServiceRequestStatus);
            CancellationStatus = CancellationStatusViewModel.FromServiceRequestDto(serviceRequest);

            CommentList = new CommentListViewModel(serviceRequest);
        }
        public int ServiceRequestId { get; set; }
        public DateTime ExpectedInvoiceDate { get; set; }
        public string ClaimantName { get; set; }
        public TimeSpan? StartTime { get; set; }
        public string SourceCompany { get; set; }
        public byte? MedicolegalTypeId { get; set; }


        public LookupViewModel<short> Service { get; set; }
        public LookupViewModel<short> Company { get; set; }
        public AddressViewModel Address { get; set; }
        public LookupViewModel<byte> MedicolegalType { get; set; }
        public LookupViewModel<short> ServiceRequestStatus { get; set; }
        public CancellationStatusViewModel CancellationStatus { get; set; }

        public CommentListViewModel CommentList { get; set; }
        public bool IsSubmitInvoiceTaskDone { get; set; }
    }
}