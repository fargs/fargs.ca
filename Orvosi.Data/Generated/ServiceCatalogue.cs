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

    // ServiceCatalogue
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class ServiceCatalogue
    {
        public short Id { get; set; } // Id (Primary key)
        public string PhysicianId { get; set; } // PhysicianId (length: 128)
        public short? ServiceId { get; set; } // ServiceId
        public short? CompanyId { get; set; } // CompanyId
        public short? LocationId { get; set; } // LocationId
        public decimal? Price { get; set; } // Price
        public System.DateTime ModifiedDate { get; set; } // ModifiedDate
        public string ModifiedUser { get; set; } // ModifiedUser (length: 100)
        public decimal? NoShowRate { get; set; } // NoShowRate
        public decimal? LateCancellationRate { get; set; } // LateCancellationRate

        public ServiceCatalogue()
        {
            ModifiedDate = System.DateTime.Now;
            ModifiedUser = "suser_name()";
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>