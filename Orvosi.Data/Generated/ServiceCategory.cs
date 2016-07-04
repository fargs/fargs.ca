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

    // ServiceCategory
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class ServiceCategory
    {
        public short Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name (length: 128)
        public string Description { get; set; } // Description
        public System.DateTime ModifiedDate { get; set; } // ModifiedDate
        public string ModifiedUser { get; set; } // ModifiedUser (length: 100)

        // Reverse navigation
        public virtual System.Collections.Generic.ICollection<Service> Services { get; set; } // Service.FK_Service_ServiceCategory
        public virtual System.Collections.Generic.ICollection<ServiceRequestTemplate> ServiceRequestTemplates { get; set; } // ServiceRequestTemplate.FK_ServiceRequestTemplate_ServiceCategory

        public ServiceCategory()
        {
            ModifiedDate = System.DateTime.Now;
            ModifiedUser = "suser_name()";
            Services = new System.Collections.Generic.List<Service>();
            ServiceRequestTemplates = new System.Collections.Generic.List<ServiceRequestTemplate>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>