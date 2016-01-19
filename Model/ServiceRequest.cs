//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ServiceRequest
    {
        public int Id { get; set; }
        public System.Guid ObjectGuid { get; set; }
        public string CompanyReferenceId { get; set; }
        public Nullable<short> ServiceCatalogueId { get; set; }
        public Nullable<long> HarvestProjectId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public Nullable<int> AddressId { get; set; }
        public Nullable<System.DateTime> RequestedDate { get; set; }
        public Nullable<System.Guid> RequestedBy { get; set; }
        public Nullable<System.DateTime> CancelledDate { get; set; }
        public Nullable<byte> StatusId { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<System.TimeSpan> StartTime { get; set; }
        public Nullable<System.TimeSpan> EndTime { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public string ServiceName { get; set; }
        public string ServiceCategoryName { get; set; }
        public string ServicePortfolioName { get; set; }
        public string PhysicianDisplayName { get; set; }
        public string CompanyName { get; set; }
        public string ParentCompanyName { get; set; }
        public Nullable<decimal> EffectivePrice { get; set; }
        public string StatusName { get; set; }
        public string AddressName { get; set; }
        public Nullable<byte> AddressTypeId { get; set; }
        public string AddressTypeName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public Nullable<short> ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public Nullable<short> CountryId { get; set; }
        public string CountryName { get; set; }
        public Nullable<short> LocationId { get; set; }
        public string LocationName { get; set; }
        public string PostalCode { get; set; }
        public string ClaimantName { get; set; }
        public Nullable<System.DateTime> AppointmentDate { get; set; }
        public Nullable<System.Guid> CaseCoordinatorId { get; set; }
        public Nullable<System.Guid> IntakeAssistantId { get; set; }
        public Nullable<System.Guid> DocumentReviewerId { get; set; }
        public Nullable<decimal> ServiceRequestPrice { get; set; }
        public Nullable<decimal> ServicePrice { get; set; }
        public Nullable<decimal> ServiceCataloguePrice { get; set; }
        public string RequestedByName { get; set; }
        public string CaseCoordinatorName { get; set; }
        public string IntakeAssistantName { get; set; }
        public string DocumentReviewerName { get; set; }
        public Nullable<short> AvailableSlotId { get; set; }
        public Nullable<short> Duration { get; set; }
        public string CalendarEventTitle { get; set; }
        public string DocumentFolderLink { get; set; }
        public string ServiceCode { get; set; }
        public Nullable<short> ServiceId { get; set; }
        public string PhysicianId { get; set; }
        public Nullable<short> CompanyId { get; set; }
        public string Notes { get; set; }
        public bool IsNoShow { get; set; }
        public Nullable<int> TotalTasks { get; set; }
        public Nullable<int> ClosedTasks { get; set; }
        public Nullable<int> OpenTasks { get; set; }
        public Nullable<int> NextTaskId { get; set; }
        public string NextTaskName { get; set; }
        public string NextTaskAssignedTo { get; set; }
        public string NextTaskAssignedtoName { get; set; }
        public bool IsLateCancellation { get; set; }
    }
}
