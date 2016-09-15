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

    // PhysicianCompanyStatus
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class PhysicianCompanyStatu
    {
        public short Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name (length: 128)

        // Reverse navigation
        public virtual System.Collections.Generic.ICollection<PhysicianCompany> PhysicianCompanies { get; set; } // PhysicianCompany.FK_PhysicianCompany_PhysicianCompanyStatus

        public PhysicianCompanyStatu()
        {
            PhysicianCompanies = new System.Collections.Generic.List<PhysicianCompany>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>