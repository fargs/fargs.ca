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

    // Organization
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class Organization
    {
        public System.Guid Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name (length: 128)
        public string Code { get; set; } // Code (length: 10)
        public string ColorCode { get; set; } // ColorCode (length: 10)
        public System.Guid OwnerId { get; set; } // OwnerId
        public System.Guid? AdministratorId { get; set; } // AdministratorId

        public Organization()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
