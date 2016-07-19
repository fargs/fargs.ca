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

    // ServiceRequestTemplate
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class ServiceRequestTemplate
    {
        public short Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name (length: 128)
        public System.DateTime ModifiedDate { get; set; } // ModifiedDate
        public string ModifiedUser { get; set; } // ModifiedUser (length: 100)

        // Reverse navigation
        public virtual System.Collections.Generic.ICollection<PhysicianServiceRequestTemplate> PhysicianServiceRequestTemplates { get; set; } // Many to many mapping
        public virtual System.Collections.Generic.ICollection<ServiceRequestTemplateTask> ServiceRequestTemplateTasks { get; set; } // ServiceRequestTemplateTask.FK_ServiceRequestTemplateTask_ServiceRequestTemplate

        public ServiceRequestTemplate()
        {
            ModifiedDate = System.DateTime.Now;
            ModifiedUser = "suser_name()";
            PhysicianServiceRequestTemplates = new System.Collections.Generic.List<PhysicianServiceRequestTemplate>();
            ServiceRequestTemplateTasks = new System.Collections.Generic.List<ServiceRequestTemplateTask>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
