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

    // Location
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class LocationView
    {
        public int Id { get; set; } // Id
        public System.Guid ObjectGuid { get; set; } // ObjectGuid
        public System.Guid? OwnerGuid { get; set; } // OwnerGuid
        public System.Guid? EntityId { get; set; } // EntityId
        public string EntityDisplayName { get; set; } // EntityDisplayName (length: 210)
        public string EntityType { get; set; } // EntityType (length: 9)
        public byte AddressTypeId { get; set; } // AddressTypeID
        public string AddressTypeName { get; set; } // AddressTypeName (length: 50)
        public string Name { get; set; } // Name (length: 256)
        public string Attention { get; set; } // Attention (length: 255)
        public string Address1 { get; set; } // Address1 (length: 255)
        public string Address2 { get; set; } // Address2 (length: 255)
        public string City { get; set; } // City (length: 50)
        public string PostalCode { get; set; } // PostalCode (length: 50)
        public short CountryId { get; set; } // CountryID
        public short? ProvinceId { get; set; } // ProvinceID
        public short? LocationId { get; set; } // LocationId
        public System.DateTime ModifiedDate { get; set; } // ModifiedDate
        public string ModifiedUser { get; set; } // ModifiedUser (length: 256)
        public string CountryName { get; set; } // CountryName (length: 100)
        public string CountryCode { get; set; } // CountryCode (length: 3)
        public string ProvinceName { get; set; } // ProvinceName (length: 100)
        public string ProvinceCode { get; set; } // ProvinceCode (length: 50)
        public string LocationName { get; set; } // LocationName (length: 128)
        public string LocationShortName { get; set; } // LocationShortName (length: 50)

        public LocationView()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
