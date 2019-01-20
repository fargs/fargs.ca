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

    public partial class City
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public short ProvinceId { get; set; }
        public System.Guid PhysicianId { get; set; }

        public virtual System.Collections.Generic.ICollection<Address> Addresses { get; set; }
        public virtual System.Collections.Generic.ICollection<AvailableDay> AvailableDays { get; set; }
        public virtual System.Collections.Generic.ICollection<TravelPrice> TravelPrices { get; set; }


        public virtual Physician Physician { get; set; }

        public virtual Province Province { get; set; }

        public City()
        {
            Addresses = new System.Collections.Generic.List<Address>();
            AvailableDays = new System.Collections.Generic.List<AvailableDay>();
            TravelPrices = new System.Collections.Generic.List<TravelPrice>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
