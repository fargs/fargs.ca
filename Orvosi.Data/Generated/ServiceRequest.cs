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
    public partial class ServiceRequest
    {
        public int Id { get; set; } // Id (Primary key)
        public System.Guid ObjectGuid { get; set; } // ObjectGuid
        public string CompanyReferenceId { get; set; } // CompanyReferenceId (length: 128)
        public string ClaimantName { get; set; } // ClaimantName (length: 128)
        public short? ServiceCatalogueId { get; set; } // ServiceCatalogueId
        public long? HarvestProjectId { get; set; } // HarvestProjectId
        public string Title { get; set; } // Title (length: 256)
        public string Body { get; set; } // Body
        public int? AddressId { get; set; } // AddressId
        public System.DateTime? RequestedDate { get; set; } // RequestedDate
        public System.Guid? RequestedBy { get; set; } // RequestedBy
        public System.DateTime? CancelledDate { get; set; } // CancelledDate
        public short? AvailableSlotId { get; set; } // AvailableSlotId
        public System.DateTime? AppointmentDate { get; set; } // AppointmentDate
        public System.TimeSpan? StartTime { get; set; } // StartTime
        public System.TimeSpan? EndTime { get; set; } // EndTime
        public System.DateTime? DueDate { get; set; } // DueDate
        public System.Guid? CaseCoordinatorId { get; set; } // CaseCoordinatorId
        public System.Guid? IntakeAssistantId { get; set; } // IntakeAssistantId
        public System.Guid? DocumentReviewerId { get; set; } // DocumentReviewerId
        public decimal? Price { get; set; } // Price
        public string Notes { get; set; } // Notes (length: 2000)
        public int? InvoiceItemId { get; set; } // InvoiceItemId
        public string DocumentFolderLink { get; set; } // DocumentFolderLink (length: 2000)
        public short? CompanyId { get; set; } // CompanyId
        public bool IsNoShow { get; set; } // IsNoShow
        public bool IsLateCancellation { get; set; } // IsLateCancellation
        public System.DateTime ModifiedDate { get; set; } // ModifiedDate
        public string ModifiedUser { get; set; } // ModifiedUser (length: 100)
        public decimal? NoShowRate { get; set; } // NoShowRate
        public decimal? LateCancellationRate { get; set; } // LateCancellationRate
        public System.Guid PhysicianId { get; set; } // PhysicianId
        public short? ServiceId { get; set; } // ServiceId
        public int? LocationId { get; set; } // LocationId
        public decimal? ServiceCataloguePrice { get; set; } // ServiceCataloguePrice
        public string BoxCaseFolderId { get; set; } // BoxCaseFolderId (length: 128)
        public string IntakeAssistantBoxCollaborationId { get; set; } // IntakeAssistantBoxCollaborationId (length: 50)
        public string DocumentReviewerBoxCollaborationId { get; set; } // DocumentReviewerBoxCollaborationId (length: 50)
        public short? ServiceRequestTemplateId { get; set; } // ServiceRequestTemplateId
        public bool IsClosed { get; set; } // IsClosed
        public string CalendarEventId { get; set; } // CalendarEventId (length: 256)
        public bool IsDeleted { get; set; } // IsDeleted
        public System.DateTime CreatedDate { get; set; } // CreatedDate
        public string CreatedUser { get; set; } // CreatedUser (length: 128)
        public short ServiceRequestStatusId { get; set; } // ServiceRequestStatusId
        public System.Guid? ServiceRequestStatusChangedBy { get; set; } // ServiceRequestStatusChangedBy
        public System.DateTime? ServiceRequestStatusChangedDate { get; set; } // ServiceRequestStatusChangedDate
        public bool HasErrors { get; set; } // HasErrors
        public bool HasWarnings { get; set; } // HasWarnings
        public bool IsOnHold { get; set; } // IsOnHold

        // Reverse navigation
        public virtual System.Collections.Generic.ICollection<InvoiceDetail> InvoiceDetails { get; set; } // InvoiceDetail.FK_InvoiceDetail_ServiceRequest
        public virtual System.Collections.Generic.ICollection<ServiceRequestBoxCollaboration> ServiceRequestBoxCollaborations { get; set; } // ServiceRequestBoxCollaboration.FK_ServiceRequestBoxCollaboration_ServiceRequest
        public virtual System.Collections.Generic.ICollection<ServiceRequestComment> ServiceRequestComments { get; set; } // ServiceRequestComment.FK_ServiceRequestComment_ServiceRequest
        public virtual System.Collections.Generic.ICollection<ServiceRequestMessage> ServiceRequestMessages { get; set; } // ServiceRequestMessage.FK_ServiceRequestMessage_ServiceRequest
        public virtual System.Collections.Generic.ICollection<ServiceRequestResource> ServiceRequestResources { get; set; } // ServiceRequestResource.FK_ServiceRequestResource_ServiceRequest
        public virtual System.Collections.Generic.ICollection<ServiceRequestTask> ServiceRequestTasks { get; set; } // ServiceRequestTask.FK_ServiceRequestTask_ServiceRequest

        // Foreign keys
        public virtual Address Address { get; set; } // FK_ServiceRequest_Address
        public virtual AspNetUser CaseCoordinator { get; set; } // FK_ServiceRequest_CaseCoordinator
        public virtual AspNetUser DocumentReviewer { get; set; } // FK_ServiceRequest_DocumentReviewer
        public virtual AspNetUser IntakeAssistant { get; set; } // FK_ServiceRequest_IntakeAssistant
        public virtual AvailableSlot AvailableSlot { get; set; } // FK_ServiceRequest_AvailableSlot
        public virtual Company Company { get; set; } // FK_ServiceRequest_Company
        public virtual Physician Physician { get; set; } // FK_ServiceRequest_Physician
        public virtual Service Service { get; set; } // FK_ServiceRequest_Service
        public virtual ServiceRequestStatu ServiceRequestStatu { get; set; } // FK_ServiceRequest_ServiceRequestStatus
        public virtual ServiceRequestTemplate ServiceRequestTemplate { get; set; } // FK_ServiceRequest_ServiceRequestTemplate

        public ServiceRequest()
        {
            ObjectGuid = System.Guid.NewGuid();
            IsNoShow = false;
            IsLateCancellation = false;
            ModifiedDate = System.DateTime.Now;
            ModifiedUser = "suser_name()";
            IsClosed = false;
            IsDeleted = false;
            ServiceRequestStatusId = 1;
            HasErrors = false;
            HasWarnings = false;
            IsOnHold = false;
            InvoiceDetails = new System.Collections.Generic.List<InvoiceDetail>();
            ServiceRequestBoxCollaborations = new System.Collections.Generic.List<ServiceRequestBoxCollaboration>();
            ServiceRequestComments = new System.Collections.Generic.List<ServiceRequestComment>();
            ServiceRequestMessages = new System.Collections.Generic.List<ServiceRequestMessage>();
            ServiceRequestResources = new System.Collections.Generic.List<ServiceRequestResource>();
            ServiceRequestTasks = new System.Collections.Generic.List<ServiceRequestTask>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
