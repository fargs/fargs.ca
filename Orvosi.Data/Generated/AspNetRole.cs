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
        public virtual System.Collections.Generic.ICollection<AspNetRolesFeature> AspNetRolesFeatures { get; set; } // AspNetRolesFeature.FK_AspNetRolesFeature_AspNetRoles
        public virtual System.Collections.Generic.ICollection<AspNetUserRole> AspNetUserRoles { get; set; } // Many to many mapping
        public virtual System.Collections.Generic.ICollection<OTask> OTasks { get; set; } // Task.FK_Task_AspNetRoles
        public virtual System.Collections.Generic.ICollection<ServiceRequestResource> ServiceRequestResources { get; set; } // ServiceRequestResource.FK_ServiceRequestResource_AspNetRoles
        public virtual System.Collections.Generic.ICollection<ServiceRequestTemplateTask> ServiceRequestTemplateTasks { get; set; } // ServiceRequestTemplateTask.FK_ServiceRequestTemplateTask_AspNetRoles

        // Foreign keys
        public virtual RoleCategory RoleCategory { get; set; } // FK_AspNetRoles_RoleCategory

        public AspNetRole()
        {
            ModifiedDate = System.DateTime.Now;
            ModifiedUser = "suser_name()";
            AspNetRolesFeatures = new System.Collections.Generic.List<AspNetRolesFeature>();
            AspNetUserRoles = new System.Collections.Generic.List<AspNetUserRole>();
            ServiceRequestResources = new System.Collections.Generic.List<ServiceRequestResource>();
            ServiceRequestTemplateTasks = new System.Collections.Generic.List<ServiceRequestTemplateTask>();
            OTasks = new System.Collections.Generic.List<OTask>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
