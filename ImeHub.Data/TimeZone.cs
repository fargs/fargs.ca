// <auto-generated>
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable EmptyNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// TargetFrameworkVersion = 4.7
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning


namespace ImeHub.Data
{

    // TimeZone
    public partial class TimeZone
    {
        public short Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name (length: 50)
        public string Iana { get; set; } // IANA (length: 50)
        public string Iso { get; set; } // ISO (length: 50)

        // Reverse navigation

        /// <summary>
        /// Child Addresses where [Address].[TimeZoneId] point to this entity (FK_Address_TimeZone)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Address> Addresses { get; set; } // Address.FK_Address_TimeZone

        public TimeZone()
        {
            Addresses = new System.Collections.Generic.List<Address>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
