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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ServiceRequest()
        {
            this.InvoiceDetails = new HashSet<InvoiceDetail>();
        }
    
        public int Id { get; set; }
        public System.Guid ObjectGuid { get; set; }
        public string CompanyReferenceId { get; set; }
        public Nullable<short> ServiceCatalogueId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public Nullable<int> AddressId { get; set; }
        public Nullable<System.DateTime> RequestedDate { get; set; }
        public Nullable<System.Guid> RequestedBy { get; set; }
        public Nullable<System.DateTime> CancelledDate { get; set; }
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
        public System.Guid PhysicianId { get; set; }
        public Nullable<short> CompanyId { get; set; }
        public string Notes { get; set; }
        public bool IsNoShow { get; set; }
        public Nullable<int> TotalTasks { get; set; }
        public Nullable<int> ClosedTasks { get; set; }
        public Nullable<int> OpenTasks { get; set; }
        public bool IsLateCancellation { get; set; }
        public Nullable<int> NextTaskId { get; set; }
        public string NextTaskName { get; set; }
        public Nullable<System.Guid> NextTaskAssignedTo { get; set; }
        public string NextTaskAssignedtoName { get; set; }
        public Nullable<byte> ServiceRequestStatusId { get; set; }
        public Nullable<byte> ServiceStatusId { get; set; }
        public string ServiceRequestStatusText { get; set; }
        public string ServiceStatusText { get; set; }
        public string PhysicianUserName { get; set; }
        public Nullable<decimal> NoShowRate { get; set; }
        public Nullable<decimal> LateCancellationRate { get; set; }
        public Nullable<System.Guid> CompanyGuid { get; set; }
        public string PhysicianInitials { get; set; }
        public string PhysicianColorCode { get; set; }
        public string CaseCoordinatorInitials { get; set; }
        public string CaseCoordinatorColorCode { get; set; }
        public string IntakeAssistantInitials { get; set; }
        public string IntakeAssistantColorCode { get; set; }
        public string DocumentReviewerInitials { get; set; }
        public string DocumentReviewerColorCode { get; set; }
        public string CaseCoordinatorUserName { get; set; }
        public string IntakeAssistantUserName { get; set; }
        public string DocumentReviewerUserName { get; set; }
        public Nullable<short> ServiceCategoryId { get; set; }
        public string BoxCaseFolderId { get; set; }
        public string BoxPhysicianFolderId { get; set; }
        public string IntakeAssistantBoxCollaborationId { get; set; }
        public string DocumentReviewerBoxCollaborationId { get; set; }
        public string IntakeAssistantBoxUserId { get; set; }
        public string DocumentReviewerBoxUserId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    }
}
