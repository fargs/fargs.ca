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

    // TravelPrice
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class TravelPrice
    {
        public System.Guid Id { get; set; } // Id (Primary key)
        public System.Guid CompanyServiceId { get; set; } // CompanyServiceId
        public decimal Price { get; set; } // Price
        public short CityId { get; set; } // CityId

        // Foreign keys
        public virtual City City { get; set; } // FK_TravelPrice_City
        public virtual CompanyService CompanyService { get; set; } // FK_TravelPrice_CompanyService

        public TravelPrice()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
