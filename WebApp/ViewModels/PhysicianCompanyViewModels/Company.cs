namespace WebApp.ViewModels.PhysicianCompanyViewModels
{
    public class Company
    {
        public int AvailabilityCount { get; set; }
        public string BillingEmail { get; set; }
        public string Code { get; set; }
        public short CompanyId { get; set; }
        public short Id { get; set; }
        public string Name { get; set; }
        public short ParentCompanyId { get; set; }
        public string ParentCompanyName { get; set; }
        public string Phone { get; set; }
        public int ServiceRequestCount { get; set; }
        public string Status { get; set; }
        public short StatusId { get; set; }
    }
}