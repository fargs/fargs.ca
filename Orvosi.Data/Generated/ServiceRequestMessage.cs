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

    // ServiceRequestMessage
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class ServiceRequestMessage
    {
        public System.Guid Id { get; set; } // Id (Primary key)
        public string Message { get; set; } // Message (length: 256)
        public System.DateTime PostedDate { get; set; } // PostedDate
        public System.Guid UserId { get; set; } // UserId
        public int ServiceRequestId { get; set; } // ServiceRequestId
        public System.DateTime ModifiedDate { get; set; } // ModifiedDate
        public string ModifiedUser { get; set; } // ModifiedUser (length: 100)

        // Reverse navigation
        public virtual System.Collections.Generic.ICollection<UserInbox> UserInboxes { get; set; } // UserInbox.FK_UserInbox_ServiceRequestMessage

        // Foreign keys
        public virtual AspNetUser AspNetUser { get; set; } // FK_ServiceRequestMessage_AspNetUsers
        public virtual ServiceRequest ServiceRequest { get; set; } // FK_ServiceRequestMessage_ServiceRequest

        public ServiceRequestMessage()
        {
            ModifiedDate = System.DateTime.Now;
            ModifiedUser = "suser_name()";
            UserInboxes = new System.Collections.Generic.List<UserInbox>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
