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

    // Country
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class Country
    {
        public short Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name (length: 100)
        public short Iso3DigitCountry { get; set; } // ISO3DigitCountry
        public string Iso2CountryCode { get; set; } // ISO2CountryCode (length: 2)
        public string Iso3CountryCode { get; set; } // ISO3CountryCode (length: 3)
        public System.DateTime ModifiedDate { get; set; } // ModifiedDate
        public string ModifiedUser { get; set; } // ModifiedUser (length: 256)

        // Reverse navigation
        public virtual System.Collections.Generic.ICollection<Address> Addresses { get; set; } // Address.FK_Address_Countries
        public virtual System.Collections.Generic.ICollection<AddressV2> AddressV2 { get; set; } // AddressV2.FK_AddressV2_Countries

        public Country()
        {
            ModifiedDate = System.DateTime.Now;
            ModifiedUser = "suser_name()";
            Addresses = new System.Collections.Generic.List<Address>();
            AddressV2 = new System.Collections.Generic.List<AddressV2>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
