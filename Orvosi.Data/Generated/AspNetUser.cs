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

    // AspNetUsers
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class AspNetUser
    {
        public System.Guid Id { get; set; } // Id (Primary key)
        public string Email { get; set; } // Email (length: 256)
        public bool EmailConfirmed { get; set; } // EmailConfirmed
        public string PasswordHash { get; set; } // PasswordHash
        public string SecurityStamp { get; set; } // SecurityStamp
        public string PhoneNumber { get; set; } // PhoneNumber
        public bool PhoneNumberConfirmed { get; set; } // PhoneNumberConfirmed
        public bool TwoFactorEnabled { get; set; } // TwoFactorEnabled
        public System.DateTime? LockoutEndDateUtc { get; set; } // LockoutEndDateUtc
        public bool LockoutEnabled { get; set; } // LockoutEnabled
        public int AccessFailedCount { get; set; } // AccessFailedCount
        public string UserName { get; set; } // UserName (length: 256)
        public string Title { get; set; } // Title (length: 50)
        public string FirstName { get; set; } // FirstName (length: 128)
        public string LastName { get; set; } // LastName (length: 128)
        public string EmployeeId { get; set; } // EmployeeId (length: 50)
        public short? CompanyId { get; set; } // CompanyId
        public string CompanyName { get; set; } // CompanyName (length: 200)
        public System.DateTime ModifiedDate { get; set; } // ModifiedDate
        public string ModifiedUser { get; set; } // ModifiedUser (length: 256)
        public System.DateTime? LastActivationDate { get; set; } // LastActivationDate
        public bool IsTestRecord { get; set; } // IsTestRecord
        public byte? RoleLevelId { get; set; } // RoleLevelId
        public decimal? HourlyRate { get; set; } // HourlyRate
        public string LogoCssClass { get; set; } // LogoCssClass (length: 50)
        public string ColorCode { get; set; } // ColorCode (length: 50)
        public string BoxFolderId { get; set; } // BoxFolderId (length: 128)
        public string BoxUserId { get; set; } // BoxUserId (length: 50)
        public string BoxAccessToken { get; set; } // BoxAccessToken (length: 128)
        public string BoxRefreshToken { get; set; } // BoxRefreshToken (length: 128)
        public string HstNumber { get; set; } // HstNumber (length: 50)
        public string Notes { get; set; } // Notes

        // Reverse navigation
        public virtual Physician Physician { get; set; } // Physician.FK_Physician_AspNetUsers
        public virtual System.Collections.Generic.ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } // AspNetUserClaims.FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId
        public virtual System.Collections.Generic.ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } // Many to many mapping
        public virtual System.Collections.Generic.ICollection<AspNetUserRole> AspNetUserRoles { get; set; } // Many to many mapping
        public virtual System.Collections.Generic.ICollection<ServiceRequest> CaseCoordinator { get; set; } // ServiceRequest.FK_ServiceRequest_CaseCoordinator
        public virtual System.Collections.Generic.ICollection<ServiceRequest> DocumentReviewer { get; set; } // ServiceRequest.FK_ServiceRequest_DocumentReviewer
        public virtual System.Collections.Generic.ICollection<ServiceRequest> IntakeAssistant { get; set; } // ServiceRequest.FK_ServiceRequest_IntakeAssistant
        public virtual System.Collections.Generic.ICollection<ServiceRequestBoxCollaboration> ServiceRequestBoxCollaborations { get; set; } // ServiceRequestBoxCollaboration.FK_ServiceRequestBoxCollaboration_AspNetUsers
        public virtual System.Collections.Generic.ICollection<ServiceRequestMessage> ServiceRequestMessages { get; set; } // ServiceRequestMessage.FK_ServiceRequestMessage_AspNetUsers
        public virtual System.Collections.Generic.ICollection<ServiceRequestTask> AssignedTo { get; set; } // ServiceRequestTask.FK_ServiceRequestTask_AspNetUsers
        public virtual System.Collections.Generic.ICollection<ServiceRequestTask> CompletedBy { get; set; } // ServiceRequestTask.FK_ServiceRequestTask_AspNetUsers1
        public virtual System.Collections.Generic.ICollection<UserInbox> UserInboxes { get; set; } // UserInbox.FK_UserInbox_AspNetUsers

        // Foreign keys
        public virtual Company Company { get; set; } // FK_AspNetUsers_Company

        public AspNetUser()
        {
            ModifiedDate = System.DateTime.Now;
            ModifiedUser = "suser_name()";
            IsTestRecord = false;
            AspNetUserClaims = new System.Collections.Generic.List<AspNetUserClaim>();
            AspNetUserLogins = new System.Collections.Generic.List<AspNetUserLogin>();
            AspNetUserRoles = new System.Collections.Generic.List<AspNetUserRole>();
            CaseCoordinator = new System.Collections.Generic.List<ServiceRequest>();
            DocumentReviewer = new System.Collections.Generic.List<ServiceRequest>();
            IntakeAssistant = new System.Collections.Generic.List<ServiceRequest>();
            ServiceRequestBoxCollaborations = new System.Collections.Generic.List<ServiceRequestBoxCollaboration>();
            ServiceRequestMessages = new System.Collections.Generic.List<ServiceRequestMessage>();
            AssignedTo = new System.Collections.Generic.List<ServiceRequestTask>();
            CompletedBy = new System.Collections.Generic.List<ServiceRequestTask>();
            UserInboxes = new System.Collections.Generic.List<UserInbox>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
