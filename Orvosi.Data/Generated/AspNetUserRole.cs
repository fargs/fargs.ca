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

    // AspNetUserRoles
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class AspNetUserRole
    {
        public string UserId { get; set; } // UserId (Primary key) (length: 128)
        public string RoleId { get; set; } // RoleId (Primary key) (length: 128)
        public System.DateTime ModifiedDate { get; set; } // ModifiedDate
        public string ModifiedUser { get; set; } // ModifiedUser (length: 256)

        // Foreign keys
        public virtual AspNetRole AspNetRole { get; set; } // FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId
        public virtual AspNetUser AspNetUser { get; set; } // FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId

        public AspNetUserRole()
        {
            ModifiedDate = System.DateTime.Now;
            ModifiedUser = "suser_name()";
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>