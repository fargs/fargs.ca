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

    // BillableEntity
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class BillableEntityConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<BillableEntity>
    {
        public BillableEntityConfiguration()
            : this("API")
        {
        }

        public BillableEntityConfiguration(string schema)
        {
            ToTable(schema + ".BillableEntity");
            HasKey(x => x.EntityType);

            Property(x => x.EntityGuid).HasColumnName(@"EntityGuid").IsOptional().HasColumnType("uniqueidentifier");
            Property(x => x.EntityName).HasColumnName(@"EntityName").IsOptional().HasColumnType("nvarchar").HasMaxLength(200);
            Property(x => x.EntityType).HasColumnName(@"EntityType").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(9);
            Property(x => x.EntityId).HasColumnName(@"EntityId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.LogoCssClass).HasColumnName(@"LogoCssClass").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.AddressName).HasColumnName(@"AddressName").IsOptional().HasColumnType("nvarchar").HasMaxLength(256);
            Property(x => x.Attention).HasColumnName(@"Attention").IsOptional().HasColumnType("nvarchar").HasMaxLength(255);
            Property(x => x.Address1).HasColumnName(@"Address1").IsOptional().HasColumnType("nvarchar").HasMaxLength(255);
            Property(x => x.Address2).HasColumnName(@"Address2").IsOptional().HasColumnType("nvarchar").HasMaxLength(255);
            Property(x => x.City).HasColumnName(@"City").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.PostalCode).HasColumnName(@"PostalCode").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.ProvinceName).HasColumnName(@"ProvinceName").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.CountryName).HasColumnName(@"CountryName").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.BillingEmail).HasColumnName(@"BillingEmail").IsOptional().HasColumnType("nvarchar").HasMaxLength(256);
            Property(x => x.Phone).HasColumnName(@"Phone").IsOptional().HasColumnType("nvarchar");
            Property(x => x.HstNumber).HasColumnName(@"HstNumber").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
