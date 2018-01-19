using System;

namespace WebApp.FormModels
{
    public class ChangeCompanyForm
    {
        public ChangeCompanyForm()
        {
        }

        public short? CompanyId { get; set; }
        public int ServiceRequestId { get; set; }
        public Guid PhysicianId { get; set; }
    }
}