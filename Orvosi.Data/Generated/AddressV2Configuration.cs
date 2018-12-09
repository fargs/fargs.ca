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

    // AddressV2
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class AddressV2Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<AddressV2>
    {
        public AddressV2Configuration()
            : this("dbo")
        {
        }

        public AddressV2Configuration(string schema)
        {
            ToTable(schema + ".AddressV2");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.CompanyId).HasColumnName(@"CompanyId").IsOptional().HasColumnType("uniqueidentifier");
            Property(x => x.PhysicianId).HasColumnName(@"PhysicianId").IsOptional().HasColumnType("uniqueidentifier");
            Property(x => x.AddressTypeId).HasColumnName(@"AddressTypeID").IsRequired().HasColumnType("tinyint");
            Property(x => x.Name).HasColumnName(@"Name").IsOptional().HasColumnType("nvarchar").HasMaxLength(256);
            Property(x => x.Attention).HasColumnName(@"Attention").IsOptional().HasColumnType("nvarchar").HasMaxLength(255);
            Property(x => x.Address1).HasColumnName(@"Address1").IsRequired().HasColumnType("nvarchar").HasMaxLength(255);
            Property(x => x.Address2).HasColumnName(@"Address2").IsOptional().HasColumnType("nvarchar").HasMaxLength(255);
            Property(x => x.CityId).HasColumnName(@"CityId").IsRequired().HasColumnType("smallint");
            Property(x => x.PostalCode).HasColumnName(@"PostalCode").IsRequired().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.CountryId).HasColumnName(@"CountryID").IsRequired().HasColumnType("smallint");
            Property(x => x.ProvinceId).HasColumnName(@"ProvinceID").IsRequired().HasColumnType("smallint");
            Property(x => x.TimeZoneId).HasColumnName(@"TimeZoneId").IsRequired().HasColumnType("smallint");

            // Foreign keys
            HasOptional(a => a.CompanyV2).WithMany(b => b.AddressV2).HasForeignKey(c => c.CompanyId); // FK_AddressV2_CompanyV2
            HasRequired(a => a.AddressType).WithMany(b => b.AddressV2).HasForeignKey(c => c.AddressTypeId).WillCascadeOnDelete(false); // FK_AddressV2_AddressType
            HasRequired(a => a.City).WithMany(b => b.AddressV2).HasForeignKey(c => c.CityId).WillCascadeOnDelete(false); // FK_AddressV2_City
            HasRequired(a => a.Country).WithMany(b => b.AddressV2).HasForeignKey(c => c.CountryId).WillCascadeOnDelete(false); // FK_AddressV2_Countries
            HasRequired(a => a.Province).WithMany(b => b.AddressV2).HasForeignKey(c => c.ProvinceId).WillCascadeOnDelete(false); // FK_AddressV2_Provinces
            HasRequired(a => a.TimeZone).WithMany(b => b.AddressV2).HasForeignKey(c => c.TimeZoneId).WillCascadeOnDelete(false); // FK_AddressV2_TimeZone
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>