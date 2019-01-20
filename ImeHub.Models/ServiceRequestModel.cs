using System;
using System.Collections.Generic;
using System.Linq;
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
        public Guid? RequestedBy { get; internal set; }
        public DateTime? RequestedDate { get; internal set; }
        public Guid? AddressId { get; internal set; }
        public DateTime? AppointmentDate { get; internal set; }
        public string ReferralSource { get; internal set; }
        public Enums.CancellationStatus CancellationStatus { get; set; }
    }

}
