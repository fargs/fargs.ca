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
    public partial class ServiceCatalogueConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ServiceCatalogue>
    {
        public ServiceCatalogueConfiguration()
            : this("dbo")
        {
        }

        public ServiceCatalogueConfiguration(string schema)
        {
            ToTable(schema + ".ServiceCatalogue");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("smallint").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.PhysicianId).HasColumnName(@"PhysicianId").IsOptional().HasColumnType("uniqueidentifier");
            Property(x => x.ServiceId).HasColumnName(@"ServiceId").IsOptional().HasColumnType("smallint");
            Property(x => x.CompanyId).HasColumnName(@"CompanyId").IsOptional().HasColumnType("smallint");
            Property(x => x.LocationId).HasColumnName(@"LocationId").IsOptional().HasColumnType("smallint");
            Property(x => x.Price).HasColumnName(@"Price").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.ModifiedDate).HasColumnName(@"ModifiedDate").IsRequired().HasColumnType("datetime");
            Property(x => x.ModifiedUser).HasColumnName(@"ModifiedUser").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.NoShowRate).HasColumnName(@"NoShowRate").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.LateCancellationRate).HasColumnName(@"LateCancellationRate").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
