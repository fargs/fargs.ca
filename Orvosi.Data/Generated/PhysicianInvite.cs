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

    // PhysicianInvite
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class PhysicianInvite
    {
        public System.Guid Id { get; set; } // Id (Primary key)
        public System.Guid PhysicianId { get; set; } // PhysicianId
        public string Email { get; set; } // Email (length: 128)
        public string Name { get; set; } // Name (length: 128)
        public System.Guid? UserId { get; set; } // UserId
        public System.DateTime? SentDate { get; set; } // SentDate
        public System.DateTime? LinkClickedDate { get; set; } // LinkClickedDate

        // Foreign keys
        public virtual PhysicianV2 PhysicianV2 { get; set; } // FK_PhysicianInvite_Physician

        public PhysicianInvite()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>