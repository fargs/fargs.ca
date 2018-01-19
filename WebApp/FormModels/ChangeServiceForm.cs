using System;

namespace WebApp.FormModels
{
    public class ChangeServiceForm
    {
        public ChangeServiceForm()
        {
        }

        public short? ServiceId { get; set; }
        public int ServiceRequestId { get; set; }
        public Guid PhysicianId { get; set; }
    }
}