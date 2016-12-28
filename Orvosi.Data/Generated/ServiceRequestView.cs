// <auto-generated>
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// TargetFrameworkVersion = 4.51
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning

namespace Orvosi.Data
{

    // ServiceRequest
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class ServiceRequestView
    {
        public int Id { get; set; } // Id
        public System.Guid ObjectGuid { get; set; } // ObjectGuid
        public string CompanyReferenceId { get; set; } // CompanyReferenceId (length: 128)
        public string ClaimantName { get; set; } // ClaimantName (length: 128)
        public short? ServiceCatalogueId { get; set; } // ServiceCatalogueId
        public string Body { get; set; } // Body
        public int? AddressId { get; set; } // AddressId
        public System.DateTime? RequestedDate { get; set; } // RequestedDate
        public System.Guid? RequestedBy { get; set; } // RequestedBy
        public System.DateTime? CancelledDate { get; set; } // CancelledDate
        public short? AvailableSlotId { get; set; } // AvailableSlotId
        public System.DateTime? DueDate { get; set; } // DueDate
        public System.Guid? CaseCoordinatorId { get; set; } // CaseCoordinatorId
        public System.Guid? IntakeAssistantId { get; set; } // IntakeAssistantId
        public System.Guid? DocumentReviewerId { get; set; } // DocumentReviewerId
        public decimal? ServiceRequestPrice { get; set; } // ServiceRequestPrice
        public string Notes { get; set; } // Notes (length: 2000)
        public short? CompanyId { get; set; } // CompanyId
        public bool IsNoShow { get; set; } // IsNoShow
        public bool IsLateCancellation { get; set; } // IsLateCancellation
        public decimal? NoShowRate { get; set; } // NoShowRate
        public decimal? LateCancellationRate { get; set; } // LateCancellationRate
        public System.DateTime ModifiedDate { get; set; } // ModifiedDate
        public string ModifiedUser { get; set; } // ModifiedUser (length: 100)
        public short? ServiceId { get; set; } // ServiceId
        public System.DateTime? AppointmentDate { get; set; } // AppointmentDate
        public System.TimeSpan? StartTime { get; set; } // StartTime
        public int? Duration { get; set; } // Duration
        public System.TimeSpan? EndTime { get; set; } // EndTime
        public string Title { get; set; } // Title (length: 210)
        public byte? ServiceRequestStatusId { get; set; } // ServiceRequestStatusId
        public string ServiceRequestStatusText { get; set; } // ServiceRequestStatusText (length: 128)
        public byte? ServiceStatusId { get; set; } // ServiceStatusId
        public string ServiceStatusText { get; set; } // ServiceStatusText (length: 128)
        public string ServiceName { get; set; } // ServiceName (length: 128)
        public string ServiceCode { get; set; } // ServiceCode (length: 10)
        public decimal? ServicePrice { get; set; } // ServicePrice
        public short? ServiceCategoryId { get; set; } // ServiceCategoryId
        public System.Guid PhysicianId { get; set; } // PhysicianId
        public string PhysicianDisplayName { get; set; } // PhysicianDisplayName (length: 210)
        public string PhysicianUserName { get; set; } // PhysicianUserName (length: 256)
        public string PhysicianInitials { get; set; } // PhysicianInitials (length: 4)
        public string PhysicianColorCode { get; set; } // PhysicianColorCode (length: 50)
        public System.Guid? CompanyGuid { get; set; } // CompanyGuid
        public string CompanyName { get; set; } // CompanyName (length: 128)
        public string ParentCompanyName { get; set; } // ParentCompanyName (length: 128)
        public decimal? ServiceCataloguePrice { get; set; } // ServiceCataloguePrice
        public decimal? EffectivePrice { get; set; } // EffectivePrice
        public string RequestedByName { get; set; } // RequestedByName (length: 210)
        public string AddressName { get; set; } // AddressName (length: 256)
        public byte? AddressTypeId { get; set; } // AddressTypeId
        public string AddressTypeName { get; set; } // AddressTypeName (length: 50)
        public string Address1 { get; set; } // Address1 (length: 255)
        public string Address2 { get; set; } // Address2 (length: 255)
        public string City { get; set; } // City (length: 50)
        public string PostalCode { get; set; } // PostalCode (length: 50)
        public short? ProvinceId { get; set; } // ProvinceId
        public string ProvinceName { get; set; } // ProvinceName (length: 50)
        public short? CountryId { get; set; } // CountryId
        public string CountryName { get; set; } // CountryName (length: 3)
        public short? LocationId { get; set; } // LocationId
        public string CaseCoordinatorName { get; set; } // CaseCoordinatorName (length: 210)
        public string CaseCoordinatorInitials { get; set; } // CaseCoordinatorInitials (length: 4)
        public string CaseCoordinatorColorCode { get; set; } // CaseCoordinatorColorCode (length: 50)
        public string CaseCoordinatorUserName { get; set; } // CaseCoordinatorUserName (length: 256)
        public string IntakeAssistantName { get; set; } // IntakeAssistantName (length: 210)
        public string IntakeAssistantInitials { get; set; } // IntakeAssistantInitials (length: 4)
        public string IntakeAssistantColorCode { get; set; } // IntakeAssistantColorCode (length: 50)
        public string IntakeAssistantUserName { get; set; } // IntakeAssistantUserName (length: 256)
        public string IntakeAssistantBoxCollaborationId { get; set; } // IntakeAssistantBoxCollaborationId (length: 50)
        public string IntakeAssistantBoxUserId { get; set; } // IntakeAssistantBoxUserId (length: 50)
        public string DocumentReviewerName { get; set; } // DocumentReviewerName (length: 210)
        public string DocumentReviewerInitials { get; set; } // DocumentReviewerInitials (length: 4)
        public string DocumentReviewerColorCode { get; set; } // DocumentReviewerColorCode (length: 50)
        public string DocumentReviewerUserName { get; set; } // DocumentReviewerUserName (length: 256)
        public string DocumentReviewerBoxCollaborationId { get; set; } // DocumentReviewerBoxCollaborationId (length: 50)
        public string DocumentReviewerBoxUserId { get; set; } // DocumentReviewerBoxUserId (length: 50)
        public string CalendarEventTitle { get; set; } // CalendarEventTitle (length: 255)
        public int? TotalTasks { get; set; } // TotalTasks
        public int? ClosedTasks { get; set; } // ClosedTasks
        public int? OpenTasks { get; set; } // OpenTasks
        public int? NextTaskId { get; set; } // NextTaskId
        public string NextTaskName { get; set; } // NextTaskName (length: 128)
        public System.Guid? NextTaskAssignedTo { get; set; } // NextTaskAssignedTo
        public string NextTaskAssignedtoName { get; set; } // NextTaskAssignedtoName (length: 210)
        public string BoxCaseFolderId { get; set; } // BoxCaseFolderId (length: 128)
        public string BoxPhysicianFolderId { get; set; } // BoxPhysicianFolderId (length: 128)
        public string ServiceCategoryName { get; set; } // ServiceCategoryName (length: 2)
        public string DocumentFolderLink { get; set; } // DocumentFolderLink (length: 2)
        public string ServicePortfolioName { get; set; } // ServicePortfolioName (length: 2)
        public string LocationName { get; set; } // LocationName (length: 2)

        public ServiceRequestView()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
