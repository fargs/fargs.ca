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

    // City
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class City
    {
        public short Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name (length: 128)
        public string Code { get; set; } // Code (length: 3)
        public short ProvinceId { get; set; } // ProvinceId
        public System.DateTime ModifiedDate { get; set; } // ModifiedDate
        public string ModifiedUser { get; set; } // ModifiedUser (length: 128)

        // Reverse navigation
        public virtual System.Collections.Generic.ICollection<Address> Addresses { get; set; } // Address.FK_Address_City

        // Foreign keys
        public virtual Province Province { get; set; } // FK_City_Province

        public City()
        {
            ModifiedDate = System.DateTime.Now;
            ModifiedUser = "suser_name()";
            Addresses = new System.Collections.Generic.List<Address>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>