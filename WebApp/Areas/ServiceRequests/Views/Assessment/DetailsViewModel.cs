using ImeHub.Models;
using ImeHub.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using WebApp.Views.Shared;
using Enums = ImeHub.Models.Enums;

namespace WebApp.Areas.ServiceRequests.Views.Assessment
{
    public class DetailsViewModel : ViewModelBase
    {
        public DetailsViewModel(ServiceRequestModel assessment, IIdentity identity, DateTime now) : base(identity, now)
        {
            var a = assessment;
            Id = a.Id;
            CaseNumber = a.CaseNumber;
            ClaimantName = a.ClaimantName;
            CancellationStatusId = a.CancellationStatusId;
            CancellationStatus = new LookupViewModel<Enums.CancellationStatus>(a.CancellationStatus);
            StatusId = a.StatusId;
            Status = new LookupViewModel<Enums.ServiceRequestStatus>(a.Status);
            CompanyId = a.CompanyId;
            Company = new LookupViewModel<Guid>(a.Company);
            ServiceId = a.ServiceId;
            Service = new LookupViewModel<Guid>(a.Service);
            MedicolegalTypeId = a.MedicolegalTypeId;
            MedicolegalType = !a.MedicolegalTypeId.HasValue ? null : new LookupViewModel<byte>(a.MedicolegalType);
            ReferralSource = a.ReferralSource;
        }
        public Guid Id { get; set; }
        public string CaseNumber { get; set; }
        public string ClaimantName { get; set; }
        public CancellationStatus CancellationStatusId { get; set; }
        public LookupViewModel<Enums.CancellationStatus> CancellationStatus { get; private set; }
        public ServiceRequestStatus StatusId { get; set; }
        public LookupViewModel<Enums.ServiceRequestStatus> Status { get; private set; }
        public Guid CompanyId { get; set; }
        public LookupViewModel<Guid> Company { get; set; }
        public Guid ServiceId { get; set; }
        public LookupViewModel<Guid> Service { get; set; }
        public byte? MedicolegalTypeId { get; set; }
        public LookupViewModel<byte> MedicolegalType { get; set; }
        public string ReferralSource { get; set; }
    }
}