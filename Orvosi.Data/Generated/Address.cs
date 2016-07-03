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

    // Address
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class Address
    {
        public int Id { get; set; } // Id (Primary key)
        public System.Guid ObjectGuid { get; set; } // ObjectGuid
        public System.Guid? OwnerGuid { get; set; } // OwnerGuid
        public byte AddressTypeId { get; set; } // AddressTypeID
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

        // Reverse navigation
        public virtual System.Collections.Generic.ICollection<ServiceRequest> ServiceRequests { get; set; } // ServiceRequest.FK_ServiceRequest_Address

        // Foreign keys
        public virtual AddressType AddressType { get; set; } // FK_Address_AddressType
        public virtual Country Country { get; set; } // FK_Address_Countries
        public virtual Province Province { get; set; } // FK_Address_Provinces

        public Address()
        {
            ObjectGuid = System.Guid.NewGuid();
            CountryId = 124;
            ModifiedDate = System.DateTime.Now;
            ModifiedUser = "suser_name()";
            ServiceRequests = new System.Collections.Generic.List<ServiceRequest>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
