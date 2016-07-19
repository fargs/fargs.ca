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

    // ServiceRequestBoxCollaboration
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class ServiceRequestBoxCollaboration
    {
        public int Id { get; set; } // Id (Primary key)
        public System.Guid UserId { get; set; } // UserId
        public int ServiceRequestId { get; set; } // ServiceRequestId
        public string BoxCollaborationId { get; set; } // BoxCollaborationId (length: 50)
        public System.DateTime ModifiedDate { get; set; } // ModifiedDate
        public string ModifiedUser { get; set; } // ModifiedUser (length: 100)

        // Foreign keys
        public virtual ServiceRequest ServiceRequest { get; set; } // FK_ServiceRequestBoxCollaboration_ServiceRequest

        public ServiceRequestBoxCollaboration()
        {
            ModifiedDate = System.DateTime.Now;
            ModifiedUser = "suser_name()";
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
