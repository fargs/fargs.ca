using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;
using WebApp.Views.Address;
using WebApp.Views.Cancellation;

namespace WebApp.Views.Shared
{
    public class CaseNotificationViewModel
    {
        public int ServiceRequestId { get; set; }
        public string ClaimantName { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public DateTime? DueDate { get; set; }
        public CancellationViewModel CancellationViewModel { get; set; }
        public short ServiceRequestStatusId { get; set; }
        public bool IsOnHold { get; set; }
        public LookupViewModel<short> Service { get; set; }
        public LookupViewModel<short> Company { get; set; }
        public AddressViewModel Address { get; set; }
        public LookupViewModel<Guid> Physician { get; set; }
        public Uri CaseDetailsUri
        {
            get
            {
                return new Uri("https://orvosi.ca/servicerequest/details/" + ServiceRequestId);
            }
        }

        public static Expression<Func<ServiceRequestDto, CaseNotificationViewModel>> FromServiceRequestDto = dto => new CaseNotificationViewModel
        {
            ServiceRequestId = dto.Id,
            ClaimantName = dto.ClaimantName,
            AppointmentDate = dto.AppointmentDate,
            StartTime = dto.StartTime,
            DueDate = dto.DueDate,
            CancellationViewModel = CancellationViewModel.FromServiceRequestDto.Invoke(dto),
            ServiceRequestStatusId = dto.ServiceRequestStatusId,
            Service = LookupViewModel<short>.FromLookupDto.Invoke(dto.Service),
            Company = LookupViewModel<short>.FromLookupDto.Invoke(dto.Company),
            Address = AddressViewModel.FromAddressDto.Invoke(dto.Address),
            Physician = LookupViewModel<Guid>.FromPersonDtoExpr.Invoke(dto.Physician),
        };
    }
}