using ImeHub.Data;
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
        public string Key { get; set; }
        public string AlternateKey { get; set; }
        public Guid PhysicianId { get; set; }
        public string ClaimantName { get; set; }
        public Enums.ServiceRequestStatus StatusId { get; set; }
        public LookupModel<byte> Status { get; set; }
        public Guid ServiceId { get; set; }
        public ServiceModel Service { get; set; }
        public DateTime DueDate { get; set; }
        public Guid AvailableSlotId { get; set; }
        public AvailableSlotModel AvailableSlot { get; set; }
        public Guid? RequestedBy { get; set; }
        public DateTime? RequestedDate { get; set; }
        public Guid? AddressId { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string ReferralSource { get; set; }
        public Enums.CancellationStatus CancellationStatusId { get; set; }
        public LookupModel<byte> CancellationStatus { get; set; }

        public static Expression<Func<ServiceRequest, ServiceRequestModel>> FromServiceRequestForAvailability = sr => sr == null ? null : new ServiceRequestModel
        {
            Id = sr.Id,
            ClaimantName = sr.ClaimantName,
            CancellationStatusId = (Enums.CancellationStatus)sr.CancellationStatusId,
            CancellationStatus = new LookupModel<byte>
            {
                Id = sr.CancellationStatu.Id,
                Name = sr.CancellationStatu.Name,
                Code = sr.CancellationStatu.Code,
                ColorCode = sr.CancellationStatu.ColorCode
            }
        };
    }

}
