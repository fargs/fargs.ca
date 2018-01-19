using System;

namespace WebApp.FormModels
{
    public class ChangeAddressForm
    {
        public int? AddressId { get; set; }
        public int ServiceRequestId { get; set; }
        public Guid PhysicianId { get; set; }
    }
}