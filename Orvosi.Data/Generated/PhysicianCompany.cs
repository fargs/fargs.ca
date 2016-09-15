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

    // PhysicianCompany
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class PhysicianCompany
    {
        public short Id { get; set; } // Id (Primary key)
        public System.Guid PhysicianId { get; set; } // PhysicianId
        public short CompanyId { get; set; } // CompanyId
        public short StatusId { get; set; } // StatusId
        public System.DateTime ModifiedDate { get; set; } // ModifiedDate
        public string ModifiedUser { get; set; } // ModifiedUser (length: 100)

        // Foreign keys
        public virtual Company Company { get; set; } // FK_PhysicianCompany_Company
        public virtual Physician Physician { get; set; } // FK_PhysicianCompany_Physician
        public virtual PhysicianCompanyStatu PhysicianCompanyStatu { get; set; } // FK_PhysicianCompany_PhysicianCompanyStatus

        public PhysicianCompany()
        {
            StatusId = 1;
            ModifiedDate = System.DateTime.Now;
            ModifiedUser = "suser_name()";
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
