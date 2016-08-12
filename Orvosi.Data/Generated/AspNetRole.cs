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

    // AspNetRoles
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class AspNetRole
    {
        public System.Guid Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name (length: 256)
        public byte? RoleCategoryId { get; set; } // RoleCategoryId
        public System.DateTime ModifiedDate { get; set; } // ModifiedDate
        public string ModifiedUser { get; set; } // ModifiedUser (length: 256)

        // Reverse navigation
        public virtual System.Collections.Generic.ICollection<AspNetUserRole> AspNetUserRoles { get; set; } // Many to many mapping
        public virtual System.Collections.Generic.ICollection<Task> Tasks { get; set; } // Task.FK_Task_AspNetRoles

        public AspNetRole()
        {
            ModifiedDate = System.DateTime.Now;
            ModifiedUser = "suser_name()";
            AspNetUserRoles = new System.Collections.Generic.List<AspNetUserRole>();
            Tasks = new System.Collections.Generic.List<Task>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
