using ImeHub.Data;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ImeHub.Models
{
    public class ServiceRequestModel
    {
        public Guid Id { get; set; }
        public string CaseNumber { get; set; }
        public string AlternateKey { get; set; }
        public Guid PhysicianId { get; set; }
        public string ClaimantName { get; set; }
        public Enums.ServiceRequestStatus StatusId { get; set; }
        public LookupModel<Enums.ServiceRequestStatus> Status { get; set; }
        public Guid CompanyId { get; set; }
        public LookupModel<Guid> Company { get; set; }
        public Guid ServiceId { get; set; }
        public LookupModel<Guid> Service { get; set; }
        public byte? MedicolegalTypeId { get; set; }
        public LookupModel<byte> MedicolegalType { get; set; }
        public DateTime DueDate { get; set; }
        public Guid AvailableSlotId { get; set; }
        public AvailableSlotModel AvailableSlot { get; set; }
        public Guid? RequestedBy { get; set; }
        public DateTime? RequestedDate { get; set; }
        public Guid? AddressId { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string ReferralSource { get; set; }
        public Enums.CancellationStatus CancellationStatusId { get; set; }
        public LookupModel<Enums.CancellationStatus> CancellationStatus { get; set; }

        public static Expression<Func<ServiceRequest, ServiceRequestModel>> FromServiceRequestForAvailability = sr => sr == null ? null : new ServiceRequestModel
        {
            Id = sr.Id,
            CaseNumber = sr.CaseNumber,
            ClaimantName = sr.ClaimantName,
            CancellationStatusId = (Enums.CancellationStatus)sr.CancellationStatusId,
            CancellationStatus = new LookupModel<Enums.CancellationStatus>
            {
                Id = (Enums.CancellationStatus)sr.CancellationStatu.Id,
                Name = sr.CancellationStatu.Name,
                Code = sr.CancellationStatu.Code,
                ColorCode = sr.CancellationStatu.ColorCode
            }
        };

        public static Expression<Func<ServiceRequest, ServiceRequestModel>> FromServiceRequest = sr => sr == null ? null : new ServiceRequestModel
        {
            Id = sr.Id,
            CaseNumber = sr.CaseNumber,
            ClaimantName = sr.ClaimantName,
            CompanyId = sr.Service.CompanyId,
            Company = LookupModel<Guid>.FromCompany.Invoke(sr.Service.Company),
            ServiceId = sr.Service.CompanyId,
            Service = LookupModel<Guid>.FromService.Invoke(sr.Service),
            StatusId = (Enums.ServiceRequestStatus)sr.StatusId,
            Status = LookupModel<Enums.ServiceRequestStatus>.FromServiceRequestStatus.Invoke(sr.ServiceRequestStatu),
            MedicolegalTypeId = sr.MedicolegalTypeId,
            MedicolegalType = LookupModel<byte>.FromMedicolegalType.Invoke(sr.MedicolegalType),
            CancellationStatusId = (Enums.CancellationStatus)sr.CancellationStatusId,
            CancellationStatus = new LookupModel<Enums.CancellationStatus>
            {
                Id = (Enums.CancellationStatus)sr.CancellationStatu.Id,
                Name = sr.CancellationStatu.Name,
                Code = sr.CancellationStatu.Code,
                ColorCode = sr.CancellationStatu.ColorCode
            }
        };
    }

}
