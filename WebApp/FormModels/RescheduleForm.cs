using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.FormModels
{
    public class RescheduleForm
    {
        [Required]
        public int ServiceRequestId { get; set; }
        [Required]
        public Guid PhysicianId { get; set; }
        [Required]
        public DateTime AppointmentDate { get; set; }
        public short? OriginalAvailableSlotId { get; set; }
        public short? AvailableSlotId { get; set; }
        public int? AddressId { get; set; }
        [Required]
        public int ServiceRequestTaskId { get; set; }
    }
}