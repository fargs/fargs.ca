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

    // CompanyService
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class CompanyService
    {
        public System.Guid Id { get; set; } // Id (Primary key)
        public System.Guid? ServiceId { get; set; } // ServiceId
        public decimal? Price { get; set; } // Price
        public System.Guid CompanyId { get; set; } // CompanyId
        public string Name { get; set; } // Name (length: 250)
        public bool IsTravelRequired { get; set; } // IsTravelRequired

        // Reverse navigation
        public virtual System.Collections.Generic.ICollection<TravelPrice> TravelPrices { get; set; } // TravelPrice.FK_TravelPrice_CompanyService

        // Foreign keys
        public virtual CompanyV2 CompanyV2 { get; set; } // FK_CompanyService_Company
        public virtual ServiceV2 ServiceV2 { get; set; } // FK_CompanyService_Service

        public CompanyService()
        {
            IsTravelRequired = false;
            TravelPrices = new System.Collections.Generic.List<TravelPrice>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>